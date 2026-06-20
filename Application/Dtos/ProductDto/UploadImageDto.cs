using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.ProductDto
{
    public class UploadImageDto
    {

        public Stream Content { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public long Length { get; set; }
        public bool IsMain { get; set; }

    }
}
