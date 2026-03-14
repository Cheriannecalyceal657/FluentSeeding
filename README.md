# FluentSeeding

A fluent, composable data seeding library for .NET. Define how your entities are generated using a clean, chainable API, then wire them up to your database with first-class Entity Framework Core and ASP.NET Core support.

## Packages

| Package | Description |
|---------|-------------|
| `FluentSeeding` | Core library with builders, rules, idempotency |
| `FluentSeeding.EntityFrameworkCore` | EF Core persistence layer |
| `FluentSeeding.AspNetCore` | Microsoft DI and hosted application integration |

## Quick Start

### 1. Define a seeder

```csharp
public sealed class UserSeeder : EntitySeeder<User>
{
    protected override void Configure(SeedBuilder<User> builder)
    {
        builder.Count(10)
            .RuleFor(u => u.Id).UseFactory(Guid.NewGuid)
            .RuleFor(u => u.Name).UseValue("Test User")
            .RuleFor(u => u.Email).UseFactory(i => $"user{i}@example.com");
    }
}
```

### 2. Register with DI

```csharp
builder.Services.AddFluentSeeding(seederConfig =>
    seederConfig
        .AddSeeder<UserSeeder>()
        .AddSeeder<ProductSeeder>()
        .AddSeeder<PurchaseSeeder>());

// Register an EF Core persistence layer
builder.Services.AddFluentSeedingEntityFrameworkCore<SampleDbContext>(options =>
    options.ConflictBehavior = ConflictBehavior.Skip);
```

Alternatively, register all seeders within an assembly:
```csharp
builder.Services.AddFluentSeeding(seeders => { seeders.AddSeedersFromAssemblyContaining<OrderSeeder>(); })
```

### 3. Run seeders

```csharp
var app = builder.Build();
await app.RunSeedersAsync();
await app.RunAsync();
```

---

## Core Concepts

### SeedBuilder\<T\>

The fluent builder that describes how to generate instances of `T`.

```csharp
var builder = new SeedBuilder<Product>();
builder.Count(5)
    .RuleFor(p => p.Id).UseFactory(Guid.NewGuid)
    .RuleFor(p => p.Name).UseFrom("Widget", "Gadget", "Doohickey")
    .RuleFor(p => p.Price).UseFactory(() => Math.Round(Random.Shared.NextDouble() * 100, 2));

IEnumerable<Product> products = builder.Build();
```

