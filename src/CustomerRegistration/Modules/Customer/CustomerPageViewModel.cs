using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using CustomerRegistration.Modules.Utils;


namespace CustomerRegistration.Modules.Customer;

public class CustomerPageViewModel : BaseViewModel
{
    #region _ PROPERTIES . . .
    private eActionType _actionType;
    public eActionType ActionType
    {
        get => _actionType;
        set => SetProperty(ref _actionType, value);
    }

    private CustomerModel? _customerModel;
    public CustomerModel? CustomerModel
    {
        get => _customerModel;
        set => SetProperty(ref _customerModel, value);
    }

    public ObservableCollection<CustomerModel>? Customers { get; set; }
    #endregion

    public CustomerPageViewModel()
    {
        FetchData();
    }

    private void FetchData()
    {
        var customer = DB<CustomerModel>.Get();

        if(!customer.Any())
        {
            DB<CustomerModel>.Add(new CustomerModel { Name = "Arthur", Lastname = "Silva", Age = 30, Address = "Rua 1" });
            DB<CustomerModel>.Add(new CustomerModel { Name = "Claudia", Lastname = "Bezerra", Age = 25, Address = "Rua 2" });
        }

        Customers = [.. DB<CustomerModel>.Get()];
    }

    public ICommand OnActionCommand => new Command<eActionType>((eActionType actions) =>
    {
        ActionType = actions;
        HandleActions();
    });

    private void HandleActions()
    {
        switch (ActionType)
        {
            case eActionType.Update: UpdateCustomer(); break;
            case eActionType.Delete: DeleteCustomer(); break;
            default: CreateCustomer(); break;
        }
    }

    public ICommand SelectedItemCommand => new Command((obj) =>
    {
        if (obj is CustomerModel customer)
        {
            ActionType = eActionType.Update;
            OpenCustomerDetail(customer);
        }
    });

    private void DeleteCustomer()
    {
        if (DB<CustomerModel>.Delete(CustomerModel.Id))
            _ = Shell.Current.ToastAlert("Delete");

        CloseLastWindow();
    }

    private void UpdateCustomer()
    {
        if (DB<CustomerModel>.Update(CustomerModel))
            _ = Shell.Current.ToastAlert("Update");

        CloseLastWindow();
    }

    private void CreateCustomer()
    {
        if (CustomerModel is null)
            OpenCustomerDetail(new());
        else
        {
            if (DB<CustomerModel>.Add(CustomerModel))
                _ = Shell.Current.ToastAlert("Create");

            CloseLastWindow();
        }
    }

    public void OpenCustomerDetail(CustomerModel customer)
    {
        CloseLastWindow();

        CustomerModel = customer;

        var page = new CustomerDetailPage { BindingContext = this };
        var newWindow = new Window(page);

        newWindow.CenteredWindow();

        App.Current?.OpenWindow(newWindow);

        newWindow.Destroying += (s, e) =>
        {
            CustomerModel = null;
            Customers?.Clear();

            foreach (var item in DB<CustomerModel>.Get())
                Customers?.Add(item);
        };
    }

    private void CloseLastWindow()
    {
        var lastWindow = Application.Current?.Windows.LastOrDefault();
        if (lastWindow != null && Application.Current!.Windows.Count > 1)
            Application.Current.CloseWindow(lastWindow);
    }
}







