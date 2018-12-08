namespace FluffyDuffyMunchkinCats.Services.Implementations
{
    using Data;
    using Data.Models;
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    using static Data.Infrastructure.DataConstants;

    public class CatService : ICatService
    {
        private readonly FluffyDuffyMunchkinCatsDbContext db;

        public CatService(FluffyDuffyMunchkinCatsDbContext db)
        {
            this.db = db;
        }

        public void Add(string name, int age, string breed, string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(breed) || string.IsNullOrWhiteSpace(imageUrl))
            {
                return;
            }

            if (age < MinAge || age > MaxAge)
            {
                return;
            }

            var cat = new Cat
            {
                Name = name,
                Age = age,
                Breed = breed,
                ImageUrl = imageUrl
            };

            this.db.Add(cat);
            this.db.SaveChanges();
        }

        public CatServiceModel ById(int id)
            => this.db
                .Cats
                .Where(c => c.Id == id)
                .Select(c => new CatServiceModel
                {
                    Age = c.Age,
                    Name = c.Name,
                    Breed = c.Breed,
                    ImageUrl = c.ImageUrl
                })
                .FirstOrDefault();

        public IEnumerable<ShortCatServiceModel> All()
            => this.db
                .Cats
                .Select(c => new ShortCatServiceModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
    }
}