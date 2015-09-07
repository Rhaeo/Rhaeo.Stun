using Windows.Networking;

namespace Rhaeo.Stun
{
  // TODO: Implement based on the RFC.
  public sealed class StunUri
  {
    #region Constructors

    public StunUri(HostName hostName, ushort port)
    {
      HostName = hostName;
      Port = port;
    }

    #endregion

    #region Properties

    public HostName HostName { get; }

    public ushort Port { get; }

    #endregion
  }
}
