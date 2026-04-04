class Program
{
  static async Task<int> Main()
  {
    // var client = await NAIClient.New();
    // var token = client.BearerToken;

    // HttpClient client = new();

    // string username = user.Username;
    // string password = user.Password;

    // // NovelAI login endpoint
    // string requestUri = "https://api.novelai.net/user/login";
    // // string key = NAIHasher.EncodeKey(username, password);

    // HttpRequestMessage request = new(HttpMethod.Post, requestUri);

    // Dictionary<string, string> header = new()
    // {
    //   { "Origin", "https://novelai.net" },
    //   { "Referer", "https://novelai.net" },
    //   { "User-Agent", "Mozilla/5.0 (X11; Linux x86_64; rv:138.0) Gecko/20100101 Firefox/138.0" }
    // };

    // // building new header
    // foreach (var entry in header)
    // {
    //   try
    //   {
    //     request.Headers.Add(entry.Key, entry.Value);
    //   }
    //   catch (Exception e)
    //   {
    //     Console.WriteLine($"{e.Message}");
    //   }
    // }

    // StringContent jsonContent = new(
    //   JsonSerializer.Serialize(new
    //   {
    //     // key = key,
    //   }),
    //   Encoding.UTF8,
    //   "application/json"
    // );

    // request.Content = jsonContent;

    // Console.WriteLine($"{request}");

    // try
    // {
    //   HttpResponseMessage response = await client.SendAsync(request);
    //   response.EnsureSuccessStatusCode();

    //   string responseBody = await response.Content.ReadAsStringAsync();

    //   Console.WriteLine($"{responseBody}");
    // }
    // catch (Exception e)
    // {
    //   Console.WriteLine($"{e.Message}");
    // }

    // return 0;
    return 0;
  }
}
