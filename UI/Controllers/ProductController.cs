﻿using Model.DTO.DTO_Ad;
using Model.DTO.DTO_Client;
using Model.DTO_Model;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using UI.Service;

namespace UI.Controllers
{
    public class ProductController : Controller
    {
        ServiceRepository service = new ServiceRepository();

        public ActionResult TypeProduct()
        {
            HttpResponseMessage responseMessage = service.GetResponse("api/Products_Ad/GetAllProductByType");
            responseMessage.EnsureSuccessStatusCode();
            List<List<DTO_Dis_Product>> dTO_Accounts = responseMessage.Content.ReadAsAsync<List<List<DTO_Dis_Product>>>().Result;
            var view = dTO_Accounts.ToPagedList(1, 50);

            return View(view);
        }
        public ActionResult Index(FormCollection fc, int? page, int? gia, int? gia_, string searchName1)
        {
            var searchName = fc["searchName"];
            var priceGiaMin = Request.Form["priceGiaMin"];
            var priceGiaMax = Request.Form["priceGiaMax"];

            if (page == null) page = 1;
            int pageSize = 25;

            int pageNumber = (page ?? 1);

            if ((searchName != null && searchName != "") || (searchName1 != "" && searchName1 != null))
            {
                try
                {
                    HttpResponseMessage responseMessage2 = service.GetResponse("api/product/GetAllProductByName/" + searchName);
                    responseMessage2.EnsureSuccessStatusCode();

                    List<DTO_Dis_Product> dTO_Accounts2 = responseMessage2.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;
                    return View(dTO_Accounts2.ToPagedList(pageNumber, pageSize));
                }
                catch
                {
                    HttpResponseMessage responseMessage3 = service.GetResponse("api/product/GetAllProductByName/" + searchName1);
                    responseMessage3.EnsureSuccessStatusCode();

                    List<DTO_Dis_Product> dTO_Accounts3 = responseMessage3.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;
                    return View(dTO_Accounts3.ToPagedList(pageNumber, pageSize));

                }
            }
            if (priceGiaMin != null && priceGiaMax != null && priceGiaMin != "" && priceGiaMax != "")
            {
                HttpResponseMessage responseMessage2 = service.GetResponse("api/product/GetAllProductByPrice/" + priceGiaMin + "/" + priceGiaMax);
                responseMessage2.EnsureSuccessStatusCode();

                List<DTO_Dis_Product> dTO_Accounts2 = responseMessage2.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;
                return View(dTO_Accounts2.ToPagedList(pageNumber, pageSize));
            }
            try
            {

                HttpResponseMessage responseMessage2 = service.GetResponse("api/product/GetAllProductByPrice/" + gia_ + "/" + gia);
                responseMessage2.EnsureSuccessStatusCode();

                List<DTO_Dis_Product> dTO_Accounts2 = responseMessage2.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;
                return View(dTO_Accounts2.ToPagedList(pageNumber, pageSize));
            }
            catch
            {
                HttpResponseMessage responseMessage = service.GetResponse("api/Products_Ad/GetAllProduct_Discount");
                responseMessage.EnsureSuccessStatusCode();
                List<DTO_Dis_Product> dTO_Accounts = responseMessage.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;
                return View(dTO_Accounts.ToPagedList(pageNumber, pageSize));

            }
        }
        public ActionResult Index_(DTO_Dis_Product dTO_Product, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 25;
            int pageNumber = (page ?? 1);

            HttpResponseMessage responseMessage = service.GetResponse("api/Products_Ad/GetAllProduct_Discount");
            responseMessage.EnsureSuccessStatusCode();
            List<DTO_Dis_Product> dTO_Accounts = responseMessage.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;

            return View(dTO_Accounts.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Search(int? page, string searchName)
        {
            if (page == null) page = 1;
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            HttpResponseMessage responseMessage2 = service.GetResponse("api/product/GetAllProductByName/" + searchName);
            responseMessage2.EnsureSuccessStatusCode();
            List<DTO_Product_Client> dTO_Accounts2 = responseMessage2.Content.ReadAsAsync<List<DTO_Product_Client>>().Result;

            return View(dTO_Accounts2.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Index2(int id, string seachBy, string search, int? page, int? gia, int? gia_)
        {

            if (page == null) page = 1;

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            if (seachBy == "NameProduct")
            {
                //return View(db.Products.Where(s => s.Name.StartsWith(search)).ToList().ToPagedList(pageNumber, pageSize));
                HttpResponseMessage responseMessage2 = service.GetResponse("api/product/GetAllProductByName/" + search);
                responseMessage2.EnsureSuccessStatusCode();
                List<DTO_Dis_Product> dTO_Accounts2 = responseMessage2.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;

                return View(dTO_Accounts2.ToPagedList(pageNumber, pageSize));
            }
            if (seachBy == "Price")
            {

                HttpResponseMessage responseMessage2 = service.GetResponse("api/product/GetAllProductByPrice/" + gia_ + "/" + gia);
                responseMessage2.EnsureSuccessStatusCode();
                List<DTO_Dis_Product> dTO_Accounts2 = responseMessage2.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;

                return View(dTO_Accounts2.ToPagedList(pageNumber, pageSize));

            }
            HttpResponseMessage responseMessage = service.GetResponse("api/Products_Ad/GetAllProductByIdItem/" + id);
            responseMessage.EnsureSuccessStatusCode();
            List<DTO_Dis_Product> dTO_Accounts = responseMessage.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;
            if (dTO_Accounts == null)
            {
                return Content("Chưa có sản phẩm bạn đang muốn tìm kiếm");
            }

            var view = dTO_Accounts.ToPagedList(1, 10);
            return View(view);

        }
        public ActionResult Details()
        {

            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart"];
            if (cart == null)
            {
                return View("~/Cart/Thankyou1");
            }
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Create(FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public PartialViewResult BagCart()
        {

            try
            {
                if (Session["cart"] != null)
                {
                    List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart"];

                    int total = cart.Count();
                    ViewBag.Quantity = total;

                }
            }
            catch
            {

                ViewBag.Quantity = 0;

            }
            return PartialView("BagCart");
        }
        public PartialViewResult BagCart_()
        {
            try
            {
                if (Session["cart_"] != null)
                {
                    List<DTO_Product_Item_Type> cart_ = (List<DTO_Product_Item_Type>)Session["cart_"];
                    int total1 = cart_.Count();
                    ViewBag.yeuthich = total1;
                }
            }
            catch
            {

                ViewBag.yeuthich = 0;

            }
            return PartialView("BagCart_");
        }

        private int isExist(string Id)
        {
            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart"];

            for (int i = 0; i < cart.Count; i++)
                if (cart[i]._id.Equals(Id))
                    return i;
            return -1;
        }

        private int isExist2(string Id)
        {
            HttpResponseMessage response2 = service.GetResponse("api/Product/GetSoLuong/" + Id);
            response2.EnsureSuccessStatusCode();
            int quantity2 = response2.Content.ReadAsAsync<int>().Result;
            if (quantity2 <= 0)
            {
                return 0;
            }
            return 1;
        }
        public ActionResult Remove(string Id)
        {
            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart"];
            int index = isExist(Id);
            cart.RemoveAt(index);
            Session["cart"] = cart;
            return RedirectToAction("Details");
        }



        public ActionResult Buy(string Id)
        {
            int checkBuy = CheckBuy(Id);
            if (checkBuy == 0)
            {
                return RedirectToAction("HetHang", "Cart");
            }
            return RedirectToAction("Details", "Product");



        }
        [HttpPost]
        public ActionResult Buy_Favorite(string Id)
        {
            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart"];
            int checkBuy = CheckBuy(Id);
            if (checkBuy == 0)
            {
                string message = (" Sản phẩm đã hết hàng");
                return Json(new { buy = 0, status = message });
            }
            if (checkBuy == 1)
            {
                return Json(new { buy = cart, status = "Thành công" });
            }

            return Json(new { buy = cart, status = "Thành công" });



        }

        public ActionResult Giam(string Id, DTO_Product_Item_Type item1)
        {

            List<DTO_Product_Item_Type> li = (List<DTO_Product_Item_Type>)Session["cart"];
            HttpResponseMessage responseUser = service.GetResponse("api/Products_Ad/GetProductItemById/" + Id);
            responseUser.EnsureSuccessStatusCode();
            DTO_Product_Item_Type proItem = responseUser.Content.ReadAsAsync<DTO_Product_Item_Type>().Result;

            int index = isExist(Id);
            if (index != -1)
            {
                if (li[index].Quantity - 1 > 0)
                {
                    li[index].Quantity--;
                    li[index].Price = proItem.Price * li[index].Quantity;
                }

            }

            Session["cart"] = li;
            return Json(new { soLuong = li });

        }
        public ActionResult Update(string Id, FormCollection fc)
        {

            List<DTO_Product_Item_Type> li = (List<DTO_Product_Item_Type>)Session["cart"];
            HttpResponseMessage responseUser = service.GetResponse("api/Products_Ad/GetProductItemById/" + Id);
            responseUser.EnsureSuccessStatusCode();
            DTO_Product_Item_Type proItem = responseUser.Content.ReadAsAsync<DTO_Product_Item_Type>().Result;

            int index = isExist(Id);
            if (index != -1)
            {
                li[index].Quantity++;
            }
            else
            {
                li.Add(new DTO_Product_Item_Type()
                {
                    _id = proItem._id,
                    Name = proItem.Name,
                    Price = proItem.Price,
                    Details = proItem.Details,
                    Photo = proItem.Photo,
                    Id_Item = proItem.Id_Item,
                    Quantity = 1
                });

            }
            Session["cart"] = li;
            return RedirectToAction("Details", "Product");
        }
        public ActionResult Tang(string Id, DTO_Product_Item_Type item1)
        {
            int checkBoy = CheckBuy(Id);
            if (checkBoy == 0)
            {
                string message = (" Sản phẩm này đã vượt quá số lượng");
                return Json(new { soLuong = 0, status = message });
            }
            List<DTO_Product_Item_Type> li = (List<DTO_Product_Item_Type>)Session["cart"];
            HttpResponseMessage responseUser = service.GetResponse("api/Products_Ad/GetProductItemById/" + Id);
            responseUser.EnsureSuccessStatusCode();
            DTO_Product_Item_Type proItem = responseUser.Content.ReadAsAsync<DTO_Product_Item_Type>().Result;
            int index = isExist(Id);
            if (index != -1)
            {

                li[index].Price = proItem.Price * li[index].Quantity;
            }
            Session["cart"] = li;
            return Json(new { soLuong = li });

        }

        public ActionResult Details1(string Id)
        {
            if (Session["cart__"] == null)
            {
                List<DTO_Product_Item_Type> li = (List<DTO_Product_Item_Type>)Session["cart"];
                HttpResponseMessage responseUser = service.GetResponse("api/Products_Ad/GetProductItemById/" + Id);
                responseUser.EnsureSuccessStatusCode();
                DTO_Product_Item_Type proItem = responseUser.Content.ReadAsAsync<DTO_Product_Item_Type>().Result;

                //var product = db.Products.Find(Id);

                new DTO_Product_Item_Type()
                {
                    _id = proItem._id,
                    Name = proItem.Name,
                    Price = proItem.Price,
                    Details = proItem.Details,
                    Photo = proItem.Photo,
                    Id_Item = proItem.Id_Item,
                    Quantity = 1
                };
                Session["cart__"] = proItem;
            }
            else
            {
                List<DTO_Product_Item_Type> li = (List<DTO_Product_Item_Type>)Session["cart"];
                HttpResponseMessage responseUser = service.GetResponse("api/Products_Ad/GetProductItemById/" + Id);
                responseUser.EnsureSuccessStatusCode();
                DTO_Product_Item_Type proItem = responseUser.Content.ReadAsAsync<DTO_Product_Item_Type>().Result;

                new DTO_Product_Item_Type()
                {
                    _id = proItem._id,
                    Name = proItem.Name,
                    Price = proItem.Price,
                    Details = proItem.Details,
                    Photo = proItem.Photo,
                    Id_Item = proItem.Id_Item,
                    Quantity = 1
                };
                Session["cart__"] = proItem;
            }
            return RedirectToAction("LuaChon", "Cart");
        }
        public PartialViewResult Search()
        {
            return PartialView("Search");
        }      

        public int CheckBuy(string Id)
        {
            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart"];
            if (cart != null)
            {
                foreach (var item in cart)
                {
                    if (item._id == Id)
                    {
                        HttpResponseMessage response = service.GetResponse("api/Product/GetSoLuong/" + item._id);

                        response.EnsureSuccessStatusCode();
                        int quantity = response.Content.ReadAsAsync<int>().Result;
                        int quantityAfterBuy = quantity - (int)item.Quantity;
                        if (quantityAfterBuy <= 0)
                        {
                            return 0;
                        }
                    }
                }
                HttpResponseMessage response2 = service.GetResponse("api/Product/GetSoLuong/" + Id);
                response2.EnsureSuccessStatusCode();
                int quantity2 = response2.Content.ReadAsAsync<int>().Result;
                if (quantity2 <= 0)
                {
                    return 0;
                }
                List<DTO_Product_Item_Type> li = (List<DTO_Product_Item_Type>)Session["cart"];
                HttpResponseMessage responseUser = service.GetResponse("api/Products_Ad/GetProductItemById/" + Id);
                responseUser.EnsureSuccessStatusCode();
                DTO_Product_Item_Type proItem = responseUser.Content.ReadAsAsync<DTO_Product_Item_Type>().Result;

                int index = isExist(Id);
                if (index != -1)
                {
                    li[index].Quantity++;
                    Session["cart"] = li;
                    return 1;
                }
                else
                {
                    li.Add(new DTO_Product_Item_Type()
                    {

                        Quantity = 1,
                        _id = proItem._id,
                        Name = proItem.Name,
                        Price = proItem.Price,
                        Details = proItem.Details,
                        Photo = proItem.Photo,
                        Id_Item = proItem.Id_Item,
                    });
                    return 2;
                }
            }
            else
            {
                HttpResponseMessage response = service.GetResponse("api/Product/GetSoLuong/" + Id);

                response.EnsureSuccessStatusCode();
                int quantity = response.Content.ReadAsAsync<int>().Result;

                if (quantity <= 0)
                {

                    return 0;
                }
                List<DTO_Product_Item_Type> li = new List<DTO_Product_Item_Type>();

                HttpResponseMessage responseUser = service.GetResponse("api/Products_Ad/GetProductItemById/" + Id);
                responseUser.EnsureSuccessStatusCode();
                DTO_Product_Item_Type proItem = responseUser.Content.ReadAsAsync<DTO_Product_Item_Type>().Result;
                li.Add(new DTO_Product_Item_Type()
                {
                    Quantity = 1,
                    _id = proItem._id,
                    Name = proItem.Name,
                    Price = proItem.Price,
                    Details = proItem.Details,
                    Photo = proItem.Photo,
                    Id_Item = proItem.Id_Item,
                });
                Session["cart"] = li;
                return 2;
            }
        }
    }
}