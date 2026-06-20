using Application.Dtos.ProductDto;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Common
{

    public interface IImageService
    {

        Task<ProductImage> UploadProductImageAsync(
               Guid tenantId,
               Guid productId,
               UploadImageDto image,
               bool isMain);


        Task DeleteImageAsync(ProductImage image);
    }

}
