using Microsoft.AspNetCore.Mvc;

using TallerIDWM_Backend.Src.Data;
using TallerIDWM_Backend.Src.DTOs;
using TallerIDWM_Backend.Src.DTOs.Product;
using TallerIDWM_Backend.Src.Extensions;
using TallerIDWM_Backend.Src.Helpers;
using TallerIDWM_Backend.Src.Interfaces;
using TallerIDWM_Backend.Src.Mappers;
using TallerIDWM_Backend.Src.Models;
using TallerIDWM_Backend.Src.RequestHelpers;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace TallerIDWM_Backend.Src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(ILogger<ProductController> logger, UnitOfWork unitOfWork, IPhotoService photoService) : ControllerBase()
    {
        private readonly ILogger<ProductController> _logger = logger;
        private readonly UnitOfWork _context = unitOfWork;
        private readonly IPhotoService _photoService = photoService;

        // Obtener todos los productos en vista de cliente 
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetProducts([FromQuery] ProductParams productParams)
        {
            try
            {
                // Obtener todos los productos visibles
                var query = _context.ProductRepository.GetQueryableProducts();

                // Se aplican los filtros según los parámetros de consulta
                query = query.Search(productParams.Search)
                             .Filter(productParams.Brands, productParams.Categories)
                             .Sort(productParams.SortBy);

                // Obtener la paginación
                var pagedList = await PagedList<Product>.ToPagedList(query, productParams.PageNumber, productParams.PageSize);

                // Establecer los encabezados de paginación
                Response.AddPaginationHeader(pagedList.Metadata);

                // Crear la respuesta
                var response = new ApiResponse<IEnumerable<ProductDto>>(
                    true,
                    "Productos obtenidos correctamente.",
                    pagedList.Select(p => p.MapToProductDto()));

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los productos.");
                return NotFound(new ApiResponse<ProductDto>(false, "No se encontraron productos."));
            }
        }

        // Obtener todos los productos en vista de administrador
        [HttpGet("admin")]
        // [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductDtoAdmin>>>> GetAllProducts([FromQuery] ProductParams productParams)
        {
            try
            {
                // Obtener todos los productos 
                var query = _context.ProductRepository.GetQueryableProducts();

                // Se aplican los filtros según los parámetros de consulta
                query = query.Search(productParams.Search)
                             .Filter(productParams.Brands, productParams.Categories)
                             .Sort(productParams.SortBy);

                // Obtener la paginación
                var pagedList = await PagedList<Product>.ToPagedList(query, productParams.PageNumber, productParams.PageSize = 20);

                // Establecer los encabezados de paginación
                Response.AddPaginationHeader(pagedList.Metadata);

                // Crear la respuesta
                var response = new ApiResponse<IEnumerable<ProductDtoAdmin>>(
                    true,
                    "Productos obtenidos correctamente.",
                    pagedList.Select(p => p.MapToProductDtoAdmin()));

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los productos.");
                return NotFound(new ApiResponse<ProductDtoAdmin>(false, "No se encontraron productos."));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductDtoAdmin>>> GetProduct(int id)
        {
            try
            {
                var product = await _context.ProductRepository.GetProductByIdAsync(id);
                var response = new ApiResponse<ProductDtoAdmin>(
                    true,
                    "Producto encontrado correctamente.",
                    product.MapToProductDtoAdmin());
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el producto mediante el título de este.");
                return NotFound(new ApiResponse<ProductDtoAdmin>(false, "El producto no fue encontrado."));
            }
        }

        [HttpPost]
        // Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ApiResponse<ProductDtoAdmin>>> AddProduct([FromForm] CreateProductDto createProductDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<ProductDtoAdmin>(false, "Error en los datos de entrada.", null, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
                }

                var urls = new List<ProductImage>();

                foreach (var image in createProductDto.Images)
                {
                    var uploadResult = await _photoService.AddPhotoAsync(image);
                    if (uploadResult.Error != null)
                    {
                        return BadRequest(new ApiResponse<ProductDtoAdmin>(false, "Error al subir la imagen.", null, new List<string> { uploadResult.Error.Message }));
                    }

                    urls.Add(new ProductImage { Url = uploadResult.SecureUrl.AbsoluteUri, PublicId = uploadResult.PublicId });
                }

                var product = ProductMapper.MapToProduct(createProductDto, urls);

                await _context.ProductRepository.AddProductAsync(product);
                await _context.SaveChangesAsync();

                var response = new ApiResponse<ProductDtoAdmin>(
                    true,
                    "Producto agregado correctamente.",
                    product.MapToProductDtoAdmin());

                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el producto.");
                return BadRequest(new ApiResponse<ProductDtoAdmin>(false, "Error al agregar el producto.", null, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }
        }

        [HttpDelete("{id}")]
        // [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ApiResponse<ProductDtoAdmin>>> RemoveProduct(int id)
        {
            try
            {
                var product = await _context.ProductRepository.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new ApiResponse<ProductDtoAdmin>(false, "El producto no fue encontrado."));
                }
                // Lógica para verificar si el producto tiene órdenes asociadas
                // if (product.Orders.Count > 0) 
                // {
                //     // Cambiar el estado del producto a no visible
                //     await _context.ProductRepository.RemoveProductAsync(product);
                //     return Ok(new ApiResponse<ProductDtoAdmin>(
                //         true, 
                //         "Producto removido correctamente, pero tiene órdenes asociadas. Se ha cambiado su estado a no visible.", 
                //         product.MapToProductDtoAdmin()));
                // }

                // Si no tiene órdenes asociadas, eliminar el producto
                _context.ProductRepository.DeleteProduct(product);
                await _context.SaveChangesAsync();
                var response = new ApiResponse<ProductDtoAdmin>(
                    true,
                    "Producto eliminado correctamente.",
                    product.MapToProductDtoAdmin());

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el producto.");
                return BadRequest(new ApiResponse<ProductDtoAdmin>(false, "Error al eliminar el producto.", null, [ex.Message]));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<ProductDtoAdmin>>> UpdateProduct(int id, [FromForm] UpdateProductDto updateProductDto)
        {
            try
            {
                // Validar el modelo
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<ProductDtoAdmin>(false, "Error en los datos de entrada.", null, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
                }

                // Obtener el producto existente
                var product = await _context.ProductRepository.GetProductByIdAsync(id);

                // Verificar si el producto existe
                if (product == null)
                {
                    return NotFound(new ApiResponse<ProductDtoAdmin>(false, "El producto no fue encontrado."));
                }

                if (updateProductDto.ImagesToDelete != null && updateProductDto.ImagesToDelete.Any())
                {
                    foreach (var url in product.ProductImages.Select(i => i.Url))
                    {
                        if (updateProductDto.ImagesToDelete.Contains(url))
                        {
                            var publicId = CloudinaryHelper.ExtractPublicIdFromUrl(url);
                            if (!string.IsNullOrEmpty(publicId))
                            {
                                await _photoService.DeletePhotoAsync(publicId);
                            }
                        } 
                    }
                }

                if (updateProductDto.ImagesToAdd != null && updateProductDto.ImagesToAdd.Count > 0)
                {
                    foreach (var image in updateProductDto.ImagesToAdd)
                    {
                        var uploadResult = await _photoService.AddPhotoAsync(image);
                        if (uploadResult.Error != null)
                        {
                            return BadRequest(new ApiResponse<ProductDtoAdmin>(false, "Error al subir la imagen.", null, [uploadResult.Error.Message]));
                        }

                        product.ProductImages.Add(new ProductImage { Url = uploadResult.SecureUrl.AbsoluteUri, PublicId = uploadResult.PublicId });
                    }
                }

                product.Title = updateProductDto.Title ?? product.Title;
                product.Description = updateProductDto.Description ?? product.Description;
                product.Price = updateProductDto.Price ?? product.Price;
                product.Stock = updateProductDto.Stock ?? product.Stock;
                product.Category = updateProductDto.Category ?? product.Category;
                product.Brand = updateProductDto.Brand ?? product.Brand;
                product.ProductCondition = updateProductDto.Condition ?? product.ProductCondition;

                _context.ProductRepository.UpdateProduct(product);
                await _context.SaveChangesAsync();
                var response = new ApiResponse<ProductDtoAdmin>(
                    true,
                    "Producto actualizado correctamente.",
                    product.MapToProductDtoAdmin());

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el producto.");
                return BadRequest(new ApiResponse<ProductDtoAdmin>(false, "Error al actualizar el producto.", null, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }
        }
    }
}