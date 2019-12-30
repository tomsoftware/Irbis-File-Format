using System;



namespace IrbImgFormat
{
    class IrbImg
    {
        private static Logging logging = new Logging("IrbImg");


        public double ShotRangeMin { get; protected set; }
        public double ShotRangeMax { get; protected set; }
        public double CalibRangeMin { get; protected set; }
        public double CalibRangeMax { get; protected set; }


        IrbFileReader reader;

        int BytePerPixel;
        int Compressed;

        float Emissivity;

        float EnvironmentalTemp;
        float Distanz;

        float PathTemperature;
        float CenterWavelength;

        float CalibRange_min;
        float CalibRange_max;

        float ShotRange_start_ERROR;
        float ShotRange_size;

        double TimeStamp_Raw;
        DateTime TimeStamp;
        DateTime TimeStampOffsetTime;

        int TimeStampOffsetMilliseconds;
        int TimeStampMilliseconds;

        string Device;
        string DeviceSerial;
        string Optics;
        string OpticsResolution;
        string OpticsText;

        public float[] Data;


        private int Width;
        private int Height;



        public IrbImg(IrbFileReader FileReader, int imageIndex = 0)
        {
            reader = FileReader;
            ReadImage(imageIndex);
        }


        /// <summary>
        /// Width of the image
        /// </summary>
        /// <returns></returns>
        public int GetWidth()
        {
            return Width;
        }

        /// <summary>
        /// Height of the image
        /// </summary>
        public int GetHeight()
        {
            return Height;
        }

        /// <summary>
        /// Return the data of the image as float array
        /// </summary>
        /// <returns></returns>
        public float[] GetData()
        {
            if (Data == null) logging.addError("getData() Accessing non initialised data!");
            return Data;
        }


        /// <summary>
        /// Read a image from the file
        /// </summary>
        public bool ReadImage(int imageIndex)
        {
            System.DateTime FrameTime = System.DateTime.Now;


            var reader = new BufferReader(this.reader.GetImageData(imageIndex));

            if (reader.Eof)
            {
                return false;
            }

            Width = 0;
            Height = 0;


            //- Image header
            BytePerPixel = reader.ReadWordBE();
            Compressed = reader.ReadWordBE();
            Width = reader.ReadWordBE();
            Height = reader.ReadWordBE();


            reader.ReadIntBE(); //-- don't know - alway 0
            reader.ReadWordBE(); //-- don't know - alway 0

            //- dont know why but it is alwasy the width -1 
            if (reader.ReadWordBE() != (Width - 1))
            {
                logging.addError("??? value != (Height - 1)");
            }


            reader.ReadWordBE(); //-- don't know - alway 0

            //- dont know why but it is alwasy the height -1 
            if (reader.ReadWordBE() != (Height - 1))
            {
                logging.addError("??? value != (Height - 1)");
            }


            reader.ReadWordBE(); //-- don't know - alway 0
            reader.ReadWordBE(); //-- don't know - alway 0

            Emissivity = reader.ReadSingleBE();

            Distanz = reader.ReadSingleBE();

            EnvironmentalTemp = reader.ReadSingleBE();


            reader.ReadWordBE(); //-- don't know - always 0
            reader.ReadWordBE(); //-- don't know - always 0

            PathTemperature = reader.ReadSingleBE();

            reader.ReadWordBE(); //-- don't know - always 0x65
            reader.ReadWordBE(); //-- don't know - always 0


            CenterWavelength = reader.ReadSingleBE();


            reader.ReadWordBE(); //-- don't know - always 0
            reader.ReadWordBE(); //-- don't know - always 0xH4080
            reader.ReadWordBE(); //-- don't know - always 0x9
            reader.ReadWordBE(); //-- don't know - always 0x101


            if ((Width > 10000) || (Height > 10000))
            {
                logging.addError("Defect Irbis Image File: Image Width(" + Width + ") or Height(" + Height + ") is out of range!");
                Width = 1;
                Height = 1;
                return false;
            }

            //- liest weitere Bildinforationen aus
            this.ReadFlags(reader, 1084);

            Data = ReadImageData(reader, 0x6C0, Width, Height, 60, Compressed);



            if (reader.Eof) logging.addError("end of file!");

            return true;
        }


