namespace Eventures.Tests.Services.Admin
{
    using AutoMapper;
    using Eventures.Common.Mapping;
    using Eventures.Data;
    using Eventures.Models;
    using Eventures.Services.Admin.Implementations;
    using Eventures.Services.Admin.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class AdminUserServiceTests
    {
        public AdminUserServiceTests()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => new AutomapperProfile());
        }

        [Fact]
        public async Task AllAsyncShouldReturnAllUsers()
        {
            using (var db = new EventuresDbContext(DbInfrastructure.GetDbOptions()))
            {
                for (var i = 0; i < 10; i++)
                {
                    var user = new User
                    {
                        Id = i.ToString(),
                        Email = $"em{i}email.com",
                        FirstName = $"Pesho {i}",
                        LastName = $"Ivanov {i}",
                        UniqueCitizenNumber = $"UNIQUE {i}",
                        UserName = $"Username {i}"
                    };

                    await db.AddAsync(user);
                }

                await db.SaveChangesAsync();

                var adminUserService = new AdminUserService(db);

                var expectedUsers = new List<AdminUserListingServiceModel>();

                for (var i = 0; i < 10; i++)
                {
                    expectedUsers.Add(new AdminUserListingServiceModel
                    {
                        Id = i.ToString(),
                        Email = $"em{i}email.com",
                        Username = $"Username {i}"
                    });
                }

                var allUsers = await adminUserService.AllAsync();

                var allUsersList = allUsers.ToList();

                for (var i = 0; i < 10; i++)
                {
                    Assert.Equal(expectedUsers[i].Id, allUsersList[i].Id);
                    Assert.Equal(expectedUsers[i].Email, allUsersList[i].Email);
                    Assert.Equal(expectedUsers[i].Username, allUsersList[i].Username);
                }
            }
        }
    }
}