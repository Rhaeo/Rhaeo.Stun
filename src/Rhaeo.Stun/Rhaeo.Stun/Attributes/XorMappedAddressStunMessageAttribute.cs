using Windows.Networking;

namespace Rhaeo.WebRtc.Stun.Attributes
{
  public sealed class XorMappedAddressStunMessageAttribute
    : StunMessageAttribute
  {
    #region Constructors

    public XorMappedAddressStunMessageAttribute(HostName hostName, ushort port)
    {
      HostName = hostName;
      Port = port;
    }

    #endregion

    #region Properties

    public override StunMessageAttributeType Type => StunMessageAttributeType.XorMappedAddress;

    public HostName HostName { get; }

    public ushort Port { get; }

    #endregion
  }
}
