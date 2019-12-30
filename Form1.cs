using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Irbis_Format
{
    public partial class Form1 : Form
    {
        private IrbImgFormat.IrbFileReader reader;
        private IrbImgFormat.IrbImg imgStream;
        private int frameIndex = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            reader = new IrbImgFormat.IrbFileReader(txtFileName.Text);
            imgStream = new IrbImgFormat.IrbImg(reader);

            lblTextInfo.Text = reader.GetTextInfo();

            labFrameCountNo.Text = reader.GetImageCount().ToString();

            ShowNextFrame();
        }

        private void ShowNextFrame()
        {
            lblFrameIndex.Text = frameIndex.ToString();

            if (!imgStream.ReadImage(frameIndex))
            {
                lblFrameIndex.Text = "eof";
                return;
            }

            frameIndex++;

            var img = imgStream.GetData();
            var w = imgStream.GetWidth();
            var h = imgStream.GetHeight();
            var dataSize = w * h;

            var maxValue = float.MinValue;
            var minValue = float.MaxValue;

            for (int i = 0; i < dataSize; i++)
            {
                maxValue = Math.Max(maxValue, img[i]);
                minValue = Math.Min(minValue, img[i]);
            }

            if (maxValue == minValue)
            {
                maxValue = minValue + 1;
            }

            lblMax.Text = maxValue.ToString();
            lblMin.Text = minValue.ToString();

            DirectBitmap bmp = new DirectBitmap(w, h);

            float scale = 255.0f / (maxValue - minValue);

            for (int i = 0; i < dataSize; i++)
            {
                var x = i % w;
                var y = i / w;
                var c = (int)((img[i] - minValue) * scale);

                bmp.SetPixel(x, y, Color.FromArgb(c, c, c));
            }
            picResult.Image = bmp.Bitmap;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowNextFrame();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
