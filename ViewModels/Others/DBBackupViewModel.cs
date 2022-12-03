using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TWP_API.ViewModels
{

    public class DBBackupViewModel
    {
        [Required]
        public string Date { get; set; }

        [Required]
        public String Time { get; set; }

        [Required]
        public String FileName { get; set; }

        [Required]
        public double Size { get; set; }
       // public string Url { get; set; }

    }
    public class DBBackupDownloadViewModel
    {
        public IFormFile _File { get; set; }
    }
}