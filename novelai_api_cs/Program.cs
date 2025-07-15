class Program
{
  static async Task<int> Main()
  {
    var client = await NAIClient.New();
    var token = client.BearerToken;

    Console.WriteLine($"{token}");

    return 0;
  }
}
