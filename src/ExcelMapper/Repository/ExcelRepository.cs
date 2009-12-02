using System;
using System.Collections.Generic;
using System.Linq;
using ExcelMapper.Configuration;
using RunTimeCodeGenerator.ClassGeneration;

namespace ExcelMapper.Repository
{
    public class ExcelRepository : IRepository
    {
        private readonly IDataProvider _dataProvider;
        private readonly IFileConfiguration _fileConfiguration;
        private readonly IFileService _fileService;
        private readonly Dictionary<string, CacheValue> _resultCache;

        public ExcelRepository(IDataProvider dataProvider, IFileConfiguration fileConfiguration,
                               IFileService fileService)
        {
            _dataProvider = dataProvider;
            _fileService = fileService;
            _fileConfiguration = fileConfiguration;
            _resultCache = new Dictionary<string, CacheValue>();
        }

        public IEnumerable<string> GetWorkSheetNames()
        {
            return _dataProvider.GetTableNames();
        }

        public ClassAttributes GetDTOClassAttributes(string workSheet)
        {
            var classAttributes = new ClassAttributes(workSheet.GetClassName());
            classAttributes.Properties.AddRange(_dataProvider.GetColumns(workSheet));
            return classAttributes;
        }

        public IEnumerable<T> Get<T>(string workSheet)
        {
            CacheValue cacheValue;
            if (!_resultCache.TryGetValue(workSheet, out cacheValue))
            {
                cacheValue = AddToCache<T>(workSheet);
            }

            if (cacheValue.LastModifiedDate != GetLastModifiedDate())
            {
                _resultCache.Remove(workSheet);
                cacheValue = AddToCache<T>(workSheet);
            }

            var resultList = (IEnumerable<T>) cacheValue.Result;
            foreach (T result in resultList)
            {
                yield return result;
            }
        }

        public void Update<T>(IEnumerable<T> values)
        {
            _dataProvider.Put(values);
        }

        public void SaveOrUpdate<T>(IEnumerable<T> values)
        {
            if (!_fileService.Exists(_fileConfiguration.FileName))
            {
                _fileService.Create(_fileConfiguration.FileName);
            }
            if (!_dataProvider.GetTableNames().Where(x => x == typeof(T).Name).Any())
            {
                _dataProvider.CreateTable<T>();
            }
            _dataProvider.Put(values);
        }

        private CacheValue AddToCache<T>(string workSheet)
        {
            var cacheValue = new CacheValue
                                 {
                                     Result = _dataProvider.Get<T>(workSheet).ToList(),
                                     LastModifiedDate = GetLastModifiedDate()
                                 };
            _resultCache.Add(workSheet, cacheValue);
            return cacheValue;
        }

        private DateTime GetLastModifiedDate()
        {
            return _fileService.GetLastModifiedDate(_fileConfiguration.FileName);
        }
    }
}