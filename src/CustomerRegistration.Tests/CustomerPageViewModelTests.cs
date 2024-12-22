using System;
using System.Collections.ObjectModel;
using CustomerRegistration.Modules.Customer;
using CustomerRegistration.Modules.Utils;
using Xunit;

namespace CustomerRegistration.Tests;

public class CustomerPageViewModelTests
{
    private readonly CustomerPageViewModel _viewModel;

    public CustomerPageViewModelTests()
    {
        // Limpa o banco antes de cada teste
        DB<CustomerModel>.DeleteAll();

        // Inicializa a ViewModel
        _viewModel = new CustomerPageViewModel();
    }

    [Fact]
    public void FetchData_ShouldLoadCustomersFromDB()
    {
        // Arrange
        DB<CustomerModel>.Add(new CustomerModel { Name = "Arthur", Lastname = "Silva", Age = 30, Address = "Rua 1" });
        DB<CustomerModel>.Add(new CustomerModel { Name = "Claudia", Lastname = "Bezerra", Age = 25, Address = "Rua 2" });

        // Act
        _viewModel.Customers = new ObservableCollection<CustomerModel>(DB<CustomerModel>.Get());

        // Assert
        Assert.Equal(2, _viewModel.Customers.Count);
    }

    [Fact]
    public void CreateCustomer_ShouldAddCustomerToDB()
    {
        // Arrange
        var customer = new CustomerModel { Name = "Novo Cliente", Lastname = "Teste", Age = 22, Address = "Rua Teste" };
        _viewModel.CustomerModel = customer;

        // Act
        _viewModel.OnActionCommand.Execute(eActionType.Create);

        // Assert
        var dbCustomer = DB<CustomerModel>.Get().FirstOrDefault(c => c.Name == "Novo Cliente");
        Assert.NotNull(dbCustomer);
        Assert.Equal("Novo Cliente", dbCustomer.Name);
    }

    [Fact]
    public void UpdateCustomer_ShouldUpdateExistingCustomerInDB()
    {
        // Arrange
        var customer = new CustomerModel { Name = "Arthur", Lastname = "Silva", Age = 30, Address = "Rua 1" };
        DB<CustomerModel>.Add(customer);

        customer.Name = "Arthur Atualizado";
        _viewModel.CustomerModel = customer;

        // Act
        _viewModel.OnActionCommand.Execute(eActionType.Update);

        // Assert
        var updatedCustomer = DB<CustomerModel>.Get(customer.Id);
        Assert.Equal("Arthur Atualizado", updatedCustomer.Name);
    }

    [Fact]
    public void DeleteCustomer_ShouldRemoveCustomerFromDB()
    {
        // Arrange
        var customer = new CustomerModel { Name = "Claudio", Lastname = "Oliveira", Age = 28, Address = "Rua 3" };
        DB<CustomerModel>.Add(customer);
        _viewModel.CustomerModel = customer;

        // Act
        _viewModel.OnActionCommand.Execute(eActionType.Delete);

        // Assert
        var dbCustomer = DB<CustomerModel>.Get(customer.Id);
        Assert.Null(dbCustomer);
    }

    [Fact]
    public void OpenCustomerDetail_ShouldSetCustomerModel()
    {
        // Arrange
        var customer = new CustomerModel { Name = "Lucas", Lastname = "Pereira", Age = 35, Address = "Rua 4" };

        // Act
        _viewModel.OnActionCommand.Execute(eActionType.Update);
        _viewModel.SelectedItemCommand.Execute(customer);

        // Assert
        Assert.Equal(customer.Name, _viewModel.CustomerModel?.Name);
    }
}
