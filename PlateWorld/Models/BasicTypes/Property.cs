using System.Text;
using System.Threading.Tasks;

namespace PlateWorld.Models.BasicTypes
{
    public class Property : IProperty
    {
        public Property(string name, IPropertyType propertyType, object value)
        {
            PropertyType = propertyType;
            Value = value;
            Name = name;
        }
        public string Name { get; }
        public IPropertyType PropertyType { get; }
        public object Value { get; }
    }
}
