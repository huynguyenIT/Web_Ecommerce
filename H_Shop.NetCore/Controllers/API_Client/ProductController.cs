﻿using DataAndServices.Admin_Services.Products;
using DataAndServices.Client_Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace H_Shop.NetCore.Controllers.API_Client
{
    [ApiController]
    [Route("api/Product")]   
    public class ProductController : ControllerBase
    {
        private readonly IProductClientServices _productClientService;
        private readonly IProductService _productAdminService;
        public ProductController(IProductClientServices productClientServices,IProductService productService)
        {
            _productClientService = productClientServices;
            _productAdminService = productService;
        }

        [HttpGet]
        [Route("GetAllproducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var listPro= await _productClientService.GetAllProducts();
            return Ok(listPro);
        }

        [HttpGet]
        [Route("GetAllProductByPrice/{giaMin:int}/{giaMax:int}")]
        public IActionResult GetAllProductByPrice(int giaMin, int giaMax)
        {
           var listProByPrice= _productClientService.GetAllProductByPrice(giaMin, giaMax);
            return Ok(listProByPrice);
        }
      
        [HttpGet]
        [Route("GetAllProductByName/{name}")]
        public IActionResult GetAllProductByName(string name)
        {
           var proByName=  _productClientService.GetAllProductByName(name);
            return Ok(proByName);
        }

        [HttpGet]
        [Route("GetAllProductItemByPageList")]
        public IActionResult GetAllProductItemByPageList()
        {
           var listProItemByPage=  _productAdminService.GetProductItemByPageList();
            return Ok(listProItemByPage);
        }

        [HttpGet]
        [Route("GetAllProductItem")]
        public async Task<IActionResult> GetAllProductItem()
        {
            var listProItem= await _productAdminService.GetAllProductItem();
            return Ok(listProItem);
        }

        [HttpGet]
        [Route("GetAllProductByIdItemClient/{id}")]
        public IActionResult GetAllProductByIdItem(int id)
        {
            var proItemById =  _productAdminService.GetProductItemById_client(id);
            return Ok(proItemById);
           
        }

        [HttpGet]
        [Route("GetProductItemById/{Id}")]
        public IActionResult GetProductItemById(string Id)
        {
            var proItemById=  _productAdminService.GetProductItemById(Id);
            return Ok(proItemById);
        }

        [HttpGet]
        [Route("GetSoLuong/{Id}")]
        public int GetSoLuong(string Id)
        {
            return  _productClientService.GetSoLuong(Id);
        }
    }
}