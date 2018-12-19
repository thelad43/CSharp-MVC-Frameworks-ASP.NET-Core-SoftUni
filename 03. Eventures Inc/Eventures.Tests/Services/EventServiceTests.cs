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
        private readonly Random rnd;

        public EventServiceTests()
        {
            this.rnd = new Random();

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

                var dateTime = new DateTime(2018, 5, 5);

                for (var i = 0; i < 15; i++)
                {
                    var newEvent = new Event
                    {
                        Name = $"Name {i + 1}",
                        TicketPrice = i * 123.321M,
                        Tickets = i - 1,
                        Place = $"Place {i * 15}",
                        Start = dateTime.AddDays(-5),
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
                    expectedStartDates.Add(dateTime.AddDays(-5));
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

                var dateTime = new DateTime(2018, 5, 5);

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

        private DbContextOptions<EventuresDbContext> GetDbOptions()
        {
            return new DbContextOptionsBuilder<EventuresDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
        }
    }
}