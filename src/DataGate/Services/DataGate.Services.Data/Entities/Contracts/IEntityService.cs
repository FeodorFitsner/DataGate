﻿// Copyright (c) DataGate Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DataGate.Services.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using DataGate.Data.Common.Repositories.UsersContext;
    using DataGate.Data.Models.Columns;
    using DataGate.Data.Models.Users;
    using DataGate.Web.ViewModels.Entities;
    using DataGate.Web.ViewModels.Queries;

    public interface IEntityService
    {
        IAsyncEnumerable<string[]> All(
                                    string function,
                                    int? id = null,
                                    DateTime? date = null,
                                    int skip = 0);

        IAsyncEnumerable<string[]> AllSelected(
                                        string function,
                                        AllSelectedDto dto,
                                        int skip = 0);

        Task<ApplicationUser> GetUser(ClaimsPrincipal user);

        HashSet<T> SetLayout<T>(EntitiesViewModel model, string id, IEnumerable<string> userColumns)
            where T : IUserColumn, new();

        IEnumerable<string> GetLayout<T>(IUserRepository<T> repository, string id)
            where T : IUserColumn;
    }
}
