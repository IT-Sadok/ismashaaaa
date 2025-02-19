using TruckFlow.Interfaces;

namespace TruckFlow.Models
{
    public class Truck : ITruck
    {
        private readonly IWarehouse _warehouse;
        private readonly bool _isDelivering;

        public Truck(IWarehouse warehouse, bool isDelivering)
        {
            _warehouse = warehouse;
            _isDelivering = isDelivering;
        }

        public void ProcessDelivery()
        {
            if (_isDelivering)
                _warehouse.AddItem();
            else
                _warehouse.RemoveItem();
        }
    }
}
