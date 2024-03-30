using System.Text.Json;

namespace CFPService.Api.Naming;

internal sealed class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return name;

        name = ConvertCamelCase(name);

        return ConvertSnakeCase(name);
    }

    private string ConvertCamelCase(string name)
    {
        return System.Text.RegularExpressions.Regex.Replace(name, @"([a-z])([A-Z])", "$1_$2");
    }

    private string ConvertSnakeCase(string name)
    {
        return name.ToLower().Replace(" ", "_");
    }
}