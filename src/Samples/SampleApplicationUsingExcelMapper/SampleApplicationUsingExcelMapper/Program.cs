using System;
using ExcelMapper.Configuration;
using ExcelMapper.Repository;
using ExcelToDTOMapper.DTO.Users;
using Microsoft.Practices.ServiceLocation;

namespace SampleApplicationUsingExcelMapper
{
    public class Program
    {
        public static void Main()
        {
            ExcelMapperServiceLocator.SetUp();
            ServiceLocator.Current.GetInstance<IFileConfiguration>().FileName = "Excel\\Users.xlsx";

            IRepository repository = ServiceLocator.Current.GetInstance<IRepository>();

            foreach (User user in repository.Get<User>("User"))
            {
                Console.WriteLine("Hello {0} {1}", user.FirstName, user.LastName);
            }
        }
    }
}