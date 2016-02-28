using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awwBreak
{
    class Converter
    {
        public static double secondsToMinutes(long sec)
        {
            return sec / 60;
        }
        public static long minutesToSeconds(double mins)
        {
            return (long) Math.Round(mins * 60);
        }

        public static String secondsToMessage(long sec)
        {
            StringBuilder str = new StringBuilder();
            str.Append("Time remaining: ");
            if (sec > 60)
            {
                long minutesRemaining = (long) Math.Floor(secondsToMinutes(sec));
                str.Append(minutesRemaining.ToString());
                str.Append("m ");
            }
            str.Append((sec % 60).ToString());
            str.Append("s");

            return str.ToString();
        }
    }
}
