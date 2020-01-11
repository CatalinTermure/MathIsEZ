using System;
using System.Collections.Generic;
using System.Text;

namespace MathIsEZ
{
    class Lesson
    {
        // We use array instead of list since the number of shapes/graphs will remain constant unless lecture is edited
        // This means we are trading a per-draw performance boost(from using array) with one-time performance drop(converting to list when editing)
        public Shape[] shapes;
        public Graph[] graphs;

        public Lesson(Shape[] shapes, Graph[] graphs)
        {
            this.shapes = shapes;
            this.graphs = graphs;
        }
    }
}
