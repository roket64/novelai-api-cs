using System.Net;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
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

  private NAIClient(string token)
  {
    var httpClient = new HttpClient();

    BearerToken = token;
    _httpClient = httpClient;
    _httpClient.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("Bearer", BearerToken);
  }

  private static HttpRequestMessage BuildNAIRequest(string key)
  {
    var endpoint = "https://api.novelai.net/user/login";
    var loginUri = new Uri(endpoint);

    var jsonContent = new StringContent(
      JsonSerializer.Serialize(new
      {
        key,
      }),
      Encoding.UTF8,
      "application/json"
    );

    var request = new HttpRequestMessage()
    {
      Method = HttpMethod.Post,
      RequestUri = loginUri,
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

    return request;
  }

  private static async Task<string> FetchBearerToken(string key)
  {
    var request = BuildNAIRequest(key);
    var response = await RequestHandler.Send(request);
    var responseBody = await response.Content.ReadAsStringAsync();

    var naiToken = JsonSerializer.Deserialize<NAIToken>(responseBody);
    var token = naiToken?.AccessToken;

    ArgumentException.ThrowIfNullOrEmpty(token);

    return token;
  }

  public static async Task<NAIClient> New()
  {
    var username = EnvLoader.Load("USERNAME");
    var password = EnvLoader.Load("PASSWORD");

    var key = NAIHasher.EncodeKey(username, password);

    try
    {
      var bearerToken = await FetchBearerToken(key);
      var naiClient = new NAIClient(bearerToken);
      return naiClient;
    }
    finally
    {
      CryptographicOperations.ZeroMemory(MemoryMarshal.AsBytes(username.AsSpan()));
      CryptographicOperations.ZeroMemory(MemoryMarshal.AsBytes(password.AsSpan()));
    }
  }
}