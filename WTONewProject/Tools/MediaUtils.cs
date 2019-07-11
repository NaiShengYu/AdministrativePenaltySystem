using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;

namespace WTONewProject.Tools
{
    public class MediaUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">0拍照 1视频</param>
        /// <param name="saveToAlbum"></param>
        /// <param name="compress"></param>
        /// <returns></returns>
        public async Task<MediaFile> TakeMedia(int type, bool saveToAlbum, int compress)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                //await DisplayAlert("无法访问摄像头", "设备不支持", "OK");
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
                MediaFile file = null;
                if(type == 0)//拍照
                {
                    file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        SaveToAlbum = saveToAlbum,
                        CompressionQuality = compress < 0 ? 50 : compress,
                    });
                }
                else if(type == 1)//拍视频
                {
                    file = await CrossMedia.Current.TakeVideoAsync(new StoreVideoOptions
                    {
                        SaveToAlbum = saveToAlbum,
                        CompressionQuality = compress < 0 ? 50 : compress,
                        DesiredLength = new TimeSpan(0, 0, 10),
                    });
                }              

                if (file != null)
                {
                    Console.WriteLine("===" + file.AlbumPath); //相册路径
                    Console.WriteLine("===" + file.Path); // 私有路径
                }
                return file;
            }
            return null;
        }

    }
}
