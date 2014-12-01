using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Verification
{
    public class VerificationCode
    {
        private string VerCode = String.Empty;
        public VerificationCode()
        {
            VerCode = this.RandomNumber();
        }
        public byte[] StreamVerCod()
        {
            MemoryStream ms = new MemoryStream();
            this.CreateVerCode().Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }
        private string RandomNumber()
        {
            /*产生随机字符*/
            char[] Character = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            string RandomChar = "";
            Random rd = new Random();
            for (int i = 1; i <= 4; i++)
            {
                RandomChar = RandomChar + Character[rd.Next(Character.Length)];
            }
            VerCode = RandomChar;
            return RandomChar;
        }

        private Bitmap CreateVerCode()
        {
            /*声明图片并赋值*/
            Bitmap image = new Bitmap(60, 30);
            Graphics g = Graphics.FromImage(image);
            Font f = new Font("", 14);
            Random random = new Random();
            g.Clear(Color.White);
            //画图片的背景噪音线
            for (int i = 0; i < 25; i++)
            {
                int x1 = random.Next(image.Width);
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);
                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }
            Font font = new System.Drawing.Font("Arial", 13, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
            System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
            g.DrawString(VerCode, font, brush, 2, 2);
            //画图片的前景噪音点
            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);
                image.SetPixel(x, y, Color.FromArgb(random.Next()));
            }
            //画图片的边框线
            g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            g.Dispose();
            return image;
        }

        public string GetVerCode()
        {
            return Convert(this.VerCode);
        }

        private static string Convert(string value)
        {
            char[] ch =value.ToCharArray();
            string res = String.Empty;
            for (int i = 1; i <= value.Length; i++)
            {
                if (ch[i - 1] >= 'A' && ch[i - 1] <= 'Z')
                {
                    ch[i - 1] = (char)(ch[i - 1] + 32);
                }
                res += ch[i - 1];
            }
            return res;
        }

        public static bool ComparisonVerCode(string value_1, string value_2)
        {
            return Convert(value_1) == Convert(value_2);
        }
    }
}