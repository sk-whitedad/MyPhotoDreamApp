using MyPhotoDreamApp.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoDreamApp.Domain.ViewModels.Order
{
    public class AllConfirmOrderViewModel
    {
        public int Id { get; set; }

        public decimal SummOrder { get; set; }

        public string? DeliveryAddress { get; set; }

        public string CheckDelivery { get; set; }

        public DateTime DateCreated { get; set; }

        public string PhoneNumber { get; set; }

    }
}
