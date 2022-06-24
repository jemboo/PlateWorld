using System.Windows;
using System.Windows.Controls;

namespace PlateWorld.Views.Parts.Plate
{
    public partial class PlateMarginControl : UserControl
    {
        public PlateMarginControl()
        {
            InitializeComponent();
        }

        public Orientation Orientation 
        {
            get { return (Orientation)GetValue(OrientationProperty); }

            set { SetValue(OrientationProperty, value); }

        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), 
                typeof(PlateMarginControl));




        //[Category("Custom Properties")]
        //public IReadOnlyList<D2Val<Color>> PlotPoints
        //{
        //    get { return (IReadOnlyList<D2Val<Color>>)GetValue(PlotPointsProperty); }
        //    set { SetValue(PlotPointsProperty, value); }
        //}

        //public static readonly DependencyProperty PlotPointsProperty =
        //    DependencyProperty.Register("PlotPoints", typeof(IReadOnlyList<D2Val<Color>>), typeof(WbImage),
        //    new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender,
        //        OnPlotPointsChanged));

        //private static void OnPlotPointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var graphicsInfo = (IReadOnlyList<D2Val<Color>>)e.NewValue;
        //    if ((graphicsInfo == null) || (graphicsInfo.Count == 0))
        //    {
        //        return;
        //    }

        //    var gridImage = d as WbImage;
        //    gridImage?.DoPlotPoints();
        //}
    }
}
