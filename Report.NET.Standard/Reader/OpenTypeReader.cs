﻿using System;
using System.IO;
using System.Text;

namespace Report.NET.Standard.Reader
{
    /// <summary>Reader Class for OpenType Fonts</summary>
    internal class OpenTypeReader : IDisposable
    {
        /// <summary>Stream</summary>
        private Stream stream;

        /// <summary>Creates a reader for open type fonts.</summary>
        /// <param name="sFileName">Name of the font file</param>
        internal OpenTypeReader(Stream stream)
        {
            this.stream = stream;
        }

        /// <summary>Skips the specified number of bytes of the stream.</summary>
        /// <param name="iBytes">Number of bytes that must be skipped.</param>
        internal void Skip(Int32 iBytes)
        {
            stream.Seek(iBytes, SeekOrigin.Current);
        }

        /// <summary>This method will set the read position to the specified offset.</summary>
        /// <param name="iOffset">New read position</param>
        internal void Seek(int iOffset)
        {
            stream.Seek(iOffset, SeekOrigin.Begin);
        }

        /// <summary>Reads the specified number of BYTE values (8-bit unsigned integer) from the stream.</summary>
        /// <param name="iLength">Number of BYTE values</param>
        /// <returns>array of BYTE values</returns>
        internal byte[] aByte_ReadBYTE(int iLength)
        {
            byte[] aByte = new byte[iLength];
            stream.Read(aByte, 0, iLength);
            return aByte;
        }

        /// <summary>Reads the specified number of CHAR values (8-bit signed integer) from the stream.</summary>
        /// <param name="iLength">Number of CHAR values</param>
        /// <returns>string with the CHAR values</returns>
        internal string sReadCHAR(int iLength)
        {
            byte[] aByte = aByte_ReadBYTE(iLength);
            string s = Encoding.GetEncoding("UTF-8").GetString(aByte);
            return s.Trim();
        }

        /// <summary>Reads an USHORT value (16-bit unsigned integer) from the stream.</summary>
        /// <returns>USHORT value</returns>
        internal int iReadUSHORT()
        {
            int i1 = stream.ReadByte();
            int i2 = stream.ReadByte();
            return (i1 << 8) + i2;
        }

        /// <summary>Reads a SHORT value (16-bit signed integer) from the stream.</summary>
        /// <returns>SHORT value</returns>
        internal Int16 int16_ReadSHORT()
        {
            int i1 = stream.ReadByte();
            int i2 = stream.ReadByte();
            return (Int16)((i1 << 8) + i2);
        }

        /// <summary>Reads an ULONG value (32-bit unsigned integer) from the stream.</summary>
        /// <returns>ULONG value</returns>
        internal UInt32 uReadULONG()
        {
            UInt32 u1 = (UInt32)stream.ReadByte();
            UInt32 u2 = (UInt32)stream.ReadByte();
            UInt32 u3 = (UInt32)stream.ReadByte();
            UInt32 u4 = (UInt32)stream.ReadByte();
            return (u1 << 24) + (u2 << 16) + (u3 << 8) + u4;
        }

        /// <summary>Reads a LONG value (32-bit signed integer) from the stream.</summary>
        /// <returns>LONG value</returns>
        internal Int32 iReadLONG()
        {
            Int32 i1 = stream.ReadByte();
            Int32 i2 = stream.ReadByte();
            Int32 i3 = stream.ReadByte();
            Int32 i4 = stream.ReadByte();
            return (i1 << 24) + (i2 << 16) + (i3 << 8) + i4;
        }

        /// <summary>Reads a FWORD value (16-bit signed integer, SHORT, quantity in FUnits) from the stream.</summary>
        /// <returns>FWORD value</returns>
        internal Int16 int16_ReadFWORD()
        {
            return int16_ReadSHORT();
        }

        /// <summary>Reads an UFWORD value (16-bit unsigned integer, USHORT, quantity in FUnits) from the stream.</summary>
        /// <returns>USHORT value</returns>
        internal Int32 iReadUFWORD()
        {
            return iReadUSHORT();
        }

        /// <summary>Reads a tag (4 uint8s) from the stream.</summary>
        /// <returns>tag value</returns>
        internal String sReadTag()
        {
            return sReadCHAR(4);
        }

        /// <summary>Reads the specified number of CHAR values (8-bit signed integer) from the stream.</summary>
        /// <param name="iLength">Number of CHAR values</param>
        /// <returns>string with the CHAR values</returns>
        internal String sReadUnicodeString(Int32 iLength)
        {
            iLength /= 2;
            StringBuilder sb = new StringBuilder(iLength);
            for (Int32 i = 0; i < iLength; ++i)
            {
                sb.Append((Char)iReadUSHORT());
            }
            return sb.ToString();
        }

        #region IDisposable Members
        /// <summary>Releases all resources used by the OpenTypeReader object.</summary>
        public void Dispose()
        {
            stream.Close();
        }
        #endregion
    }
}