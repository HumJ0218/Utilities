using System;
using System.Drawing;

namespace HumJ.Utilities
{
    public static class Graphics
    {
        /// <summary>
        /// 计算矩形旋转任意角度后的宽高
        /// </summary>
        public static (double width, double height) Graphics_GetRotateRectangle(this (double width, double height) shape, double deg)
        {
            double width = shape.width;
            double height = shape.height;
            double rad = deg * Math.PI / 180;
            double cos = Math.Cos(rad);
            double sin = Math.Sin(rad);

            double newWidth = Math.Max(Math.Abs(width * cos - height * sin), Math.Abs(width * cos + height * sin));
            double newHeight = Math.Max(Math.Abs(width * sin - height * cos), Math.Abs(width * sin + height * cos));

            return (newWidth, newHeight);
        }

        /// <summary>
        /// 获取旋转任意角度后的图像
        /// </summary>
        public static Image Graphics_Rotate(this Image srcImage, double deg, double antiAlias = 2)
        {
            deg %= 360;

            double srcWidth = srcImage.Width * antiAlias;
            double srcHeight = srcImage.Height * antiAlias;

            (double width, double height) = (srcWidth, srcHeight).Graphics_GetRotateRectangle(deg);
            double rotateWidth = width;
            double rotateHeight = height;

            using Bitmap msaaImage = new Bitmap((int)rotateWidth, (int)rotateHeight);
            using System.Drawing.Graphics msaaGraphics = System.Drawing.Graphics.FromImage(msaaImage);

            PointF centerPoint = new PointF((float)(rotateWidth / 2), (float)(rotateHeight / 2));

            msaaGraphics.TranslateTransform(centerPoint.X, centerPoint.Y);
            msaaGraphics.RotateTransform((float)deg);
            msaaGraphics.TranslateTransform(-centerPoint.X, -centerPoint.Y);

            PointF Offset = new PointF((float)((rotateWidth - srcWidth) / 2), (float)((rotateHeight - srcHeight) / 2));
            msaaGraphics.DrawImage(srcImage, Offset.X, Offset.Y, (float)(srcImage.Width * antiAlias), (float)(srcImage.Height * antiAlias));

            msaaGraphics.ResetTransform();
            msaaGraphics.Save();

            Bitmap resultImage = new Bitmap((int)(msaaImage.Width / antiAlias), (int)(msaaImage.Height / antiAlias));
            using System.Drawing.Graphics resultGraphics = System.Drawing.Graphics.FromImage(resultImage);
            resultGraphics.DrawImage(msaaImage, 0, 0, resultImage.Width, resultImage.Height);
            resultGraphics.Save();

            return resultImage;
        }
    }
}