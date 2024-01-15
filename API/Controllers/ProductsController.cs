/* The code provided is a C# class that represents a controller for handling HTTP requests related to
products in an API. */
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;
using API.Errors;
using API.Helpers;

namespace API.Controllers
{

    public class ProductsController : BaseApiController
    {

        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productsRepo,
         IGenericRepository<ProductBrand> productBrandRepo,
          IGenericRepository<ProductType> productTypeRepo,
          IMapper mapper)
        {
            _mapper = mapper;
            _productTypeRepo = productTypeRepo;
            _productBrandRepo = productBrandRepo;
            _productsRepo = productsRepo;



        }
        /// <summary>
        /// The function retrieves a list of products based on specified parameters, including
        /// pagination, and returns the result along with the total count of items.
        /// </summary>
        /// <param name="ProductSpecParams">ProductSpecParams is a class that contains various
        /// parameters for filtering and sorting products. It is used to specify the criteria for
        /// retrieving a list of products from the database.</param>
        /// <returns>
        /// The method is returning an ActionResult of type Pagination<ProductsToReturnDto>.
        /// </returns>
        [HttpGet]
        
        public async Task<ActionResult<Pagination<ProductsToReturnDto>>> GetProducts([FromQuery]ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var products = await _productsRepo.ListAsync(spec);
            var countSpec = new ProductsWithFiltersForCountSpecification(productParams);

            var totalItems = await _productsRepo.CountAsync(countSpec);
            var data = _mapper.Map<IReadOnlyList<ProductsToReturnDto>>(products);

            return Ok(new Pagination<ProductsToReturnDto>(productParams.PageIndex,
                productParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductsToReturnDto>> GetProduct(int id)
        {
            var spec=new ProductsWithTypesAndBrandsSpecification(id);
            var product= await _productsRepo.GetEntityWithSpec(spec);
            if (product == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product,ProductsToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandRepo.ListAllAsync());
        }

        /// <summary>
        /// The above function is an HTTP GET endpoint that returns a list of product types.
        /// </summary>
        /// <returns>
        /// The method is returning an ActionResult containing an IReadOnlyList of ProductType objects.
        /// </returns>
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _productTypeRepo.ListAllAsync());
        }
        
      
    }
}