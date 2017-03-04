using Morpheus.Domain.Entities;
using Morpheus.Domain.Exceptions;
using Morpheus.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Morpheus.Service.Base
{
	public abstract class DbService<T> : IDbService<T> where T : _BaseEntity
	{
		private readonly IDbRepository<T> _repository;
		private readonly ILogger _logger;

		public DbService(ILogger<T> logger, IDbRepository<T> repository)
		{
			_repository = repository;
			_logger = logger;
		}

		public virtual async Task Add(T entity)
		{
			await _repository.Add(entity);
		}

		public virtual async Task<IList<T>> Find(Expression<Func<T, bool>> predicate)
		{
			return await _repository.Find(predicate);
		}

		public virtual async Task<T> Get(int id)
		{
			return await _repository.Get(id);
		}

		public virtual async Task<IList<T>> GetAll()
		{
			return await _repository.GetAll();
		}

		public virtual async Task Remove(int id)
		{
			await _repository.Remove(id);
		}

		public virtual async Task Update(T entity)
		{
			await _repository.Update(entity);
		}
	}
}
