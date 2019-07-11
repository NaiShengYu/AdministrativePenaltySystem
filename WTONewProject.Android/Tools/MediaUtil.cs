using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Sample;
using SimpleAudioForms;
using System;
using System.Threading.Tasks;
using WTONewProject.Services;
using Xamarin.Forms;

namespace WTONewProject.Tools
{
    public class MediaUtil
    {

        public event EventHandler<EventArgs> SaveVideoPath;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">0拍照 1视频</param>
        /// <param name="saveToAlbum"></param>
        /// <param name="compress"></param>
        /// <returns></returns>
        public async Task<Java.IO.File> TakePhoto(bool saveToAlbum, int compress)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                DependencyService.Get<IToast>().LongAlert("无法访问摄像头");
                return null;
            }
            //检查照相机和存储权限，没有的话进行一次请求
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }

            if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
            {
                MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    SaveToAlbum = saveToAlbum,
                    CompressionQuality = compress < 0 ? 50 : compress,
                });

                if (file != null)
                {
                    Console.WriteLine("===" + file.AlbumPath); //相册路径
                    Console.WriteLine("===" + file.Path); // 私有路径
                    return new Java.IO.File(file.Path);
                }
            }
            return null;
        }


        public async void TakeVideo()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                DependencyService.Get<IToast>().LongAlert("无法访问摄像头");
                return;
            }
            //检查照相机和存储权限，没有的话进行一次请求
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }

            if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
            {
                DependencyService.Get<IAudio>().TakeVideo();
                MessagingCenter.Unsubscribe<Object, string>(this, "RecordVideo");
                MessagingCenter.Subscribe<Object, string>(this, "RecordVideo", async (arg1, arg2) =>
                {
                    string _videoPartPath = arg2 as string;//相对路径
                    if (string.IsNullOrWhiteSpace(_videoPartPath)) return;
                    string dirPath = DependencyService.Get<IFileService>().GetExtrnalStoragePath(Constants.STORAGE_TYPE_MOVIES) + "/";
                    MessagingCenter.Unsubscribe<ContentPage, string>(this, "RecordVideo");
                    string path = dirPath + _videoPartPath;//视频完整路径
                    SaveVideoPath.Invoke(path, new EventArgs());
                });
            }
        }
    }
}
