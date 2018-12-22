namespace Eventures.Tests
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System;

    public static class DbInfrastructure
    {
        public static EventuresDbContext GetDatabase()
        {
            var dbOptions = new DbContextOptionsBuilder<EventuresDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new EventuresDbContext(dbOptions);
        }
    }
}