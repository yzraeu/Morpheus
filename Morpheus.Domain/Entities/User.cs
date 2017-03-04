using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Morpheus.Domain.Entities
{
    public class User : _BaseEntity
    {
		[Required]
		[MinLength(2)]
		public string Name { get; set; }
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		[MinLength(3)]
		public string UId { get; set; }
		public string PhotoUrl { get; set; }
		public DateTime TokenExpirationDate { get; set; }
		public DateTime DateOfBirth { get; set; }
		public DateTime LastAccessDate { get; set; }

		//Address
		public string Address { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
		public string ZipCode { get; set; }
	}
}
