namespace FluffyDuffyMunchkinCats.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Infrastructure.DataConstants;

    public class Cat
    {
        public int Id { get; set; }

        [Required]
        [MinLength(NameMinLength)]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Range(MinAge, MaxAge)]
        public int Age { get; set; }

        [Required]
        [MinLength(BreedMinLength)]
        [MaxLength(BreedMaxLength)]
        public string Breed { get; set; }

        [Url]
        [Required]
        public string ImageUrl { get; set; }
    }
}