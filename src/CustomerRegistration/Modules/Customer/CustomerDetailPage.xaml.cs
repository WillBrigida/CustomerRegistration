namespace CustomerRegistration.Modules.Customer;

public partial class CustomerDetailPage : ContentPage
{
	public CustomerDetailPage()
	{
		InitializeComponent();
	}

    private async void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
		var label = (Label)sender;
		if(!(int.TryParse(e.NewTextValue, out int value)))
		{
			await Shell.Current.ToastAlert("Apenas números");
			label.Text = new string(label.Text.Where(char.IsDigit).ToArray());
		}
		

    }
}