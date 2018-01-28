using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Morpheus.API;
using Morpheus.Repository.MySQL;
using MySQL.Data.EntityFrameworkCore.Extensions;

namespace UnitTest.Configuration
{
    public class BaseTestFixture : IDisposable
    {
        public readonly HttpClient Client;
        public readonly RepositoryContext TestDataContext;
        private readonly IConfigurationRoot Configuration;

        public BaseTestFixture()
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }

        void IDisposable.Dispose()
        {
            TestDataContext.Dispose();
            Client.Dispose();
        }
    }
}