using System;
using System.Collections.Generic;
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
        public float[] data;

        public Shape(ShapeType type, float[] points)
        {
            this.type = type;
            data = points;
        }
    }
}
