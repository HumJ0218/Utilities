// https://github.com/jiaguobing/CoordCheck/blob/master/CoordCheck/CoordCheckDlg.cpp

using System;

namespace HumJ.Utilities
{
    public static partial class GIS
    {
        public static (double x, double y) WGS84_BJ54((double lon, double lat) wgs, int E0)
        {
            (double lon, double lat) = (wgs.lon, wgs.lat);

            double sa, sb, sep, sn, sy2, st, sm, sx, hb;
            double xx, yy, hd, sd;
            double x, y;

            //判断值的范围
            if (lon > 360 || lon < 0 || lat > 360 || lat < 0)
            {
                x = lon;
                y = lat;
                return (x, y);
            }

            lon -= E0;
            sa = 6378245;
            sb = 6356863.019;
            sep = 0.006738525415;
            hd = lat * Math.PI;
            hb = hd / 180.0;
            st = Math.Tan(hb);

            sn = Math.Pow(sa, 2) / Math.Sqrt(Math.Pow(sa, 2) * Math.Pow(Math.Cos(hb), 2)
                + Math.Pow(sb, 2) * Math.Pow(Math.Sin(hb), 2));

            sy2 = sep * Math.Pow(Math.Cos(hb), 2);
            sd = Math.Cos(hb) * lon * Math.PI;
            sm = sd / 180.0;
            sx = 111134.861 * lat - (32005.78 * Math.Sin(hb) + 133.924 * Math.Pow(Math.Sin(hb), 3) + 0.697 * Math.Pow(Math.Sin(hb), 5)) * Math.Cos(hb);
            xx = sx + sn * st * (0.5 * Math.Pow(sm, 2) + 1.0 / 24.0 * (5.0 - Math.Pow(st, 2) + 9.0 * sy2) * Math.Pow(sm, 4));
            yy = sn * (sm + 1.0 / 6.0 * (1.0 - Math.Pow(st, 2) + sy2) * Math.Pow(sm, 3) + 1.0 / 120.0 * (5.0 - 18.0 * Math.Pow(st, 2) + Math.Pow(st, 4)) * Math.Pow(sm, 5));
            x = xx;
            y = yy + 500000;

            return (x, y);
        }

        public static (double lat, double lon) BJ54_WGS84((double x, double y)bj, double e0)
        {
            (double x, double y) = (bj.x, bj.y);

            double bf, vf, nf, ynf, tf, yf2, hbf;
            double sa, sb, se2, sep2, mf;
            double w1, w2, w, w3, w4;
            double lat, lon;

            x /= 1000000.0;
            y -= 500000.0;

            bf = 9.04353692458 * x - 0.00001007623 * Math.Pow(x, 2.0) - 0.00074438304 * Math.Pow(x, 3.0) - 0.00000463064 * Math.Pow(x, 4.0) + 0.00000505846 * Math.Pow(x, 5.0) - 0.00000016754 * Math.Pow(x, 6.0);
            hbf = bf * Math.PI / 180.0;
            sa = 6378245.0;
            sb = 6356863.019;
            //0.0066934216239182 0.0067385254156487
            se2 = 0.006693421623;
            sep2 = 0.006738525415;

            w1 = Math.Sin(hbf);
            w2 = 1.0 - se2 * Math.Pow(w1, 2);
            w = Math.Sqrt(w2);
            mf = sa * (1.0 - se2) / Math.Pow(w, 3);
            w3 = Math.Cos(hbf);

            w4 = Math.Pow(sa, 2) * Math.Pow(w3, 2) + Math.Pow(sb, 2) * Math.Pow(w1, 2);
            nf = Math.Pow(sa, 2) / Math.Sqrt(w4);

            ynf = y / nf;
            vf = nf / mf;
            tf = Math.Tan(hbf);

            yf2 = sep2 * Math.Pow(w3, 2);

            lat = bf - 1.0 / 2.0 * vf * tf * (Math.Pow(ynf, 2) - 1.0 / 12.0 * (5.0 + 3.0 * Math.Pow(tf, 2) + yf2 - 9.0 * yf2 * Math.Pow(tf, 2)) * Math.Pow(ynf, 4)) * 180.0 / Math.PI;
            lon = 1.0 / w3 * ynf * (1.0 - 1.0 / 6.0 * (1.0 + 2.0 * Math.Pow(tf, 2) + yf2) * Math.Pow(ynf, 2) + 1.0 / 120.0 * (5.0 + 28.0 * Math.Pow(tf, 2) + 24.0 * Math.Pow(tf, 2) + 6.0 * yf2 + 8.0 * yf2 * Math.Pow(tf, 2)) * Math.Pow(ynf, 4)) * 180.0 / Math.PI;
            lon = e0 + lon;

            return (lat, lon);
        }
    }
}