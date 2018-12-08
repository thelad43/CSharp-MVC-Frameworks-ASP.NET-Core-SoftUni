namespace FluffyDuffyMunchkinCats.Web.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using static Data.Infrastructure.DataConstants;
    using static Data.Infrastructure.ErrorMessages;

    public class CatFormModel
    {
        [Required(ErrorMessage = CatNameRequiredErrorMessage)]
        [MinLength(NameMinLength, ErrorMessage = CatNameMinLengthErrorMessage)]
        [MaxLength(NameMaxLength, ErrorMessage = CatNameMaxLengthErrorMessage)]
        public string Name { get; set; }

        [Range(MinAge, MaxAge, ErrorMessage = CatAgeLengthErrorMessage)]
        public int Age { get; set; }

        [Required(ErrorMessage = CatBreedRequiredErrorMessage)]
        [MinLength(BreedMinLength, ErrorMessage = CatBreedMinLengthErrorMessage)]
        [MaxLength(BreedMaxLength, ErrorMessage = CatBreedMaxLengthErrorMessage)]
        public string Breed { get; set; }

        [DisplayName("Image URL")]
        [Required(ErrorMessage = ImageUrlRequiredErrorMessage)]
        [Url(ErrorMessage = ImageUrlNotValidErrorMessage)]
        public string ImageUrl { get; set; }
    }
}