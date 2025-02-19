using TruckFlow.Interfaces;
using TruckFlow.Models;
using TruckFlow.Services;

IWarehouse warehouse = new Warehouse();
var logisticsManager = new LogisticsManager(warehouse);
await logisticsManager.ManageLogistics();

Console.WriteLine($"Final stock value: {warehouse.GetStock()}");
