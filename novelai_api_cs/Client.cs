using System.Net;
using System.Net.Http.Headers;
using System.Text;

using System.Text.Json;
using System.Text.Json.Serialization;

class NAIToken
{
  [JsonPropertyName("accessToken")]
  public string? AccessToken { get; set; }
}

class NAIClient
{
  private readonly HttpClient _httpClient;
  public readonly string BearerToken;

  private NAIClient(string token, HttpClient httpClient)
  {
    BearerToken = token;
    _httpClient = httpClient;
    _httpClient.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("Bearer", BearerToken);
  }

  private static async Task<string> FetchBearerToken(HttpClient client, string key)
  {
    string uriString = "https://api.novelai.net/user/login";
    var requestUri = new Uri(uriString);

    var jsonContent = new StringContent(
      JsonSerializer.Serialize(new
      {
        key = key,
      }),
      Encoding.UTF8,
      "application/json"
    );

    var request = new HttpRequestMessage()
    {
      Method = HttpMethod.Post,
      RequestUri = requestUri,
      Headers = {
          {
            HttpRequestHeader.ContentType.ToString(),
            "application/json"
          },
          {
            HttpRequestHeader.Referer.ToString(),
            "https://novelai.net"
          },
          {
            HttpRequestHeader.UserAgent.ToString(),
            "Mozilla/5.0 (X11; Linux x86_64; rv:138.0) Gecko/20100101 Firefox/138.0"
          },
        },
      Content = jsonContent,
    };

    HttpResponseMessage response = await client.SendAsync(request);
    response.EnsureSuccessStatusCode();

    string responseBody = await response.Content.ReadAsStringAsync();

    var naiToken = JsonSerializer.Deserialize<NAIToken>(responseBody);
    var token = naiToken?.AccessToken;

    ArgumentException.ThrowIfNullOrEmpty(token);

    return token;
  }

  public static async Task<NAIClient> New()
  {
    var httpClient = new HttpClient();

    string username = DotEnvLoader.Load("USERNAME");
    string password = DotEnvLoader.Load("PASSWORD");

    string key = NAIHasher.EncodeKey(username, password);

    var token = await FetchBearerToken(httpClient, key);

    var naiClient = new NAIClient(token, httpClient);

    return naiClient;
  }
}