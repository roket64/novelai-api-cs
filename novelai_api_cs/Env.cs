using DotNetEnv;

class EnvLoader
{
  private EnvLoader() { }

  public static char[] Load(string var)
  {
    Env.Load();
    // FIXME: plain text is allocated in heap as string, which is not secure..
    // SecureString is not ideal, it still saves the string without encryption on Linux
    // maybe I can use a native system call such as getenv(Linux) or GetEnvironmentVariableW(Windows)
    string? value = Environment.GetEnvironmentVariable(var);
    ArgumentException.ThrowIfNullOrEmpty(value);

    // this also allocates a new char array in heap, must be wiped in the caller function
    char[] result = value.ToCharArray();

    return result;
  }
}