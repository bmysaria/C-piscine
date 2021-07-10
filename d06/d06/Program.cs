﻿using System;
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
        private const string JsonConfig = "/Users/bmysaria/Desktop/d06/d06/appsettings.json";
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
            int i = 1;
            while (store.IsOpen)
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
                var customer = new Customer(10+i);
                customer.FillCart(cartCapacity);
                Register register = customer.GetInLineByItems(store.Registers);
                Console.WriteLine($"{customer} in for {register}");
                i++;
            }

            foreach (Register register in store.Registers)
                register.Thread.Join();
        }
    }
}