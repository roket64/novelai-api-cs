using System.Security;
using DotNetEnv;

class DotEnvLoader
{
  public static string Load(string var)
  {
    Env.Load();
    string? value = Environment.GetEnvironmentVariable(var);
    ArgumentException.ThrowIfNullOrEmpty(value);

    return value;
  }

  public static SecureString LoadSecret(string var)
  {
    Env.Load();
    string? secretValue = Environment.GetEnvironmentVariable(var);
    ArgumentException.ThrowIfNullOrEmpty(secretValue);

    SecureString secret = SecretHandler.ConvertToSecureString(secretValue);

    return secret;
  }
}