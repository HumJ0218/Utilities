using System;

namespace HumJ.Utilities
{
    public static partial class GIS
    {
        public static (int tileX, int tileY, double tx, double ty) GetGisTileInfo(this (double lat, double lon) point, int z)
        {
            double lat = point.lat;
            double lon = point.lon;

            double lat_rad = lat * Math.PI / 180;

            double x = (lon + 180) / 360 * Math.Pow(2, z);
            double y = (1 - Math.Log(Math.Tan(lat_rad) + 1 / Math.Cos(lat_rad)) / Math.PI) * Math.Pow(2, z - 1);

            int tileX = (int)x;
            int tileY = (int)y;

            double tx = (x - tileX) * 256;
            double ty = (y - tileY) * 256;

            return (tileX, tileY, tx, ty);
        }
    }
}
