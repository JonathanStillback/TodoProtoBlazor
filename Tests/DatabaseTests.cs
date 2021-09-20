using Microsoft.VisualStudio.TestTools.UnitTesting;
using Implementations;
using Models;
using Configurations;
using System.Reflection;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Tests
{
    [TestClass]
    public class DatabaseTests
    {
		private readonly IServiceProvider _services;
		private readonly IDBProvider<Todo> _todoDBProvider;

		public DatabaseTests()
        {
            _services = Configurations.HostBuilderConfiguration.BuildHost(new string[]{}, this.GetType().GetTypeInfo().Assembly.FullName).Services;
            _todoDBProvider = _services.GetRequiredService<IDBProvider<Todo>>();
        }

        [TestMethod]
        public void DatabaseConnection()
        {
            var todos = _todoDBProvider.GetAll();
        }
    }
}
