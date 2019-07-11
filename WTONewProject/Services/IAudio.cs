using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SimpleAudioForms
{
	public interface IAudio
	{
        void PlayNetFile(string fileName);
        void PlayLocalFile(string fileName);
        void stopPlay();
        ImageSource GenerateThumbImage(string savePath, string url, long usecond);//单位秒
        void SaveThumbImage(string savePath, string fileName, string url, long usecond);
        Task<bool> CompressVideo(string inputPath, string outputPath);
        void TakeVideo();
        void VideoTranscoding(string vidoPath, string url);
    }
}