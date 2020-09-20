using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HumJ.Utilities
{
    public class GameLibraryBundleFileReader
    {
        public string Metadata { get; }
        public int ItemCount { get; }
        public IEnumerable<(int StartOffset, int Length, string Title)> Index { get; }
        public IEnumerable<(string Title, byte[] Bytes)> Item { get; }

        public GameLibraryBundleFileReader(string path)
        {
            using Stream fs = File.OpenRead(path);
            using BinaryReader br = new BinaryReader(fs);

            StringChunk metaChunk = DataChunk.ReadStringFromStream(fs, 8);
            Int32Chunk countChunk = DataChunk.ReadInt32FromStream(fs, br);
            List<(Int32Chunk StartOffset, Int32Chunk Length, StringChunk Title)> indexChunks = new List<(Int32Chunk StartOffset, Int32Chunk Length, StringChunk Title)>();
            List<ByteArrayChunk> dataChunks = new List<ByteArrayChunk>();

            int itemCount = countChunk.Value;
            for (int i = 0; i < itemCount; i++)
            {
                DataChunk.ReadInt32FromStream(fs, br);

                Int32Chunk so = DataChunk.ReadInt32FromStream(fs, br);
                Int32Chunk eo = DataChunk.ReadInt32FromStream(fs, br);
                StringChunk t = DataChunk.ReadStringFromStream(fs, 16);

                indexChunks.Add((so, eo, t));
            }

            foreach ((Int32Chunk so, Int32Chunk l, StringChunk t) in indexChunks)
            {
                dataChunks.Add(DataChunk.ReadByteArrayFromStream(fs, so.Value, l.Value, t.Value));
            }

            Metadata = metaChunk.Value;
            ItemCount = countChunk.Value;
            Index = indexChunks.Select(m => (m.StartOffset.Value, m.Length.Value, m.Title.Value)).ToArray();
            Item = dataChunks.Select(m => (m.Title, m.Bytes));
        }

        public IEnumerable<DataChunk> Chunks { get; private set; }

        public abstract class DataChunk
        {
            public virtual byte[] Bytes { get; protected set; }
            public Type ValueType { get; protected set; }

            internal static StringChunk ReadStringFromStream(Stream stream, int length)
            {
                byte[] bytes = new byte[length];
                stream.Read(bytes);

                return new StringChunk
                {
                    Bytes = bytes,
                    Value = Encoding.ASCII.GetString(bytes).TrimEnd('\0')
                };
            }
            internal static Int32Chunk ReadInt32FromStream(Stream stream, BinaryReader br)
            {
                byte[] bytes = new byte[sizeof(int)];
                stream.Read(bytes);

                stream.Seek(-bytes.Length, SeekOrigin.Current);

                return new Int32Chunk
                {
                    Bytes = bytes,
                    Value = br.ReadInt32()
                };
            }
            internal static ByteArrayChunk ReadByteArrayFromStream(Stream stream, int startOffset, int length, string title)
            {
                byte[] bytes = new byte[length];
                stream.Seek(startOffset, SeekOrigin.Begin);
                stream.Read(bytes);

                return new ByteArrayChunk(title)
                {
                    Bytes = bytes,
                };
            }
        }

        public abstract class DataChunk<T> : DataChunk
        {
            public abstract T Value { get; internal set; }
        }

        public sealed class StringChunk : DataChunk<string>
        {
            internal StringChunk()
            {
                ValueType = typeof(string);
            }

            public override string Value { get; internal set; }

            public override string ToString()
            {
                return Value.ToString();
            }
        }
        public sealed class Int32Chunk : DataChunk<int>
        {
            internal Int32Chunk()
            {
                ValueType = typeof(int);
            }

            public override int Value { get; internal set; }

            public override string ToString()
            {
                return Value.ToString();
            }

        }
        public sealed class ByteArrayChunk : DataChunk<byte[]>
        {

            internal ByteArrayChunk(string title)
            {
                ValueType = typeof(int);
                Title = title;
            }

            public override byte[] Value { get; internal set; }
            public string Title { get; private set; }

            public override string ToString()
            {
                return $"\"{Title}\", {Bytes.Length} byte(s)";
            }
        }
    }
}