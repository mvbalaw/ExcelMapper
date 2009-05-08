using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.ServiceLocation;

using StructureMap;

namespace ExcelMapper.Configuration
{
    public class StructureMapServiceLocator : ServiceLocatorImplBase
    {
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return ObjectFactory.GetAllInstances(serviceType).Cast<object>().AsEnumerable();
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            return (string.IsNullOrEmpty(key) ? ObjectFactory.GetInstance(serviceType) : ObjectFactory.GetNamedInstance(serviceType, key));
        }
    }
}