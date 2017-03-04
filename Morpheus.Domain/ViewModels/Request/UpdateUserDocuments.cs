using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Morpheus.Domain.ViewModels.Request
{
    public class UpdateUserDocuments
	{
		[Required]
		public long CPF { get; set; }
		public string RG { get; set; }
	}
}
