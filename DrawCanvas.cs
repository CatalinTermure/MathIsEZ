using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace MathIsEZ
{
    public class DrawCanvas : UIElement
    {
        /// <summary>
        /// Delegate used for drawing onto a DrawCanvas
        /// </summary>
        /// <param name="dc"> Subsitute for the drawing context of the canvas </param>
        public delegate void RenderFunction(DrawingContext dc);

        public DrawCanvas()
        {
            //
        }

        /// <summary>
        /// Delegate used for drawing onto a DrawCanvas
        /// </summary>
        public RenderFunction ToDraw;

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            ToDraw(dc);
        }
    }
}
