using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using VismaLibraryManagement.Services;
using VismaLibraryManagement.Utils;

namespace VismaLibraryManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            var collection = new ServiceCollection();
            collection.AddSingleton<ILibraryService, LibraryService>();
            collection.AddSingleton<ConsoleUI>();
            collection.AddSingleton<IFileIO, JSONFileIO>();
            var serviceProvider = collection.BuildServiceProvider();
            serviceProvider.GetService<ConsoleUI>().Init();
            if (serviceProvider is IDisposable)
            {
                ((IDisposable)serviceProvider).Dispose();
            }
        }
    }
}