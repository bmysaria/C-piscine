using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using d06.Extensions;
using d06.Models;

namespace d06.Models
{
    namespace d06.Models
    {
        public class Store
        {
            public List<Register> Registers { get; }
            public Storage Storage { get; }

            public bool IsOpen => !Storage.IsEmpty;

            public Store(int registerCount,
                int storageCapacity, TimeSpan timePerItem, TimeSpan timePerCustomer)
            {
                Storage = new Storage(storageCapacity);
                Registers = Enumerable.Range(1, registerCount)
                    .Select(i => new Register(i, timePerCustomer, timePerItem, this))
                    .ToList();
            }

            public void OpenRegisters()
            {
                foreach (var register in Registers)
                    register.Thread.Start();
            }
        }
    }
}