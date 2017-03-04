using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Morpheus.Domain.Entities
{
    public class Yell : _BaseEntity
    {
		[Required]
		public User User { get; set; }
		[Required]
		public decimal Latitude { get; set; }
		[Required]
		public decimal Longitude { get; set; }
		[Required]
		public byte[] Audio { get; set; }
	}
}
