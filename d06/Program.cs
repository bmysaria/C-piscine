using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using d06.Extensions;
using d06.Models;
using d06.Models.d06.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace d06
{
    class Program
    {
        private const string JsonConfig = "/Users/bmysaria/Desktop/c_sharp_piscine/d06/appsettings.json";
        static void Main(string[] args)
        {
            const int registers = 4;
            const int storageCapacity = 50;
            const int cartCapacity = 7;
            int customers = 10;
            double timePerItem;
            double timePerCustomer;
            try
            {
                var configuration = new ConfigurationBuilder().AddJsonFile(JsonConfig).Build();
                timePerItem = double.Parse(configuration["timePerItem"]);
                timePerCustomer = double.Parse(configuration["timePerCustomer"]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            var Customers = Enumerable.Range(1, customers).Select(x => new Customer(x)).ToArray();
            Store store = new Store(registers, storageCapacity, TimeSpan.FromSeconds(timePerItem), TimeSpan.FromSeconds(timePerCustomer));
            Parallel.ForEach(Customers, customer =>
            {
               customer.FillCart(cartCapacity);
               Register register = customer.GetInLineByPeople(store.Registers);
               Console.WriteLine($"{customer} in for {register}");
            });
            store.OpenRegisters();
            while (store.IsOpen)
            {
				customers++;
                Thread.Sleep(TimeSpan.FromSeconds(5));
                var customer = new Customer(customers);
                customer.FillCart(cartCapacity);
                Register register = customer.GetInLineByItems(store.Registers);
                Console.WriteLine($"{customer} in for {register}");
            }

            foreach (Register register in store.Registers)
                register.Thread.Join();
        }
    }
}
