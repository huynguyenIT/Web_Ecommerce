﻿using Model.DTO_Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.DTO.DTO_Client
{
    public class DTO_Product
    {
        public string _id { get; set; }

        public string Name { get; set; }

        public int? Price { get; set; }

        public string Photo { get; set; }

        public string Photo2 { get; set; }

        public string Photo3 { get; set; }

        public string Details { get; set; }

        public int Id_Item { get; set; }

        public string AccountId { get; set; }

    }
}
