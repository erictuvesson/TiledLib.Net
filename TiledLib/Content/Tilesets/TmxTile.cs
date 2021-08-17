namespace TiledLib.Content.Tilesets
{
    using System.Xml.Serialization;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Doc: https://doc.mapeditor.org/en/stable/reference/tmx-map-format/#tile
    /// </remarks>
    [XmlRoot("tile")]
    public record TmxTile
    {
        /// <summary>
        /// The local tile ID within its tileset.
        /// </summary>
        [XmlAttribute("id")]
        public int Id { get; init; }

        /// <summary>
        /// The type of the tile.
        /// 
        /// Refers to an object type and is used by tile objects. (optional) (since 1.0)
        /// </summary>
        [XmlAttribute("type")]
        public float Type { get; init; } = 1.0f;

        /// <summary>
        /// Defines the terrain type of each corner of the tile,
        /// given as comma-separated indexes in the terrain types array in the order
        /// top-left, top-right, bottom-left, bottom-right.
        /// 
        /// Leaving out a value means that corner has no terrain. (optional)
        /// </summary>
        [XmlAttribute("terrain")]
        public int[] Terrain { get; init; }

        /// <summary>
        /// A percentage indicating the probability that this tile is chosen when it
        /// competes with others while editing with the terrain tool. (defaults to 0)
        /// </summary>
        [XmlAttribute("probability")]
        public float Probability { get; init; } = 0.0f;

        public ObjectGroups.TmxObjectGroup ObjectGroups { get; init; }
    }
}
