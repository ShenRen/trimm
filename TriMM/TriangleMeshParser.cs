// Part of TriMM, the TriangleMesh Manipulator.
//
// Copyright (C) 2008  Christian Moritz
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along
// with this program. If not, see <http://www.gnu.org/licenses/>.

using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace TriMM {

    /// <summary>
    /// The TriangleMeshParser parses a TriangleMesh from a compressed file or stores it.
    /// </summary>
    public static class TriangleMeshParser {

        #region Methods

        /// <summary>
        /// Imports a TriangleMesh saved as a *.tam file.
        /// </summary>
        /// <param name="fileStream">The FileStream to import the TriangleMesh from.</param>
        public static TriangleMesh Parse(FileStream fileStream) {
            BinaryFormatter binFormatter = new BinaryFormatter();
            GZipStream compressedStream = new GZipStream(fileStream, CompressionMode.Decompress);
            TriangleMesh mesh = (TriangleMesh)binFormatter.Deserialize(compressedStream);
            compressedStream.Close();

            return mesh;
        }

        /// <summary>
        /// Exports the given mesh as a compressed file, with the given path, with the extension .tam.
        /// </summary>
        /// <param name="fileStream">The FileStream to save the TriangleMesh to.</param>
        /// <param name="mesh">The TriangleMesh to be saved.</param>
        public static void WriteTAM(FileStream fileStream, TriangleMesh mesh) {
            MemoryStream memoryStream = new MemoryStream();
            GZipStream compressedStream = new GZipStream(fileStream, CompressionMode.Compress);
            BinaryFormatter binFormatter = new BinaryFormatter();
            binFormatter.Serialize(memoryStream, mesh);
            memoryStream.WriteTo(compressedStream);
            compressedStream.Flush();
            compressedStream.Close();
        }

        #endregion
    }
}
