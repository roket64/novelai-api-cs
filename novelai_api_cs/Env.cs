using System.Security;
using DotNetEnv;

class DotEnvLoader
{
  public static string Load(string var)
  {
    Env.Load();
    string? value = Environment.GetEnvironmentVariable(var);

    if (string.IsNullOrEmpty(value))
    {
      return "";
    }

    return value;
  }

  public static SecureString LoadSecret(string var)
  {
    Env.Load();
    string? secretValue = Environment.GetEnvironmentVariable(var);

    if (string.IsNullOrEmpty(secretValue))
    {
      return new();
    }

    SecureString secureSecret = new();

    return new();
  }
}