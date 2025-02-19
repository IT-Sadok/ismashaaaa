using TruckFlow.Interfaces;

namespace TruckFlow.Models
{
    public class Warehouse : IWarehouse
    {
        private int _stock = 0;
        private readonly object _lock = new();

        public void AddItem()
        {
            lock (_lock)
            {
                _stock++;
            }
        }

        public void RemoveItem()
        {
            lock (_lock) 
            { 
                _stock--;
            }
        }

        public int GetStock()
        {
            lock (_lock)
            {
                return _stock;
            }
        }
    }
}
