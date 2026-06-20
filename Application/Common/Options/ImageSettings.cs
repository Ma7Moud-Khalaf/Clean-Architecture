using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Options
{

    public class ProductImageSettings
    {
        public int MaxImagesPerProduct { get; set; }
        public int MaxSizeInMB { get; set; }
        public List<string> AllowedExtensions { get; set; } = [];
        public string UploadFolder { get; set; } = null!;
    }

}
