using System;
using System.IO;

namespace ExcelMapper.Repository
{
	public interface IFileService
	{
		void CreateNewIfNotExists(string filePath);
		DateTime GetLastModifiedDate(string filePath);
	}

	public class FileService : IFileService
	{
		public void CreateNewIfNotExists(string filePath)
		{
			if (!File.Exists(filePath))
			{
				File.Create(filePath);
			}
		}

		public DateTime GetLastModifiedDate(string filePath)
		{
			return File.GetLastWriteTime(filePath);
		}
	}
}