using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Talabat.Domain.Layer.Entities;
using Talabat.Domain.Layer.IRepositories;
using Talabat.Domain.Layer.Specifications;
using System.Linq.Expressions;
using Talabat.APIs.DTOs;
using AutoMapper;
namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            //return Ok(await _productRepository.GetAllAsync());

            var specifications = new ProductWithBrandAndTypeSpecifications();
            
            var products = await _productRepository.GetAllAsync(specifications);

            //var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);
            var productsDTO = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);

            return Ok(productsDTO);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            //return Ok(await _productRepository.GetByIdAsync(id));

            var specifications = new ProductWithBrandAndTypeSpecifications(id);

            var product = await _productRepository.GetByIdAsync(specifications);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            //var productDTO = _mapper.Map<Product, ProductDTO>(product);
            var productDTO = _mapper.Map<ProductDTO>(product);

            return Ok(productDTO);
        }
    }
}
