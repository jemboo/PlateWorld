using System;

namespace PlateWorld.Models.BasicTypes
{
    public class PropertyType : IPropertyType
    {
        public PropertyType(string name, string description,
            DataType dataType, Func<object, bool> isValid)
        {
            Name = name;
            Description = description;
            DataType = dataType;
            IsValid = isValid;
        }
        public string Name { get; }
        public DataType DataType { get; }
        public string Description { get; }
        public Func<object, bool> IsValid { get; }
        public bool IsDiscrete { get { return false; } }
    }


    public static class PropertyTypeExt
    {
        public static IPropertyType MakePh()
        {
            return new PropertyType(
                name:"Ph", 
                description:"Ph", 
                dataType:DataType.Float,
                isValid: new Func<object, bool>(o =>
                {
                    var db = (double)o;
                    return ((db > -1) && (db < 15));
                }));
        }

        public static IPropertyType MakeDillution()
        {
            return new PropertyType(
                name: "Dillution",
                description: "Log Dillution",
                dataType: DataType.Float,
                isValid: new Func<object, bool>(o =>
                {
                    var db = (double)o;
                    return ((db > 0) && (db < 12));
                }));
        }

        public static IPropertyType MakeReplicateNumber()
        {
            return new PropertyType(
                name: "Replicate",
                description: "Replicate number",
                dataType: DataType.Int,
                isValid: new Func<object, bool>(o =>
                {
                    var db = (int)o;
                    return (db > 0);
                }));
        }

        public static IPropertyType MakeBuffer()
        {
            return new PropertyType(
                name: "Buffer",
                description: "Buffer",
                dataType: DataType.String,
                isValid: new Func<object, bool>(o =>
                {
                    var db = (string)o;
                    return (! string.IsNullOrEmpty(db));
                }));
        }

        public static IPropertyType MakeEnzyme()
        {
            return new PropertyType(
                name: "Enzyme",
                description: "Enzyme",
                dataType: DataType.String,
                isValid: new Func<object, bool>(o =>
                {
                    var db = (string)o;
                    return (!string.IsNullOrEmpty(db));
                }));
        }

        public static IPropertyType MakeSubstrate()
        {
            return new PropertyType(
                name: "Substrate",
                description: "Substrate",
                dataType: DataType.String,
                isValid: new Func<object, bool>(o =>
                {
                    var db = (string)o;
                    return (!string.IsNullOrEmpty(db));
                }));
        }

    }
}
