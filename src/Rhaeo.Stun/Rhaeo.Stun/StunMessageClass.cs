using System;
using System.Linq;

namespace Rhaeo.WebRtc.Stun
{
  public sealed class StunMessageClass
  {
    #region Fields

    public static readonly StunMessageClass Request = new StunMessageClass("Request", false, false);

    public static readonly StunMessageClass SuccessResponse = new StunMessageClass("Success Response", true, false);

    public static readonly StunMessageClass FailureResponse = new StunMessageClass("Failure Response", true, true);

    public static readonly StunMessageClass Indication = new StunMessageClass("Indication", false, true);

    #endregion

    #region Constructors

    private StunMessageClass(string name, params bool[] bits)
    {
      if (name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }

      if (bits.Length != 2)
      {
        throw new ArgumentException("The number of bits must be 2.", nameof(bits));
      }

      Bits = bits;
      Name = name;
    }

    #endregion

    #region Properties

    public string Name { get; }

    public bool[] Bits { get; }

    #endregion

    #region Methods

    public static StunMessageClass Parse(Bits bits)
    {
      if (bits.Count != 2)
      {
        throw new ArgumentException("The number of bits must be 2.", nameof(bits));
      }

      if (Request.Bits.SequenceEqual(bits.ToBitArray()))
      {
        return Request;
      }

      if (SuccessResponse.Bits.SequenceEqual(bits.ToBitArray()))
      {
        return SuccessResponse;
      }

      if (FailureResponse.Bits.SequenceEqual(bits.ToBitArray()))
      {
        return FailureResponse;
      }

      if (Indication.Bits.SequenceEqual(bits.ToBitArray()))
      {
        return Indication;
      }

      throw new InvalidOperationException("Invalid class.");
    }

    public override string ToString() => Name;

    #endregion
  }
}
