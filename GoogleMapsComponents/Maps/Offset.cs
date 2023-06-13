using System.Collections.Generic;

namespace GoogleMapsComponents.Maps;

public class Offset : List<int>
{
    public Offset(int y, int x)
    {
        Add(y);
        Add(x);
    }
}