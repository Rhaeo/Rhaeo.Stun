using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Rhaeo.WebRtc.Stun;
using Rhaeo.WebRtc.Stun.Attributes;
using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;

namespace Rhaeo.Stun.Tests
{
  [TestClass]
  public class RhaeoStunTests
  {
    #region Methods

    [TestMethod]
    public async void DoesAThing()
    {
      var hostName = new HostName("stun.l.google.com");
      var port = 19302;
      var taskCompletionSource = new TaskCompletionSource<StunUri>();
      using (var datagramSocket = new DatagramSocket())
      {
        datagramSocket.MessageReceived += async (sender, e) =>
        {
          var buffer = await e.GetDataStream().ReadAsync(null, 100, InputStreamOptions.None).AsTask();
          var stunMessage = StunMessage.Parse(buffer.ToArray());
          var xorMappedAddressStunMessageAttribute = stunMessage.Attributes.OfType<XorMappedAddressStunMessageAttribute>().Single();
          taskCompletionSource.SetResult(new StunUri(xorMappedAddressStunMessageAttribute.HostName, xorMappedAddressStunMessageAttribute.Port));
        };

        using (var inMemoryRandomAccessStream = new InMemoryRandomAccessStream())
        {
          var stunMessageId = new StunMessageId(CryptographicBuffer.GenerateRandom(12).ToArray());
          var stunMessageType = StunMessageType.BindingRequest;
          var stunMessageAttributes = new StunMessageAttribute[] { };
          var stunMessage = new StunMessage(stunMessageType, stunMessageAttributes, stunMessageId);
          var bytes = stunMessage.ToLittleEndianByteArray();
          var outputStream = await datagramSocket.GetOutputStreamAsync(hostName, $"{port}");
          var written = await outputStream.WriteAsync(bytes.AsBuffer());
        }
      }

      var result = await taskCompletionSource.Task;
      Assert.AreEqual(result.HostName, new HostName("200.100.50.25"));
      Assert.AreEqual(result.Port, 12345);
    }

    #endregion
  }
}
