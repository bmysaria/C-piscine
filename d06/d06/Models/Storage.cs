using System;
using System.Threading;

namespace d06.Models
{
    public class Storage
    {
        public int ItemsInStorage { get; set; }
        public bool IsEmpty => ItemsInStorage <= 0;
        private const int Unlocked = 0;
        private const int Locked = 1;
        private int _locked = Unlocked;
        public Storage(int totalItemCount)
        {
            ItemsInStorage = totalItemCount;
        }

        public void TakeWares(Customer customer)
        {
            while (Interlocked.Exchange(ref _locked, Locked) != Unlocked)
                Thread.Sleep(1);
            ItemsInStorage -= ItemsInStorage >= customer.ItemsInCart
            ? customer.ItemsInCart
            : ItemsInStorage;
            Interlocked.Exchange(ref _locked, Unlocked);
        }
    }
}