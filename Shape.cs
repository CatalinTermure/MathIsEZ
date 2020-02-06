using System;
using System.Collections.Generic;
using System.Windows;
using System.Text;
using System.Windows.Media;

namespace MathIsEZ
{
    enum ShapeType
    {
        ELLIPSE,
        RECTANGLE,
        TRIANGLE,
        POLYGON
    }

    /// <summary>
    /// Class for handling shapes inside a lesson
    /// </summary>
    class Shape
    {
        public ShapeType type;

        /// <summary>
        /// Array of points representing vertices(in the case of triangles and polygons), top-left/bottom-right(in the case of ellipses and rectangles).
        /// </summary>
        public Point[] data;

        /// <summary>
        /// Brushes for drawing the shape
        /// </summary>
        public Brush stroke, fill;

        /// <summary>
        /// Time(in milliseconds) at which to start showing the shape.
        /// </summary>
        public int start;

        /// <summary>
        /// Time(in milliseconds) at which to stop showing the shape.
        /// </summary>
        public int end;

        /// <summary>
        /// Constructs shape from all the necessary data
        /// </summary>
        /// <param name="type"> The type of the shape. </param>
        /// <param name="points"> Array of points representing vertices(in the case of triangles and polygons), top-left/bottom-right(in the case of ellipses and rectangles). </param>
        /// <param name="stroke"> Brush for drawing the outline of the shape. </param>
        /// <param name="fill"> Brush for filling the shape, can be null. </param>
        /// <param name="start"> Time(in milliseconds) at which to start showing the shape. </param>
        /// <param name="end"> Time(in milliseconds) at which to stop showing the shape. </param>
        public Shape(ShapeType type, Point[] points, Brush stroke, Brush fill, int start, int end)
        {
            this.type = type;
            data = points;
            this.stroke = stroke;
            this.fill = fill;
            this.start = start;
            this.end = end;
        }
    }
}
