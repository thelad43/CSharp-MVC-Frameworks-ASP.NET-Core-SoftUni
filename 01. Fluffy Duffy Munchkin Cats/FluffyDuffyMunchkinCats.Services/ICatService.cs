namespace FluffyDuffyMunchkinCats.Services
{
    using Models;
    using System.Collections.Generic;

    public interface ICatService
    {
        void Add(string name, int age, string breed, string imageUrl);

        CatServiceModel ById(int id);

        IEnumerable<ShortCatServiceModel> All();
    }
}