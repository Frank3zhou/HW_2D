﻿using _6524.Class;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _6524
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
          //  HObject img;
           //  HOperatorSet.ReadImage(out img, @"C:/Users/HW/Desktop/12.bmp");

            Application.Run(new Form1());
           // Application.Run(new 像素匹配());
          //  Application.Run(new 形状匹配(img));
            //  Application.Run(new Teach_point());
        }
    }
}
