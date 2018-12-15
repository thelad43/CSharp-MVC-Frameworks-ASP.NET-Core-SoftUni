namespace Eventures.Web.Areas.Admin.Models.Events
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Common.WebConstants;

    public class CreateEventFormModel
    {
        [Required]
        [MinLength(NameMinLength)]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        public string Place { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:MM}")]
        [Display(Name = "Start date")]
        public DateTime Start { get; set; }

        [Display(Name = "End date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:MM}")]
        public DateTime End { get; set; }

        [Display(Name = "Tickets count")]
        public int Tickets { get; set; }

        [Display(Name = "Price per ticket")]
        public decimal TicketPrice { get; set; }
    }
}