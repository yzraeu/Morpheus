using System.Net.Http;
using System.Threading.Tasks;
using Morpheus.Repository.MySQL;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UnitTest.Configuration
{
    [Collection("Base collection")]
    public abstract class BaseIntegrationTest 
    {
        protected readonly TestServer Server;
        protected readonly HttpClient Client;
        protected readonly RepositoryContext TestDataContext;
        protected readonly BaseTestFixture Fixture;

        protected BaseIntegrationTest(BaseTestFixture fixture) {
            this.Fixture = fixture;
            this.TestDataContext = fixture.TestDataContext;
            this.Server = fixture.Server;
            this.Client = fixture.Client;

            //ClearDb().Wait();
        }

        private async Task ClearDb(){
            var commands = new [] {
                "EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'",
                "EXEC sp_MSForEachTable 'DELETE FROM ?'",
                "EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'"
            };

            await TestDataContext.Database.OpenConnectionAsync();

            foreach (var command in commands) {
                await TestDataContext.Database.ExecuteSqlCommandAsync(command);
            }

            TestDataContext.Database.CloseConnection();            
        }

    }
}