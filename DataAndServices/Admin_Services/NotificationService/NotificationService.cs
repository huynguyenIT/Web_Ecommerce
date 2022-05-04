﻿using DataAndServices.Common;
using DataAndServices.Data;
using DataAndServices.DataModel;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAndServices.Admin_Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly IMongoCollection<MerchantNotification> _db;       

        public NotificationService(DataContext db)
        {
            _db = db.GetMerchantNotificationCollection();
        }
        public bool AddNotication(List<MerchantNotification> request)
        {
            try
            {
                
                _db.InsertMany(request);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<MerchantNotification>> GetMerchantNotification(string merchantId)
        {
            var merchantNotis = await _db.FindAsync(n => n.AccountId == merchantId);
            return merchantNotis.ToList();
        }

        public MerchantNotification ChangeStatusNotification(string notiId)
        {

                var eqfilter = Builders<MerchantNotification>.Filter.Where(s => s._id == notiId && s.Status == NotificationConstant.PENDING);

                var update = Builders<MerchantNotification>.Update.Set(s => s.Status, NotificationConstant.READED);
                   
                var options = new UpdateOptions { IsUpsert = true };

                _db.UpdateOneAsync(eqfilter, update, options);

                var notiUpdated = _db.Find(n => n._id == notiId).FirstOrDefault();
                return notiUpdated;
            
        }
    }
}
