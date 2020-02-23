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
        POLYGON,
        NONE
    }

    /// <summary>
    /// Class for handling shapes inside a lesson
    /// </summary>
    class Shape
    {
        public ShapeType Type;

        /// <summary>
        /// Array of points representing vertices(in the case of triangles and polygons), or top-left/bottom-right coordinates(in the case of ellipses and rectangles).
        /// </summary>
        public Point[] Points;

        /// <summary>
        /// Brush for drawing the shape
        /// </summary>
        public Brush Stroke;

        public double StrokeThickness;

        /// <summary>
        /// Brush for filling the interior of the shape, or null if the shape should be empty.
        /// </summary>
        public Brush Fill;

        /// <summary>
        /// Time(in seconds) at which to start showing the shape.
        /// </summary>
        public int Start;

        /// <summary>
        /// Time(in seconds) at which to stop showing the shape, or -1 if it should remain visible for the whole lesson.
        /// </summary>
        public int End;

        /// <summary>
        /// Constructs shape from all the necessary data
        /// </summary>
        /// <param name="type"> The type of the shape. </param>
        /// <param name="points"> Array of points representing vertices(in the case of triangles and polygons), or top-left/bottom-right(in the case of ellipses and rectangles). </param>
        /// <param name="stroke"> Brush for drawing the outline of the shape. </param>
        /// <param name="fill"> Brush for filling the shape, can be null. </param>
        /// <param name="start"> Time(in seconds) at which to start showing the shape. </param>
        /// <param name="end"> Time(in seconds) at which to stop showing the shape, or -1 if it should remain visible for the whole lesson. </param>
        public Shape(ShapeType type, Point[] points, Brush stroke, double strokeThickness, Brush fill, int start, int end)
        {
            Type = type;
            Points = points;
            Stroke = stroke;
            Fill = fill;
            Start = start;
            End = end;
            StrokeThickness = strokeThickness;
        }

        /// <summary>
        /// Constructs shape with only the type. Other properties should be added before rendering.
        /// </summary>
        public Shape(ShapeType type)
        {
            Type = type;
        }

        /// <summary>
        /// Constructs shape with no data. Other properties should be added before rendering.
        /// </summary>
        public Shape()
        {
            Type = ShapeType.NONE;
        }

        /// <summary>
        /// Creates and returns a geometry specific to this shape.
        /// </summary>
        /// <returns> Returns a geometry to be drawn. </returns>
        public Geometry CreateGeometry()
        {
            switch(Type)
            {
                case ShapeType.POLYGON:
                    PathGeometry polygonGeometry = new PathGeometry();
                    polygonGeometry.Figures.Add(new PathFigure() { StartPoint = Points[0] });
                    for(int i = 1; i < Points.Length; i++)
                    {
                        polygonGeometry.Figures[0].Segments.Add(new LineSegment(Points[i], true));
                    }
                    return polygonGeometry;
                default:
                    return null;
            }
        }
    }
}
