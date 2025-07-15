using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;

public class SecretArray : IDisposable
{
  private byte[] _data;
  private GCHandle _handle;
  private bool _disposed;
  private int _size;
  private enum PassType
  {
    Zero,
    Random,
    Pattern,
  }
  private readonly RandomNumberGenerator Rand;

  private readonly byte[] Patterns =
  [
    0x0,  0xFF, 0x55, 0xAA,
    0x11, 0x22, 0x33, 0x44,
    0x55, 0x66, 0x77, 0x88,
    0x99, 0xBB, 0xCC, 0xDD,
    0xEE
  ];

  private void Fill(PassType passType)
  {
    switch (passType)
    {
      case PassType.Zero:
        return;
      case PassType.Random:
        return;
      case PassType.Pattern:
        return;
    }
  }

  private void Shred(int iter, bool zeroFill = false)
  {
    for (int i = 0; i < iter; ++i)
    {
      Fill(PassType.Random);
    }

    if (zeroFill)
    {
      Fill(PassType.Zero);
    }
  }

  public SecretArray(byte[] secret, bool pinMemory = false)
  {
    Rand = RandomNumberGenerator.Create();

    _size = secret.Length == 0 ? 0 : secret.Length;
    _data = new byte[_size];
    _disposed = false;

    Buffer.BlockCopy(secret, 0, _data, 0, _size);

    if (pinMemory)
    {
      _handle = GCHandle.Alloc(_data, GCHandleType.Pinned);
    }
  }

  public void Dispose()
  {
    throw new NotImplementedException();
  }

  public byte[] ConvertToBytes()
  {
    if (_disposed || _data == null)
    {
      throw new ObjectDisposedException("Cannot access to disposed object");
    }

    return _data;
  }
}
