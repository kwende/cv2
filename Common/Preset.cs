using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Preset
    {
		public string Game { get; set; } = string.Empty;
		public string Character { get; set; } = string.Empty; 
		public PresetTileSize TileSize { get; set; } = new(); 
		public Dictionary<string, PresetPalette> Palettes { get; set; } = new(); 
		public List<PresetCompositeTile> Presets { get; set; } = new();
	}
}