Call `Build()` to materialize the entities. Rules execute in dependency order (see [Dependencies](#dependencies)).

### RuleFor

`RuleFor(selector)` opens a rule for a property. Chain a **terminal** to set its value source:

| Terminal | Description |
|----------|-------------|
| `UseValue(value)` | Same constant value for every entity |
| `UseFactory(Func<TProperty>)` | Invoked once per entity |
| `UseFactory(Func<int, TProperty>)` | Index-aware factory that receives the entity's zero-based position |
| `UseFrom(params TProperty[])` | Random selection from a fixed pool |
| `UseFrom(IEnumerable<TProperty>)` | Random selection from a sequence |

After a terminal, you are back on `SeedBuilder<T>` and can continue chaining.

### Modifiers

Add modifiers **before** the terminal to control behaviour:

```csharp
builder.RuleFor(u => u.Email)
    .Unique()                               // enforce uniqueness across all generated entities
    .When(u => u.Role == "admin")           // only apply this rule when predicate is true
    .DependsOn(u => u.Role)                 // declare execution order explicitly
    .UseFactory(i => $"admin{i}@corp.com");
```

Rules by default are executed in the order they are declared, using `DependsOn` is more of a safety net or more control over it.

### Dependencies

`DependsOn()` causes `Build()` to topologically sort rules so that a rule always runs after the properties it depends on have already been set. Circular dependencies are detected and throw `InvalidOperationException`.

```csharp
builder.RuleFor(u => u.Role).UseValue("admin");

builder.RuleFor(u => u.Permissions)
    .DependsOn(u => u.Role)
    .When(u => u.Role == "admin")
    .UseValue(Permission.All);
```

### Nested Objects

**HasOne**: a single nested instance per parent

```csharp
builder.HasOne(u => u.Profile, profile =>
    profile.RuleFor(p => p.Bio).UseValue("Hello!"));
```

**HasMany**: a collection per parent

```csharp
builder.Count(3)
    .HasMany(u => u.Purchases, purchases =>
        purchases.Count(5)
            .RuleFor(p => p.Id).UseFactory(Guid.NewGuid)
            .RuleFor(p => p.Quantity).UseFactory(() => Random.Shared.Next(1, 10)));
// Creates 3 users, each with their own list of 5 purchases
```

---

## Idempotency

When you need the same data every run, no matter the occasion, use the `Idempotent` helpers. Values are derived deterministically from the entity type, entity index, and an optional seed string using UUID v5 (RFC 4122).

```csharp
Idempotent.Guid<User>(index: 0);              // always the same GUID for User #0
Idempotent.Int<Product>(index: 1);            // deterministic int for Product #1
Idempotent.Long<Order>(index: 2);             // deterministic long for Order #2
Idempotent.Slug<Category>(index: 0, "cat");   // "cat-0"
```

The idempotent terminals are available directly on `SeedRule<T, TProperty>`:

```csharp
builder.Count(5)
    .RuleFor(u => u.Id).UseIdempotentGuid()
    .RuleFor(u => u.ExternalRef).UseIdempotentSlug("user");
// Produces "user-0", "user-1", ... every single time
```

---

## EntitySeeder\<T\>

`EntitySeeder<T>` is an abstract base class for reusable, injectable seeders. The generated data is cached after the first call to `Data`, making it safe to reference from other seeders.

```csharp
public sealed class PurchaseSeeder : EntitySeeder<Purchase>
{
    private readonly UserSeeder _users;
    private readonly ProductSeeder _products;

    public PurchaseSeeder(UserSeeder users, ProductSeeder products)
    {
        _users = users;
        _products = products;
    }

    protected override void Configure(SeedBuilder<Purchase> builder)
    {
        builder.Count(50)
            .RuleFor(p => p.Id).UseFactory(Guid.NewGuid)
            .RuleFor(p => p.UserId).UseFrom(_users.Data.Select(u => u.Id))
            .RuleFor(p => p.ProductId).UseFrom(_products.Data.Select(p => p.Id))
            .RuleFor(p => p.Quantity).UseFactory(() => Random.Shared.Next(1, 100));
    }
}
```

---

## Persistence

### IPersistenceLayer

Implement `IPersistenceLayer` to back seeding with any storage:

```csharp
public interface IPersistenceLayer
{
    void Persist<T>(IEnumerable<T> entities);
    Task PersistAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    void Flush();
    Task FlushAsync(CancellationToken cancellationToken = default);
}
```

### Entity Framework Core

`EntityFrameworkCorePersistenceLayer` stages entities and commits with `SaveChanges()`. Configure how to handle pre-existing records via `ConflictBehavior`:

| Behavior | Description |
|----------|-------------|
| `ConflictBehavior.Insert` | Always insert (default). Throws on key conflict |
| `ConflictBehavior.Skip` | Skip entities whose primary key already exists |
| `ConflictBehavior.Update` | Update existing, insert new |

```csharp
services.AddScoped<IPersistenceLayer>(sp =>
    new EntityFrameworkCorePersistenceLayer(
        sp.GetRequiredService<AppDbContext>(),
        ConflictBehavior.Skip));
```

---

## ASP.NET Core Integration

### SeederRunner

`SeederRunner` orchestrates multiple seeders in registration order. All entities are staged first; then a single `FlushAsync()` commits everything atomically.

```csharp
// Manual usage (without DI)
var runner = new SeederRunner(persistenceLayer, new IEntitySeeder[] { userSeeder, productSeeder });
await runner.RunAsync();
```

When using `AddFluentSeeding`, `SeederRunner` is registered automatically and resolved by `RunSeedersAsync()`.
