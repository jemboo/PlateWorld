namespace PlateWorld.Models.BasicTypes
{
    public interface IProperty
    {
        IPropertyType PropertyType { get; }
        public object Value { get; }
    }
}
