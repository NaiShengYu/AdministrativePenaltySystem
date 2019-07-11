using Android.App;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using WTONewProject.Droid.Servers;
using WTONewProject.Tools;
using Xamarin.Forms;
using static Android.Hardware.Camera;

namespace WTONewProject.Droid
{
    [Activity]
    public class RecordActivity : Activity, ISurfaceHolderCallback, ChronometerCallback
    {
        ImageView ivStart;
        ImageView ivBack;
        ImageView ivDelete;
        VideoView vv;
        Chronometer chronometer;
        long rangeTime;
        bool isRecording;

        TickListener tickListener;
        MediaRecorder recorder;
        string savePath = "";
        string dirPath = "";
        string videoName = "";


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            SetContentView(Resource.Layout.layout_record);
            initContent();
            initView();
        }

        private void initContent()
        {
            dirPath = new FileService().GetExtrnalStoragePath(Constants.STORAGE_TYPE_MOVIES);
            tickListener = new TickListener(this);
            resetSavePath();
        }

        private void resetSavePath()
        {
            videoName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".mp4";
            savePath = dirPath + "/" + videoName;
        }


        private void initView()
        {
            ivStart = FindViewById<ImageView>(Resource.Id.Start);
            ivBack = FindViewById<ImageView>(Resource.Id.Ok);
            ivDelete = FindViewById<ImageView>(Resource.Id.Delete);
            vv = FindViewById<VideoView>(Resource.Id.video);
            chronometer = FindViewById<Chronometer>(Resource.Id.timer);
            ivBack.Visibility = ViewStates.Gone;
            ivDelete.Visibility = ViewStates.Gone;
            ivStart.Click += TvStart_Click;
            ivBack.Click += TvBack_Click;
            ivDelete.Click += TvDelete_Click;
            vv.Holder.AddCallback(this);
        }

        private void TvBack_Click(object sender, EventArgs e)
        {
            back();
        }


        private void TvDelete_Click(object sender, EventArgs e)
        {
            delete();
        }


        private void TvStart_Click(object sender, EventArgs e)
        {
            if (isRecording)//暂停
            {
                stop(false);
                ivStart.SetImageResource(Resource.Drawable.nim_video_capture_start_btn);
                ivBack.Visibility = ViewStates.Visible;
                ivDelete.Visibility = ViewStates.Visible;
            }
            else//录制
            {
                if (rangeTime > 0)
                {
                    return;
                }
                ivStart.SetImageResource(Resource.Drawable.nim_video_capture_stop_btn);
                ivBack.Visibility = ViewStates.Gone;
                ivDelete.Visibility = ViewStates.Gone;
                prepare();
                start();
            }
            isRecording = !isRecording;
        }

        Android.Hardware.Camera camera;
        int bestIndex;
        

        private void prepare()
        {
            if (recorder == null)
            {
                recorder = new MediaRecorder();
            }
            recorder.Reset();
            if (camera != null)
            {
                camera.Unlock();
                recorder.SetCamera(camera);
            }
            recorder.SetVideoSource(VideoSource.Camera);
            recorder.SetAudioSource(AudioSource.Mic);
            recorder.SetOutputFormat(OutputFormat.Mpeg4);
            recorder.SetVideoEncoder(VideoEncoder.H264);
            recorder.SetAudioEncoder(AudioEncoder.Aac);
            recorder.SetOrientationHint(90);
            recorder.SetOutputFile(savePath);
            recorder.SetMaxDuration(15 * 1000);
            recorder.SetMaxFileSize(5 * 1000 * 1000);
            recorder.SetVideoEncodingBitRate(2 * 1024 * 1024);
            recorder.SetVideoFrameRate(15);
            //recorder.SetPreviewDisplay(vv.Holder.Surface);
            if (videoSizeList != null && videoSizeList.Count > 0)
            {
                recorder.SetVideoSize(videoSizeList[bestIndex].Width, videoSizeList[bestIndex].Height);
            }
            try
            {
                recorder.Prepare();
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("===prepare error:" + e);
            }
        }

        private void start()
        {
            if (recorder == null || chronometer == null) return;
            try
            {
                recorder.Start();
                chronometer.Base = SystemClock.ElapsedRealtime();
                chronometer.OnChronometerTickListener = tickListener;
                chronometer.Start();
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("===start error:" + e);
            }
        }

