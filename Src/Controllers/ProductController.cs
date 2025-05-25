using Microsoft.AspNetCore.Authorization;
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
            _logger.LogInformation("Solicitud para obtener productos recibida. Parámetros: {@ProductParams}", productParams);
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

                _logger.LogInformation("Productos obtenidos correctamente. Total: {Count}", pagedList.Count);
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductDtoAdmin>>>> GetAllProducts([FromQuery] ProductParams productParams)
        {
            _logger.LogInformation("Solicitud para obtener productos (vista admin) recibida. Parámetros: {@ProductParams}", productParams);
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

                _logger.LogInformation("Productos (vista admin) obtenidos correctamente. Total: {Count}", pagedList.Count);
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
            _logger.LogInformation("Solicitud para obtener producto por ID: {Id}", id);
            try
            {
                var product = await _context.ProductRepository.GetProductByIdAsync(id);
                var response = new ApiResponse<ProductDtoAdmin>(
                    true,
                    "Producto encontrado correctamente.",
                    product.MapToProductDtoAdmin());
                _logger.LogInformation("Producto con ID {Id} encontrado correctamente.", id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el producto mediante el id de este.");
                return NotFound(new ApiResponse<ProductDtoAdmin>(false, "El producto no fue encontrado."));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ProductDtoAdmin>>> AddProduct([FromForm] CreateProductDto createProductDto)
        {
            _logger.LogInformation("Solicitud para agregar producto recibida. Título: {Title}", createProductDto.Title);
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Error de validación al agregar producto: {@Errors}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return BadRequest(new ApiResponse<ProductDtoAdmin>(false, "Error en los datos de entrada.", null, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
                }

                var urls = new List<ProductImage>();

                foreach (var image in createProductDto.Images)
                {
                    var uploadResult = await _photoService.AddPhotoAsync(image);
                    if (uploadResult.Error != null)
                    {
                        _logger.LogWarning("Error al subir la imagen: {Error}", uploadResult.Error.Message);
                        return BadRequest(new ApiResponse<ProductDtoAdmin>(false, "Error al subir la imagen.", null, new List<string> { uploadResult.Error.Message }));
                    }

                    _logger.LogInformation("Imagen subida correctamente: {Url}", uploadResult.SecureUrl.AbsoluteUri);
                    urls.Add(new ProductImage { Url = uploadResult.SecureUrl.AbsoluteUri, PublicId = uploadResult.PublicId });
                }

                var product = ProductMapper.MapToProduct(createProductDto, urls);

                await _context.ProductRepository.AddProductAsync(product);
                await _context.SaveChangesAsync();

                var response = new ApiResponse<ProductDtoAdmin>(
                    true,
                    "Producto agregado correctamente.",
                    product.MapToProductDtoAdmin());

                _logger.LogInformation("Producto agregado correctamente. ID: {Id}", product.Id);
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el producto.");
                return BadRequest(new ApiResponse<ProductDtoAdmin>(false, "Error al agregar el producto.", null, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ProductDtoAdmin>>> RemoveProduct(int id)
        {
            _logger.LogInformation("Solicitud para eliminar producto recibida. ID: {Id}", id);
            try
            {
                var product = await _context.ProductRepository.GetProductByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Intento de eliminar producto no encontrado. ID: {Id}", id);
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

                // Eliminar todas las imágenes del producto de Cloudinary
                foreach (var url in product.ProductImages.Select(x => x.Url))
                {
                    var oldPublicId = CloudinaryHelper.ExtractPublicIdFromUrl(url);
                    if (!string.IsNullOrEmpty(oldPublicId))
                    {
                        await _photoService.DeletePhotoAsync(oldPublicId);
                    }
                }

                _logger.LogInformation("Imágenes del producto eliminadas de Cloudinary.");
                // Si no tiene órdenes asociadas, eliminar el producto
                await _context.ProductRepository.DeleteProduct(product);
                await _context.SaveChangesAsync();
                var response = new ApiResponse<ProductDtoAdmin>(
                    true,
                    "Producto eliminado correctamente.",
                    product.MapToProductDtoAdmin());

                _logger.LogInformation("Producto eliminado correctamente. ID: {Id}", id);
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
            _logger.LogInformation("Solicitud para actualizar producto recibida. ID: {Id}", id);
            try
            {
                // Validar el modelo
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Error de validación al actualizar producto: {@Errors}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return BadRequest(new ApiResponse<ProductDtoAdmin>(false, "Error en los datos de entrada.", null, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
                }

                // Obtener el producto existente
                var product = await _context.ProductRepository.GetProductByIdAsync(id);

                // Verificar si el producto existe
                if (product == null)
                {
                    _logger.LogWarning("Intento de actualizar producto no encontrado. ID: {Id}", id);
                    return NotFound(new ApiResponse<ProductDtoAdmin>(false, "El producto no fue encontrado."));
                }

                // Si se sube una nueva imagen, elimina la anterior y actualiza la URL
                if (updateProductDto.Images != null && updateProductDto.Images.Count > 0)
                {
                    // Elimina todas las imágenes anteriores de Cloudinary y de la base de datos
                    foreach (var url in product.ProductImages.Select(x => x.Url))
                    {
                        var oldPublidId = CloudinaryHelper.ExtractPublicIdFromUrl(url);
                        if (!string.IsNullOrEmpty(oldPublidId))
                        {
                            await _photoService.DeletePhotoAsync(oldPublidId);
                        }
                    }

                    _logger.LogInformation("Imágenes del producto eliminadas de Cloudinary.");

                    // Elimina las imágenes de la base de datos
                    await _context.ProductRepository.RemoveAllProductsImagesAsync(product);

                    _logger.LogInformation("Imágenes del producto eliminadas de la base de datos.");

                    // Sube la nueva imagen y actualiza la URL en la base de datos
                    foreach (var image in updateProductDto.Images)
                    {
                        var uploadResult = await _photoService.AddPhotoAsync(image);
                        if (uploadResult.Error != null)
                        {
                            _logger.LogWarning("Error al subir la imagen durante actualización: {Error}", uploadResult.Error.Message);
                            return BadRequest(new ApiResponse<ProductDtoAdmin>(
                                false,
                                "Error al subir la imagen.",
                                null,
                                new List<string> { uploadResult.Error.Message }));
                        }

                        product.ProductImages.Add(new ProductImage
                        {
                            Url = uploadResult.SecureUrl.AbsoluteUri,
                            PublicId = uploadResult.PublicId
                        });
                    }
                    _logger.LogInformation("Nuevas imágenes subidas correctamente.");
                }

                product.Title = updateProductDto.Title ?? product.Title;
                product.Description = updateProductDto.Description ?? product.Description;
                product.Price = updateProductDto.Price ?? product.Price;
                product.Stock = updateProductDto.Stock ?? product.Stock;
                product.Category = updateProductDto.Category ?? product.Category;
                product.Brand = updateProductDto.Brand ?? product.Brand;
                product.ProductCondition = updateProductDto.Condition ?? product.ProductCondition;
                product.UpdatedAt = DateTime.UtcNow;

                await _context.ProductRepository.UpdateProductAsync(product);
                await _context.SaveChangesAsync();
                var response = new ApiResponse<ProductDtoAdmin>(
                    true,
                    "Producto actualizado correctamente.",
                    product.MapToProductDtoAdmin());

                _logger.LogInformation("Producto actualizado correctamente. ID: {Id}", id);
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