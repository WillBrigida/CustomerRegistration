
namespace CustomerRegistration.Modules.Customer;

public partial class CustomerPage : ContentPage
{
    public CustomerPage()
    {
        InitializeComponent();
    }
    private void OnPointerEntered(object sender, EventArgs e)
    {
        addButton.Stroke = Colors.White;

    }

    private void OnPointerExited(object sender, EventArgs e)
    {
        addButton.Stroke = Colors.Transparent;

    }


}
