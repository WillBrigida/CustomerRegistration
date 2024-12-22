using System.Diagnostics;

namespace CustomerRegistration.Modules.Utils;

public class DB<T> where T : Customer.CustomerModel
{
    private static IList<T> CustomersDB { get; set; } = new List<T>();
    private static int _idCounter = 1;

    static DB()
    {
        CustomersDB = new List<T>(){};
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
