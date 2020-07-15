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

namespace HumJ.Utilities
{
    public static class GIS
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
        public static double distance((double lat, double lon) a, (double lat, double lon) b)
        {
            Func<double, double> hav = new Func<double, double>((double θ) =>
            {
                return Math.Pow(Math.Sin(θ / 2), 2);
            });

            (double lat, double lon) Δ = _coord_diff(a, b);

            return 2 * EARTH_R * Math.Asin(Math.Sqrt(
                hav(Δ.lat * Math.PI / 180) +
                    Math.Cos(a.lat * Math.PI / 180) *
                    Math.Cos(b.lat * Math.PI / 180) *
                    hav(Δ.lon * Math.PI / 180)
            ));
        }

        public static (double lat, double lon) wgs_gcj((double lat, double lon) wgs, bool checkChina = true)
        {
            if (checkChina && !sanity_in_china_p(wgs))
            {
                Console.WriteLine($"Non-Chinese coords found, returning as-is: {_stringify(wgs)}");
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
            double dLat_m = -100 + 2 * x + 3 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.Sqrt(Math.Abs(x)) + (2 * Math.Sin(x * 6 * Math.PI) + 2 * Math.Sin(x * 2 * Math.PI) + 2 * Math.Sin(y * Math.PI) + 4 * Math.Sin(y / 3 * Math.PI) + 16 * Math.Sin(y / 12 * Math.PI) + 32 * Math.Sin(y / 30 * Math.PI)) * 20 / 3;
            double dLon_m = 300 + x + 2 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.Sqrt(Math.Abs(x)) + (2 * Math.Sin(x * 6 * Math.PI) + 2 * Math.Sin(x * 2 * Math.PI) + 2 * Math.Sin(x * Math.PI) + 4 * Math.Sin(x / 3 * Math.PI) + 15 * Math.Sin(x / 12 * Math.PI) + 30 * Math.Sin(x / 30 * Math.PI)) * 20 / 3;

            double radLat = wgs.lat / 180 * Math.PI;
            double magic = 1 - GCJ_EE * Math.Pow(Math.Sin(radLat), 2); // just a common expr

            // [[:en:Latitude#Length_of_a_degree_of_latitude]]
            double lat_deg_arclen = (Math.PI / 180) * (GCJ_A * (1 - GCJ_EE)) / Math.Pow(magic, 1.5);
            // [[:en:Longitude#Length_of_a_degree_of_longitude]]
            double lon_deg_arclen = (Math.PI / 180) * (GCJ_A * Math.Cos(radLat) / Math.Sqrt(magic));

            // The screwers pack their deviations into degrees and disappear.
            // Note how they are mixing WGS-84 and Krasovsky 1940 ellipsoids here...
            return (lat: wgs.lat + (dLat_m / lat_deg_arclen), lon: wgs.lon + (dLon_m / lon_deg_arclen));
        }

        // rev_transform_rough; accuracy ~2e-6 deg (meter-level)
        public static (double lat, double lon) gcj_wgs((double lat, double lon) gcj, bool checkChina = true)
        {
            return _coord_diff(gcj, _coord_diff(wgs_gcj(gcj, checkChina), gcj));
        }

        public static (double lat, double lon) gcj_bd((double lat, double lon) gcj) => gcj_bd_d(gcj);

        // Yes, we can implement a "precise" one too.
        // accuracy ~1e-7 deg (decimeter-level; exceeds usual data accuracy)
        public static (double lat, double lon) bd_gcj((double lat, double lon) bd) => bd_gcj_d(bd);

        public static (double lat, double lon) bd_wgs((double lat, double lon) bd, bool checkChina = true)
        {
            return gcj_wgs(bd_gcj(bd), checkChina);
        }

        public static (double lat, double lon) wgs_bd((double lat, double lon) bd, bool checkChina = true)
        {
            return gcj_bd(wgs_gcj(bd, checkChina));
        }

        // generic "bored function" factory, Caijun 2014
        // gcj: 4 calls to wgs_gcj; ~0.1mm acc
        public static Func<(double lat, double lon), bool, (double lat, double lon)> __bored__(Func<(double lat, double lon), bool, (double lat, double lon)> fwd, Func<(double lat, double lon), bool, (double lat, double lon)> rev)
        {
            return new Func<(double lat, double lon), bool, (double lat, double lon)>(((double lat, double lon) heck, bool checkChina) =>
            {
                (double lat, double lon) curr = rev(heck, checkChina);
                (double lat, double lon) diff = (lat: double.PositiveInfinity, lon: double.PositiveInfinity);

                // Wait till we hit fixed point or get bored
                int i = 0;

                while (Math.Max(Math.Abs(diff.lat), Math.Abs(diff.lon)) > PRC_EPS && i++ < 10)
                {
                    diff = _coord_diff(fwd(curr, checkChina), heck);
                    curr = _coord_diff(curr, diff);
                }

                return curr;
            });
        }

        // Precise functions using caijun 2014 method
        //
        // Why "bored"? Because they usually exceed source data accuracy -- the
        // original GCJ implementation contains noise from a linear-modulo PRNG,
        // and Baidu seems to do similar things with their API too.

        public static Func<(double lat, double lon), bool, (double lat, double lon)> gcj_wgs_bored = __bored__(wgs_gcj, gcj_wgs);
        public static Func<(double lat, double lon), bool, (double lat, double lon)> bd_gcj_bored = __bored__(gcj_bd_d, bd_gcj_d);
        public static Func<(double lat, double lon), bool, (double lat, double lon)> bd_wgs_bored = __bored__(wgs_bd, bd_wgs);

        private static bool sanity_in_china_p((double lat, double lon) coords)
        {
            return coords.lat >= 0.8293 && coords.lat <= 55.8271 && coords.lon >= 72.004 && coords.lon <= 137.8347;
        }

        private static (double lat, double lon) _coord_diff((double lat, double lon) a, (double lat, double lon) b)
        {
            return (lat: a.lat - b.lat, lon: a.lon - b.lon);
        }

        private static string _stringify((double lat, double lon) c)
        {
            return $"({c.lat}, {c.lon})";
        }

        private static (double lat, double lon) gcj_bd_d((double lat, double lon) gcj, bool dummyArgument = true)
        {
            double x = gcj.lon;
            double y = gcj.lat;

            // trivia: pycoordtrans actually describes how these values are calculated
            double r = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * Math.PI * 3000 / 180);
            double θ = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * Math.PI * 3000 / 180);

            // Hard-coded default deviations again!
            return (lat: r * Math.Sin(θ) + BD_DLAT, lon: r * Math.Cos(θ) + BD_DLON);
        }

        // Yes, we can implement a "precise" one too.
        // accuracy ~1e-7 deg (decimeter-level; exceeds usual data accuracy)
        private static (double lat, double lon) bd_gcj_d((double lat, double lon) bd, bool dummyArgument = true)
        {
            double x = bd.lon - BD_DLON;
            double y = bd.lat - BD_DLAT;

            // trivia: pycoordtrans actually describes how these values are calculated
            double r = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * Math.PI * 3000 / 180);
            double θ = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * Math.PI * 3000 / 180);

            return (lat: r * Math.Sin(θ), lon: r * Math.Cos(θ));
        }
    }
}