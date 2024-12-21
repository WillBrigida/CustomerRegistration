namespace CustomerRegistration;

public partial class App : Application
{
	public App()
	{
		Current!.UserAppTheme = AppTheme.Dark; //Forcing dark theme
		InitializeComponent();

		MainPage = new AppShell();
	}
}
