using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhaeo.WebRtc.Stun
{
  public sealed class StunMessageMethod
  {
    #region Fields

    public static readonly StunMessageMethod Binding = new StunMessageMethod("Binding", new[] { StunMessageClass.Request, StunMessageClass.SuccessResponse, StunMessageClass.FailureResponse, StunMessageClass.Indication }, false, false, false, false, false, false, false, false, false, false, false, true);

    #endregion

    #region Constructors

    private StunMessageMethod(string name, IEnumerable<StunMessageClass> permittedClasses, params bool[] bits)
    {
      Name = name;
      PermittedClasses = permittedClasses;
      Bits = bits;
    }

    #endregion

    #region Properties

    public string Name { get; }

    public IEnumerable<StunMessageClass> PermittedClasses { get; }

    public bool[] Bits { get; }

    #endregion

    #region Methods

    public static StunMessageMethod Parse(Bits bits)
    {
      if (bits.Count != 12)
      {
        throw new ArgumentException("The number of bits must be 12.", nameof(bits));
      }

      if (Binding.Bits.SequenceEqual(bits.ToBitArray()))
      {
        return Binding;
      }

      throw new InvalidOperationException("Invalid method.");
    }

    public override string ToString() => Name;

    #endregion
  }
}
