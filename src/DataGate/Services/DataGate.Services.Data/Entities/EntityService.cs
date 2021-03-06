﻿// Copyright (c) DataGate Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DataGate.Services.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Hosting;

    using DataGate.Common;
    using DataGate.Common.Settings;
    using DataGate.Data.Common.Repositories.UsersContext;
    using DataGate.Data.Models.Columns;
    using DataGate.Data.Models.Users;
    using DataGate.Services.Redis;
    using DataGate.Services.Redis.Configuration;
    using DataGate.Services.SqlClient;
    using DataGate.Services.SqlClient.Contracts;
    using DataGate.Web.ViewModels.Queries;
    using DataGate.Web.Infrastructure.Extensions;
    using DataGate.Web.ViewModels.Entities;

    public class EntityService : IEntityService
    {
        private readonly ISqlQueryManager sqlManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IHostEnvironment env;

        public EntityService(
            ISqlQueryManager sqlQueryManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IHostEnvironment env)
        {
            this.sqlManager = sqlQueryManager;
            this.userManager = userManager;
            this.configuration = configuration;
            this.env = env;
        }

        // ________________________________________________________
        //
        // Retrieve query table DB based entities
        // with table functions
        public async IAsyncEnumerable<string[]> All(string function, int? id, DateTime? date, int skip)
        {
            var query = this.sqlManager
                .ExecuteQueryAsync(function, date, id)
                .Skip(skip);

            await foreach (var item in query)
            {
                yield return item;
            }

            // ________________________________________________________
            //
            // Working Redis code on localhost and dedicated server, not working in shared server
            //var data = SetupRedis(this.configuration, this.env.ContentRootPath, function);
            //await data.Expire(GlobalConstants.RedisCacheExpirationTimeInSeconds);

            //if (data.Values().Result.Count == 0 || data.Keys().Result.Count == 0)
            //{
            //    var query = this.sqlManager
            //       .ExecuteQueryAsync(function, date, id)
            //       .Skip(skip);

            //    await foreach (var item in query)
            //    {
            //        await data.Set(item[IndexEntityId], item);
            //    }
            //}

            //var ordered = data.OrderBy(d => int.TryParse(d.Key, out var x) ? x : -1);

            //await foreach (var item in ordered)
            //{
            //    yield return item.Value;
            //}
        }

        public async IAsyncEnumerable<string[]> AllSelected(
                                                    string function,
                                                    AllSelectedDto dto,
                                                    int skip)
        {
            // Create new collection to store
            // selected without change
            List<string> resultColumns = this.Columns(dto.PreSelectedColumns, dto.SelectedColumns);

            var query = this.sqlManager
                .ExecuteQueryAsync(function, dto.Date, dto.Id, resultColumns)
                .Skip(skip);

            await foreach (var item in query)
            {
                yield return item;
            }
        }

        public async Task<ApplicationUser> GetUser(ClaimsPrincipal user)
        => await UserExtensions.ByUserFundColumns(this.userManager, user);

        public IEnumerable<string> GetLayout<T>(IUserRepository<T> repository, string id)
            where T : IUserColumn
        => repository.All()
            .Where(uc => uc.UserId == id)
            .Select(uc => uc.Name)
            .ToList();

        public HashSet<T> SetLayout<T>(EntitiesViewModel model, string id, IEnumerable<string> userColumns)
            where T : IUserColumn, new()
        {
            var columnsToDb = new HashSet<T>();
            if (model.Command == "Save" && model.SelectedColumns != null)
            {
                foreach (var column in model.SelectedColumns)
                {
                    T userColumn = new T
                    {
                        Name = column,
                        UserId = id,
                    };

                    columnsToDb.Add(userColumn);
                }

                model.SelectedColumns = columnsToDb.Select(c => c.Name).ToList();
            }
            else if (model.Command != "Apply" && userColumns.Count() > 0)
            {
                model.SelectedColumns = userColumns;
            }

            return columnsToDb;
        }

        private static RedisHash<string, string[]> SetupRedis(IConfiguration configuration, string dirPath, string function)
        {
            var optionsRedis = configuration
                .GetSection(AppSettingsSections.RedisSection)
                .Get<RedisOptions>();

            var connection = new RedisConnection($"{optionsRedis.Host}:{optionsRedis.Port}, {GlobalConstants.AbortConnect}", dirPath);
            var container = new RedisContainer(connection, optionsRedis.InstanceName);

            var data = container.GetKey<RedisHash<string, string[]>>(GlobalConstants.RedisCacheRecords + function);
            return data;
        }

        public List<string> Columns(IReadOnlyCollection<string> preSelectedColumns, IEnumerable<string> selectedColumns)
        {
            List<string> resultColumns = new List<string>(preSelectedColumns);

            resultColumns.AddRange(selectedColumns);

            // Prepare items for DB query with []
            resultColumns = resultColumns.Select(col => string.Format(GlobalConstants.SqlItemFormatRequired, col)).ToList();
            return resultColumns;
        }

        //private IEnumerable<string> FormatColumns(IEnumerable<string> columns)
        //{
        //    var result = new List<string>();

        //    if (columns == null)
        //    {
        //        return result;
        //    }

        //    var enumerable = columns as string[] ?? Enumerable.ToArray(columns);

        //    if (enumerable.Length <= 0)
        //    {
        //        return result;
        //    }

        //    foreach (var column in enumerable)
        //    {
        //        if (!string.IsNullOrEmpty(column))
        //        {
        //            result.Add(column.Contains(" ") ? $"[{column.Trim()}]" : $"{column.Trim()}");
        //        }
        //    }

        //    return result;
        //}
    }
}
