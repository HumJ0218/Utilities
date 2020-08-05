// https://github.com/Artoria2e5/PRCoords/blob/master/js/PRCoords.js

// <nowiki>
/**
 * People's Rectified Coordinates
 * @file Utils for inserting valid WGS-84 coords from GCJ-02/BD-09 input
 * @author User:Artoria2e5
 * @url https://github.com/Artoria2e5/PRCoords
 * 
 * @see [[:en:GCJ-02]]
 * @see https://en.wikipedia.org/wiki/User:Artoria2e5/coord-notice
 * @see https://github.com/caijun/geoChina (GPLv3)
 * @see https://github.com/googollee/eviltransform (MIT)
 * @see https://on4wp7.codeplex.com/SourceControl/changeset/view/21483#353936 (Anonymous)
 * @see https://github.com/zxteloiv/pycoordtrans (BSD-3)
 * 
 * @license CC0
 * To the greatest extent possible, this implementation of obfuscations designed
 * in hope that they will screw y'all up is dedicated into the public domain
 * under CC0 1.0 <https://creativecommons.org/publicdomain/zero/1.0/>.
 *
 * Happy geotagging/ingressing/whatever.
 * 
 * To make my FSF membership shine brighter, this conversion implementation is
 * additionally licensed under GPLv3+:
 * @license GPLv3+
 * @copyright 2016 Mingye Wang (User:Artoria2e5)
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Diagnostics;
using static System.Math;

namespace HumJ.Utilities
{
    public static partial class GIS
    {
        /// Krasovsky 1940 ellipsoid
        /// @const
        private static readonly double GCJ_A = 6378245;
        private static readonly double GCJ_EE = 0.00669342162296594323; // f = 1/298.3; e^2 = 2*f - f**2

        /// Epsilon to use for "exact" iterations.
        /// Wanna troll? Use Number.EPSILON. 1e-13 in 15 calls for gcj.
        /// @const
        private static readonly double PRC_EPS = 1e-5;

        /// Baidu's artificial deviations
        /// @const
        private static readonly double BD_DLAT = 0.0060;
        private static readonly double BD_DLON = 0.0065;

        /// Mean Earth Radius
        /// @const
        private static readonly double EARTH_R = 6371000;

        /// Distance for haversine method; suitable over short distances like
        /// conversion deviation checking
        public static double Distance((double lat, double lon) a, (double lat, double lon) b)
        {
            Func<double, double> hav = new Func<double, double>((double θ) =>
            {
                return Pow(Sin(θ / 2), 2);
            });

            (double lat, double lon) = CoordDiff(a, b);

            return 2 * EARTH_R * Asin(Sqrt(
                hav(lat * PI / 180) +
                    Cos(a.lat * PI / 180) *
                    Cos(b.lat * PI / 180) *
                    hav(lon * PI / 180)
            ));
        }

        public static (double lat, double lon) WGS84_GCJ02((double lat, double lon) wgs, bool checkChina = true)
        {
            if (checkChina && !SanityInChina_P(wgs))
            {
                Debug.WriteLine($"Non-Chinese coords found, returning as-is: {wgs}");
                return wgs;
            }

            double x = wgs.lon - 105;
            double y = wgs.lat - 35;

            // These distortion functions accept (x = lon - 105, y = lat - 35).
            // They return distortions in terms of arc lengths, in meters.
            //
            // In other words, you can pretty much figure out how much you will be off
            // from WGS-84 just through evaulating them...
            //
            // For example, at the (mapped) center of China (105E, 35N), you get a
            // default deviation of <300, -100> meters.
            double dLat_m = -100 + 2 * x + 3 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Sqrt(Abs(x)) + (2 * Sin(x * 6 * PI) + 2 * Sin(x * 2 * PI) + 2 * Sin(y * PI) + 4 * Sin(y / 3 * PI) + 16 * Sin(y / 12 * PI) + 32 * Sin(y / 30 * PI)) * 20 / 3;
            double dLon_m = 300 + x + 2 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Sqrt(Abs(x)) + (2 * Sin(x * 6 * PI) + 2 * Sin(x * 2 * PI) + 2 * Sin(x * PI) + 4 * Sin(x / 3 * PI) + 15 * Sin(x / 12 * PI) + 30 * Sin(x / 30 * PI)) * 20 / 3;

            double radLat = wgs.lat / 180 * PI;
            double magic = 1 - GCJ_EE * Pow(Sin(radLat), 2); // just a common expr

            // [[:en:Latitude#Length_of_a_degree_of_latitude]]
            double lat_deg_arclen = (PI / 180) * (GCJ_A * (1 - GCJ_EE)) / Pow(magic, 1.5);
            // [[:en:Longitude#Length_of_a_degree_of_longitude]]
            double lon_deg_arclen = (PI / 180) * (GCJ_A * Cos(radLat) / Sqrt(magic));

            // The screwers pack their deviations into degrees and disappear.
            // Note how they are mixing WGS-84 and Krasovsky 1940 ellipsoids here...
            return (lat: wgs.lat + (dLat_m / lat_deg_arclen), lon: wgs.lon + (dLon_m / lon_deg_arclen));
        }

        // rev_transform_rough; accuracy ~2e-6 deg (meter-level)
        public static (double lat, double lon) GCJ02_WGS84((double lat, double lon) gcj, bool checkChina = true)
        {
            return CoordDiff(gcj, CoordDiff(WGS84_GCJ02(gcj, checkChina), gcj));
        }

        public static (double lat, double lon) GCJ02_BD09((double lat, double lon) gcj)
        {
            return GCJ_BD_D(gcj);
        }

        // Yes, we can implement a "precise" one too.
        // accuracy ~1e-7 deg (decimeter-level; exceeds usual data accuracy)
        public static (double lat, double lon) BD09_GCJ02((double lat, double lon) bd)
        {
            return BD_GCJ_D(bd);
        }

        public static (double lat, double lon) BD09_WGS84((double lat, double lon) bd, bool checkChina = true)
        {
            return GCJ02_WGS84(BD09_GCJ02(bd), checkChina);
        }

        public static (double lat, double lon) WGS84_BD09((double lat, double lon) bd, bool checkChina = true)
        {
            return GCJ02_BD09(WGS84_GCJ02(bd, checkChina));
        }

        // generic "bored function" factory, Caijun 2014
        // gcj: 4 calls to wgs_gcj; ~0.1mm acc
        private static Func<(double lat, double lon), bool, (double lat, double lon)> Bored(Func<(double lat, double lon), bool, (double lat, double lon)> fwd, Func<(double lat, double lon), bool, (double lat, double lon)> rev)
        {
            return new Func<(double lat, double lon), bool, (double lat, double lon)>(((double lat, double lon) heck, bool checkChina) =>
            {
                (double lat, double lon) curr = rev(heck, checkChina);
                (double lat, double lon) diff = (lat: double.PositiveInfinity, lon: double.PositiveInfinity);

                // Wait till we hit fixed point or get bored
                int i = 0;

                while (Max(Abs(diff.lat), Abs(diff.lon)) > PRC_EPS && i++ < 10)
                {
                    diff = CoordDiff(fwd(curr, checkChina), heck);
                    curr = CoordDiff(curr, diff);
                }

                return curr;
            });
        }

        // Precise functions using caijun 2014 method
        //
        // Why "bored"? Because they usually exceed source data accuracy -- the
        // original GCJ implementation contains noise from a linear-modulo PRNG,
        // and Baidu seems to do similar things with their API too.

        public static Func<(double lat, double lon), bool, (double lat, double lon)> GCJ02_WGS84_bored = Bored(WGS84_GCJ02, GCJ02_WGS84);
        public static Func<(double lat, double lon), bool, (double lat, double lon)> BD09_GCJ02_bored = Bored(GCJ_BD_D, BD_GCJ_D);
        public static Func<(double lat, double lon), bool, (double lat, double lon)> BD09_WGS84_bored = Bored(WGS84_BD09, BD09_WGS84);

        private static bool SanityInChina_P((double lat, double lon) coords)
        {
            return coords.lat >= 0.8293 && coords.lat <= 55.8271 && coords.lon >= 72.004 && coords.lon <= 137.8347;
        }

        private static (double lat, double lon) CoordDiff((double lat, double lon) a, (double lat, double lon) b)
        {
            return (lat: a.lat - b.lat, lon: a.lon - b.lon);
        }

        private static (double lat, double lon) GCJ_BD_D((double lat, double lon) gcj, bool dummyArgument = true)
        {
            double x = gcj.lon;
            double y = gcj.lat;

            // trivia: pycoordtrans actually describes how these values are calculated
            double r = Sqrt(x * x + y * y) + 0.00002 * Sin(y * PI * 3000 / 180);
            double θ = Atan2(y, x) + 0.000003 * Cos(x * PI * 3000 / 180);

            // Hard-coded default deviations again!
            return (lat: r * Sin(θ) + BD_DLAT, lon: r * Cos(θ) + BD_DLON);
        }

        // Yes, we can implement a "precise" one too.
        // accuracy ~1e-7 deg (decimeter-level; exceeds usual data accuracy)
        private static (double lat, double lon) BD_GCJ_D((double lat, double lon) bd, bool dummyArgument = true)
        {
            double x = bd.lon - BD_DLON;
            double y = bd.lat - BD_DLAT;

            // trivia: pycoordtrans actually describes how these values are calculated
            double r = Sqrt(x * x + y * y) - 0.00002 * Sin(y * PI * 3000 / 180);
            double θ = Atan2(y, x) - 0.000003 * Cos(x * PI * 3000 / 180);

            return (lat: r * Sin(θ), lon: r * Cos(θ));
        }
    }
}