using System.Runtime.InteropServices;
using System.Security;

class SecretHandler
{
  private SecretHandler() { }

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