using Model.DTO.DTO_Ad;
using Model.DTO.DTO_Client;
using Model.DTO_Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using UI.Security_;
using UI.Service;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        ServiceRepository service = new ServiceRepository();

        public string userName;
        public ActionResult Index()
        {
            string id = String.Empty;
            if (Request.Cookies["idCustomer"] != null)
                id = Request.Cookies["idCustomer"].Value;
            if(id == String.Empty)
            {
                if (Request.Cookies["firstname"] != null)
                {
                    HttpCookie ck1 = new HttpCookie("firstname");
                    ck1.Expires = DateTime.Now.AddHours(-48);
                    Response.Cookies.Add(ck1);
                }
                return View();
            }
            return View();

        }
        public PartialViewResult ListTypeProduct()
        {
            HttpResponseMessage responseUser = service.GetResponse("api/Home/GetAllItemTypeUsed");

            responseUser.EnsureSuccessStatusCode();
            List<DTO_Item_Type> result = responseUser.Content.ReadAsAsync<List<DTO_Item_Type>>().Result;

            return PartialView(result);
        }

        [AuthorizeLoginEndUser]
        public ActionResult saveFeedbacks(FormCollection fc, DTO_Feedback fb)
        {

            try
            {

                fb.Name = fc["Name"];
                fb.Email = fc["Email"];
                fb.Details = fc["details"];
                fb.SDT = (fc["SDT"]);
                fb.Content = fc["content"];
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/Home/Create/", fb);
                response.EnsureSuccessStatusCode();
                ViewData["ErrorMessageFeedback"] = ("Gửi phản hồi thành công");
                return View("Index");
            }
            catch
            {
                return View("~/Views/Shared/Error_.cshtml");
            }

        }

        [AuthorizeLoginEndUser]
        public ActionResult saveFeedbacks2(FormCollection fc, DTO_Product_Item_Type fb)
        {

            try
            {

                var fullName = Request.Cookies["firstname"].Value;
                //fb.Comments = fc["content"];
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/Home/Create/", fb);
                response.EnsureSuccessStatusCode();
                ViewData["ErrorMessageFeedback"] = ("Gửi phản hồi thành công");
                return RedirectToAction("Details", "Product");
            }
            catch
            {
                return View("~/Views/Shared/Error_.cshtml");
            }
        }

        [AuthorizeLoginEndUser]
        public ActionResult saveFeedbacksYeuThich(FormCollection fc, DTO_Feedback fb )
        {
            try
            {
                fb.Name = fc["Name"];
                fb.Email = fc["Email"];
                fb.Details = fc["details"];
                fb.SDT = (fc["SDT"]);
                fb.Content = fc["content"];
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/Home/Create/", fb);
                response.EnsureSuccessStatusCode();
                ViewData["ErrorMessage"] = ("Gửi phản hồi thành công");

                return RedirectToAction("YeuThich", "Cart");
            }
            catch
            {
                return View("~/Views/Shared/Error_.cshtml");
            }

           


        }

        [AuthorizeLoginEndUser]
        public ActionResult saveFeedbacksLuaChon(FormCollection fc, DTO_Product_Item_Type product)
        {

            try
            {
                DTO_Product_Item_Type item = (DTO_Product_Item_Type)Session["cart__"];

                var dtoComment = new DtoProductComment()
                {
                    ProductId = item._id,
                    DateTimeComment = DateTime.Now,
                    FullName = Request.Cookies["firstname"].Value,
                    Content = fc["details"]
                };

                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/Home/AddComment/", dtoComment);
                response.EnsureSuccessStatusCode();

                HttpResponseMessage responseUser = service.GetResponse("api/Products_Ad/GetProductItemById/" + item._id);
                DTO_Product_Item_Type proItem = responseUser.Content.ReadAsAsync<DTO_Product_Item_Type>().Result;
                Session["cart__"] = proItem;

                ViewData["ErrorMessage"] = ("Gửi bình luận thành công");
                return RedirectToAction("Luachon", "Cart");
            }
            catch
            {
                return View("~/Views/Shared/Error_.cshtml");
            }
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
