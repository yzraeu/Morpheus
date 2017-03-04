using Morpheus.Domain.DTOs;
using Morpheus.Domain.Entities;
using Morpheus.Domain.Exceptions;
using Morpheus.Domain.ViewModels.Request;
using Morpheus.Repository.Interfaces;
using Morpheus.Service.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Morpheus.Service
{
	public class UserService : DbService<User>
	{
		private readonly IDbRepository<User> _repository;
		private readonly ILogger<User> _logger;

		public UserService(ILogger<User> logger, IDbRepository<User> repository)
			 : base(logger, repository)
		{
			_repository = repository;
			_logger = logger;
		}

		public async Task<User> Login(TokenPayloadDTO tokenPayload)
		{
			var users = await _repository.Find(u => u.UId.Equals(tokenPayload.user_id) && u.Email.Equals(tokenPayload.email));

			var user = new User();
			var newUser = users.Count() == 0;

			if (newUser)
				user.UId = tokenPayload.user_id;
			else
				user = users.First();

			user.Name = tokenPayload.name;
			user.Email = tokenPayload.email;
			user.PhotoUrl = tokenPayload.picture;
			user.TokenExpirationDate = DateTimeOffset.FromUnixTimeSeconds(tokenPayload.exp).UtcDateTime;
			user.LastAccessDate = DateTime.UtcNow;

			if (newUser)
				await _repository.Add(user);
			else
				await _repository.Update(user);

			return user;
		}

		public async Task<User> UpdateAddress(int userId, UpdateAddress updateAddressVM)
		{
			var user = await _repository.Get(userId);

			if (user == null)
				throw new NotFoundException();

			user.Address = updateAddressVM.Address;
			user.Address2 = updateAddressVM.Address2;
			user.City = updateAddressVM.City;
			user.State = updateAddressVM.State;
			user.Country = updateAddressVM.Country;
			user.ZipCode = updateAddressVM.ZipCode;

			await _repository.Update(user);

			return user;
		}
	}
}
