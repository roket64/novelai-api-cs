using System.Text;
using System.Runtime.InteropServices;
using Konscious.Security.Cryptography;
using Isopoh.Cryptography.Blake2b;
using System.Security.Cryptography;

// reference: https://github.com/Aedial/novelai-api/blob/main/novelai_api/utils.py
class NAIHasher
{
  private static byte[] HashBlake2b(byte[] data)
  {
    byte[] result = Blake2B.ComputeHash(
      data,
      new Blake2BConfig() { OutputSizeInBytes = 16 },
      default
    );

    return result;
  }

  private static byte[] HashArgon2(byte[] data, byte[] salt)
  {
    Argon2id argon2 = new(data)
    {
      Salt = salt,
      MemorySize = 1953,
      Iterations = 2,
      DegreeOfParallelism = 1
    };

    byte[] result = argon2.GetBytes(64);

    return result;
  }

  private static byte[] BuildPreSaltBytes(char[] username, char[] password)
  {
    char[] suffix = "novelai_data_access_key".ToCharArray();
    char[] full = [.. password[..6], .. username, .. suffix];

    try
    {
      byte[] result = Encoding.UTF8.GetBytes(full);
      return result;
    }
    finally
    {
      CryptographicOperations.ZeroMemory(MemoryMarshal.AsBytes(full.AsSpan()));
    }
  }

  private static string EncodeBase64Url(byte[] bytes)
  {
    var result = Convert.ToBase64String(bytes)[..64]
      .TrimEnd('=')
      .Replace('+', '-')
      .Replace('/', '_');

    return result;
  }

  public static string EncodeKey(char[] username, char[] password)
  {
    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
    // prevent passwordBytes from being moved by GC
    GCHandle handle = GCHandle.Alloc(passwordBytes, GCHandleType.Pinned);

    byte[] preSaltBytes = BuildPreSaltBytes(username, password);
    byte[]? saltBytes = null;
    byte[]? keyBytes = null;

    try
    {
      saltBytes = HashBlake2b(preSaltBytes);
      keyBytes = HashArgon2(passwordBytes, saltBytes);

      var result = EncodeBase64Url(keyBytes);
      return result;
    }
    finally
    {
      CryptographicOperations.ZeroMemory(passwordBytes);
      CryptographicOperations.ZeroMemory(preSaltBytes);
      handle.Free();

      if (saltBytes != null)
      {
        CryptographicOperations.ZeroMemory(saltBytes);
      }
      if (keyBytes != null)
      {
        CryptographicOperations.ZeroMemory(keyBytes);
      }
    }
  }
}