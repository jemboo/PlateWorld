using PlateWorld.ViewModels.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PlateWorld.Views.Parts
{
    public class DynoGrid : DataGrid
    {
        public List<DataGridColumnInfo> ColumnInfo
        {
            get { return (List<DataGridColumnInfo>)GetValue(ColumnInfoProperty); }

            set { SetValue(ColumnInfoProperty, value); }

        }

        public static readonly DependencyProperty ColumnInfoProperty =
            DependencyProperty.Register("ColumnInfo", 
                typeof(List<DataGridColumnInfo>),
                typeof(DynoGrid), 
                new FrameworkPropertyMetadata(OnColumnInfoPropertyChanged));

        private static void OnColumnInfoPropertyChanged(
                                DependencyObject d,
                                DependencyPropertyChangedEventArgs e)
        {
            var dynaGrid = (DynoGrid)d;
            if (dynaGrid == null) return;
            dynaGrid.Columns.Clear();
            if (dynaGrid.ColumnInfo == null) return;
            foreach (var info in dynaGrid.ColumnInfo)
            {
                var dgc = new DataGridTextColumn();
                dgc.Header = info.Header;
                dgc.Binding = info.Binding;
                dynaGrid.Columns.Add(dgc);
            }
        }
    }
}
