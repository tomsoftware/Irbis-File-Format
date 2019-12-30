using System;
using System.IO;


namespace IrbImgFormat
{
    class StreamReader
    {
        private static Logging logging = new Logging("StreamReader");
        private BinaryReader m_reader;
        private bool m_eof;

        private int m_offset;
        private int m_blocklen;
        private string filename;

        public StreamReader(string filename)
        {
            this.m_eof = true;
            this.m_offset = 0;

            Open(filename);
        }

        /// <summary>
        /// return the filename of the stream
        /// </summary>
        /// <returns></returns>
        public string GetFileName()
        {
            try
            {
                return System.IO.Path.GetFileName(filename);
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }


        /// <summary>
        /// Open the stream
        /// </summary>
        /// <param name="filename"></param>
        private void Open(string filename)
        {
            this.filename = filename;

            //- Datei öffnen
            try
            {
                m_reader = new System.IO.BinaryReader(System.IO.File.Open(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read));

                this.m_eof = false;
                m_blocklen = (int)m_reader.BaseStream.Length; //- Beschränkung auf max 2GB Dateigröße
            }
            catch (System.Exception ex)
            {
                logging.addError("IO.BinaryReader fail", ex.Message, filename);
                return;
            }

        }


        /// <summary>
        /// Close the stream
        /// </summary>
        public void Close()
        {
            if (m_reader != null) m_reader.Close();
        }


        /// <summary>
        /// is the stream pointer at the end of the file?
        /// </summary>
        public bool Eof
        {
            get
            {
                return ((m_reader == null) || (m_eof));
            }
        }

        /// <summary>
        /// Sets the stream read position
        /// </summary>
        /// <param name="offset"></param>
        public void SetOffset(int offset)
        {
            if ((offset < m_blocklen) && (offset >= 0))
            {
                m_offset = offset;
                m_reader.BaseStream.Seek(m_offset, System.IO.SeekOrigin.Begin);
                m_eof = false;
            }
            else
            {
                m_eof = true;
                if (offset < 0) m_offset = 0;
                if (offset >= m_blocklen) m_offset = m_blocklen;
                m_reader.BaseStream.Seek(m_offset, System.IO.SeekOrigin.Begin);
            }
        }


        /// <summary>
        /// Read length bytes from the stream
        /// </summary>
        public byte[] ReadByte(int length, int offset)
        {
            if (length < 0) length = 0;
            if (offset > -1)
            {
                m_reader.BaseStream.Seek(offset, System.IO.SeekOrigin.Begin);
                m_offset = offset;
            }

            byte[] dataArray = null;


            if (m_offset < m_blocklen)
            {
                if ((m_offset + length) > m_blocklen)
                {
                    m_eof = true;
                    length = m_blocklen - m_offset;
                }

                dataArray = new byte[length];


                try
                {
                    length = m_reader.Read(dataArray, 0, length);
                }
                catch (Exception e)
                {
                    logging.addError("readByte(): length: " + length + " /  offset: " + offset + "  - " + e.Message);
                    length = 0;
                    m_eof = true;
                }

                if (length != dataArray.Length) System.Array.Resize(ref dataArray, length);
            }
            else
            {
                if (length > 0)
                {
                    logging.addWarning("ReadStr:EOF!");
                    length = 0;
                }

                m_eof = true;
            }


            if (length > 0) m_offset = m_offset + length;

            return dataArray;
        }


        /// <summary>
        /// Read a string from the stream
        /// </summary>
        public string ReadStr(int length, int offset = -1)
        {
            string outVal = "";

            byte[] tmpData = this.ReadByte(length, offset);

            if (tmpData != null)
            {
                outVal = System.Text.Encoding.Default.GetString(tmpData);
            }

            return outVal;
        }



        /// <summary>
        /// Read big ending int from the stream
        /// </summary>
        public int ReadIntBE(int offset = -1)
        {
            byte[] tmpData = this.ReadByte(4, offset);

            if ((tmpData == null) || (tmpData.Length != 4))
            {
                return 0;
            }

            return (int)(tmpData[0] | (tmpData[1] << 8) | (tmpData[2] << 16) | (tmpData[3] << 24));

        }


    }
}
