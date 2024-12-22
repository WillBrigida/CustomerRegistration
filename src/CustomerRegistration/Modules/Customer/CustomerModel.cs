using System;

namespace CustomerRegistration.Modules.Customer;

public class CustomerModel : Customer
{
    public string? Name { get; set; }
    public string? Lastname { get; set; }
    public int Age { get; set; }
    public string? Address { get; set; }

}

public class Customer
{
    public int Id { get; set; }

}

public enum eActionType { Create, Update, Delete }
