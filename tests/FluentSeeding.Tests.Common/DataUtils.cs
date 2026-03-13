namespace FluentSeeding.Tests.Common;

public static class DataUtils
{
    public static string GenerateEmail(string name)
    {
        var sanitized = new string(name.Where(char.IsLetterOrDigit).ToArray()).ToLower();
        return $"{sanitized}@email.com";
    }
    
    
}