        /// <summary>
        /// Read image flags from the file
        /// </summary>
        public void ReadFlags(BufferReader reader, int offset)
        {
            CalibRange_min = reader.ReadSingleBE(offset + 92);
            CalibRange_max = reader.ReadSingleBE(offset + 96);


            Device = reader.ReadNullTerminatedString(offset + 142, 12);
            DeviceSerial = reader.ReadNullTerminatedString(offset + 186, 16);
            Optics = reader.ReadNullTerminatedString(offset + 202, 32);
            OpticsResolution = reader.ReadNullTerminatedString(offset + 234, 32);
            OpticsText = reader.ReadNullTerminatedString(offset + 554, 48);

            ShotRange_start_ERROR = reader.ReadSingleBE(offset + 532);
            ShotRange_size = reader.ReadSingleBE(offset + 536);


            TimeStamp_Raw = reader.ReadDoubleBE(8, offset + 540);
            TimeStampMilliseconds = reader.ReadIntBE(offset + 548);

            TimeStamp = Double2DateTime(TimeStamp_Raw, TimeStampMilliseconds);
        }




        /// <summary>
        /// Read the compressing "pallet" from file 
        /// </summary>
        private float[] ReadPallet(BufferReader reader, int offset)
        {
            float[] palette = new float[256];

            int pos = offset;

            for (int i = 0; i < 256; i++)
            {
                palette[i] = reader.ReadSingleBE(pos);
                pos += 4;
            }

            return palette;
        }



        /// <summary>
        /// Read the image data from file
        /// </summary>
        /// <returns></returns>
        private float[] ReadImageData(BufferReader reader, int bindata_offset, int width, int height, int palette_offset, int useCompression)
        {
            int data_size = width * height; //- count of pixles
            bool useComp = (useCompression == 1);

            int pixelCount = data_size;
            float[] matrixData = new float[pixelCount];

            int matrixDataPos = 0;

            int v1_pos = bindata_offset;
            int v2_pos = v1_pos + width * height; //- used if data are compressed

            //byte data_v1 = &bindata[v1_pos];
            //unsigned char* data_v2 = &bindata[v2_pos];

            int v1 = 0;
            int v2 = 0;


            float[] Palette = ReadPallet(reader, palette_offset);


            int v2_count = 0;
            float v = 0;
            float f;


            if (!useComp)
            {
                for (int i = pixelCount; i > 0; i--)
                {
                    //- read values
                    v1 = reader.ReadByte(v1_pos);
                    v1_pos++;
                    v2 = reader.ReadByte(v1_pos);
                    v1_pos++;

                    f = (float)v1 * (1.0f / 256.0f);

                    //- lineare interpolation
                    v = Palette[v2 + 1] * f + Palette[v2] * (1.0f - f);

                    if (v < 0) v = 0; //- oder 255

                    matrixData[matrixDataPos] = v;
                    matrixDataPos++;
                }
            }
            else
            {
                for (int i = pixelCount; i > 0; i--)
                {
                    //- werte lesen
                    if (v2_count-- < 1) //- ok... neuen wert für V2 lesen
                    {
                        v2_count = reader.ReadByte(v2_pos) - 1;
                        v2_pos++;

                        v2 = reader.ReadByte(v2_pos);
                        v2_pos++;
                    }

                    v1 = reader.ReadByte(v1_pos);
                    v1_pos++;

                    f = (float)v1 * (1.0f / 256.0f);

                    //- lineare interpolation
                    v = Palette[v2 + 1] * f + Palette[v2] * (1.0f - f);

                    if (v < 0) v = 0; //- oder 255

                    matrixData[matrixDataPos] = v;
                    matrixDataPos++;
                }
            }

            return matrixData;

        }


        /// <summary>
        /// convert a double value to a date time
        /// </summary>
        private System.DateTime Double2DateTime(double date, int Milliseconds = 0)
        {
            System.DateTime d = DateTime.FromOADate(date);

            //- calc the time from the Date + Milliseconds
            if ((TimeStampOffsetTime != DateTime.MinValue) && (Milliseconds > 0) && (Milliseconds > TimeStampOffsetMilliseconds) && (d >= TimeStampOffsetTime))
            {
                return TimeStampOffsetTime.AddMilliseconds(Milliseconds - TimeStampOffsetMilliseconds);
            }
            else
            {
                //- never set so save start-date/time
                TimeStampOffsetMilliseconds = Milliseconds;
                TimeStampOffsetTime = d;
                return d;
            }

        }


    }
}
