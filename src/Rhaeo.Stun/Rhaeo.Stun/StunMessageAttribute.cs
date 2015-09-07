using Rhaeo.WebRtc.Stun.Attributes;
using System;

namespace Rhaeo.WebRtc.Stun
{
  public abstract class StunMessageAttribute
  {
    #region Properties

    public abstract StunMessageAttributeType Type { get; }

    #endregion

    #region Methods

    public static StunMessageAttribute Parse(BitSequence bits)
    {
      var type = (StunMessageAttributeType)(BitConverter.ToUInt16(bits.PopLittleEndianBytes(2), 0));
      var length = BitConverter.ToUInt16(bits.PopLittleEndianBytes(2), 0);

      switch (type)
      {
        case StunMessageAttributeType.XorMappedAddress:
          {
            return ParseXorMappedAddress(bits);
          }
      }

      throw new InvalidOperationException("Invalid attribute.");
    }

    private static XorMappedAddressStunMessageAttribute ParseXorMappedAddress(BitSequence bits)
    {
      return new XorMappedAddressStunMessageAttribute("test", 51777);
    }

    #endregion
  }
}
