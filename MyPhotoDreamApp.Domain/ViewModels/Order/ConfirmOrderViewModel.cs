using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoDreamApp.Domain.ViewModels.Order
{
	public class ConfirmOrderViewModel
	{
		public int Id { get; set; }
		public string PhoneNumber { get; set; }
		public decimal FinalSumm { get; set; }
	}
}
