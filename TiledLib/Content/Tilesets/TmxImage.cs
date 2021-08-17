namespace TiledLib.Content.Tilesets
{
    using System.Xml.Serialization;
    
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Doc: https://doc.mapeditor.org/en/stable/reference/tmx-map-format/#image
    /// </remarks>
    [XmlRoot("image")]
    public record TmxImage
    {
        /// <summary>
        /// Used for embedded images, in combination with a data child element.
        /// 
        /// Valid values are file extensions like png, gif, jpg, bmp, etc.
        /// </summary>
        [XmlAttribute("format")]
        public string Format { get; init; }

        /// <summary>
        /// The reference to the tileset image file (Tiled supports most common image formats).
        /// 
        /// Only used if the image is not embedded.
        /// </summary>
        [XmlAttribute("source")]
        public string Source { get; init; }

        /// <summary>
        /// Defines a specific color that is treated as transparent
        /// 
        /// (example value: “#FF00FF” for magenta). Including the “#” is optional
        /// and Tiled leaves it out for compatibility reasons. (optional)
        /// </summary>
        [XmlAttribute("trans")]
        public string TransparentColor { get; init; }

        /// <summary>
        /// The image width in pixels (optional, used for tile index correction when the image changes)
        /// </summary>
        [XmlAttribute("width")]
        public int Width { get; init; }

        /// <summary>
        /// The image height in pixels (optional)
        /// </summary>
        [XmlAttribute("height")]
        public int Height { get; init; }
    }
}
