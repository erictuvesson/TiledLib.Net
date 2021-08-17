using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using TiledLib.Content.Tilesets;

namespace TiledLib
{
    [XmlRoot("tileset")]
    public class Tileset : ITileset, IXmlSerializable
    {
        /// <summary>
        /// Gets or sets whether this should be saved as external or internal when saving <see cref="Tileset"/>.
        /// </summary>
        [XmlIgnore]
        public bool External { get; set; } = true;

        [XmlIgnore]
        public string Source { get; set; }

        /// <summary>
        /// The first global tile ID of this tileset (this global ID maps
        /// to the first tile in this tileset).
        /// 
        /// source: If this tileset is stored in an external TSX (Tile Set XML) file,
        /// this attribute refers to that file. That TSX file has the same structure
        /// as the <tileset> element described here. (There is the firstgid attribute
        /// missing and this source attribute is also not there.These two attributes
        /// are kept in the TMX map, since they are map specific.)
        /// </summary>
        public int FirstGid { get; set; }

        /// <summary>
        /// The name of this tileset.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// The margin around the tiles in this tileset
        /// (applies to the tileset image, defaults to 0)
        /// </summary>
        public int Margin { get; set; } = 0;

        /// <summary>
        /// The spacing in pixels between the tiles in this tileset
        /// (applies to the tileset image, defaults to 0)
        /// </summary>
        [XmlAttribute("spacing")]
        public int Spacing { get; set; } = 0;

        /// <summary>
        /// The (maximum) width of the tiles in this tileset.
        /// </summary>
        [XmlAttribute("tilewidth")]
        public int TileWidth { get; set; }

        /// <summary>
        /// The (maximum) height of the tiles in this tileset.
        /// </summary>
        [XmlAttribute("tileheight")]
        public int TileHeight { get; set; }

        [XmlElement("image")]
        public TmxImage Image { get; set; } = new TmxImage();

        #region JSON TilesetImage
        [JsonProperty("image")]
        [Obsolete("Use Image.Source")]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string ImagePath
        {
            get => Image.Source;
            set => Image = Image with { Source = value };
        }

        [Obsolete("Use Image.Width")]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int ImageWidth
        {
            get => Image.Width;
            set => Image = Image with { Width = value };
        }

        [Obsolete("Use Image.Height")]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int ImageHeight
        {
            get => Image.Height;
            set => Image = Image with { Height = value };
        }

        [Obsolete("Use Image.TransparentColor")]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string TransparentColor
        {
            get => Image.TransparentColor;
            set => Image = Image with { TransparentColor = value };
        }
        #endregion

        public Grid Grid { get; set; }

        [JsonProperty("tileoffset")]
        public TileOffset TileOffset { get; set; }

        [JsonProperty("properties")]
        [JsonConverter(typeof(PropertiesConverter))]
        [XmlIgnore]
        public Dictionary<string, string> Properties { get; } = new Dictionary<string, string>();

        [JsonProperty("tileproperties")]
        [XmlIgnore]
        public Dictionary<int, Dictionary<string, string>> TileProperties { get; } = new Dictionary<int, Dictionary<string, string>>();

        [XmlIgnore]
        public Dictionary<int, TmxTile> Tiles { get; set; } = new Dictionary<int, TmxTile>();

        [JsonIgnore] //TODO: Add json support
        [XmlIgnore]
        public Dictionary<int, Frame[]> TileAnimations { get; } = new Dictionary<int, Frame[]>();

        public Tile this[int gid]
        {
            get
            {
                if (gid == 0)
                    return default;

                var orientation = Utils.GetOrientation(gid);

                var columns = Columns;
                var rows = Rows;
                var index = Utils.GetId(gid) - FirstGid;
                if (index < 0 || index >= rows * columns)
                    throw new ArgumentOutOfRangeException();

                var row = index / columns;

                return new Tile
                {
                    Top = row * (TileHeight + Spacing) + Margin,
                    Left = (index - row * columns) * (TileWidth + Spacing) + Margin,
                    Width = TileWidth,
                    Height = TileHeight,
                    Orientation = orientation
                };
            }
        }

        public string this[int gid, string property]
        {
            get
            {
                gid = Utils.GetId(gid);
                return gid != 0
                       && TileProperties.TryGetValue(gid - FirstGid, out var tile)
                       && tile.TryGetValue(property, out var value) ? value : default;
            }
        }

        public int Columns => (Image.Width + Spacing - Margin * 2) / (TileWidth + Spacing);
        public int Rows => (Image.Height + Spacing - Margin * 2) / (TileHeight + Spacing);
        public int TileCount => Columns * Rows;

        public static Tileset FromStream(System.IO.Stream stream)
        {
            using (var reader = new System.IO.StreamReader(stream))
            {
                if (Utils.ContainsJson(reader))
                    return (Tileset)Utils.JsonSerializer.Deserialize(reader, typeof(Tileset));
                else
                    return (Tileset)new XmlSerializer(typeof(Tileset)).Deserialize(reader);
            }
        }

        public XmlSchema GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            reader.ReadTileset(this);
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}