using System;
using System.Xml.Serialization;

namespace imbSCI.Graph.Graphics.SvgAPI.Containers
{
    [XmlRoot("marker")]
    public class svgMarker : svgContainerElement
    {
        public override String name { get { return "marker"; } }

        /// <summary>
        /// Creates new marker object with 10x10 dimensions and refX/refY at center
        /// </summary>
        /// <param name="_id">The identifier.</param>
        /// <param name="size">The size.</param>
        public svgMarker(String _id, Int32 size = 10)
        {
            id = _id;
            point.width = size;
            point.height = size;

            DeployAttributes();
        }

        /// <summary>
        /// Deploys refX, refX, orient, markerWidth, marketHeight ... attibutes
        /// </summary>
        public void DeployAttributes()
        {
            attributes.Set("refY", point.yCenter.ToString());
            attributes.Set("refX", point.xCenter.ToString());

            attributes.Set("orient", "auto");

            attributes.Set("markerWidth", point.width.ToString());
            attributes.Set("markerHeight", point.height.ToString());

            SetViewBox(point);
        }
    }
}