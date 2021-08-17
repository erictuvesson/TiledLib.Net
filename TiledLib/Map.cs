using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using TiledLib.Layer;

namespace TiledLib
{
    [XmlRoot("map")]
    public class Map : IXmlSerializable
    {
        /// <summary>
        /// The TMX format version. Was “1.0” so far, and will be incremented to match minor Tiled releases.
        /// </summary>
        [JsonRequired]
        [JsonProperty("version")]
        public string Version { get; set; } = "1.0";

        /// <summary>
        /// The Tiled version used to save the file (since Tiled 1.0.1). May be a date (for snapshot builds). (optional)
        /// </summary>
        [JsonProperty("tiledversion")]
        public string TiledVersion { get; set; }

        /// <summary>
        /// Map orientation. Tiled supports “orthogonal”, “isometric”, “staggered” and “hexagonal” (since 0.11).
        /// </summary>
        [JsonRequired]
        [JsonProperty("orientation")]
        public Orientation Orientation { get; set; }

        /// <summary>
        /// The order in which tiles on tile layers are rendered. Valid values are right-down (the default),
        /// right-up, left-down and left-up. In all cases, the map is drawn row-by-row.
        /// (only supported for orthogonal maps at the moment)
        /// </summary>
        [JsonProperty("renderorder")]
        public RenderOrder RenderOrder { get; set; }

        /// <summary>
        /// Gets or sets the map width in tiles.
        /// </summary>
        [JsonProperty("width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the map height in tiles.
        /// </summary>
        [JsonProperty("height")]
        public int Height { get; set; }

        #region Grid
        [JsonProperty("tilewidth")]
        public int CellWidth { get; set; }
        [JsonProperty("tileheight")]
        public int CellHeight { get; set; }
        #endregion

        /// <summary>
        /// Only for hexagonal maps. Determines the width or height (depending on the staggered axis) of the tile’s edge, in pixels.
        /// </summary>
        [JsonProperty("hexsidelength", NullValueHandling = NullValueHandling.Ignore)]
        public int? HexSideLength { get; set; }

        /// <summary>
        /// Whether this map is infinite. An infinite map has no fixed size and can grow in all directions.
        /// Its layer data is stored in chunks. (0 for false, 1 for true, defaults to 0)
        /// </summary>
        [JsonProperty("infinite")]
        public bool Infinite { get; set; }

        [JsonProperty("nextlayerid")]
        public int NextLayerId { get; set; }

        [JsonProperty("nextobjectid")]
        public int NextObjectId { get; set; }

        [JsonProperty("layers")]
        public BaseLayer[] Layers { get; set; }

        [JsonProperty("tilesets")]
        public Tileset[] Tilesets { get; set; }

        [JsonProperty("staggeraxis", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public StaggerAxis StaggerAxis { get; set; }

        [JsonProperty("staggerindex", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public StaggerIndex StaggerIndex { get; set; }

        [JsonProperty("backgroundcolor")]
        public string BackgroundColor { get; set; }

        [JsonProperty("properties")]
        [JsonConverter(typeof(PropertiesConverter))]
        public Dictionary<string, string> Properties { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Parses map from JSON stream
        /// </summary>
        /// <param name="stream">JSON stream</param>
        /// <returns>Tiled Map</returns>
        public static Map FromStream(Stream stream, Func<Tileset, Stream> tsLoader = null)
        {
            using (var reader = new StreamReader(stream, System.Text.Encoding.UTF8, true, 1024, true))
            {
                // var map = reader.ContainsJson() ? reader.ReadJsonMap() : reader.ReadTmxMap();
                var map = reader.ReadTmxMap();

                if (tsLoader != null)
                {
                    for (int i = 0; i < map.Tilesets.Length; i++)
                    {
                        using var memoryStream = new MemoryStream();
                        using (var tilesetStream = tsLoader(map.Tilesets[i]))
                            tilesetStream.CopyTo(memoryStream);
                        memoryStream.Position = 0;

                        var newTileset = Tileset.FromStream(memoryStream);
                        newTileset.FirstGid = map.Tilesets[i].FirstGid;
                        newTileset.Source = map.Tilesets[i].Source;
                        map.Tilesets[i] = newTileset;
                    }
                }

                return map;
            }
        }

        public XmlSchema GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            reader.ReadMapAttributes(this);
            reader.ReadMapElements(this);
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteMapAttributes(this);
            writer.WriteMapElements(this);
            //HACK: throw new NotImplementedException();
        }
    }
}