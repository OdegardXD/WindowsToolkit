using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Text.Json;


namespace WindowsToolkit
{
    internal class CheckForUpdate
    {
        public static async Task<Version?> GetLatestReleaseVersionAsync()
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("WindowsToolkit"); // GitHub API requires a User-Agent or it 403s

            var json = await client.GetStringAsync("https://api.github.com/repos/OdegardXD/WindowsToolkit/releases/latest");
            using var doc = JsonDocument.Parse(json);
            var tag = doc.RootElement.GetProperty("tag_name").GetString(); // "v1.0.2"

            return Version.TryParse(tag?.TrimStart('v'), out var v) ? v : null;
        }
    }
}
