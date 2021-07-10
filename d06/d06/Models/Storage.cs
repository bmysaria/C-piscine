using System;
using System.Threading;

namespace d06.Models
{
    public class Storage
    {
        public int ItemsInStorage { get; set; }
        public bool IsEmpty => ItemsInStorage <= 0;

        public Storage(int totalItemCount)
        {
            ItemsInStorage = totalItemCount;
        }
    }
}