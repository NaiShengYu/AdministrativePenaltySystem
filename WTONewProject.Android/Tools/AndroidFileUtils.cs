

using Android.Content;
using Android.Database;
using Android.Net;
using Android.Provider;
using Java.IO;

namespace WTONewProject.Droid.Tools
{
    public class AndroidFileUtils
    {
        public byte[] GetBytesFromFile(Java.IO.File f)
        {
            if (f == null)
            {
                return null;
            }
            try
            {
                FileInputStream stream = new FileInputStream(f);
                Java.IO.ByteArrayOutputStream outs = new ByteArrayOutputStream(1000);
                //ByteArrayOutputStream out = new ByteArrayOutputStream(1000);
                byte[] b = new byte[1000];
                int n;
                while ((n = stream.Read(b)) != -1)
                    outs.Write(b, 0, n);
                stream.Close();
                outs.Close();
                return outs.ToByteArray();
            }
            catch (System.IO.IOException e)
            {
            }
            return null;
        }


        public static Uri GetImageContentUri(Context context, string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return null;
            }
            ICursor cursor = context.ContentResolver.Query(MediaStore.Images.Media.ExternalContentUri,
                    new string[] { MediaStore.Images.Media.InterfaceConsts.Id }, MediaStore.Images.Media.InterfaceConsts.Data + "=? ",
                    new string[] { filePath }, null);
            if (cursor != null && cursor.MoveToFirst())
            {
                int id = cursor.GetInt(cursor.GetColumnIndex(MediaStore.MediaColumns.Id));
                Uri baseUri = Uri.Parse("content://media/external/images/media");
                cursor.Close();
                return Uri.WithAppendedPath(baseUri, "" + id);
            }
            else
            {
                ContentValues values = new ContentValues();
                values.Put(MediaStore.Images.Media.InterfaceConsts.Data, filePath);
                return context.ContentResolver.Insert(MediaStore.Images.Media.ExternalContentUri, values);
            }
        }
    }
}