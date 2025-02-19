using System;

namespace TruckFlow.Interfaces
{
    public interface IWarehouse
    {
        void AddItem();

        void RemoveItem();

        int GetStock();
    }
}
