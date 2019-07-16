using Android.OS;
using Android.Widget;
using System;

namespace WTONewProject.Droid
{
    public class TickListener : Java.Lang.Object, Chronometer.IOnChronometerTickListener
    {
        static int MAX_TIME = 10 * 1000;//ms

        //public IntPtr Handle => throw new NotImplementedException();
        private ChronometerCallback _callback;

        public TickListener(ChronometerCallback callback)
        {
            _callback = callback;
        }

        public void Dispose()
        {

        }

        public void OnChronometerTick(Chronometer chronometer)
        {
            long time = chronometer.DrawingTime;
            long rangeTime = SystemClock.ElapsedRealtime() - chronometer.Base;
            Console.WriteLine("=====video time=====" + rangeTime);
            if (rangeTime >= MAX_TIME)
            {
                if (_callback != null)
                {
                    _callback.onStop();
                }
            }
        }
    }
}