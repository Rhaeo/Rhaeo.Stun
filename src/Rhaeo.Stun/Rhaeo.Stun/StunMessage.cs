using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhaeo.WebRtc.Stun
{
  public sealed class StunMessage
  {
    #region Fields

    public static readonly uint MagicCookie = 0x2112a442;

    #endregion

    #region Constructors

    public StunMessage(StunMessageType type, IEnumerable<StunMessageAttribute> attributes, StunMessageId id)
    {
      Type = type;
      Attributes = attributes;
      Id = id;
    }

    #endregion

    #region Properties

    public StunMessageType Type { get; }

    public IEnumerable<StunMessageAttribute> Attributes { get; }

    public StunMessageId Id { get; }

    #endregion

    #region Methods

    public byte[] ToLittleEndianByteArray()
    {
      var bits = new Bits(20);

      bits.AddOffBit();
      bits.AddOffBit();

      bits.AddBits(Type.Bits);

      var length = (ushort)(Attributes.Count() * 4 * 8);
      bits.AddUInt16LittleEndian(length);

      bits.AddUInt32LittleEndian(MagicCookie);

      bits.AddBytesLittleEndian(Id.Bytes);

      return bits.ToLittleEndianByteArray();
    }

    public static StunMessage Parse(byte[] bytes)
    {
      var bits = new Bits(bytes);

      if (bits.Pop() != false || bits.Pop() != false)
      {
        throw new Exception("First two bits must be zeroes.");
      }

      var type = StunMessageType.Parse(bits.PopBits(14));

      var length = BitConverter.ToUInt16(bits.PopLittleEndianBytes(2), 0);

      var magicCookie = BitConverter.ToUInt32(bits.PopLittleEndianBytes(4), 0);
      if (magicCookie != MagicCookie)
      {
        throw new ArgumentException($"The parse magic cookie {magicCookie} doesn't match {MagicCookie}.", nameof(MagicCookie));
      }

      var attributes = new List<StunMessageAttribute>();
      for (var index = 0; index < length; index++)
      {
        attributes.Add(StunMessageAttribute.Parse(bits.PopBits(12 * 8)));
      }

      var id = StunMessageId.Parse(bits.PopLittleEndianBytes(12));

      return new StunMessage(type, attributes, id);
    }

    public override string ToString() => $"{Type} (#{Id}) with {Attributes.Count()} attributes…";

    #endregion
  }
}
