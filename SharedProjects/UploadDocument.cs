using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedProjects
{
    public class UploadDocument
    {
        [Required]
        public IFormFile formFile { get; set; }
    }
}
