using System;

using ExcelMapper.Configuration;
using ExcelMapper.Repository;

using ExcelToDTOMapper.DTO;

using Microsoft.Practices.ServiceLocation;

namespace SampleApplicationUsingExcelMapper
{
    public class Program
    {
        public static void Main()
        {
            ExcelMapperServiceLocator.SetUp();
            IRepository repository = ServiceLocator.Current.GetInstance<IRepository>();
            
            foreach (User user in repository.Get<User>("Excel\\Users.xlsx", "User"))
            {
                Console.WriteLine("Hello {0} {1}", user.FirstName, user.LastName);
            }
        }
    }
}