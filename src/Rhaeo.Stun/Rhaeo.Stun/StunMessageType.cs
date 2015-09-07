using System;
using System.Linq;

namespace Rhaeo.WebRtc.Stun
{
  public sealed class StunMessageType
  {
    #region Fields

    public static readonly StunMessageType BindingRequest = new StunMessageType(StunMessageMethod.Binding, StunMessageClass.Request);

    #endregion

    #region Constructors

    public StunMessageType(StunMessageMethod method, StunMessageClass @class)
    {
      if (!method.PermittedClasses.Contains(@class))
      {
        throw new ArgumentException($"The class {@class} is not permitted by the method {method}.", nameof(@class));
      }
      
      Name = method.Name + " " + @class.Name;

      var methodBits = new Bits(method.Bits);
      var classBits = new Bits(@class.Bits);
      var bits = new Bits(14);
      bits.AddBits(methodBits.PopBits(5));
      bits.AddBit(classBits.Pop());
      bits.AddBits(methodBits.PopBits(3));
      bits.AddBit(classBits.Pop());
      bits.AddBits(methodBits);

      Bits = bits.ToBitArray();
    }

    #endregion

    #region Properties

    public string Name { get; }

    public bool[] Bits { get; }

    #endregion

    #region Methods

    public static StunMessageType Parse(Bits bits)
    {
      if (bits.Count != 14)
      {
        throw new ArgumentException("The number of bits must be 14.", nameof(bits));
      }

      var methodBits = new Bits(12);
      var classBits = new Bits(2);

      methodBits.AddBits(bits.PopBits(5));
      classBits.AddBit(bits.Pop());
      methodBits.AddBits(bits.PopBits(3));
      classBits.AddBit(bits.Pop());
      methodBits.AddBits(bits);

      return new StunMessageType(StunMessageMethod.Parse(methodBits), StunMessageClass.Parse(classBits));
    }

    public override string ToString() => Name;

    #endregion
  }
}
