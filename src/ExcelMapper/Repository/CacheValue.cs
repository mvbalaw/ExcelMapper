using System;

namespace ExcelMapper.Repository
{
	public class CacheValue
	{
		public DateTime LastModifiedDate { get; set; }
		public object Result { get; set; }
	}
}