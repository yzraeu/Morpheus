using Morpheus.Domain.Entities;
using Morpheus.Domain.Exceptions;
using Morpheus.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Morpheus.Repository.MySQL
{
	public class Repository<T> : IDbRepository<T> where T : _BaseEntity
	{
		private readonly ILogger<T> _logger;
		private readonly DbContext _context;
		private DbSet<T> _dbSet;

		public Repository(RepositoryContext context, ILogger<T> logger)
		{
			_context = context;
			_logger = logger;

			_dbSet = context.Set<T>();
		}

		public virtual async Task Add(T entity)
		{
			var sw = Stopwatch.StartNew();
			_logger.LogDebug($"Add {typeof(T).Name}");

			_context.Set<T>().Add(entity);
			await Save();

			_logger.LogDebug($"Add Took: {sw.ElapsedMilliseconds} ms");
		}

		public virtual async Task<IList<T>> Find(Expression<Func<T, bool>> predicate)
		{
			var sw = Stopwatch.StartNew();
			_logger.LogDebug($"Find {typeof(T).Name}");

			var output = await _dbSet.Where(predicate).Where(e => !e.IsDeleted).ToListAsync();

			_logger.LogDebug($"Find Took: {sw.ElapsedMilliseconds} ms");
			return output;
		}

		public virtual async Task<T> Get(int id)
		{
			var sw = Stopwatch.StartNew();
			_logger.LogDebug($"Get {typeof(T).Name}");

			var output = await _dbSet.FirstOrDefaultAsync(e => !e.IsDeleted && e.Id.Equals(id));

			_logger.LogDebug($"Get Took: {sw.ElapsedMilliseconds} ms");
			return output;
		}

		public virtual async Task<IList<T>> GetAll()
		{
			var sw = Stopwatch.StartNew();
			_logger.LogDebug($"GetAll {typeof(T).Name}");

			var output = await _dbSet.Where(e => !e.IsDeleted).ToListAsync();

			_logger.LogDebug($"GetAll Took: {sw.ElapsedMilliseconds} ms");
			return output;

		}

		public virtual async Task Remove(int id)
		{
			_logger.LogDebug($"Remove {typeof(T).Name}");

			var entity = await Get(id);
			entity.IsDeleted = true;
			await Update(entity);
		}

		public virtual async Task Update(T entity)
		{
			var sw = Stopwatch.StartNew();

			if (Get(entity.Id) == null) throw new NotFoundException();

			_logger.LogDebug($"Update {typeof(T).Name}");

			_context.Entry(entity).State = EntityState.Modified;

			await Save();
			_logger.LogDebug($"Update Took: {sw.ElapsedMilliseconds} ms");
		}

		private void AuditEntries()
		{
			var entries = _context.ChangeTracker.Entries<T>();

			foreach (var entry in entries)
			{
				if (entry.State == EntityState.Modified)
				{
					entry.Entity.ModifiedDate = DateTime.UtcNow;
					entry.Property(p => p.AddedDate).IsModified = false;
				}

				if (entry.State == EntityState.Added)
				{
					entry.Entity.AddedDate = entry.Entity.ModifiedDate = DateTime.UtcNow;
				}
			}
		}

		private async Task Save()
		{
			await _context.Database.BeginTransactionAsync();

			AuditEntries();

			await _context.SaveChangesAsync();

			_context.Database.CommitTransaction();
		}
	}
}
