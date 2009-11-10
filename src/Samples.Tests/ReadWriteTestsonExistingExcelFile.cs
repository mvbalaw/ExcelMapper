using System;
using System.Collections.Generic;
using System.Linq;

using ExcelMapper.Configuration;
using ExcelMapper.Repository;

using ExcelToDTOMapper.DTO.Users;

using Microsoft.Practices.ServiceLocation;

using NUnit.Framework;

namespace Samples.Tests
{
	[TestFixture]
	public class ReadWriteExistingExcelFileTests
	{
		private IRepository _repository;

		[SetUp]
		public void BeforeEachTest()
		{
			const string file = "Excel\\Users.xlsx";

			ExcelMapper.Configuration.ExcelMapper.SetUp();
			ServiceLocator.Current.GetInstance<IFileConfiguration>().FileName = file;
			_repository = ServiceLocator.Current.GetInstance<IRepository>();
		}

		[Test]
		public void ReadFromExcel()
		{
			foreach (User user in _repository.Get<User>("User"))
			{
				Console.WriteLine("Hello {0} {1}", user.FirstName, user.LastName);
			}
		}

		[Test]
		public void WriteToExcel()
		{
			var expectedUser = new User
				{
					Id = 8,
					LastName = "Doe",
					FirstName = "John",
					DateOfBirth = Convert.ToDateTime("2/2/1990")
				};
			List<User> users = new List<User>
				{
					expectedUser
				};
			_repository.Put(users);

			var actualUser = _repository.Get<User>("User").Where(x => x.Id == 8).First();
			Assert.IsNotNull(actualUser);
			Assert.AreEqual(expectedUser.LastName, actualUser.LastName);
			Assert.AreEqual(expectedUser.FirstName, actualUser.FirstName);
			Assert.AreEqual(expectedUser.DateOfBirth, actualUser.DateOfBirth);
		}
	}
}