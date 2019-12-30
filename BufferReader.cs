using System;
using System.Runtime.InteropServices;

namespace IrbImgFormat
{
    class BufferReader
    {
        private static Logging logging = new Logging("BufferReader");

        /// <summary>
        /// Union to cast a 4-byte int to a float
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        private struct BinaryConvertIntToFloat
        {
            [FieldOffset(0)]
            public float toFloat;
            [FieldOffset(0)]
            public int toInt;
        }
 


        /// <summary>
        /// Union to cast a 8 byte long into a double
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        private struct BinaryConvertLongToDouble
        {
            [FieldOffset(0)]
            public double toDouble;
            [FieldOffset(0)]
            public long toLong;
        }


        byte[] data;
        int offset = 0;
        int dataLength = 0;

        public BufferReader(byte[] data)
        {
            this.data = data;
            dataLength = (data != null) ? data.Length : 0;
        }

        /// <summary>
        /// is the buffer Pointer at the end of the buffer
        /// </summary>
        public bool Eof
        {
            get
            {
                return ((offset < 0) || (offset >= dataLength) || (data == null));
            }
        }

        /// <summary>
        /// Return the lenght of the buffer
        /// </summary>
        public int Length
        {
            get
            {
                return data.Length;
            }
        }

        /// <summary>
        /// Read a string from the the buffer
        /// </summary>
        public string ReadStr(int length, int offset)
        {
            if (length < 0) length = 0;
            if (offset > -1) this.offset = offset;

            string outVal = "";


            if (this.offset < dataLength)
            {
                if ((this.offset + length) > dataLength)
                {
                    length = dataLength - this.offset;
                }

                outVal = System.Text.Encoding.Default.GetString(data, this.offset, length);
            }
            else
            {
                if (length > 0)
                {
                    logging.addWarning("ReadStr:EOF!");
                    length = 0;
                }
            }


            if (length > 0) this.offset = this.offset + length;

            return outVal;
        }


        /// <summary>
        /// read a NULL terminated string from buffer
        /// </summary>
        public string ReadNullTerminatedString(int offset, int size)
        {
            string s = this.ReadStr(size, offset);

            int pos = s.IndexOf('\0');

            if (pos > 0)
            {
                return s.Substring(0, pos);
            }
            return s;
        }


        /// <summary>
        /// Read 8 byte big ending long from buffer
        /// </summary>
        public long ReadLongBE(int offset = -1)
        {
            if (offset > -1) this.offset = offset;

            if (this.offset < dataLength)
            {
                if ((this.offset + 8) > dataLength)
                {
                    this.offset = dataLength;
                    return 0;
                }


                byte[] d = data;
                int i = this.offset;

                byte[] bytes = { d[i + 0], d[i + 1], d[i + 2], d[i + 3], d[i + 4], d[i + 5], d[i + 6], d[i + 7] };

                return BitConverter.ToInt64(bytes, 0);
            }
            return 0;


        }

        /// <summary>
        /// Read one byte from buffer
        /// </summary>
        public int ReadByte(int offset = -1)
        {
            if (offset > -1) this.offset = offset;

            if (this.offset >= dataLength)
            {
                return 0;
            }

            if ((this.offset + 1) > dataLength)
            {
                this.offset = dataLength;
                return 0;
            }


            var result = data[this.offset];
            this.offset++;
            return result;


        }


        /// <summary>
        /// Read big ending word (2 Bytes) from buffer
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public int ReadWordBE(int offset = -1)
        {
            if (offset > -1)
            {
                this.offset = offset;
            }


            if (this.offset >= dataLength)
            {
                return 0;
            }

            if ((this.offset + 2) > dataLength)
            {
                this.offset = dataLength;
                return 0;
            }


            var result = (int)data[this.offset] + (data[this.offset + 1] << 8);

            this.offset += 2;
            return result;


        }



        /// <summary>
        /// Read big-ending int from buffer
        /// </summary>
        public int ReadIntBE(int offset = -1)
        {
            if (offset > -1)
            {
                this.offset = offset;
            }


            if (this.offset >= dataLength)
            {
                return 0;
            }

            if ((this.offset + 4) > dataLength)
            {
                this.offset = dataLength;
                return 0;
            }


            var result = (int)data[this.offset] + (data[this.offset + 1] << 8) + (data[this.offset + 2] << 16) + (data[this.offset + 3] << 24);

            this.offset += 4;
            return result;
        }


        /// <summary>
        /// read double from buffer
        /// </summary>
        public double ReadDoubleBE(int length = 8, int offset = -1)
        {
            BinaryConvertLongToDouble converterLongDouble;
            converterLongDouble.toDouble = 0.0; /// need to be set to avoid compiler errors

            converterLongDouble.toLong = ReadLongBE(offset);
            return converterLongDouble.toDouble;
        }


        /// <summary>
        /// read float from buffer
        /// </summary>
        public float ReadSingleBE(int offset = -1)
        {
            BinaryConvertIntToFloat converterIntFloat;
            converterIntFloat.toFloat = 0.0f; /// need to be set to avoid compiler errors

            converterIntFloat.toInt = ReadIntBE(offset);
            return converterIntFloat.toFloat;
        }

    }
}
