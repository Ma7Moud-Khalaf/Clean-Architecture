using Application.Common;
using Application.Common.Options;
using Application.Dtos.ProductDto;
using Application.Interfaces.Common;
using Domain.Entities;
using Microsoft.Extensions.Options;

public class ProductService : IProductService
{
    private readonly IGenericRepository<Product> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;
    private readonly ICurrentTenantService _currentTenant;
    private readonly ProductImageSettings _imageSettings;

    public ProductService(
        IGenericRepository<Product> repository,
        IUnitOfWork unitOfWork,
        IImageService imageService,
        ICurrentTenantService currentTenant,
        IOptions<ProductImageSettings> imageSettings)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _imageService = imageService;
        _currentTenant = currentTenant;
        _imageSettings = imageSettings.Value;
    }

    public async Task<Guid> CreateAsync(CreateProductDto dto)
    {
        if (dto.Images == null || !dto.Images.Any())
            throw new InvalidOperationException("At least one image is required");

        if (dto.Images.Count > _imageSettings.MaxImagesPerProduct)
            throw new InvalidOperationException("Image limit exceeded");

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new InvalidOperationException("Product name is required");

        if (dto.Price <= 0)
            throw new InvalidOperationException("Invalid price");

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            StockQuantity = dto.StockQuantity,
            TenantId = _currentTenant.TenantId
                       ?? throw new InvalidOperationException("Tenant not found"),
            CategoryId = dto.CategoryId
        };

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            await _repository.AddAsync(product);

            var uploadedImages = new List<ProductImage>();

            try
            {
                var tasks = dto.Images.Select((image, index) =>
                {
                    var uploadDto = new UploadImageDto
                    {
                        FileName = image.FileName,
                        Length = image.Length,
                        Content = image.OpenReadStream(),
                        IsMain = index == 0
                    };

                    return _imageService.UploadProductImageAsync(
                        product.TenantId,
                        product.Id,
                        uploadDto,
                        uploadDto.IsMain);
                });

                var results = await Task.WhenAll(tasks);

                product.Images = results.ToList();
            }
            catch
            {
                foreach (var img in uploadedImages)
                    await _imageService.DeleteImageAsync(img);

                throw;
            }
        });

        return product.Id;
    }
    public async Task UpdateAsync(UpdateProductDto dto)
    {
        var product = await _repository.GetByIdAsync(dto.Id);

        if (product == null)
            throw new Exception("Product not found");

        if (product.TenantId != _currentTenant.TenantId)
            throw new UnauthorizedAccessException();

        // =========================
        // ✅ Update basic info
        // =========================
        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.StockQuantity = dto.StockQuantity;
        product.CategoryId = dto.CategoryId;

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            // =========================
            // ❌ Remove images
            // =========================
            if (dto.RemovedImageIds.Any())
            {
                var imagesToRemove = product.Images
                    .Where(i => dto.RemovedImageIds.Contains(i.Id))
                    .ToList();

                foreach (var img in imagesToRemove)
                {
                    await _imageService.DeleteImageAsync(img);
                    product.Images.Remove(img);
                }
            }

            // =========================
            // ➕ Add new images
            // =========================
            var newUploadedImages = new List<ProductImage>();

            foreach (var image in dto.NewImages)
            {
                var uploadDto = new UploadImageDto
                {
                    FileName = image.FileName,
                    Length = image.Length,
                    Content = image.OpenReadStream()
                };

                var savedImage = await _imageService.UploadProductImageAsync(
                    product.TenantId,
                    product.Id,
                    uploadDto,
                    false // main later
                );

                newUploadedImages.Add(savedImage);
            }

            foreach (var img in newUploadedImages)
                product.Images.Add(img);

            // =========================
            // ⭐ Set Main Image
            // =========================
            if (dto.MainImageId.HasValue)
            {
                foreach (var img in product.Images)
                    img.IsMain = img.Id == dto.MainImageId.Value;
            }
            else if (!product.Images.Any(i => i.IsMain))
            {
                // fallback: أول صورة
                var first = product.Images.FirstOrDefault();
                if (first != null)
                    first.IsMain = true;
            }
        });
    }

    public async Task<ProductDetailsDto> GetByIdWithImagesAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id, p=> p.Images);

        if (product == null)
            throw new Exception("Product not found");

        if (product.TenantId != _currentTenant.TenantId)
            throw new UnauthorizedAccessException();

        var result = new ProductDetailsDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CategoryId = product.CategoryId,
            Images = product.Images
                .Select(img => new ProductImageDto
                {
                    Id = img.Id,
                    Url = img.ImageUrl, // ✅ already relative URL
                    IsMain = img.IsMain
                })
                .OrderByDescending(i => i.IsMain) // main first
                .ToList()
        };

        return result;
    }
    public async Task<PaginatedResult<ProductListDto>> GetAllAsync(int pageNumber,int pageSize)
    {
        var result = await _repository.GetPagedAsync(
            pageNumber,
            pageSize, 
            p=> p.TenantId == _currentTenant.TenantId,
            p => p.Images
        );

        return new PaginatedResult<ProductListDto>
        {
            Items = result.Items.Select(p =>
            {
                var mainImage = p.Images.FirstOrDefault(i => i.IsMain);

                return new ProductListDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    MainImageUrl = mainImage?.ImageUrl
                };
            }).ToList(),

            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}