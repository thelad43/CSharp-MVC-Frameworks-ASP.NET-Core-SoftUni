namespace Chushka.Services.Models
{
    using System;

    public class OrderInfoServiceModel
    {
        public int Id { get; set; }

        public string UserUsername { get; set; }

        public string ProductName { get; set; }

        public DateTime OrderedOn { get; set; }
    }
}