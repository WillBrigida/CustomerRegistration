using CustomerRegistration.Modules.Customer;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
#elif MACCATALYST
using UIKit;
using ObjCRuntime;
#endif
namespace CustomerRegistration;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
#if WINDOWS
    Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping("FullScreenWithTitleBar", (handler, view) =>
	{
		var nativeWindow = handler.PlatformView;
		var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
		var appWindow = AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(windowHandle));

		// var titleBar = appWindow.TitleBar;

        //     // Define a cor da barra de título
        //     titleBar.BackgroundColor = Windows.UI.Color.FromArgb(255, 34, 139, 230); // Azul
        //     titleBar.ForegroundColor = Windows.UI.Color.FromArgb(255, 255, 255, 255); // Branco (Texto)

        //     // Define a cor dos botões de controle (Fechar, Minimizar, Maximizar)
        //     titleBar.ButtonBackgroundColor = Windows.UI.Color.FromArgb(255, 34, 139, 230); // Azul
        //     titleBar.ButtonForegroundColor = Windows.UI.Color.FromArgb(255, 255, 255, 255); // Branco (Ícones)

        //     // Cor quando passa o mouse
        //     titleBar.ButtonHoverBackgroundColor = Windows.UI.Color.FromArgb(255, 0, 122, 204); // Azul mais escuro
        //     titleBar.ButtonHoverForegroundColor = Windows.UI.Color.FromArgb(255, 255, 255, 255); // Branco

        //     // Cor quando o botão está pressionado
        //     titleBar.ButtonPressedBackgroundColor = Windows.UI.Color.FromArgb(255, 0, 102, 204); // Azul ainda mais escuro
        //     titleBar.ButtonPressedForegroundColor = Windows.UI.Color.FromArgb(255, 255, 255, 255); // Branco

        //     // Remove o sistema padrão de título (opcional)
        //     titleBar.IconShowOptions = IconShowOptions.HideIconAndSystemMenu;

		if (appWindow.Presenter is OverlappedPresenter presenter)
		{
			// Maximiza a janela com barra de título
			presenter.Maximize();
			presenter.IsResizable = false; // Permite redimensionar
		}
	});
	#elif MACCATALYST
    Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping("FullScreen", (handler, view) =>
    {
        var nativeWindow = handler.PlatformView as UIWindow;
        if (nativeWindow != null)
        {
            nativeWindow.WindowScene?.Titlebar?.PerformSelector(new Selector("toggleFullScreen:"));
        }
    });
#endif

		builder.Services.AddTransient<CustomerPageViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
