using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Morpheus.Domain.ViewModels.Request
{
    public class UpdateAddress
    {
		[Required]
		public string Address { get; set; }
		public string Address2 { get; set; }
		[Required]
		public string City { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
		public string ZipCode { get; set; }
	}
}
