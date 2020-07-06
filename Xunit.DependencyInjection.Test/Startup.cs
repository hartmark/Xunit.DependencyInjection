﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection;
using Xunit.DependencyInjection.Demystifier;
using Xunit.DependencyInjection.Logging;

namespace Xunit.DependencyInjection.Test
{
    public class Startup
    {
        private readonly AssemblyName _assemblyName;

        public Startup(AssemblyName assemblyName) => _assemblyName = assemblyName;

        protected void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging().AddScoped<IDependency, DependencyClass>();

            services.AddSingleton<IAsyncExceptionFilter, DemystifyExceptionFilter>();
        }

        public void ConfigureServices(IHostBuilder hostBuilder) =>
            hostBuilder
                 .ConfigureServices(ConfigureServices)
                 .ConfigureHostConfiguration(builder => builder.AddInMemoryCollection(new Dictionary<string, string> { { HostDefaults.ApplicationKey, _assemblyName.Name! } }));

        public void Configure(ILoggerFactory loggerFactory, ITestOutputHelperAccessor accessor) =>
            loggerFactory.AddProvider(new XunitTestOutputLoggerProvider(accessor));
    }
}
