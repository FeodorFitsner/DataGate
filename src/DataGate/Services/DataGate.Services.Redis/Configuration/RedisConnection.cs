﻿namespace DataGate.Services.Redis.Configuration
{
    using System;

    using DataGate.Workers.BatchPrograms;
    using DataGate.Workers.BatchPrograms.Contracts;
    using StackExchange.Redis;

    public class RedisConnection
    {
        private static Lazy<ConnectionMultiplexer> connection;
        private readonly IExecutor executor;

        public RedisConnection(string configuration, string dirPath)
        {
            this.executor = new RedisServer();

            connection = new Lazy<ConnectionMultiplexer>(() =>
            {
                var connMultiplexer = ConnectionMultiplexer.Connect(configuration);

                if (!connMultiplexer.IsConnected)
                {
                    this.executor.Execute(dirPath);
                }

                return connMultiplexer;
            });
        }

        public ConnectionMultiplexer Connection()
        => connection.Value;
    }
}