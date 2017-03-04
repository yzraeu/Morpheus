using Morpheus.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Morpheus.Repository.MySQL
{
	public class RepositoryContext : DbContext
	{
		public DbSet<User> Users { get; set; }

		public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
		{
			
		}
	}
}
