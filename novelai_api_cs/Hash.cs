using System.Text;

using Konscious.Security.Cryptography;
using Isopoh.Cryptography.Blake2b;
using System.Security;
using DotNetEnv;
using System.Runtime.InteropServices;

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

class SecretHandler
{
  public static SecureString ConvertToSecureString(string value)
  {
    ArgumentException.ThrowIfNullOrEmpty(value);

    SecureString secure = new();

    foreach (char ch in value)
    {
      secure.AppendChar(ch);
    }

    secure.MakeReadOnly();

    return secure;
  }

  public static string ConvertToString(SecureString value)
  {
    IntPtr ptr = IntPtr.Zero;

    try
    {
      ptr = Marshal.SecureStringToBSTR(value);
      return Marshal.PtrToStringBSTR(ptr);
    }
    finally
    {
      if (ptr != IntPtr.Zero)
      {
        Marshal.ZeroFreeBSTR(ptr);
      }
    }
  }
}

// reference: https://github.com/Aedial/novelai-api/blob/main/novelai_api/utils.py
class NAIHasher
{
  private static byte[] HashBlake2(byte[] data)
  {
    byte[] hashedBytes = Blake2B.ComputeHash(
      data,
      new Blake2BConfig() { OutputSizeInBytes = 16 },
      default
    );

    return hashedBytes;
  }

  private static byte[] HashArgon2(byte[] salt, byte[] data)
  {
    Argon2id argon2 = new(data)
    {
      Salt = salt,
      MemorySize = 1953,
      Iterations = 2,
      DegreeOfParallelism = 1
    };

    byte[] hashedBytes = argon2.GetBytes(64);

    return hashedBytes;
  }

  private static string EncodeBase64(byte[] data)
  {
    // using url-safe base64 encoding
    // TODO: trimming '=' at the end may cause some problem
    string encodedString = Convert.ToBase64String(data)[..64]
      .TrimEnd('=')
      .Replace('+', '-')
      .Replace('/', '_');

    return encodedString;
  }

  public static string EncodeKey(string username, string password)
  {
    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

    string? preSalt = $"{password[..6]}{username}novelai_data_access_key";
    byte[] preSaltBytes = Encoding.UTF8.GetBytes(preSalt);
    preSalt = null;

    byte[] saltBytes = HashBlake2(preSaltBytes);
    byte[] keyBytes = HashArgon2(saltBytes, passwordBytes);
    string encodedKey = EncodeBase64(keyBytes);

    return encodedKey;
  }
}