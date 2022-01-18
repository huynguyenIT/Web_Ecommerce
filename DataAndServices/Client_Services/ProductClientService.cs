﻿using DataAndServices.CommonModel;
using DataAndServices.Data;
using DataAndServices.DataModel;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAndServices.Client_Services
{
    public class ProductClientService : IProductClientServices
    {
        private readonly IMongoCollection<Product_Client> _db;
        private readonly IMongoCollection<Item> _dbItem;
        private readonly IMongoCollection<Discount_Product> _dbDis;
        private DataContext db = new DataContext("mongodb://localhost:27017", "OnlineShop");
        public ProductClientService(DataContext db)
        {
            _db = db.GetProductClientCollection();
            _dbItem = db.GetItemCollection();
            _dbDis = db.GetDiscountProductCollection();
        }
        public List<Dis_Product> GetAllProductByName(string name)
        {
            var discountCollection = db.GetDiscountProductCollection();
            var productCollection = db.GetProductClientCollection();
            var Info = (from dis in discountCollection.AsQueryable()
                        join product in productCollection.AsQueryable() on dis._id equals product._id

                        select new Dis_Product()
                        {
                            _id = dis._id,
                            Name = product.Name,
                            Price = product.Price,
                            Details = product.Details,
                            Photo = product.Photo,
                            Photo2 = product.Photo2,
                            Photo3 = product.Photo3,
                            Id_Item = product.Id_Item,
                            Content = dis.Content,
                            Price_Dis = dis.Price_Dis,
                            Start = dis.Start,
                            End = dis.End
                        }).Where(s => s.Name.StartsWith(name)).ToList();
            return Info;
        }

        public List<Dis_Product> GetAllProductByPrice(int? gia, int? gia_)
        {
            var discountCollection = db.GetDiscountProductCollection();
            var productCollection = db.GetProductClientCollection();
            var Info = (from dis in discountCollection.AsQueryable()
                        join product in productCollection.AsQueryable() on dis._id equals product._id

                        select new Dis_Product()
                        {
                            _id = dis._id,
                            Name = product.Name,
                            Price = product.Price,
                            Details = product.Details,
                            Photo = product.Photo,
                            Photo2 = product.Photo2,
                            Photo3 = product.Photo3,
                            Id_Item = product.Id_Item,
                            Content = dis.Content,
                            Price_Dis = dis.Price_Dis,
                            Start = dis.Start,
                            End = dis.End
                        });
            List<Dis_Product> dis_Product = new List<Dis_Product>();
            List<Dis_Product> dis_Product2 = new List<Dis_Product>();
            Dis_Product dis_Product3 = new Dis_Product();
            Dis_Product dis_Product4 = new Dis_Product();
            List<Dis_Product> dis_Product5 = new List<Dis_Product>();
            List<Dis_Product> dis_Product6 = new List<Dis_Product>();

            foreach (var item in Info)
            {
                //dis_Product.Add(item);
                if (GetPriceDiscountById(item._id) != 0)
                {
                    //item.Price = Convert.ToInt32(GetPriceDiscountById((Convert.ToInt32(item.Id_SanPham))));

                    bool dis_Product7 = (item.Price_Dis <= gia_ && item.Price_Dis >= gia);
                    if (dis_Product7 == true)
                    {
                        dis_Product5.Add(item);
                    }

                }
                else
                {
                    bool dis_Product8 = (item.Price <= gia_ && item.Price >= gia);
                    if (dis_Product8 == true)
                    {
                        dis_Product2.Add(item);
                    }

                }
            }
            dis_Product6.AddRange(dis_Product2);
            dis_Product6.AddRange(dis_Product5);

            return dis_Product6;
        }
        public double GetPriceDiscountById(string id)
        {
            DateTime dateTime = DateTime.Today;
            var item_discount = _dbDis.Find(t => t._id == id && t.End.Value >= dateTime).FirstOrDefault();

            if (item_discount != null && item_discount.Price_Dis != null)
            {
                return Convert.ToDouble(item_discount.Price_Dis);
            }
            return 0;
        }

        public async Task<List<Product_Client>> GetAllProducts()
        {
            return await _db.Find(s => true).ToListAsync();
        }


        public int GetSoLuong(string id)
        {
            var temp = _dbItem.Find(s => s._id == id).FirstOrDefault();
            if (temp != null)
            {
                return (int)temp.Quantity;
            }
            return 0;
        }
    }
}