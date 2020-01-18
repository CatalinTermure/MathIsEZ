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
