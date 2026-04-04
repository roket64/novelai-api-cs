class Program
{
  static async Task<int> Main()
  {
    try
    {
      var client = await NAIClient.New();
    }
    catch (Exception e)
    {
      Console.WriteLine($"Failed to create NAIClient {e.Message}");
      return 1;
    }

    return 0;
  }
}
