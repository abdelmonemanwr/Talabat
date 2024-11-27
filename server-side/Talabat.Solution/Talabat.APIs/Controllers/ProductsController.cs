using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Talabat.Domain.Layer.Entities;
using Talabat.Domain.Layer.IRepositories;
using Talabat.Domain.Layer.Specifications;
using System.Linq.Expressions;
using Talabat.APIs.DTOs;
using Talabat.APIs.Helpers;
using AutoMapper;
namespace Talabat.APIs.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductType> _typeRepository;
        private readonly IGenericRepository<ProductBrand> _brandRepository;

        public ProductsController(IGenericRepository<Product> productRepository, IGenericRepository<ProductBrand> brandRepository, IGenericRepository<ProductType> typeRepository, IMapper mapper)
        {
            _mapper = mapper;
            _typeRepository = typeRepository;
            _brandRepository = brandRepository;
            _productRepository = productRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts([FromQuery]ProductSpecificationParams productSpecificationParams)
        {
            var productsSpecifications = new ProductWithBrandAndTypeSpecifications(productSpecificationParams);

            var products = await _productRepository.GetAllAsync(productsSpecifications);

            var count = await _productRepository.GetCountAsync(
                new ProductWithBrandAndTypeSpecifications(
                    new ProductSpecificationParams {
                        BrandId = productSpecificationParams.BrandId,
                        TypeId = productSpecificationParams.TypeId,
                        Search = productSpecificationParams.Search,
                    }
                )
             );

            var result = new Pagination<ProductDTO>
            {
                PageIndex = productSpecificationParams.PageIndex,
                PageSize = productSpecificationParams.PageSize,
                Count = count,
                Data = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products),
            };

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var specifications = new ProductWithBrandAndTypeSpecifications(id);

            var product = await _productRepository.GetByIdAsync(specifications);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            return Ok(_mapper.Map<ProductDTO>(product));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<ProductBrand>>> GetAllBrands()
        {
            return Ok(await _brandRepository.GetAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<ProductType>>> GetAllTypes()
        {
            return Ok(await _typeRepository.GetAllAsync());
        }
    }
}
