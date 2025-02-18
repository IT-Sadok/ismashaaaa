using BookingSystem.Data;
using BookingSystem.Interfaces;
using BookingSystem.Menu;
using BookingSystem.Services;
using Microsoft.Extensions.DependencyInjection;


var serviceProvider = new ServiceCollection()
            .AddSingleton<IDataService, JsonDataService>()
            .AddSingleton<IBookingService, BookingService>()
            .BuildServiceProvider();

var bookingService = serviceProvider.GetRequiredService<IBookingService>();
BookingMenu.DisplayMenu(bookingService);