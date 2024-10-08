﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEccomerce.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductTypeController : ControllerBase
	{
		private readonly IProductTypeService _productTypeService;

        public ProductTypeController(IProductTypeService productTypeService)
        {
            _productTypeService = productTypeService;
        }

		[HttpGet/*, Authorize(Roles = "Admin")*/]
		public async Task<ActionResult<ServiceResponse<List<ProductType>>>> GetProductTypes()
		{
			var response = await _productTypeService.GetProductTypes();
			return Ok(response);
		}

		[HttpPost/*, Authorize(Roles = "Admin")*/]
		public async Task<ActionResult<ServiceResponse<List<ProductType>>>> AddProductType(ProductType productType)
		{
			var response = await _productTypeService.AddProductType(productType);
			return Ok(response);
		}

		[HttpPut/*, Authorize(Roles = "Admin")*/]
		public async Task<ActionResult<ServiceResponse<List<ProductType>>>> UpdateProductType(ProductType productType)
		{
			var response = await _productTypeService.UpdateProductType(productType);
			return Ok(response);
		}
	}
}
