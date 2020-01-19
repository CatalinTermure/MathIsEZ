using System;
using System.Collections.Generic;
using System.Windows;
using System.Text;

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
        public Point[] data;

        public Shape(ShapeType type, Point[] points)
        {
            this.type = type;
            data = points;
        }
    }
}
