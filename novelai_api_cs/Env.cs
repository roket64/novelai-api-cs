using System.Runtime.InteropServices;
using System.Security.Cryptography;
using DotNetEnv;

class EnvLoader
{
  private EnvLoader() { }

  [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  private static extern uint GetEnvironmentVariableW(string lpName, IntPtr lpBuffer, uint nSize);

  private static char[] LoadWindows(string var)
  {
    var bufferSize = GetEnvironmentVariableW(var, IntPtr.Zero, 0);

    if (bufferSize == 0)
    {
      throw new ArgumentException($"Failed to load environment variable: {var}.");
    }

    IntPtr buffer = Marshal.AllocHGlobal((int)(bufferSize * 2));
    try
    {
      GetEnvironmentVariableW(var, buffer, bufferSize);
      var result = new char[bufferSize - 1];
      Marshal.Copy(buffer, result, 0, (int)(bufferSize - 1));
      return result;
    }
    finally
    {
      Marshal.FreeHGlobal(buffer);
      unsafe
      {
        var ptr = (byte*)buffer.ToPointer();
        CryptographicOperations.ZeroMemory(new Span<byte>(ptr, (int)(bufferSize * 2)));
      }
    }
  }

  [DllImport("libc", EntryPoint = "getenv", CharSet = CharSet.Ansi)]
  private static extern IntPtr getenv(string name);

  private static char[] LoadLinux(string var)
  {
    var ptr = getenv(var);

    if (ptr == IntPtr.Zero)
    {
      throw new ArgumentException($"Failed to load environment variable: {var}.");
    }

    var bufferSize = 0;
    while (Marshal.ReadByte(ptr, bufferSize) != 0)
    {
      ++bufferSize;
    }

    var buffer = Marshal.AllocHGlobal(bufferSize + 1);
    try
    {
      for (int i = 0; i < bufferSize; ++i)
      {
        Marshal.WriteByte(buffer, i, Marshal.ReadByte(ptr, i));
      }
      Marshal.WriteByte(buffer, bufferSize, 0x0);

      var result = new char[bufferSize];
      for (int i = 0; i < bufferSize; ++i)
      {
        result[i] = (char)Marshal.ReadByte(buffer, i);
      }
      return result;
    }
    finally
    {
      Marshal.FreeHGlobal(buffer);
      unsafe
      {
        var bufferPtr = (byte*)buffer.ToPointer();
        CryptographicOperations.ZeroMemory(new Span<byte>(bufferPtr, bufferSize + 1));
      }
    }
  }

  public static char[] Load(string var)
  {
    Env.Load();
    string? value = Environment.GetEnvironmentVariable(var);
    ArgumentException.ThrowIfNullOrEmpty(value);

    // this also allocates a new char array in heap, must be wiped in the caller function
    char[] result = value.ToCharArray();

    return result;
  }
}