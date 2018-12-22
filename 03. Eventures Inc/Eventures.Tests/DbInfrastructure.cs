﻿namespace Eventures.Tests
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System;

    public static class DbInfrastructure
    {
        public static DbContextOptions<EventuresDbContext> GetDbOptions()
        {
            return new DbContextOptionsBuilder<EventuresDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
        }
    }
}