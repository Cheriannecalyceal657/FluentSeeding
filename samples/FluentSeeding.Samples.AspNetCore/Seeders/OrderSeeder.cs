using FluentSeeding;
using FluentSeeding.Samples.AspNetCore.Entities;

namespace FluentSeeding.Samples.AspNetCore.Seeders;

public class OrderSeeder : EntitySeeder<Order>
{
    private readonly CustomerSeeder _customerSeeder;
    private readonly ProductSeeder _productSeeder;

    public OrderSeeder(CustomerSeeder customerSeeder, ProductSeeder productSeeder)
    {
        _customerSeeder = customerSeeder;
        _productSeeder = productSeeder;
    }

    protected override void Configure(SeedBuilder<Order> builder)
    {
        var products = _productSeeder.Data;

        builder
            .Count(15);
        builder.RuleFor(x => x.Id).UseIdempotentGuid();
        builder.RuleFor(x => x.CustomerId).UseFrom(_customerSeeder.Data.Select(c => c.Id));
        builder.RuleFor(x => x.CreatedAt).UseFactory(() => DateTime.UtcNow.AddDays(-Random.Shared.Next(0, 365)));
        builder.RuleFor(x => x.Status).UseFrom(OrderStatus.Pending, OrderStatus.Processing, OrderStatus.Completed,
            OrderStatus.Cancelled);
        builder.HasMany(x => x.OrderItems, items =>
            {
                items.Count(1, 5);
                items.RuleFor(i => i.Id).UseIdempotentGuid("productorderitem");
                items.RuleFor(i => i.ProductId).UseFrom(products.Select(p => p.Id));
                items.RuleFor(i => i.Quantity).UseFactory(() => Random.Shared.Next(1, 10));
                items.RuleFor(i => i.UnitPrice)
                    .UseFactory(() => Math.Round((decimal)(Random.Shared.NextDouble() * 195 + 5), 2));
            }
        );
    }
}