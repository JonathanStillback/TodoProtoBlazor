using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Configurations
{
	public class HostBuilderConfiguration
	{
		public static IHost BuildHost(string[] args, string assemblyName)
		{
			return Host.CreateDefaultBuilder(args)
			.ConfigureAppConfiguration((hostingContext, config) =>
			{
				var defaultContentPath = Path.Combine(hostingContext.HostingEnvironment.ContentRootPath, "appsettings.json");
				var newContentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
				config
				// Todo: Check this
				// Works for development (and production?)
				.AddJsonFile(newContentPath, 
						optional: true, reloadOnChange: true
				)
				// // For production
				// .AddJsonFile("appsettings.json", 
				// 		optional: true, reloadOnChange: true
				// );

				;
			})
			.ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder
				.UseStartup<Startup>()
				.UseSetting(WebHostDefaults.ApplicationKey, assemblyName);
			}).Build();
		}
		// public static IWebHostBuilder CreateWebHostBuilder()
		// {

		// }
	}
}
