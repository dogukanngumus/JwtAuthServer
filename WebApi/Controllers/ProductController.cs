using Core.Dtos;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
public class ProductController : BaseController
{
     private readonly IProductService _productService;

     public ProductController(IProductService productService)
     {
          _productService = productService;
     }

     [HttpGet]
     public async Task<IActionResult> GetProducts()
     {
          return ActionResultInstance(await _productService.GetAllAsync());
     }
     
     [HttpPost]
     public async Task<IActionResult> SaveProduct(ProductDto productDto)
     {
          return ActionResultInstance(await _productService.AddAsync(productDto));
     }

     [HttpPut]
     public async Task<IActionResult> UpdateProduct(ProductDto productDto)
     {
          return ActionResultInstance(await _productService.Update(productDto, productDto.Id));
     }

     [HttpDelete]
     public async Task<IActionResult> DeleteProduct(int id)
     {
          return ActionResultInstance(await _productService.Remove(id));
     }

}