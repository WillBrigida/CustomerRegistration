using System;
using Microsoft.Maui.Controls.PlatformConfiguration;
#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#elif MACCATALYST
using UIKit;
using ObjCRuntime;
using CoreGraphics;
#endif
namespace CustomerRegistration.Modules.Utils;

public static class WindowExtensions
{
    public static Microsoft.Maui.Controls.Window CenteredWindow(this Microsoft.Maui.Controls.Window window)
    {
        window.Created += (s, e) =>
        {

            #if WINDOWS
                var nativeWindow = (Microsoft.UI.Xaml.Window)window.Handler.PlatformView;
                if (nativeWindow != null)
                {
                    var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
                    var appWindow = AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(windowHandle));

                    if (appWindow != null)
                    {
                        var displayArea = DisplayArea.GetFromWindowId(appWindow.Id, DisplayAreaFallback.Primary);

                        // Define metade do tamanho da tela
                        var windowWidth = displayArea.WorkArea.Width / 3;
                        var windowHeight = displayArea.WorkArea.Height / 1;

                        // Redimensiona a janela
                        appWindow.Resize(new SizeInt32(windowWidth, windowHeight));

                        // Calcula a posição para centralizar a janela
                        var centerX = (displayArea.WorkArea.Width - windowWidth) / 2;
                        var centerY = (displayArea.WorkArea.Height - windowHeight) / 2;

                        appWindow.Move(new PointInt32(centerX, centerY));
                    }
                }

#elif MACCATALYST
                window.Created += (s, e) =>
                {
                    var screen = UIScreen.MainScreen.Bounds;
                    var width = screen.Width / 2;
                    var height = screen.Height / 2;
                    var x = (screen.Width - width) / 2;
                    var y = (screen.Height - height) / 2;

                    // var nsWindow = Platform.GetCurrentWindow(newWindow).WindowScene.Windows[0];
                    // nsWindow.SetFrame(new CGRect(x, y, width, height), true);
                };
#endif

        

        };

        return window;

    }

}
