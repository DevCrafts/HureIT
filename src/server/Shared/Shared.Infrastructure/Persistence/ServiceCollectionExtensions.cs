﻿// --------------------------------------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="HureIT">
// Copyright (c) HureIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Net;
using HureIT.Shared.Core.Constants;
using HureIT.Shared.Core.Exceptions;
using HureIT.Shared.Core.Extensions;
using HureIT.Shared.Core.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace HureIT.Shared.Infrastructure.Persistence
{
    public static class ServiceCollectionExtensions
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(ServiceCollectionExtensions));

        public static IServiceCollection AddDatabaseContext<T>(this IServiceCollection services)
            where T : DbContext
        {
            var options = services.GetOptions<PersistenceSettings>(nameof(PersistenceSettings));
            if (options.UseMsSql)
            {
                if (string.IsNullOrEmpty(options.ConnectionStrings.MSSQL))
                {
                    throw new InvalidOperationException($"Data Provider {DataProviderKeys.SqlServer.ToUpper()} is not configured.");
                }

                string connectionString = options.ConnectionStrings.MSSQL;
                services.AddMSSQL<T>(connectionString);
            }

            _logger.Information($"Current Data Provider: {DataProviderKeys.SqlServer.ToUpper()}");
            return services;
        }

        private static IServiceCollection AddMSSQL<T>(this IServiceCollection services, string connectionString)
            where T : DbContext
        {
            try
            {
                services.AddDbContext<T>(x => x.UseSqlServer(connectionString, o => o.MigrationsAssembly(typeof(T).Assembly.FullName)));
                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<T>();
                dbContext.Database.Migrate();
                _logger.Information($"Migration Data Provider {DataProviderKeys.SqlServer.ToUpper()} Successful");
                return services;
            }
            catch (Exception)
            {
                throw new CustomException($"Migration Data Provider {DataProviderKeys.SqlServer.ToUpper()} is not supported. An errors occurred.", statusCode: HttpStatusCode.BadRequest);
            }
        }
    }
}