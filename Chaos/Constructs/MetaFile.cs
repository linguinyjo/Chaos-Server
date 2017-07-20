﻿using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Chaos
{
    internal sealed class MetaFile
    {
        internal string Name { get; }
        internal byte[] Data { get; }
        internal List<MetafileNode> Nodes { get; private set; }

        /// <summary>
        /// Serverside representation of the MetaFiles. This will hold MetaFile data.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        private MetaFile(string name, byte[] data)
        {
            Name = name;
            Data = data;
            Nodes = new List<MetafileNode>();
        }

        /// <summary>
        /// Loads the metafile with the given name.
        /// </summary>
        /// <param name="name"></param>
        internal static MetaFile Load(string name)
        {
            MetaFile metaFile;

            FileStream fileStream = File.Open(Paths.MetaFiles + name, FileMode.Open, FileAccess.Read, FileShare.Read);
            using (MemoryStream dataStream = new MemoryStream())
            using (BinaryReader binaryReader = new BinaryReader(fileStream, Encoding.GetEncoding(949)))
            {
                fileStream.CopyTo(dataStream);
                metaFile = new MetaFile(name, dataStream.ToArray());
                fileStream.Position = 0;

                int num1 = binaryReader.ReadByte() << 8 | binaryReader.ReadByte();
                for (int index1 = 0; index1 < num1; ++index1)
                {
                    MetafileNode metafileNode = new MetafileNode(binaryReader.ReadString());
                    int num2 = binaryReader.ReadByte() << 8 | binaryReader.ReadByte();
                    for (int index2 = 0; index2 < num2; ++index2)
                    {
                        int count = binaryReader.ReadByte() << 8 | binaryReader.ReadByte();
                        byte[] bytes = binaryReader.ReadBytes(count);
                        metafileNode.Properties.Add(Encoding.GetEncoding(949).GetString(bytes));
                    }
                    metaFile.Nodes.Add(metafileNode);
                }
            }
            return metaFile;
        }
    }
}