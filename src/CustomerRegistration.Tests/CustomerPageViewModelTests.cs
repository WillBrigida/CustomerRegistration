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
        // Inicializa o ViewModel antes de cada teste
        _viewModel = new CustomerPageViewModel();
    }

    [Fact]
    public void FetchData_Should_Populate_Customers()
    {
        // Act
        _viewModel.Customers = new ObservableCollection<CustomerModel>(DB<CustomerModel>.Get());

        // Assert
        Assert.NotNull(_viewModel.Customers);
        Assert.True(_viewModel.Customers.Any());
    }

    [Fact]
    public void CreateCustomer_Should_Add_New_Customer_When_Valid()
    {
        // Arrange
        _viewModel.CustomerModel = new CustomerModel
        {
            Name = "Test",
            Lastname = "Customer",
            Age = 25,
            Address = "Rua Teste"
        };

        // Act
        _viewModel.OnActionCommand.Execute(eActionType.Create);

        // Assert
        var customer = DB<CustomerModel>.Get().FirstOrDefault(c => c.Name == "Test");
        Assert.NotNull(customer);
    }

    [Fact]
    public void CreateCustomer_Should_Not_Add_Invalid_Customer()
    {
        // Arrange
        _viewModel.CustomerModel = new CustomerModel
        {
            Name = "",
            Lastname = "",
            Age = 0,
            Address = ""
        };

        // Act
        _viewModel.OnActionCommand.Execute(eActionType.Create);

        // Assert
        var customer = DB<CustomerModel>.Get().FirstOrDefault(c => c.Name == "");
        Assert.Null(customer);
    }

    [Fact]
    public void UpdateCustomer_Should_Update_Existing_Customer()
    {
        // Arrange
        var customer = new CustomerModel
        {
            Id = 1,
            Name = "Old Name",
            Lastname = "Old Lastname",
            Age = 30,
            Address = "Old Address"
        };
        DB<CustomerModel>.Add(customer);

        _viewModel.CustomerModel = new CustomerModel
        {
            Id = 1,
            Name = "New Name",
            Lastname = "New Lastname",
            Age = 35,
            Address = "New Address"
        };

        // Act
        _viewModel.OnActionCommand.Execute(eActionType.Update);

        // Assert
        var updatedCustomer = DB<CustomerModel>.Get(1);
        Assert.Equal("New Name", updatedCustomer.Name);
        Assert.Equal("New Lastname", updatedCustomer.Lastname);
        Assert.Equal(35, updatedCustomer.Age);
        Assert.Equal("New Address", updatedCustomer.Address);
    }

    [Fact]
    public void DeleteCustomer_Should_Remove_Customer()
    {
        // Arrange
        var customer = new CustomerModel
        {
            Name = "ToDelete",
            Lastname = "User",
            Age = 28,
            Address = "Rua Delete"
        };
        DB<CustomerModel>.Add(customer);

        _viewModel.CustomerModel = customer;

        // Act
        _viewModel.OnActionCommand.Execute(eActionType.Delete);

        // Assert
        var deletedCustomer = DB<CustomerModel>.Get().FirstOrDefault(c => c.Name == "ToDelete");
        Assert.Null(deletedCustomer);
    }

    // [Fact]
    // public void SelectedItemCommand_Should_Open_Update_Window()
    // {
    //     // Arrange
    //     var customer = new CustomerModel
    //     {
    //         Id = 3,
    //         Name = "Selected",
    //         Lastname = "Item",
    //         Age = 22,
    //         Address = "Rua Selecionada"
    //     };
    //     DB<CustomerModel>.Add(customer);

    //     // Act
    //     _viewModel.SelectedItemCommand.Execute(customer);

    //     // Assert
    //     Assert.Equal(eActionType.Update, _viewModel.ActionType);
    //     Assert.Equal(customer.Id, _viewModel.CustomerModel?.Id);
    // }

    [Fact]
    public void Validate_Should_Return_False_When_Fields_Are_Invalid()
    {
        // Arrange
        _viewModel.CustomerModel = new CustomerModel
        {
            Name = "",
            Lastname = "",
            Age = 0,
            Address = ""
        };

        // Act
        var isValid = _viewModel.CustomerModel != null &&
                      !string.IsNullOrEmpty(_viewModel.CustomerModel.Name) &&
                      !string.IsNullOrEmpty(_viewModel.CustomerModel.Lastname) &&
                      _viewModel.CustomerModel.Age > 0 &&
                      !string.IsNullOrEmpty(_viewModel.CustomerModel.Address);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void Validate_Should_Return_True_When_Fields_Are_Valid()
    {
        // Arrange
        _viewModel.CustomerModel = new CustomerModel
        {
            Name = "Valid",
            Lastname = "User",
            Age = 25,
            Address = "Rua Válida"
        };

        // Act
        var isValid = _viewModel.CustomerModel != null &&
                      !string.IsNullOrEmpty(_viewModel.CustomerModel.Name) &&
                      !string.IsNullOrEmpty(_viewModel.CustomerModel.Lastname) &&
                      _viewModel.CustomerModel.Age > 0 &&
                      !string.IsNullOrEmpty(_viewModel.CustomerModel.Address);

        // Assert
        Assert.True(isValid);
    }
}


