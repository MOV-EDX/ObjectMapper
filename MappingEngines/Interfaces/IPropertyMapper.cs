using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YaMapper.MappingEngines.Interfaces
{
    internal interface IPropertyMapper
    {
        internal object MapSimpleProperty<TDest>(PropertyInfo source, TDest destination);
    }
}