        private void delete()
        {
            isRecording = false;
            rangeTime = 0;
            ivStart.SetImageResource(Resource.Drawable.nim_video_capture_start_btn);
            ivBack.Visibility = ViewStates.Gone;
            ivDelete.Visibility = ViewStates.Gone;
            try
            {
                if (camera != null)
                {
                    camera.StartPreview();
                }
                chronometer.Base = SystemClock.ElapsedRealtime();
                System.IO.File.Delete(savePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("===delete error:" + ex);
            }
            finally
            {
                resetSavePath();
            }
        }

        private void stop(bool release)
        {
            try
            {
                if (vv != null)
                {
                    vv.Pause();
                }
                if (camera != null)
                {
                    camera.StopPreview();
                }
                if (recorder != null && rangeTime > 0)
                {
                    recorder.Stop();
                }
                if (release)
                {
                    if (recorder != null)
                    {
                        recorder.Release();
                        recorder = null;
                    }
                    if (camera != null)
                    {
                        camera.Release();
                        camera = null;
                    }

                }
                chronometer.Stop();
                rangeTime = SystemClock.ElapsedRealtime() - chronometer.Base;//ms
            }
            catch (Exception ex)
            {


            }
        }

        private void back()
        {
            stop(true);
            Xamarin.Forms.MessagingCenter.Send<ContentPage, string>(new ContentPage(), "RecordVideo", videoName);
            Finish();
        }

        protected override void OnDestroy()
        {
            stop(true);
            base.OnDestroy();
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {

        }

        List<Android.Hardware.Camera.Size> prviewSizeList;
        List<Android.Hardware.Camera.Size> videoSizeList;
        int cameraPreviewWidth = 0;//预览尺寸
        public void SurfaceCreated(ISurfaceHolder holder)
        {
            try
            {
                camera = Android.Hardware.Camera.Open();
                if (camera != null)
                {
                    Parameters parameter = camera.GetParameters();
                    IList<Android.Hardware.Camera.Size> prviewList = parameter.SupportedPreviewSizes;
                    IList<Android.Hardware.Camera.Size> videoList = parameter.SupportedVideoSizes;
                    if (prviewList != null && prviewList.Count > 0)
                    {
                        prviewSizeList = new List<Android.Hardware.Camera.Size>();
                        for (int i = 0; i < prviewList.Count; i++)
                        {
                            if (prviewList[i] != null)
                            {
                                prviewSizeList.Add(prviewList[i]);
                            }
                        }
                    }
                    if (videoList != null && videoList.Count > 0)
                    {
                        videoSizeList = new List<Android.Hardware.Camera.Size>();
                        for (int i = 0; i < videoList.Count; i++)
                        {
                            if (videoList[i] != null)
                            {
                                videoSizeList.Add(videoList[i]);
                            }
                        }
                    }
                    if (prviewSizeList != null && prviewSizeList.Count > 0)
                    {
                        cameraPreviewWidth = prviewSizeList[0].Width / 3;
                        bestIndex = BestVideoSize(cameraPreviewWidth);
                        parameter.SetPreviewSize(prviewSizeList[0].Width, prviewSizeList[0].Height);
                    }
                    camera.SetPreviewDisplay(vv.Holder);
                    camera.SetDisplayOrientation(90);
                    camera.StartPreview();
                }
            }
            catch (Exception e)
            {

            }
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {

        }

        //查找出最接近的视频录制分辨率
        public int BestVideoSize(int _w)
        {
            if (videoSizeList == null || videoSizeList.Count == 0) return 0;
            //降序排列
            videoSizeList.Sort((lhs, rhs) =>
            {
                if (lhs.Width > rhs.Width)
                {
                    return -1;
                }
                else if (lhs.Width == rhs.Width)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            });
            for (int i = 0; i < videoSizeList.Count; i++)
            {
                if (videoSizeList[i].Width < _w)
                {
                    if (i >= 1 && videoSizeList[i].Width < videoSizeList[i].Height)
                    {
                        return i - 1;
                    }
                    else
                    {
                        return i;
                    }
                }
            }
            return videoSizeList.Count / 2;
        }

        //超过MAX_TIME录制暂停
        public void onStop()
        {
            stop(false);
        }
    }
}