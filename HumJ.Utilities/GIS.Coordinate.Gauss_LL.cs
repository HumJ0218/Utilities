// https://download.csdn.net/download/hnjyzc/5839545
// https://www.cnblogs.com/mine2832/p/9970582.html

using System;

namespace HumJ.Utilities
{
    public static partial class GIS
    {
        public static (double lat, double lon) Gauss_LL((double x, double y) gauss, Coordinate coordinate, int zoneWide = 6)
        {
            (double x, double y) = (gauss.x, gauss.y);

            int ProjNo;
            double longitude1, latitude1, longitude0, X0, Y0, xval, yval;
            double e1, e2, f, a, ee, NN, T, C, M, D, R, u, fai, iPI = Math.PI / 180.0;

            (a, f) = CoordinateArguments[coordinate];

            ProjNo = (int)(x / 1000000L); // 查找带号
            longitude0 = (ProjNo - 1) * zoneWide + zoneWide / 2;
            longitude0 *= iPI; // 中央经线
            X0 = ProjNo * 1000000L + 500000L;
            Y0 = 0;
            xval = x - X0; yval = y - Y0; // 带内大地坐标
            e2 = 2 * f - f * f;
            e1 = (1.0 - Math.Sqrt(1 - e2)) / (1.0 + Math.Sqrt(1 - e2));
            ee = e2 / (1 - e2);
            M = yval;
            u = M / (a * (1 - e2 / 4 - 3 * e2 * e2 / 64 - 5 * e2 * e2 * e2 / 256));
            fai = u + (3 * e1 / 2 - 27 * e1 * e1 * e1 / 32) * Math.Sin(2 * u) + (21 * e1 * e1 / 16 - 55 * e1 * e1 * e1 * e1 / 32) * Math.Sin(4 * u) + (151 * e1 * e1 * e1 / 96) * Math.Sin(6 * u) + (1097 * e1 * e1 * e1 * e1 / 512) * Math.Sin(8 * u);
            C = ee * Math.Cos(fai) * Math.Cos(fai);
            T = Math.Tan(fai) * Math.Tan(fai);
            NN = a / Math.Sqrt(1.0 - e2 * Math.Sin(fai) * Math.Sin(fai));
            R = a * (1 - e2) / Math.Sqrt((1 - e2 * Math.Sin(fai) * Math.Sin(fai)) * (1 - e2 * Math.Sin(fai) * Math.Sin(fai)) * (1 - e2 * Math.Sin(fai) * Math.Sin(fai)));
            D = xval / NN;

            // 计算经度(Longitude) 纬度(Latitude)
            longitude1 = longitude0 + (D - (1 + 2 * T + C) * D * D * D / 6 + (5 - 2 * C + 28 * T - 3 * C * C + 8 * ee + 24 * T * T) * D * D * D * D * D / 120) / Math.Cos(fai);
            latitude1 = fai - (NN * Math.Tan(fai) / R) * (D * D / 2 - (5 + 3 * T + 10 * C - 4 * C * C - 9 * ee) * D * D * D * D / 24 + (61 + 90 * T + 298 * C + 45 * T * T - 256 * ee - 3 * C * C) * D * D * D * D * D * D / 720);

            // 转换为度 DD
            double lon = longitude1 / iPI;
            double lat = latitude1 / iPI;

            return (lat, lon);
        }

        public static (double x, double y) LL_Gauss((double lat, double lon) ll, Coordinate coordinate, int zoneWide = 6)
        {
            (double lat, double lon) = (ll.lat, ll.lon);

            double longitude1, latitude1, longitude0, X0, Y0, xval, yval;
            double a, f, e2, ee, NN, T, C, A, M, iPI = Math.PI / 180.0;

            (a, f) = CoordinateArguments[coordinate];

            int ProjNo = (int)(lon / zoneWide);

            longitude0 = ProjNo * zoneWide + zoneWide / 2; // 中央子午线

            longitude0 *= iPI; // 中央子午线转换为弧度

            longitude1 = lon * iPI; // 经度转换为弧度
            latitude1 = lat * iPI; // 纬度转换为弧度

            e2 = 2 * f - f * f;
            ee = e2 * (1.0 - e2);
            NN = a / Math.Sqrt(1.0 - e2 * Math.Sin(latitude1) * Math.Sin(latitude1));
            T = Math.Tan(latitude1) * Math.Tan(latitude1);
            C = ee * Math.Cos(latitude1) * Math.Cos(latitude1);
            A = (longitude1 - longitude0) * Math.Cos(latitude1);
            M = a * ((1 - e2 / 4 - 3 * e2 * e2 / 64 - 5 * e2 * e2 * e2 / 256) * latitude1 - (3 * e2 / 8 + 3 * e2 * e2 / 32 + 45 * e2 * e2 * e2 / 1024) * Math.Sin(2 * latitude1) + (15 * e2 * e2 / 256 + 45 * e2 * e2 * e2 / 1024) * Math.Sin(4 * latitude1) - (35 * e2 * e2 * e2 / 3072) * Math.Sin(6 * latitude1));
            xval = NN * (A + (1 - T + C) * A * A * A / 6 + (5 - 18 * T + T * T + 72 * C - 58 * ee) * A * A * A * A * A / 120);
            yval = M + NN * Math.Tan(latitude1) * (A * A / 2 + (5 - T + 9 * C + 4 * C * C) * A * A * A * A / 24 + (61 - 58 * T + T * T + 600 * C - 330 * ee) * A * A * A * A * A * A / 720);


            // 这里要对带分界进行特殊处理（如114度）

            if (lon == zoneWide * ProjNo)
            {
                X0 = 1000000L * (ProjNo) + 500000L;

                xval = Math.Abs(xval);
            }
            else
            {
                X0 = 1000000L * (ProjNo + 1) + 500000L;
            }

            // 	X0 = 500000L;

            Y0 = 0;
            xval += X0; yval += Y0;

            double x = xval;
            double y = yval;

            return (x, y);
        }
    }
}