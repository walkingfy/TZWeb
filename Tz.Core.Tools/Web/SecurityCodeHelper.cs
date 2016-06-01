using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Web.Management;

namespace Tz.Core.Tools
{

    public class Rand
    {
        #region 生成随机数字
        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Number(int length)
        {
            return Number(length, false);
        }
        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <param name="sleep">是否要在生成前将当前线程阻塞以避免重复生成</param>
        /// <returns></returns>
        public static string Number(int length, bool sleep)
        {
            if (sleep)
            {
                System.Threading.Thread.Sleep(3);
            }
            string result = "";
            System.Random random=new Random();
            for (int i = 0; i < length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }
        #endregion

        #region 生成随机字母与数字
        /// <summary>
        /// 生成随机数字与字母
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Str(int length)
        {
            return Str(length, false);
        }
        /// <summary>
        /// 生成随机数字与字母
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <param name="sleep">是否要在生成前将当前线程阻塞以避免重复</param>
        /// <returns></returns>
        public static string Str(int length, bool sleep)
        {
            if (sleep)
            {
                System.Threading.Thread.Sleep(3);
            }
            char[] pattern=new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = pattern.Length;
            Random random = new Random(~unchecked((int) DateTime.Now.Ticks));
            for (int i = 0; i < length; i++)
            {
                int rnd = random.Next(0, n);
                result += pattern[rnd];
            }
            return result;
        }
        #endregion

        #region 生成随机纯字母随机数
        /// <summary>
        /// 生成随机纯字母随机数
        /// </summary>
        /// <param name="IntStr">生成长度</param>
        public static string Str_char(int Length)
        {
            return Str_char(Length, false);
        }

        /// <summary>
        /// 生成随机纯字母随机数
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string Str_char(int Length, bool Sleep)
        {
            if (Sleep) System.Threading.Thread.Sleep(3);
            char[] Pattern = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = Pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];
            }
            return result;
        }
        #endregion
    }

    public class SecurityCodeHelper
    {
        #region 私有字段

        private string text;
        private Bitmap image;
        private int letterCount = 4;//验证码位数
        private int letterWidth = 16;//单个字体宽度范围
        private int letterHeight = 20;//单个字体高度范围
        private  static byte[] randb=new byte[4];
        private static RNGCryptoServiceProvider rand=new RNGCryptoServiceProvider();

        private Font[] fonts =
        {
            new Font(new FontFamily("Times New Roman"), 10 + Next(1), System.Drawing.FontStyle.Regular),
            new Font(new FontFamily("Georgia"), 10+Next(1),System.Drawing.FontStyle.Regular),
            new Font(new FontFamily("Arial"), 10 + Next(1),System.Drawing.FontStyle.Regular),
            new Font(new FontFamily("Comic Sans MS"), 10 + Next(1),System.Drawing.FontStyle.Regular)
        };

        #endregion

        #region 公有属性
        /// <summary>
        /// 验证码
        /// </summary>
        public string Text
        {
            get { return this.text; }
        }
        /// <summary>
        /// 验证码图片
        /// </summary>
        public Bitmap Image
        {
            get { return this.image; }
        }
        #endregion

        #region 构造函数

        #endregion

        #region 私有方法
        /// <summary>
        /// 获得下一个随机数
        /// </summary>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        private static int Next(int max)
        {
            rand.GetBytes(randb);
            int value = BitConverter.ToInt32(randb, 0);
            value = value%(max + 1);
            if (value < 0)
            {
                value = -value;
            }
            return value;
        }
        /// <summary>
        /// 获得下一个随机数
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Next(int min, int max)
        {
            int value = Next(max - min) + max;
            return value;
        }
        #endregion

        #region 公共方法

        public void CreateImage()
        {
            int int_ImageWidth = this.text.Length*letterWidth;
            Bitmap image = new Bitmap(int_ImageWidth, letterHeight);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);

        }

        /// <summary>
        /// 正弦曲线Wave扭曲图片
        /// </summary>
        /// <param name="srcBmp">图片路径</param>
        /// <param name="bXDir">如果扭曲则选择为True</param>
        /// <param name="dMultValue">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
        /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI]</param>
        /// <returns></returns>
        public System.Drawing.Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            double PI = 6.283185307179586476925286766559;
            Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
            Graphics graph=Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();
            double dBaseAxisLen = bXDir ? (double) destBmp.Height : (double) destBmp.Width;
            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI*(double) j)/dBaseAxisLen : (PI*(double) i)/dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int) (dy*dMultValue) : i;
                    nOldY = bXDir ? j : j + (int) (dy*dMultValue);

                    Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            srcBmp.Dispose();
            return destBmp;
        }

        #endregion
    }
}