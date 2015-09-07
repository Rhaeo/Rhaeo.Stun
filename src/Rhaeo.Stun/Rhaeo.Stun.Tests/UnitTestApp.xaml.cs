using Microsoft.VisualStudio.TestPlatform.TestExecutor;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Rhaeo.Stun.Tests
{
  sealed partial class App
    : Application
  {
    #region Methods

    protected override void OnLaunched(LaunchActivatedEventArgs e)
    {
      var rootFrame = Window.Current.Content as Frame;
      if (rootFrame == null)
      {
        rootFrame = new Frame();
        Window.Current.Content = rootFrame;
      }

      UnitTestClient.CreateDefaultUI();
      Window.Current.Activate();
      UnitTestClient.Run(e.Arguments);
    }

    #endregion
  }
}
