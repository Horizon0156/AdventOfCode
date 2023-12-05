using System.Text.RegularExpressions;
using Flurl.Http;
using Microsoft.Extensions.Options;

namespace AdventOfCode.Runtime;

internal class HttpPuzzleLoader : IPuzzleLoader
{
    private readonly Settings _settings;

    public HttpPuzzleLoader(IOptions<Settings> settings)
    {
        _settings = settings.Value;
    }

    public async Task<string> GetPuzzleName(DateOnly date, CancellationToken cancellationToken)
    {
        try
        {
            var pageContent = await $"https://adventofcode.com/{date.Year}/day/{date.Day}"
                                .WithCookie("session", _settings.SessionToken)
                                .GetStringAsync(cancellationToken);
            var titleMatch = Regex.Match(pageContent, @"--- Day \d+: (.+) ---");
            return titleMatch.Success 
                ? titleMatch.Groups[1].Value
                : string.Empty;
        }
        catch (FlurlHttpException)
        {
            return string.Empty;
        }
    }

    public async Task<string> LoadPuzzleInputAsync(DateOnly date, CancellationToken cancellationToken)
    {
        try
        {
            var input = await $"https://adventofcode.com/{date.Year}/day/{date.Day}/input"
                                .WithCookie("session", _settings.SessionToken)
                                .GetStringAsync(cancellationToken);
                    
            return input.ReplaceLineEndings()
                        .TrimEnd(Environment.NewLine.ToCharArray());
        }
        catch (FlurlHttpException e)
        {
            switch (e.StatusCode)
            {
                case 400:
                    throw new ApiException("The session seems to be invalid, please update the token in your appsettings.", e);
                case 404: 
                    throw new ApiException("The puzzle wasn't found... probably the Elves are still working on it", e);
                default:
                    throw new ApiException("Yikes! Something unexpected occured... WiFi enabled?", e);
            }
        }
    }
}
