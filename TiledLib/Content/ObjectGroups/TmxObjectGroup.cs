namespace TiledLib.Content.ObjectGroups
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using TiledLib.Objects;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Doc: https://doc.mapeditor.org/en/stable/reference/tmx-map-format/#objectgroup
    /// </remarks>
    public record TmxObjectGroup
    {
        /// <summary>
        /// Unique ID of the layer.Each layer that added to a map gets a unique id.Even if a layer is deleted, no layer ever gets the same ID.Can not be changed in Tiled. (since Tiled 1.2)
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; init; }

        /// <summary>
        /// The name of the object group. (defaults to “”)
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; init; }

        /// <summary>
        /// The color used to display the objects in this group. (defaults to gray(“#a0a0a4”))
        /// </summary>
        [XmlAttribute("color")]
        public string Color { get; init; }

        /// <summary>
        /// The x coordinate of the object group in tiles.Defaults to 0 and can no longer be changed in Tiled.
        /// </summary>
        [XmlAttribute("x")]
        public int X { get; init; }

        /// <summary>
        /// The y coordinate of the object group in tiles.Defaults to 0 and can no longer be changed in Tiled.
        /// </summary>
        [XmlAttribute("y")]
        public int Y { get; init; }

        /// <summary>
        /// The width of the object group in tiles.Meaningless.
        /// </summary>
        [XmlAttribute("width")]
        public int Width { get; init; }

        /// <summary>
        /// The height of the object group in tiles.Meaningless.
        /// </summary>
        [XmlAttribute("height")]
        public int Height { get; init; }

        /// <summary>
        /// The opacity of the layer as a value from 0 to 1. (defaults to 1)
        /// </summary>
        [XmlAttribute("opacity")]
        public float Opacity { get; init; }

        /// <summary>
        /// Whether the layer is shown(1) or hidden(0). (defaults to 1)
        /// </summary>
        [XmlAttribute("visible")]
        public int Visible { get; init; }

        /// <summary>
        /// A color that is multiplied with any tile objects drawn by this layer, in #AARRGGBB or #RRGGBB format (optional).
        /// </summary>
        [XmlAttribute("tintcolor")]
        public string TintColor { get; init; }

        /// <summary>
        /// Horizontal offset for this object group in pixels. (defaults to 0) (since 0.14)
        /// </summary>
        [XmlAttribute("offsetx")]
        public int OffsetX { get; init; }

        /// <summary>
        /// Vertical offset for this object group in pixels. (defaults to 0) (since 0.14)
        /// </summary>
        [XmlAttribute("offsety")]
        public int OffsetY { get; init; }

        /// <summary>
        /// Whether the objects are drawn according to the order of appearance(“index”)
        /// or sorted by their y-coordinate(“topdown”). (defaults to “topdown”)
        /// </summary>
        [XmlAttribute("draworder")]
        public string DrawOrder { get; init; }

        public Properties.TmxProperties Properties { get; init; }

        public List<BaseObject> Objects { get; init; } = new List<BaseObject>();
    }
}