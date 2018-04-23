using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Xpf.Controls;

namespace UploadControl_ImplementingWebHandler {
    public partial class MainPage : UserControl {
        public MainPage() {
            InitializeComponent();
        }

        // Handles the FileUploadCompleted event to show a preview image.
        void FileUploadCompletedHandler(object sender, UploadItemEventArgs e) {
            string path = GetPath(uploadControl.WebHandlerUri,
                uploadControl.UploadServerPath,
                e.ItemInfo.Name);
            ImageSource source = new BitmapImage() { UriSource = new Uri(path) };
            uploadControl.ShowPreviewImage(e.ItemInfo, source);
        }

        // Gets the path to the uploaded copy of the image located on the target server.
        string GetPath(Uri uri, string uploadServerPath, string fileName) {
            string basePath = uri.Port == -1 ?
                string.Format("{0}://{1}", uri.Scheme, uri.Host) :
                string.Format("{0}://{1}:{2}", uri.Scheme, uri.Host, uri.Port);
            string fullPath = string.Format("{0}/{1}/{2}", basePath, uploadServerPath, fileName);
            return fullPath;
        }
    }
}
