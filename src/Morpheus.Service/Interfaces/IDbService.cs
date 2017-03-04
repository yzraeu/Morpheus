using Morpheus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Morpheus.Service.Base
{
    public interface IDbService<T> where T : _BaseEntity
    {
		Task<IList<T>> GetAll();
		Task<T> Get(int id);
		Task<IList<T>> Find(Expression<Func<T, bool>> predicate);
		Task Add(T entity);
		Task Update(T entity);
		Task Remove(int id);
	}
}
