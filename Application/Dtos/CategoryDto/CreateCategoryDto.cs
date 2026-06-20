using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.CategoryDto
{
    public class CreateCategoryDto
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
