using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace CookieAuthWithIdentity.Core
{
    public class Program
    {
        public static string KeyRingPath;

        public static void Main(string[] args)
        {
            Console.WriteLine("Enter location of Key Ring:");
            KeyRingPath = Console.ReadLine();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
