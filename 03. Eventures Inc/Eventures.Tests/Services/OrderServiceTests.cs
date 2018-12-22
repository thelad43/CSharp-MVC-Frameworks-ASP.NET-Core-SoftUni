namespace Eventures.Tests.Services
{
    using Eventures.Services.Implementations;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class OrderServiceTests
    {
        private const string Event = "Event";
        private const string Place = "Place";
        private const string Username = "Peshovec";
        private const string FirstUserEmail = "user@email.com";
        private const string SecondUserEmail = "stamat@email.com";
        private const string SecondUserId = "USERTWO";

        private readonly Random rnd;

        public OrderServiceTests()
        {
            this.rnd = new Random();
            Tests.Initialize();
        }

        [Fact]
        public async Task AllAsyncShouldReturnOnly10Orders()
        {
            using (var db = DbInfrastructure.GetDatabase())
            {
                var events = new EventService(db);

                for (var i = 0; i < 20; i++)
                {
                    var newEvent = new Event
                    {
                        Name = $"Name {i + 1}",
                        TicketPrice = i * 123.321M,
                        Tickets = i * 123,
                        Place = $"Place {i * 15}",
                        Start = new DateTime(2018 - i, this.rnd.Next(1, 13), this.rnd.Next(1, 10) + i),
                        End = new DateTime(2018 - i + 2, this.rnd.Next(1, 13), this.rnd.Next(1, 10) + i)
                    };

                    await db.AddAsync(newEvent);
                }

                await db.SaveChangesAsync();

                var orders = new OrderService(db);

                for (var i = 0; i < 50; i++)
                {
                    var order = new Order
                    {
                        Tickets = (i + 1) * 50,
                        OrderedOn = new DateTime(2018, 10, 15),
                        EventId = this.rnd.Next(1, 20)
                    };

                    await db.AddAsync(order);
                }

                await db.SaveChangesAsync();

                var ordersFromPageOne = await orders.AllAsync(1);

                Assert.Equal(10, ordersFromPageOne.Count());
            }
        }

        [Fact]
        public async Task AllAsyncShouldReturnOrdersOrderedByDescByOrderedDate()
        {
            using (var db = DbInfrastructure.GetDatabase())
            {
                var events = new EventService(db);

                for (var i = 0; i < 20; i++)
                {
                    var newEvent = new Event
                    {
                        Name = $"Name {i + 1}",
                        TicketPrice = i * 123.321M,
                        Tickets = i * 123,
                        Place = $"Place {i * 15}",
                        Start = new DateTime(2018 - i, this.rnd.Next(1, 13), this.rnd.Next(1, 10) + i),
                        End = new DateTime(2018 - i + 2, this.rnd.Next(1, 13), this.rnd.Next(1, 10) + i)
                    };

                    await db.AddAsync(newEvent);
                }

                await db.SaveChangesAsync();

                var orders = new OrderService(db);

                for (var i = 0; i < 20; i++)
                {
                    var order = new Order
                    {
                        Tickets = (i + 1) * 50,
                        OrderedOn = new DateTime(2018, 11, 30).AddDays(i + 1),
                        EventId = this.rnd.Next(1, 20)
                    };

                    await db.AddAsync(order);
                }

                await db.SaveChangesAsync();

                var ordersFromPageOne = await orders.AllAsync();

                var ordersFromPageOneList = ordersFromPageOne.ToList();

                var expectedDates = new List<DateTime>
                {
                    new DateTime(2018, 12, 20),
                    new DateTime(2018, 12, 19),
                    new DateTime(2018, 12, 18),
                    new DateTime(2018, 12, 17),
                    new DateTime(2018, 12, 16),
                    new DateTime(2018, 12, 15),
                    new DateTime(2018, 12, 14),
                    new DateTime(2018, 12, 13),
                    new DateTime(2018, 12, 12),
                    new DateTime(2018, 12, 11),
                };

                for (var i = 0; i < 10; i++)
                {
                    Assert.Equal(ordersFromPageOneList[i].OrderedOn, expectedDates[i]);
                }
            }
        }

        [Fact]
        public async Task CreateAsyncShouldCreateSuccessfullyOrderAndAddItToDb()
        {
            using (var db = DbInfrastructure.GetDatabase())
            {
                var events = new EventService(db);

                var allEvents = await events.AllAsync();

                var orders = new OrderService(db);

                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = FirstUserEmail,
                    FirstName = "Pesho",
                    LastName = "Ivanov",
                    UniqueCitizenNumber = "UNIQUE",
                    UserName = Username
                };

                var newEvent = new Event
                {
                    Name = Event,
                    Place = Place,
                    Start = new DateTime(),
                    End = new DateTime(),
                    Tickets = 1000,
                    TicketPrice = 100
                };

                await orders.CreateAsync(user, newEvent, 550);

                var allOrders = await orders.AllAsync();

                var actualOrder = allOrders.FirstOrDefault();

                Assert.Equal(Event, actualOrder.EventName);
                Assert.Equal(DateTime.UtcNow.Minute, actualOrder.OrderedOn.Minute);
                Assert.Equal(Username, actualOrder.UserUserName);
            }
        }

        [Fact]
        public async Task CreateAsyncShouldNotCreateOrderIfUserIsNull()
        {
            using (var db = DbInfrastructure.GetDatabase())
            {
                var events = new EventService(db);

                var allEvents = await events.AllAsync();

                var orders = new OrderService(db);

                var newEvent = new Event
                {
                    Name = Event,
                    Place = Place,
                    Start = new DateTime(),
                    End = new DateTime(),
                    Tickets = 1000,
                    TicketPrice = 100
                };

                await orders.CreateAsync(null, newEvent, 550);

                var allOrders = await orders.AllAsync();

                Assert.Empty(allOrders);
            }
        }

        [Fact]
        public async Task CreateAsyncShouldNotCreateOrderIfEventIsNull()
        {
            using (var db = DbInfrastructure.GetDatabase())
            {
                var events = new EventService(db);

                var allEvents = await events.AllAsync();

                var orders = new OrderService(db);

                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = FirstUserEmail,
                    FirstName = "Pesho",
                    LastName = "Ivanov",
                    UniqueCitizenNumber = "UNIQUE",
                    UserName = Username
                };

                await orders.CreateAsync(user, null, 550);

                var allOrders = await orders.AllAsync();

                Assert.Empty(allOrders);
            }
        }

        [Fact]
        public async Task CreateAsyncShouldNotCreateOrderIfTicketsCountIsZeroOrLess()
        {
            using (var db = DbInfrastructure.GetDatabase())
            {
                var events = new EventService(db);

                var allEvents = await events.AllAsync();

                var orders = new OrderService(db);

                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = FirstUserEmail,
                    FirstName = "Pesho",
                    LastName = "Ivanov",
                    UniqueCitizenNumber = "UNIQUE",
                    UserName = Username
                };

                var newEvent = new Event
                {
                    Name = Event,
                    Place = Place,
                    Start = new DateTime(),
                    End = new DateTime(),
                    Tickets = 1000,
                    TicketPrice = 100
                };

                await orders.CreateAsync(user, newEvent, 0);

                await orders.CreateAsync(user, newEvent, -590);

                var allOrders = await orders.AllAsync();

                Assert.Empty(allOrders);
            }
        }

        [Fact]
        public async Task CreateAsyncShouldDecreaseEventsTicketsByOrderTickets()
        {
            using (var db = DbInfrastructure.GetDatabase())
            {
                var events = new EventService(db);

                var allEvents = await events.AllAsync();

                var orders = new OrderService(db);

                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = FirstUserEmail,
                    FirstName = "Pesho",
                    LastName = "Ivanov",
                    UniqueCitizenNumber = "UNIQUE",
                    UserName = Username
                };

                var newEvent = new Event
                {
                    Name = Event,
                    Place = Place,
                    Start = new DateTime(),
                    End = new DateTime(),
                    Tickets = 1000,
                    TicketPrice = 100
                };

                await orders.CreateAsync(user, newEvent, 550);

                var actualEvent = db.Events.FirstOrDefault();

                Assert.Equal(450, actualEvent.Tickets);
            }
        }

        [Fact]
        public async Task MyAsyncShouldReturnOnlyOrdersCreatedByUser()
        {
            using (var db = DbInfrastructure.GetDatabase())
            {
                var events = new EventService(db);

                var allEvents = await events.AllAsync();

                var orders = new OrderService(db);

                var firstUser = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = FirstUserEmail,
                    FirstName = "Pesho",
                    LastName = "Ivanov",
                    UniqueCitizenNumber = "UNIQUE",
                    UserName = Username
                };

                var secondUser = new User
                {
                    Id = SecondUserId,
                    Email = SecondUserEmail,
                    FirstName = "Stamat",
                    LastName = "Petrov",
                    UniqueCitizenNumber = "UNIQUE 2345",
                    UserName = Username + "Two"
                };

                var firstEvent = new Event
                {
                    Name = Event,
                    Place = Place,
                    Start = new DateTime(),
                    End = new DateTime(),
                    Tickets = 1000,
                    TicketPrice = 100
                };

                var secondEvent = new Event
                {
                    Name = Event + "Two",
                    Place = "Place2",
                    Start = new DateTime(),
                    End = new DateTime(),
                    Tickets = 5555,
                    TicketPrice = 555
                };

                await orders.CreateAsync(firstUser, firstEvent, 52);
                await orders.CreateAsync(firstUser, firstEvent, 52);
                await orders.CreateAsync(firstUser, firstEvent, 52);
                await orders.CreateAsync(firstUser, firstEvent, 52);
                await orders.CreateAsync(firstUser, firstEvent, 52);

                await orders.CreateAsync(secondUser, secondEvent, 54);

                var secondUserEvents = await orders.MyAsync(SecondUserId);

                var secondUserEvent = secondUserEvents.FirstOrDefault();

                Assert.Equal(Event + "Two", secondUserEvent.EventName);
                Assert.Equal(54, secondUserEvent.Tickets);
            }
        }

        [Fact]
        public async Task TotalAsyncByUserIdShouldReturnCorrectCount()
        {
            using (var db = DbInfrastructure.GetDatabase())
            {
                var events = new EventService(db);

                var allEvents = await events.AllAsync();

                var orders = new OrderService(db);

                var firstUser = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = FirstUserEmail,
                    FirstName = "Pesho",
                    LastName = "Ivanov",
                    UniqueCitizenNumber = "UNIQUE",
                    UserName = Username
                };

                var secondUser = new User
                {
                    Id = SecondUserId,
                    Email = SecondUserEmail,
                    FirstName = "Stamat",
                    LastName = "Petrov",
                    UniqueCitizenNumber = "UNIQUE 2345",
                    UserName = Username + "Two"
                };

                var firstEvent = new Event
                {
                    Name = Event,
                    Place = Place,
                    Start = new DateTime(),
                    End = new DateTime(),
                    Tickets = 1000,
                    TicketPrice = 100
                };

                var secondEvent = new Event
                {
                    Name = Event + "Two",
                    Place = "Place2",
                    Start = new DateTime(),
                    End = new DateTime(),
                    Tickets = 5555,
                    TicketPrice = 555
                };

                await orders.CreateAsync(firstUser, firstEvent, 52);
                await orders.CreateAsync(firstUser, firstEvent, 52);
                await orders.CreateAsync(firstUser, firstEvent, 52);
                await orders.CreateAsync(firstUser, firstEvent, 52);
                await orders.CreateAsync(firstUser, firstEvent, 52);

                await orders.CreateAsync(secondUser, secondEvent, 54);
                await orders.CreateAsync(secondUser, firstEvent, 54);
                await orders.CreateAsync(secondUser, secondEvent, 54);

                var secondUserTotalOrders = await orders.TotalAsyncByUserId(SecondUserId);

                Assert.Equal(3, secondUserTotalOrders);
            }
        }

        [Fact]
        public async Task TotalAsyncShouldReturnCorrectCountOrders()
        {
            using (var db = DbInfrastructure.GetDatabase())
            {
                var events = new EventService(db);

                var allEvents = await events.AllAsync();

                var orders = new OrderService(db);

                var firstUser = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = FirstUserEmail,
                    FirstName = "Pesho",
                    LastName = "Ivanov",
                    UniqueCitizenNumber = "UNIQUE",
                    UserName = Username
                };

                var secondUser = new User
                {
                    Id = SecondUserId,
                    Email = SecondUserEmail,
                    FirstName = "Stamat",
                    LastName = "Petrov",
                    UniqueCitizenNumber = "UNIQUE 2345",
                    UserName = Username + "Two"
                };

                var firstEvent = new Event
                {
                    Name = Event,
                    Place = Place,
                    Start = new DateTime(),
                    End = new DateTime(),
                    Tickets = 1000,
                    TicketPrice = 100
                };

                var secondEvent = new Event
                {
                    Name = Event + "Two",
                    Place = "Place2",
                    Start = new DateTime(),
                    End = new DateTime(),
                    Tickets = 5555,
                    TicketPrice = 555
                };

                await orders.CreateAsync(firstUser, firstEvent, 52);
                await orders.CreateAsync(firstUser, firstEvent, 52);
                await orders.CreateAsync(firstUser, firstEvent, 52);
                await orders.CreateAsync(firstUser, firstEvent, 52);
                await orders.CreateAsync(firstUser, firstEvent, 52);

                await orders.CreateAsync(secondUser, secondEvent, 54);
                await orders.CreateAsync(secondUser, firstEvent, 54);
                await orders.CreateAsync(secondUser, secondEvent, 54);

                var totalOrders = await orders.TotalAsync();

                Assert.Equal(8, totalOrders);
            }
        }
    }
}