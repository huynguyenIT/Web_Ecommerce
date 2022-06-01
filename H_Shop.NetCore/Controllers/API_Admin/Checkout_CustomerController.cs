﻿using DataAndServices.Admin_Services.Checkout_Customer_Services;
using DataAndServices.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace H_Shop.NetCore.Controllers.API_Admin
{
    [Route("api/Checkout_Customer")]
    [ApiController]
    public class Checkout_CustomerController : ControllerBase
    {
        private readonly ICheckoutCustomerService _checkoutCustomerService;

        public Checkout_CustomerController(ICheckoutCustomerService checkoutCustomerService)
        {
            _checkoutCustomerService = checkoutCustomerService;
        }

        [HttpPost]
        [Route("Update")]
        public  bool Update(CheckoutCustomerOrder dTO_Account)
        {
            return _checkoutCustomerService.Update_Ad_acc(dTO_Account);
        }

        [HttpDelete]
        [Route("DeleteCustomer/{id}")]
        public async Task<bool> DeleteCustomer(string id)
        {
            return await _checkoutCustomerService.DeleteAccount(id);
        }

        [HttpGet]
        [Route("GetAllCustomer/{userLogin}")]
        public IActionResult GetAllCustomer(string userLogin)
        {   
             var listAccount=  _checkoutCustomerService.GetAllAccounts(userLogin);
            return Ok(listAccount);

            
        }

        [HttpGet]
        [Route("GetMonthlyRevenue/{monthDate}")]
        public IActionResult GetMonthlyRevenue(string monthDate)
        {
            var listMonthlyRevenue = _checkoutCustomerService.GetMonthlyRevenue(monthDate);
            return Ok(listMonthlyRevenue);

        }

        [HttpGet]
        [Route("GetDateRevenue")]
        public IActionResult GetDateRevenue(DateTime date )
        {
            var listMonthlyRevenue = _checkoutCustomerService.GetDateRevenue(date);
            return Ok(listMonthlyRevenue);

        }

        //[HttpGet]
        //[Route("GetAllAccounts2")]
        //public JsonResult<List<DTO_Account_Role>> GetAllAccounts2()
        //{
        //    return Json<List<DTO_Account_Role>>(_checkoutCustomerService.GetAllAccounts2());
        //}

        [HttpGet]
        [Route("GetCustomerById/{Id}")]
        public async Task<IActionResult> GetCustomerById(string Id)
        {
            var listAccount = await _checkoutCustomerService.GetAccountById(Id);
            return Ok(listAccount);  
        }

        [HttpGet]
        [Route("GetListCustomerById/{Id}")]
        public  async Task<IActionResult> GetListCustomerById(string Id)
        {
            var listAccount = await _checkoutCustomerService.GetListAccountById(Id);
        
            return Ok(listAccount);
            
        }
    }
}
