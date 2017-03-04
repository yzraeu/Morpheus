using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Morpheus.Domain.ViewModels.Request
{
    public class Login
    {
		[Required]
		public string UId { get; set; }
	}
}
