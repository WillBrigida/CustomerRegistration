using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif
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

    private object? _selectedItem;
    public object? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (_selectedItem != value && value is CustomerModel customer)
            {
                _selectedItem = value;
                ActionType = eActionType.Update;
                OpenCustomerDetail(customer);
            }
        }
    }



    public ObservableCollection<CustomerModel>? Customers { get; set; }
    #endregion

    public CustomerPageViewModel()
    {
        FetchData();
    }

    private void FetchData()
    {
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

        newWindow.Created += (s, e) =>
        {

#if WINDOWS
            var nativeWindow = newWindow.Handler?.PlatformView;
            if (nativeWindow != null)
            {
                var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
                var appWindow = AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(windowHandle));

                if (appWindow != null)
                {
                    var displayArea = DisplayArea.GetFromWindowId(appWindow.Id, DisplayAreaFallback.Primary);

                    // Define metade do tamanho da tela
                    var windowWidth = displayArea.WorkArea.Width / 3;
                    var windowHeight = displayArea.WorkArea.Height / 2;

                    // Redimensiona a janela
                    appWindow.Resize(new SizeInt32(windowWidth, windowHeight));

                    // Calcula a posição para centralizar a janela
                    var centerX = (displayArea.WorkArea.Width - windowWidth) / 2;
                    var centerY = (displayArea.WorkArea.Height - windowHeight) / 2;

                    appWindow.Move(new PointInt32(centerX, centerY));
                }
            }

#elif MACCATALYST
            
            var screen = UIScreen.MainScreen.Bounds;
            var width = screen.Width / 2;
            var height = screen.Height / 2;
            var x = (screen.Width - width) / 2;
            var y = (screen.Height - height) / 2;

            newWindow.Frame = new CoreGraphics.CGRect(x, y, width, height);
#endif
        };
        App.Current?.OpenWindow(newWindow);

        newWindow.Destroying += (s, e) =>
        {
            SelectedItem = null;
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

public class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null!)
    {
        if (EqualityComparer<T>.Default.Equals(backingField, value))
            return false;

        backingField = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}


public class DB<T> where T : CustomerModel
{
    private static IList<T> CustomersDB { get; set; } = new List<T>();
    private static int _idCounter = 1;

    static DB()
    {
        CustomersDB = new List<T>();
    }

    public static IList<T> Get()
    {
        return new List<T>(CustomersDB);
    }

    public static T? Get(int id)
    {
        return CustomersDB.FirstOrDefault(x => x.Id == id);
    }

    public static bool Add(T customer)
    {
        try
        {
            if (customer == null)
            {
                Debug.WriteLine("Tentativa de adicionar um cliente nulo.");
                return false;
            }

            customer.Id = _idCounter++;
            CustomersDB.Add(customer);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erro ao adicionar cliente: {ex.Message}");
            return false;
        }
    }

    public static bool Update(T customer)
    {
        try
        {
            var existingCustomer = CustomersDB.FirstOrDefault(x => x.Id == customer.Id);
            if (existingCustomer == null)
            {
                Debug.WriteLine($"Cliente com ID {customer.Id} não encontrado para atualização.");
                return false;
            }

            existingCustomer.Name = customer.Name;
            existingCustomer.Lastname = customer.Lastname;
            existingCustomer.Age = customer.Age;
            existingCustomer.Address = customer.Address;

            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erro ao atualizar cliente: {ex.Message}");
            return false;
        }
    }

    public static bool Delete(int id)
    {
        try
        {
            var customer = CustomersDB.FirstOrDefault(x => x.Id == id);
            if (customer == null)
            {
                Debug.WriteLine($"Cliente com ID {id} não encontrado para exclusão.");
                return false;
            }

            CustomersDB.Remove(customer);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erro ao excluir cliente: {ex.Message}");
            return false;
        }
    }

    public static bool DeleteAll()
    {
        try
        {
            CustomersDB.Clear();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erro ao excluir todos os clientes: {ex.Message}");
            return false;
        }
    }
}


