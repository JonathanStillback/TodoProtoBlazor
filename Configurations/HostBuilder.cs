using System;
using System.Collections.Generic;
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
		public static IHostBuilder CreateHostBuilder(string[] args) =>
		Host.CreateDefaultBuilder(args)
		.ConfigureWebHostDefaults(webBuilder =>
		{
				webBuilder.UseStartup<Startup>();
		});
	}
}
