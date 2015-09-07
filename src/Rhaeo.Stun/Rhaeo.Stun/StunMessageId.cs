using System;

namespace Rhaeo.WebRtc.Stun
{
  public sealed class StunMessageId
  {
    #region Constructors

    public StunMessageId(byte[] bytes)
    {
      if (bytes.Length != 12)
      {
        throw new ArgumentException("The number of bites must be 12.", nameof(bytes));
      }

      Bytes = bytes;
    }

    #endregion

    #region Properties

    public byte[] Bytes { get; }

    #endregion

    #region Methods

    public static StunMessageId Parse(byte[] bytes)
    {
      return new StunMessageId(bytes);
    }

    public override string ToString() => BitConverter.ToString(Bytes);

    #endregion
  }
}
