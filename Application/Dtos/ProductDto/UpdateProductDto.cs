using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.ProductDto
{
    public class UpdateProductDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public Guid CategoryId { get; set; }

        // صور جديدة
        public List<IFormFile> NewImages { get; set; } = new();

        // صور هتتمسح
        public List<Guid> RemovedImageIds { get; set; } = new();

        // الصورة الأساسية
        public Guid? MainImageId { get; set; }
    }
}
