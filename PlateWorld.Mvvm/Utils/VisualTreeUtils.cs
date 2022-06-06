using System;
using System.Windows;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PlateWorld.Mvvm.Utils
{

    public static class VisualTreeUtils
    {
        /// <summary>
        /// Gets all descendents on the visual tree that are FrameworkElements.
        /// </summary>
        public static IEnumerable<FrameworkElement> FindFEChildren(FrameworkElement parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
                if (child != null)
                {
                    yield return child;

                    foreach (var grandchild in FindFEChildren(child))
                    {
                        yield return grandchild;
                    }
                }
            }

            yield break;
        }

        /// <summary>
        /// Gets the first parent Framework element with the given type
        /// </summary>
        public static FrameworkElement FindAncestorOfType(Type ancestorType, Visual visual)
        {
            while (visual != null && !ancestorType.IsInstanceOfType(visual))
            {
                visual = (Visual)VisualTreeHelper.GetParent(visual);
            }
            return visual as FrameworkElement;
        }

        /// <summary>
        /// Gets the first parent Framework element with the given type
        /// </summary>
        public static FrameworkElement FindAncestorWithDataContextOfType(Type ancestorType, Visual visual)
        {
            while (visual != null)
            {
                var fe = visual as FrameworkElement;
                if (fe != null && ancestorType.IsInstanceOfType(fe.DataContext))
                {
                    return fe;
                }
                visual = (Visual)VisualTreeHelper.GetParent(visual);
            }
            return null;
        }

        /// <summary>
        /// Gets the first parent Framework element with the given name
        /// </summary>
        public static FrameworkElement FindAncestorFrameworkElementNamed(Visual visual, string name)
        {
            FrameworkElement fweRet = null;
            while (visual != null)
            {
                visual = (Visual)VisualTreeHelper.GetParent(visual);
                fweRet = visual as FrameworkElement;
                if ((fweRet != null) && (fweRet.Name == name))
                {
                    return fweRet;
                }
            }
            return null;
        }

        public static bool IsMovementBigEnough(System.Drawing.Point initialMousePosition, System.Drawing.Point currentPosition)
        {
            return (Math.Abs(currentPosition.X - initialMousePosition.X) >= SystemParameters.MinimumHorizontalDragDistance ||
                 Math.Abs(currentPosition.Y - initialMousePosition.Y) >= SystemParameters.MinimumVerticalDragDistance);
        }

        public static bool IsMovementBigEnough(double x_delta, double y_value)
        {
            return (Math.Abs(x_delta) >= SystemParameters.MinimumHorizontalDragDistance ||
                 Math.Abs(y_value) >= SystemParameters.MinimumVerticalDragDistance);
        }


        static System.Drawing.Point ActualUpLeft(System.Drawing.Point[] corners)
        {
            var newUpLeft = new System.Drawing.Point(corners.Min(T => T.X), corners.Min(T => T.Y));
            return newUpLeft;
        }

        static System.Drawing.Point ActualBottomRight(System.Drawing.Point[] corners)
        {
            var newBottomRight = new System.Drawing.Point(corners.Max(T => T.X), corners.Max(T => T.Y));
            return newBottomRight;
        }

        static void PrintCorners(System.Drawing.Point[] corners)
        {
            System.Diagnostics.Debug.WriteLine("---------------------");
            foreach (var pt in corners)
            {
                System.Diagnostics.Debug.WriteLine(pt.X.ToString("0.0") + ", " + pt.Y.ToString("0.0"));
            }
        }

        /// <summary>
        /// Returns the rotation angle for a visual with given center and upleft corners
        /// </summary>
        /// <param name="center">center point </param>
        /// <param name="upLeft">upleft corner</param>
        /// <returns>the angle in degrees</returns>
        //public static double VisualRotationAngle(System.Drawing.Point center, System.Drawing.Point upLeft)
        //{
        //    Vector zero = System.Drawing.Point.Subtract(new System.Drawing.Point(1, 1), new System.Drawing.Size(0, 0));
        //    Vector eval = System.Drawing.Point.Subtract(center, upLeft);
        //    return Vector.AngleBetween(zero, eval);
        //}

    }
}
