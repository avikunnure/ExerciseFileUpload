using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedProjects
{
    public class MultiUploadDocument
    {
        [Required]
        public List<IFormFile> formFile { get; set; }
    }
}
