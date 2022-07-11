using PlateWorld.Models.BasicTypes;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace PlateWorld.ViewModels.Utils
{
    public class DataGridColumnInfo
    {
        public DataGridColumnInfo(string binding, string header)
        {
            Binding = new Binding(binding)
            {
                Mode = BindingMode.OneWay
            };
            Header = header;
        }

        public Binding Binding { get; }
        public string Header { get; }
    }

    public static class DataGridColumnInfoExt
    {
        public static IEnumerable<DataGridColumnInfo> MakeDataGridColumnInfo(
            this IEnumerable<IPropertyType> propertyTypes, string containerName)
        {
            return propertyTypes.Select((p, dex) =>
                new DataGridColumnInfo(
                    binding: $"{containerName}[{dex}].Value",
                    header: p.Name));
        }
    }
}
