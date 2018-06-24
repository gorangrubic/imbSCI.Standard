using imbSCI.Graph.MXGraph.utils;
using imbSCI.Graph.MXGraph.view;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace imbSCI.Graph.MXGraph.canvas
{
    /// <summary>
    /// Implements a canvas that draws onto an image.
    /// </summary>
    internal class mxImageCanvas : mxICanvas
    {
        /// <summary>
        /// Inner canvas used for the actual rendering.
        /// </summary>
        protected mxGdiCanvas canvas;

        /// <summary>
        /// Stores the previous graphics reference of the inner canvas.
        /// </summary>
        protected System.Drawing.Graphics previousGraphics;

        /// <summary>
        /// Stores the image that holds the graphics.
        /// </summary>
        protected Image image;

        /// <summary>
        ///
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="background"></param>
        /// <param name="antiAlias"></param>
        public mxImageCanvas(mxGdiCanvas canvas, int width, int height,
            Color? background, bool antiAlias)
        {
            this.canvas = canvas;
            previousGraphics = canvas.Graphics;
            image = mxUtils.CreateImage(width, height, background);

            if (image != null)
            {
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);

                if (antiAlias)
                {
                    g.SmoothingMode = SmoothingMode.HighQuality;
                }

                canvas.Graphics = g;
            }
        }

        /// <summary>
        /// Returns the inner canvas.
        /// </summary>
        public mxGdiCanvas GdiCanvas
        {
            get { return canvas; }
        }

        /// <summary>
        /// Returns the image that hold the graphics.
        /// </summary>
        public Image Image
        {
            get { return image; }
        }

        /// <summary>
        /// see com.mxgraph.mxICanvas.Translate
        /// </summary>
        public Point Translate
        {
            get { return canvas.Translate; }
            set { canvas.Translate = value; }
        }

        /// <summary>
        /// see com.mxgraph.mxICanvas.Translate
        /// </summary>
        public double Scale
        {
            get { return canvas.Scale; }
            set { canvas.Scale = value; }
        }

        /// <summary>
        /// see com.mxgraph.mxICanvas.DrawCell()
        /// </summary>
        public Object DrawCell(mxCellState state)
        {
            return canvas.DrawCell(state);
        }

        /// <summary>
        /// see com.mxgraph.mxICanvas.DrawLabel()
        /// </summary>
        public Object DrawLabel(string text, mxCellState state, bool html)
        {
            return canvas.DrawLabel(text, state, html);
        }

        /// <summary>
        /// Destroys this canvas and all allocated resources.
        /// </summary>
        public Image Destroy()
        {
            Image tmp = image;

            canvas.Graphics.Dispose();
            canvas.Graphics = previousGraphics;

            previousGraphics = null;
            canvas = null;
            image = null;

            return tmp;
        }
    }
}