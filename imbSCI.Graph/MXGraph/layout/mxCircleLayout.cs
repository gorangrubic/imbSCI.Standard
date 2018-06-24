//using java.util.ArrayList;
//using java.util.List;

using com.mxgraph;
using com.mxgraph;

using System;
using System.Collections.Generic;

namespace imbSCI.Graph.MXGraph.layout
{
    public class mxCircleLayout : mxIGraphLayout
    {
        /// <summary>
        /// Holds the enclosing graph.
        /// </summary>
        protected mxGraph graph;

        /**
         * Integer specifying the size of the radius. Default is 100.
         */
        protected double radius;

        /**
         * Boolean specifying if the circle should be moved to the top,
         * left corner specified by x0 and y0. Default is false.
         */
        protected Boolean moveCircle = true;

        /**
         * Integer specifying the left coordinate of the circle.
         * Default is 0.
         */
        protected double x0 = 0;

        /**
         * Integer specifying the top coordinate of the circle.
         * Default is 0.
         */
        protected double y0 = 0;

        /**
         * Specifies if all edge points of traversed edges should be removed.
         * Default is true.
         */
        protected Boolean resetEdges = false;

        /**
         *  Specifies if the STYLE_NOEDGESTYLE flag should be set on edges that are
         * modified by the result. Default is true.
         */
        protected Boolean disableEdgeStyle = true;

        /**
         * Constructs a new stack layout layout for the specified graph,
         * spacing, orientation and offset.
         */
        public mxCircleLayout(mxGraph graph)
        {
            radius = 100;
            this.graph = graph;
        }

        /**
         * Constructs a new stack layout layout for the specified graph,
         * spacing, orientation and offset.
         */
        public mxCircleLayout(mxGraph graph, double radius)
        {
            this.graph = graph;
            this.radius = radius;
        }

        /**
         * @return the radius
         */
        public double getRadius()
        {
            return radius;
        }

        /**
         * @param radius the radius to set
         */
        public void setRadius(double radius)
        {
            this.radius = radius;
        }

        /**
         * @return the moveCircle
         */
        public Boolean isMoveCircle()
        {
            return moveCircle;
        }

        /**
         * @param moveCircle the moveCircle to set
         */
        public void setMoveCircle(Boolean moveCircle)
        {
            this.moveCircle = moveCircle;
        }

        /**
         * @return the x0
         */
        public double getX0()
        {
            return x0;
        }

        /**
         * @param x0 the x0 to set
         */
        public void setX0(double x0)
        {
            this.x0 = x0;
        }

        /**
         * @return the y0
         */
        public double getY0()
        {
            return y0;
        }

        /**
         * @param y0 the y0 to set
         */
        public void setY0(double y0)
        {
            this.y0 = y0;
        }

        /**
         * @return the resetEdges
         */
        public Boolean isResetEdges()
        {
            return resetEdges;
        }

        /**
         * @param resetEdges the resetEdges to set
         */
        public void setResetEdges(Boolean resetEdges)
        {
            this.resetEdges = resetEdges;
        }

        /**
         * @return the disableEdgeStyle
         */
        public Boolean isDisableEdgeStyle()
        {
            return disableEdgeStyle;
        }

        /**
         * @param disableEdgeStyle the disableEdgeStyle to set
         */
        public void setDisableEdgeStyle(Boolean disableEdgeStyle)
        {
            this.disableEdgeStyle = disableEdgeStyle;
        }

        public void execute(Object parent)
        {
            mxIGraphModel model = graph.Model; //.GetModel();

            // Moves the vertices to build a circle. Makes sure the
            // radius is large enough for the vertices to not
            // overlap
            model.BeginUpdate();
            try
            {
                // Gets all vertices inside the parent and finds
                // the maximum dimension of the largest vertex
                double max = 0;
                Double top = 0;
                Double left = 0;
                List<Object> vertices = new List<Object>();
                int childCount = model.GetChildCount(parent);

                for (int i = 0; i < childCount; i++)
                {
                    Object cell = model.GetChildAt(parent, i);

                    if (!isVertexIgnored(cell))
                    {
                        vertices.add(cell);
                        mxRectangle bounds = getVertexBounds(cell);

                        if (top == null)
                        {
                            top = bounds.getY();
                        }
                        else
                        {
                            top = Math.Min(top, bounds.getY());
                        }

                        if (left == null)
                        {
                            left = bounds.getX();
                        }
                        else
                        {
                            left = Math.Min(left, bounds.getX());
                        }

                        max = Math.Max(max, Math.Max(bounds.getWidth(), bounds
                                .getHeight()));
                    }
                    else if (!isEdgeIgnored(cell))
                    {
                        if (isResetEdges())
                        {
                            graph.resetEdge(cell);
                        }

                        if (isDisableEdgeStyle())
                        {
                            setEdgeStyleEnabled(cell, false);
                        }
                    }
                }

                int vertexCount = vertices.size();
                double r = Math.Max(vertexCount * max / Math.PI, radius);

                // Moves the circle to the specified origin
                if (moveCircle)
                {
                    left = x0;
                    top = y0;
                }

                circle(vertices.ToArray(), r, left, top);
            }
            finally
            {
                model.EndUpdate();
            }
        }

        /**
         * Executes the circular layout for the specified array
         * of vertices and the given radius.
         */
        public void circle(Object[] vertices, double r, double left, double top)
        {
            int vertexCount = vertices.Length;
            double phi = 2 * Math.PI / vertexCount;

            for (int i = 0; i < vertexCount; i++)
            {
                if (isVertexMovable(vertices[i]))
                {
                    setVertexLocation(vertices[i],
                            left + r + r * Math.Sin(i * phi), top + r + r
                                    * Math.Cos(i * phi));
                }
            }
        }
    }
}