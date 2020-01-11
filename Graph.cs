using System;
using System.Collections.Generic;
using System.Text;

namespace MathIsEZ
{
    public class Graph
    {
        public delegate double Function(double x);
        public Function function;

        public bool hasAxis;

        public Graph(bool drawAxis, Function functionToDraw)
        {
            hasAxis = drawAxis;
            function = functionToDraw;
        }
    }
}