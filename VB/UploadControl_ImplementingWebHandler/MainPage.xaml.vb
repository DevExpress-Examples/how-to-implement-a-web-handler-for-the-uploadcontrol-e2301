Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Controls
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports DevExpress.Xpf.Controls

Namespace UploadControl_ImplementingWebHandler
	Partial Public Class MainPage
		Inherits UserControl
		Public Sub New()
			InitializeComponent()
		End Sub

		' Handles the FileUploadCompleted event to show a preview image.
		Private Sub FileUploadCompletedHandler(ByVal sender As Object, ByVal e As UploadItemEventArgs)
			Dim path As String = _
				GetPath(uploadControl.WebHandlerUri, uploadControl.UploadServerPath, e.ItemInfo.Name)
			Dim source As ImageSource = New BitmapImage() With {.UriSource = New Uri(path)}
			uploadControl.ShowPreviewImage(e.ItemInfo, source)
		End Sub

		' Gets the path to the uploaded copy of the image located on the target server.
		Private Function GetPath(ByVal uri As Uri, _
					ByVal uploadServerPath As String, _
					ByVal fileName As String) As String
			Dim basePath As String = _
				If(uri.Port = -1, _
					String.Format("{0}://{1}", uri.Scheme, uri.Host), _
					String.Format("{0}://{1}:{2}", uri.Scheme, uri.Host, uri.Port))
			Dim fullPath As String = String.Format("{0}/{1}/{2}", basePath, uploadServerPath, fileName)
			Return fullPath
		End Function
	End Class
End Namespace
