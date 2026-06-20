using Application.Common;
using Application.Dtos.ProductDto;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Common
{
    public interface IProductService
    {
        Task<Guid> CreateAsync(CreateProductDto dto);
        Task UpdateAsync(UpdateProductDto dto);
        Task<ProductDetailsDto> GetByIdWithImagesAsync(Guid id);
        Task<PaginatedResult<ProductListDto>> GetAllAsync(int pageNumber, int pageSize);


    }
}
