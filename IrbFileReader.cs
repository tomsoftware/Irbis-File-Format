namespace IrbImgFormat
{
    class IrbFileReader
    {
        public enum enumBlockType
        {
            enumBlockTypeUnknown = -1,
            enumBlockTypeEmpty = 0,
            enumBlockTypeImage = 1,
            enumBlockTypePreview = 2,
            enumBlockTypeTextInfo = 3,
            enumBlockTypeHeader = 4,
            enumBlockTypeAudio = 7

        };

        public enum enumFileType
        {
            enumFileTypeImage = 1,
            enumFileTypeSequenz = 2
        };

        private struct tyBlock
        {
            public enumBlockType BlockType;
            public int DWord2;
            public int FrameIndex;
            public int offset;
            public int size;
            public int DWord6;
            public int DWord7;
            public int DWord8;
            public int headerOffset;
            public int headerSize;
            public int imageOffset;
            public int imageSize;
        };


        private struct tyHead
        {
            public string MagicNumber;
            public string FileType;
            public string FileType2;
            public enumFileType FileType2enum;

            public int Flag1;
            public int FirstBlockCount;

            public int BlockOffset;
            public int BlockCount;
            public tyBlock[] Block;
            public int BlockCountMax;
        };

        private static Logging logging = new Logging("IrbFileReader");


        private tyHead Head;

        private StreamReader reader;
        private int m_imageCount;



        //-------------------------------------//
        public IrbFileReader(string filename)
        {
            this.m_imageCount = 0;

            reader = new StreamReader(filename);


            Head.MagicNumber = reader.ReadStr(5);

            //- ID
            if (string.Compare(Head.MagicNumber, "\xFFIRB\0") != 0) //-- soll "\xFF" "IRB" "\0" aber C schneidet das \0 weg!
            {
                logging.addError("Irb File - ''Magische Number'' wrong");
                return;
            }

            //- FileType
            Head.FileType = reader.ReadStr(8);

            if (string.Compare(Head.FileType, "IRBACS\0\0") != 0)
            {
                Head.FileType2enum = enumFileType.enumFileTypeImage;
            }
            else if (string.Compare(Head.FileType, "IRBIS 3\0") != 0)
            {
                Head.FileType2enum = enumFileType.enumFileTypeSequenz;
            }
            else
            {
                logging.addError("Unknown Irbis File Type");
                return;
            }



            Head.FileType2 = reader.ReadStr(8);

            Head.Flag1 = reader.ReadIntBE();
            Head.BlockOffset = reader.ReadIntBE(); //- starts at 0
            Head.FirstBlockCount = reader.ReadIntBE();

            Head.BlockCount = 0;
            this.AddHead(Head.BlockOffset, Head.FirstBlockCount);

            int i = 0;
            while (i < Head.BlockCount)
            {
                if (Head.Block[i].BlockType == enumBlockType.enumBlockTypeHeader)
                {
                    this.AddHead(Head.Block[i].offset, 2);
                }
                i++;
            }


        }

        //-------------------------------------------------------//
        public void Close()
        {
            if (reader != null)
            {
                reader.Close();
            }
        }



        //-------------------------------------//
        private void AddHead(int offset, int count)
        {
            if ((Head.BlockCount + count) > Head.BlockCountMax)
            {
                Head.BlockCountMax = Head.BlockCountMax + count + 100;

                System.Array.Resize(ref Head.Block, Head.BlockCountMax);
            }


            reader.SetOffset(offset);

            if (reader.Eof) return;


            for (int i = 0; i < count; i++)
            {
                if (reader.Eof) return;

                SetHeadBlockVars(ref Head.Block[Head.BlockCount]);

                Head.BlockCount++;
            }
        }


        //-------------------------------------//
        private void SetHeadBlockVars(ref tyBlock block)
        {
            block.BlockType = (enumBlockType)reader.ReadIntBE();

            block.DWord2 = reader.ReadIntBE();
            block.FrameIndex = reader.ReadIntBE();


            block.offset = reader.ReadIntBE(); // starts at 0

            block.size = reader.ReadIntBE();

            //- head is wlways 0x6C0 Byte in lengtg
            block.headerSize = 0x6C0;
            if (block.headerSize > block.size) block.headerSize = block.size;


            block.headerOffset = 0;

            block.imageOffset = block.headerSize;
            block.imageSize = block.size - block.imageOffset;



            block.DWord6 = reader.ReadIntBE();
            block.DWord7 = reader.ReadIntBE();
            block.DWord8 = reader.ReadIntBE();

            if (block.BlockType == enumBlockType.enumBlockTypeImage)
            {
                this.m_imageCount++;
            }
        }


        //-------------------------------------//
        public int GetImageCount()
        {
            return this.m_imageCount;
        }




        //-------------------------------------//
        public int GetBlockCount()
        {
            return Head.BlockCount;
        }


        //-------------------------------------//
        public int GetBlockSize(int index)
        {
            if ((index >= 0) && (index < Head.BlockCount))
            {
                return Head.Block[index].size;
            }
            else
            {
                return 0;
            }
        }


        //-------------------------------------//
        public bool IsBlockImage(int index)
        {
            if ((index >= 0) && (index < Head.BlockCount))
            {
                return (Head.Block[index].BlockType == enumBlockType.enumBlockTypeImage);
            }
            else
            {
                return false;
            }
        }


        //-------------------------------------//
        public bool IsBlockTextInfo(int index)
        {
            if ((index >= 0) && (index < Head.BlockCount))
            {
                return (Head.Block[index].BlockType == enumBlockType.enumBlockTypeTextInfo);
            }
            else
            {
                return false;
            }
        }


        //-------------------------------------//
        public enumBlockType GetBlockType(int index)
        {
            if ((index >= 0) && (index < Head.BlockCount))
            {
                return Head.Block[index].BlockType;
            }
            else
            {
                return enumBlockType.enumBlockTypeUnknown;
            }
        }



        //-------------------------------------//
        public bool IsBlockPreview(int index)
        {
            if ((index >= 0) && (index < Head.BlockCount))
            {
                return (Head.Block[index].BlockType == enumBlockType.enumBlockTypePreview);
            }
            else
            {
                return false;
            }
        }



        /// <summary>
        /// Return the info text of this file
        /// </summary>
        public string GetTextInfo(int index = 0)
        {
            var blockIndex = FindBlockIndexByType(enumBlockType.enumBlockTypeTextInfo, index);

            if (blockIndex < 0)
            {
                return string.Empty;
            }

            tyBlock block = Head.Block[blockIndex];

             return reader.ReadStr(block.size, block.offset);
         
        }



       /// <summary>
       /// return the data of an image
       /// </summary>
        public byte[] GetImageData(int imageIndex)
        {
            int blockIndex = GetImageBlockIndex(imageIndex);

            if (blockIndex < 0)
            {
                logging.addError("getImageData(imageIndex) fail - ImageIndex: " + imageIndex + " not found");
                return null;
            }

            //- Header zurückgeben
            tyBlock block = Head.Block[blockIndex];
            return reader.ReadByte(block.size, block.offset);
        }


        /// <summary>
        /// Return the n-th index of a given data block type
        /// </summary>
        private int FindBlockIndexByType(enumBlockType type, int number)
        {
            if ((number < 0) || (number >= Head.BlockCount))
            {
                return -1;
            }

            for (int i = 0; i < Head.BlockCount; i++)
            {
                tyBlock block = Head.Block[i];

                //- find all images
                if (block.BlockType == type)
                {
                    /// return image if
                    number--;
                    if (number < 0)
                    {
                        return i;
                    }
                }
            }

            return -1;

        }

        /// <summary>
        /// Retrun the data block for a given image index
        /// </summary>
        private int GetImageBlockIndex(int imageIndex)
        {
            return FindBlockIndexByType(enumBlockType.enumBlockTypeImage, imageIndex);
        }



       /// <summary>
       /// Return the file offset for a given data block
       /// </summary>
        public int GetBlockOffset(int index)
        {
            if ((index >= 0) && (index < Head.BlockCount))
            {
                return Head.Block[index].offset;
            }
            else
            {
                return 0;
            }

        }


      

    }
}
