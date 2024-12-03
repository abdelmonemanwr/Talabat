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
using Microsoft.AspNetCore.Authorization;
namespace Talabat.APIs.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork unit;
        private readonly IMapper mapper;

        public ProductsController(IUnitOfWork unit, IMapper mapper)
        {
            this.unit = unit;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDTO>>> GetAllProducts([FromQuery]ProductSpecificationParams productSpecificationParams)
        {
            var productsSpecifications = new ProductWithBrandAndTypeSpecifications(productSpecificationParams);

            var products = await unit.Repository<Product>().GetAllAsync(productsSpecifications);

            var count = await unit.Repository<Product>().GetCountAsync(
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
                Data = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDTO>>(products),
            };

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var specifications = new ProductWithBrandAndTypeSpecifications(id);

            var product = await unit.Repository<Product>().GetByIdAsync(specifications);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            return Ok(mapper.Map<ProductDTO>(product));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrands()
        {
            return Ok(await unit.Repository<ProductBrand>().GetAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetAllTypes()
        {
            return Ok(await unit.Repository<ProductType>().GetAllAsync());
        }
    }
}
