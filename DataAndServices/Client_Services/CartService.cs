﻿using DataAndServices.Data;
using DataAndServices.DataModel;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAndServices.Client_Services
{
    public class CartService : ICartServices
    {
        private readonly IMongoCollection<Checkout_Oder> _dbCheckOrder;
        private readonly IMongoCollection<CodeDiscount> _dbCodeDis;
        private readonly IMongoCollection<Checkout_Customer> _dbCheckCustomer;
        private readonly IMongoCollection<CheckoutCustomerOrder> _dbCheckCustomerOrder;
        private readonly IMongoCollection<Item> _dbItem;

        public CartService(DataContext db)
        {
            _dbCheckOrder = db.GetCheckout_OderCollection();
            _dbCodeDis = db.GetCodeDiscountCollection();
            _dbCheckCustomer = db.GetCheckout_CustomerCollection();
            _dbCheckCustomerOrder = db.GetCheckoutCustomerOrderCollection();
            _dbItem = db.GetItemCollection();
            
        }
        public async Task<double> GetGiamGia(string zipcode)
        {

            var temp = await _dbCodeDis.Find(s => s.Zipcode == zipcode).FirstOrDefaultAsync();
            if (temp != null)
            {
                return (double)temp.Discount; // vidu: 0.3 0.4
            }
            return 0;
        }

        public async Task<List<string>> InsertBill(CheckoutCustomerOrder checkoutCustomer_Order)
        {
          
            try
            {
                var accs = checkoutCustomer_Order.ProductOrder.Select(s => s.AccountId).Distinct();
                foreach (var item in checkoutCustomer_Order.ProductOrder)
                {
                    if(item.ItemId != null && item.ItemId!= string.Empty)
                    {
                        int quantity = (int)item.SoLuong;

                        UpdateQuantityItem(item.ItemId, quantity);
                    }
                   
                }
                var newCheckouts = new List<CheckoutCustomerOrder>();
                foreach(var item in accs)
                {
                    var checkoutCustomerOrder = new CheckoutCustomerOrder();
                    checkoutCustomerOrder = checkoutCustomer_Order;
                    checkoutCustomerOrder.AccountId = item;
                    newCheckouts.Add(checkoutCustomerOrder);
                }
           
                await _dbCheckCustomerOrder.InsertManyAsync(newCheckouts);

                return newCheckouts.Select(s=>s.AccountId).ToList();
            }
            catch
            {
                return null;
            }
        }
        public bool UpdateQuantityItem(string _id, int quantity)
        {
            var itemNow = _dbItem.Find(s => s._id == _id).FirstOrDefault();

            if (itemNow != null)
            {
                var quantityNow = itemNow.Quantity; // vidu: 0.3 0.4
                var quantityPay = quantityNow - quantity;

                if (quantityNow - quantityPay > 0)
                {
                    itemNow.Quantity = quantityPay;

                    var eqfilter = Builders<Item>.Filter.Where(s => s._id == _id);
                    var update = Builders<Item>.Update.Set(s => s.Quantity, itemNow.Quantity);
                    var options = new UpdateOptions { IsUpsert = true };

                    _dbItem.UpdateOne(eqfilter, update, options);
                    return true;
                }
            }
            return false;
        }

        public bool InsertCheckoutOrder(Checkout_Oder checkout_Order)
        {
            try
            {
                _dbCheckOrder.InsertOne(checkout_Order);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
