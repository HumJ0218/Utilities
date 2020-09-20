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
            var RIFF = "RIFF".Select(m => (byte)m).ToArray();
            var WAVEfmt_ = "WAVEfmt ".Select(m => (byte)m).ToArray();

            var rootPath = @"D:\GlbTest";

            foreach (var path in Directory.GetFiles(rootPath, "*.glb"))
            {
                var fi = new FileInfo(path);

                GameLibraryBundleFileReader gfr = new GameLibraryBundleFileReader(fi.FullName);

                List<string> logLines = new List<string>
                {
                    fi.Name,
                    "----------------------------------------",
                    "",
                    gfr.Metadata,
                    "",
                    "#\tStart\tLength\tTitle\tFile",
                    "----------------------------------------"
                };
                int i = 0;
                (int StartOffset, int Length, string Title)[] index = gfr.Index.ToArray();

                var di = Directory.CreateDirectory(rootPath + "\\" + fi.Name + ".export");
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

                    if (item.Bytes.Length >= 16 && item.Bytes.AsSpan(0, 4).SequenceEqual(RIFF) && item.Bytes.AsSpan(8, 8).SequenceEqual(WAVEfmt_))
                    {
                        var sfn = safeFileName.Split('.');
                        sfn[^1] = "WAV";
                        safeFileName = string.Join(".", sfn);

                        if (safeFileName == "WAV")
                        {
                            safeFileName = ".WAV";
                        }
                    }

                    File.WriteAllBytes(di.FullName + "\\" + safeFileName, item.Bytes);

                    (int StartOffset, int Length, string Title) = index[i];
                    logLines.Add($"{i}\t{StartOffset}\t{Length}\t{Title}\t{safeFileName}");
                    i++;
                }

                File.WriteAllLines(rootPath + "\\" + fi.Name + ".list.txt", logLines);
            }

            Assert.Pass();
        }
    }
}