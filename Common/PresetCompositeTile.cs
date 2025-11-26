using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class PresetCompositeTile
    {
		public string Id { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public int CompositeWidth { get; set; } = new();
		public int CompositeHeight { get; set; } = new();	
		public string Palette { get; set; } = string.Empty;	
		public List<PresetTile> Tiles { get; set; } = new();
	}
}
