using Android.Widget;

namespace WTONewProject.Droid
{
    public class TickListener : Java.Lang.Object, Chronometer.IOnChronometerTickListener
    {
        static int MAX_TIME = 10;

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
            if (time >= MAX_TIME)
            {
                if (_callback != null)
                {
                    _callback.onStop();
                }
            }
        }
    }
}