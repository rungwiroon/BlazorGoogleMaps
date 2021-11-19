using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    public class MapType
    {
        public Size TileSize { get; set; }

        public int MaxZoom { get; set; }

        public int MinZoom { get; set; } = 0;

        public string? Name { get; set; }

        public string? Alt { get; set; }

        public ElementReference GetTile(Point tileCoord, int zoom, IJSObjectReference ownerDocument)
        {
            throw new NotImplementedException();
        }

        public void ReleaseTile(ElementReference tile)
        {
            throw new NotImplementedException();
        }
    }
}
