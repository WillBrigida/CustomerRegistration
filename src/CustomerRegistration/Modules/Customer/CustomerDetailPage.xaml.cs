namespace CustomerRegistration.Modules.Customer;

public partial class CustomerDetailPage : ContentPage
{
	public CustomerDetailPage()
	{
		InitializeComponent();
	}

    private async void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
		if(!int.TryParse(e.NewTextValue, out int value))
		{
			if(!string.IsNullOrEmpty((sender as Entry)!.Text))
				await Shell.Current.ToastAlert("Valor inválido. Informe uma idade válida", isCurrentPage: false);

			(sender as Entry)!.Text = string.Empty;

		}
		
    }
}