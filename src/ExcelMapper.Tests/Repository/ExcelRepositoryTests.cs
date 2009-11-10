using System;
using System.Collections.Generic;
using System.Linq;

using ExcelMapper.Repository;
using ExcelMapper.Tests.DTO;

using NUnit.Framework;

using Rhino.Mocks;

using StructureMap.AutoMocking;

namespace ExcelMapper.Tests.Repository
{
	public class ExcelRepositoryTests
	{
		[TestFixture]
		public class When_asked_to_retrieve_an_entity_from_an_Excel_file
		{
			private IDataProvider _dataProvider;
			private DateTime _fileModifiedDate;
			private IFileService _fileService;
			private IRepository _repository;
			private List<User> _users;

			[SetUp]
			public void SetUp()
			{
				_users = new List<User>
					{
						new User
							{
								Id = 1,
								LastName = "LastName",
								FirstName = "FirstName"
							}
					};
				_fileModifiedDate = Convert.ToDateTime("1/1/2009");

				var autoMocker = new RhinoAutoMocker<ExcelRepository>();
				_fileService = autoMocker.Get<IFileService>();
				_dataProvider = autoMocker.Get<IDataProvider>();
				_repository = autoMocker.ClassUnderTest;

				_fileService.Expect(x => x.GetLastModifiedDate("")).IgnoreArguments().Return(_fileModifiedDate).Repeat.Any();
				_dataProvider.Expect(x => x.Get<User>("")).IgnoreArguments().Return(_users);
			}

			[Test]
			public void Should_get_the_values_from_cache_or_file()
			{
				var result = _repository.Get<User>("").ToList();
				result = _repository.Get<User>("").ToList();
				Assert.AreEqual(_users.Count, result.Count);
			}
		}
	}
}