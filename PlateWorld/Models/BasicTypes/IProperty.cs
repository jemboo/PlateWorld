namespace PlateWorld.Models.BasicTypes
{
    public interface IProperty
    {
        public string Name { get; }
        IPropertyType PropertyType { get; }
        public object Value { get; }
    }
}
