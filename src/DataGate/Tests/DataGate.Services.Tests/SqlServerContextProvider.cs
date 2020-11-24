﻿namespace DataGate.Services.Tests
{
    using Xunit;

    using Microsoft.Extensions.Configuration;

    using DataGate.Data;
    using DataGate.Services.Tests.ClassFixtures;
    using DataGate.Services.Tests.Factories;

    public class SqlServerContextProvider : IClassFixture<MappingsProvider>
    {
        protected readonly ApplicationDbContext context;
        protected readonly IConfiguration configuration;

        public SqlServerContextProvider()
        {
            //this.configuration = new IConfiguration;
            this.context = ConnectionFactory.CreateContextForSqlServer(this.configuration);
        }
    }
}