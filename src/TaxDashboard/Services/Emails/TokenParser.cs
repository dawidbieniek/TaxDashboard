namespace TaxDashboard.Services.Emails;

internal class TokenParser
{
    private readonly Dictionary<string, string> _tokens = [];

    public void RegisterToken(string token, string value) => _tokens.Add(token, value);

    public string ReplaceTokens(string template)
    {
        string result = template;

        foreach (var pair in _tokens)
        {
            string tokenPattern = $"{{{pair.Key}}}";
            result = result.Replace(tokenPattern, pair.Value?.ToString() ?? "");
        }

        return result;
    }
}