namespace Eventures.Tests.Services
{
    using AutoMapper;
    using Eventures.Common.Mapping;
    using Eventures.Data;
    using Eventures.Models;
    using Eventures.Services.Implementations;
    using Eventures.Services.Models.Events;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class EventServiceTests
    {
        private const string Name = "Gosho";
        private const string Place = "Placee";

        private const int TicketPrice = 100;
        private const int TicketsCount = 1000;

        private readonly Random rnd;

        private readonly DateTime dateTime;
        private readonly DateTime startDate;
        private readonly DateTime endDate;

        public EventServiceTests()
        {
            this.rnd = new Random();

            this.dateTime = new DateTime(2018, 5, 5);
            this.startDate = new DateTime(2018, 3, 5);
            this.endDate = new DateTime(2018, 3, 8);

            Mapper.Reset();
            Mapper.Initialize(cfg => new AutomapperProfile());
        }

        [Fact]
        public async Task AllAsyncShouldReturn10EventsFromPage1()
        {
            using (var db = new EventuresDbContext(this.GetDbOptions()))
            {
                var events = new EventService(db);

                for (var i = 0; i < 15; i++)
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

                var eventsFromPageOne = await events.AllAsync(1);

                Assert.Equal(10, eventsFromPageOne.Count());
            }
        }

        [Fact]
        public async Task AllAsyncShouldReturnOnlyEventsThatHaveMoreThan1Ticket()
        {
            using (var db = new EventuresDbContext(this.GetDbOptions()))
            {
                var events = new EventService(db);

                for (var i = 0; i < 10; i++)
                {
                    var newEvent = new Event
                    {
                        Name = $"Name {i + 1}",
                        TicketPrice = i * 123.321M,
                        Tickets = i - 1,
                        Place = $"Place {i * 15}",
                        Start = new DateTime(2018 - i, this.rnd.Next(1, 13), this.rnd.Next(1, 10) + i),
                        End = new DateTime(2018 - i + 2, this.rnd.Next(1, 13), this.rnd.Next(1, 10) + i)
                    };

                    await db.AddAsync(newEvent);
                }

                await db.SaveChangesAsync();

                var eventsFromPageOne = await events.AllAsync(1);

                Assert.Equal(8, eventsFromPageOne.Count());
            }
        }

        [Fact]
        public async Task AllAsyncShouldReturnEventsInDescendingOrderByStartDate()
        {
            using (var db = new EventuresDbContext(this.GetDbOptions()))
            {
                var events = new EventService(db);

                for (var i = 0; i < 15; i++)
                {
                    var newEvent = new Event
                    {
                        Name = $"Name {i + 1}",
                        TicketPrice = i * 123.321M,
                        Tickets = i - 1,
                        Place = $"Place {i * 15}",
                        Start = this.dateTime.AddDays(-5),
                        End = new DateTime(2018 - i + 2, this.rnd.Next(1, 13), this.rnd.Next(1, 10) + i)
                    };

                    await db.AddAsync(newEvent);
                }

                await db.SaveChangesAsync();

                var eventsFromPageOne = await events.AllAsync(1);

                var expectedStartDates = new List<DateTime>();
                var actualStartDates = new List<DateTime>();

                foreach (var currentEvent in eventsFromPageOne)
                {
                    expectedStartDates.Add(this.dateTime.AddDays(-5));
                    actualStartDates.Add(currentEvent.Start);
                }

                expectedStartDates.OrderByDescending(e => e);

                Assert.Equal(expectedStartDates, actualStartDates);
            }
        }

        [Fact]
        public async Task AllAsyncShouldReturnOnlyTheEventsLocatedAtTheGivenPage()
        {
            using (var db = new EventuresDbContext(this.GetDbOptions()))
            {
                var events = new EventService(db);

                for (var i = 0; i < 22; i++)
                {
                    var newEvent = new Event
                    {
                        Name = $"Name {i}"
                    };

                    await db.AddAsync(newEvent);
                }

                await db.SaveChangesAsync();

                var eventsFromPageTwo = await events.AllAsync(1);

                var expectedEvents = new List<EventServiceModel>();

                for (var i = 10; i < eventsFromPageTwo.Count() + 10; i++)
                {
                    expectedEvents.Add(new EventServiceModel
                    {
                        Name = $"Name {i}"
                    });
                }

                Assert.Equal(expectedEvents, eventsFromPageTwo);
            }
        }

        [Fact]
        public async Task ByIdAsyncShoulReturnCorrectEvent()
        {
            using (var db = new EventuresDbContext(this.GetDbOptions()))
            {
                var events = new EventService(db);

                for (var i = 0; i < 30; i++)
                {
                    var newEvent = new Event
                    {
                        Name = $"Name {i + 1}",
                        TicketPrice = i * 123.321M,
                        Tickets = i * 123,
                        Place = $"Place {i * 15}",
                    };

                    await db.AddAsync(newEvent);
                }

                await db.SaveChangesAsync();

                var actualEvent = await events.ByIdAsync(10);

                Assert.Equal(10, actualEvent.Id);
                Assert.Equal("Name 10", actualEvent.Name);
                Assert.Equal(9 * 123.321M, actualEvent.TicketPrice);
                Assert.Equal($"Place {9 * 15}", actualEvent.Place);
            }
        }

        [Fact]
        public async Task CreateAsyncShouldCreateSuccessfullyEventAndAddItToDb()
        {
            using (var db = new EventuresDbContext(this.GetDbOptions()))
            {
                var events = new EventService(db);

                await events.CreateAsync(Name, Place, this.startDate, this.endDate, TicketsCount, TicketPrice);

                var allEvents = await events.AllAsync();

                var actualEvent = allEvents.FirstOrDefault();

                var expectedEvent = new Event
                {
                    Name = Name,
                    Place = Place,
                    Start = this.startDate,
                    End = this.endDate,
                    Tickets = TicketsCount,
                    TicketPrice = TicketPrice
                };

                Assert.True(expectedEvent.Name == actualEvent.Name);
                Assert.True(expectedEvent.Start == actualEvent.Start);
                Assert.True(expectedEvent.End == actualEvent.End);
                Assert.True(expectedEvent.Place == actualEvent.Place);
                Assert.True(expectedEvent.Tickets == actualEvent.Tickets);
            }
        }

        [Fact]
        public async Task CreateAsyncShouldNotCreateEventIfNameIsNullOrWhiteSpace()
        {
            using (var db = new EventuresDbContext(this.GetDbOptions()))
            {
                var events = new EventService(db);

                await events.CreateAsync(null, Place, this.startDate, this.endDate, TicketsCount, TicketPrice);

                await events.CreateAsync("", Place, this.startDate, this.endDate, TicketsCount, TicketPrice);

                await events.CreateAsync("               ", Place, this.startDate, this.endDate, TicketsCount, TicketPrice);

                await events.CreateAsync(Environment.NewLine, Place, this.startDate, this.endDate, TicketsCount, TicketPrice);

                var allEvents = await events.AllAsync();

                Assert.Empty(allEvents);
            }
        }

        [Fact]
        public async Task CreateAsyncShouldNotCreateEventIfPlaceIsNullOrWhiteSpace()
        {
            using (var db = new EventuresDbContext(this.GetDbOptions()))
            {
                var events = new EventService(db);

                await events.CreateAsync(Name, null, this.startDate, this.endDate, TicketsCount, TicketPrice);

                await events.CreateAsync(Name, "", this.startDate, this.endDate, TicketsCount, TicketPrice);

                await events.CreateAsync(Name, "               ", this.startDate, this.endDate, TicketsCount, TicketPrice);

                await events.CreateAsync(Name, Environment.NewLine, this.startDate, this.endDate, TicketsCount, TicketPrice);

                var allEvents = await events.AllAsync();

                Assert.Empty(allEvents);
            }
        }

        [Fact]
        public async Task CreateAsyncShouldNotCreateEventIfTicketPriceIsZeroOrLess()
        {
            using (var db = new EventuresDbContext(this.GetDbOptions()))
            {
                var events = new EventService(db);

                await events.CreateAsync(Name, Place, this.startDate, this.endDate, TicketsCount, 0);

                await events.CreateAsync(Name, Place, this.startDate, this.endDate, 565, -56);

                var allEvents = await events.AllAsync();

                Assert.Empty(allEvents);
            }
        }

        [Fact]
        public async Task CreateAsyncShouldNotCreateEventIfTicketsAreZeroOrLess()
        {
            using (var db = new EventuresDbContext(this.GetDbOptions()))
            {
                var events = new EventService(db);

                await events.CreateAsync(Name, Place, this.startDate, this.endDate, 0, TicketPrice);

                await events.CreateAsync(Name, Place, this.startDate, this.endDate, -567, TicketPrice);

                var allEvents = await events.AllAsync();

                Assert.Empty(allEvents);
            }
        }

        [Fact]
        public async Task TotalAsyncShouldReturnCorrectCount()
        {
            using (var db = new EventuresDbContext(this.GetDbOptions()))
            {
                var events = new EventService(db);

                for (var i = 0; i < 45; i++)
                {
                    var newEvent = new Event
                    {
                        Name = Name,
                        Place = Place,
                        Start = this.startDate,
                        End = this.endDate,
                        Tickets = TicketsCount,
                        TicketPrice = TicketPrice
                    };

                    await db.AddAsync(newEvent);
                }

                await db.SaveChangesAsync();

                Assert.Equal(45, await events.TotalAsync());
            }
        }

        private DbContextOptions<EventuresDbContext> GetDbOptions()
        {
            return new DbContextOptionsBuilder<EventuresDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
        }
    }
}