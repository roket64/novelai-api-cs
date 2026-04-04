
public class RequestHandler
{
  private RequestHandler() { }

  public async static Task<HttpResponseMessage> Send(HttpRequestMessage request)
  {
    try
    {
      var response = await new HttpClient().SendAsync(request);
      return response;
    }
    catch (Exception e)
    {
      Console.WriteLine($"Failed to send request: {e.Message}");
      throw;
    }
  }
}