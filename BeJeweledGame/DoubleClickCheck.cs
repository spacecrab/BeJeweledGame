//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using WinformsTimer = System.Windows.Forms.Timer;

namespace BeJeweledGame
{
    internal static class DoubleClickCheck
    {
        private static bool isDoubleClickFlag;
        private static bool isTicking;
        private static WinformsTimer timer;
        private static Point pointFirstClick;
        private static int singleClickCounter;
        private static int doubleClickCounter;

        public static bool IsDoubleClickFlag { get => isDoubleClickFlag; set => isDoubleClickFlag = value; }
        public static bool IsTicking { get => isTicking; set => isTicking = value; }
        public static WinformsTimer Timer { get => timer; set => timer = value; }
        public static Point PointFirstClick { get => pointFirstClick; set => pointFirstClick = value; }
        public static int SingleClickCounter { get => singleClickCounter; set => singleClickCounter = value; }
        public static int DoubleClickCounter { get => doubleClickCounter; set => doubleClickCounter = value; }


        static DoubleClickCheck()
        {
            isDoubleClickFlag = false;
            isTicking = false;
            timer = new WinformsTimer() { Interval = SystemInformation.DoubleClickTime + 1 };
            timer.Tick += timer_Tick;
            pointFirstClick = new Point();
        }

        public static bool IsDoubleClick(Point pointClick)
        {
            TimerTicking(pointClick);
            return isDoubleClickFlag;
        }

        private static void timer_Tick(object? sender, EventArgs e)
        {
            timer.Stop();

            isDoubleClickFlag = false;

            isTicking = false;
        }

        public static void TimerTicking(Point pointClick)
        {
            if (!isTicking) //Click #1
            {
                timer.Start();
                isTicking = true;

                pointFirstClick = pointClick;

                isDoubleClickFlag = false;

                return;
            }

            timer.Stop(); //Click #2
            isTicking = false;

            if (Math.Abs(pointClick.X - pointFirstClick.X) < SystemInformation.DoubleClickSize.Width / 2
             && Math.Abs(pointClick.Y - pointFirstClick.Y) < SystemInformation.DoubleClickSize.Height / 2)
            {
                isDoubleClickFlag = true;
                return;
            }
            isDoubleClickFlag = false;
        }
    }
}
