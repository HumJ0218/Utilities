using HumJ.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NUnitTest
{
    public class GlbFileReaderTests
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ToHexString()
        {
            string path = @"D:\test.glb";
            GlbFileReader gfr = new GlbFileReader(path);

            List<string> logLines = new List<string>
            {
                path,
                "----------------------------------------",
                "",
                gfr.Metadata,
                "",
                "#\tStart\tLength\tTitle\tFile",
                "----------------------------------------"
            };
            int i = 0;
            (int StartOffset, int Length, string Title)[] index = gfr.Index.ToArray();

            Directory.CreateDirectory(@"D:\glbExport");
            foreach ((string Title, byte[] Bytes) item in gfr.Item)
            {
                string safeFileName = string.Join(".", item.Title
                    .Replace("\\", "_")
                    .Replace("/", "_")
                    .Replace(":", "_")
                    .Replace("*", "_")
                    .Replace("?", "_")
                    .Replace("\"", "_")
                    .Replace("<", "_")
                    .Replace(">", "_")
                    .Replace("|", "_")
                    .Split('_').Reverse())
                ;
                File.WriteAllBytes(@"D:\glbExport\" + safeFileName, item.Bytes);

                (int StartOffset, int Length, string Title) = index[i];
                logLines.Add($"{i}\t{StartOffset}\t{Length}\t{Title}\t{safeFileName}");
                i++;
            }

            File.WriteAllLines(@"D:\glbExport\_list.txt", logLines);

            Assert.Pass();
        }
    }
}