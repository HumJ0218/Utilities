using System;

namespace HumJ.Utilities
{
    public static partial class GIS
    {
        public static (int x, int y) GIS_GetTileXY(this (double lat, double lon) point, int z)
        {
            double lat = point.lat;
            double lon = point.lon;

            double lat_rad = lat * Math.PI / 180;

            double x = Math.Floor((lon + 180) / 360 * Math.Pow(2, z));
            double y = Math.Floor((1 - Math.Log(Math.Tan(lat_rad) + 1 / Math.Cos(lat_rad)) / Math.PI) * Math.Pow(2, z - 1));

            return ((int)x, (int)y);
        }
    }
}