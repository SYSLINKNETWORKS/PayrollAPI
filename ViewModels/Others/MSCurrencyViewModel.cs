using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TWP_API.ViewModels
{
    public class MSCurrencyViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public String Name { get; set; }
        [Required]
        public string Type { get; set; }

    }
}