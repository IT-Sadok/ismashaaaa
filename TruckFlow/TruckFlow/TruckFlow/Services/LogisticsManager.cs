using TruckFlow.Interfaces;
using TruckFlow.Models;

namespace TruckFlow.Services
{
    public class LogisticsManager
    {
        private readonly IWarehouse _warehouse;

        public LogisticsManager(IWarehouse warehouse)
        {
            _warehouse = warehouse;
        }

        public async Task ManageLogistics()
        {
            var tasks = new List<Task>();

            for (int i = 0; i < 100; i++)
            {
                tasks.Add(Task.Run(() => new Truck(_warehouse, true).ProcessDelivery()));
                tasks.Add(Task.Run(() => new Truck(_warehouse, false).ProcessDelivery()));
            }

            await Task.WhenAll(tasks);
        }
    }
}