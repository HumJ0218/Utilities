using System.Collections.Generic;

namespace HumJ.Utilities
{
    public static partial class GIS
    {
        public enum Coordinate
        {
            BD09,
            BJ54,
            GCJ02,
            WGS84,
            XA80
        };

        public static Dictionary<Coordinate, (double a, double f)> CoordinateArguments { get; } = new Dictionary<Coordinate, (double a, double f)> {
            { Coordinate.BJ54, (6378245.0, 1.0 / 298.3) },
            { Coordinate.WGS84, (6378137, 1 / 298.257223563) },
            { Coordinate.XA80, (6378140, 1 / 298.257) }
        };
    }
}