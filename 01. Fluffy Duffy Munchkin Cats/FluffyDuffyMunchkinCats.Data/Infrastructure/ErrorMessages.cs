namespace FluffyDuffyMunchkinCats.Data.Infrastructure
{
    public static class ErrorMessages
    {
        public const string CatNameRequiredErrorMessage = "The field name is required!";
        public const string CatNameMinLengthErrorMessage = "The field name cannot be less than 2 symbols!";
        public const string CatNameMaxLengthErrorMessage = "The field name cannot be more than 25 symbols!";

        public const string CatAgeLengthErrorMessage = "Age must be between 0 and 10!";

        public const string CatBreedRequiredErrorMessage = "The field breed is required!";
        public const string CatBreedMinLengthErrorMessage = "The field breed cannot be less than 5 symbols!";
        public const string CatBreedMaxLengthErrorMessage = "The field breed cannot be more than 30 symbols!";

        public const string ImageUrlRequiredErrorMessage = "The field Image URL is required!";
        public const string ImageUrlNotValidErrorMessage = "The field Image URL must be a valid URL!";
    }
}