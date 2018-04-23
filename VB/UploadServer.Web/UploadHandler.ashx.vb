Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Web
Imports System.Web.Services

Namespace UploadServer.Web
	Public Class UploadHandler
		Implements IHttpHandler
		Private privateFilePath As String
		Private Property FilePath() As String
			Get
				Return privateFilePath
			End Get
			Set(ByVal value As String)
				privateFilePath = value
			End Set
		End Property
		Private privateFileName As String
		Private Property FileName() As String
			Get
				Return privateFileName
			End Get
			Set(ByVal value As String)
				privateFileName = value
			End Set
		End Property
		Private privatePackageCount As Integer
		Private Property PackageCount() As Integer
			Get
				Return privatePackageCount
			End Get
			Set(ByVal value As Integer)
				privatePackageCount = value
			End Set
		End Property
		Private privatePackageNumber As Integer
		Private Property PackageNumber() As Integer
			Get
				Return privatePackageNumber
			End Get
			Set(ByVal value As Integer)
				privatePackageNumber = value
			End Set
		End Property
		Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
			Get
				Return False
			End Get
		End Property

		' Implements the IHttpHandler.ProcessRequest method.
		' Is called each time a new package is received from the client.
		Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
			ProcessQueryString(context)
			ProcessFile(context)
		End Sub

		' Processes the query string to extract the parameters' values.
		Private Sub ProcessQueryString(ByVal context As HttpContext)
			Dim query = context.Request.QueryString
			FilePath = Uri.UnescapeDataString(query("filePath"))
			FileName = Uri.UnescapeDataString(query("fileName"))
			PackageCount = Integer.Parse(query("packageCount"))
			PackageNumber = Integer.Parse(query("packageNumber"))
		End Sub

		' Reads data from the input stream and writes it to the specified file.
		Private Sub ProcessFile(ByVal context As HttpContext)
			Dim serverFileName As String = GetServerPath(context.Server, FilePath, FileName)
			Dim fileMode As FileMode = _
				If(File.Exists(serverFileName) AndAlso PackageNumber > 0, _
					FileMode.Append, _
					FileMode.Create)
			Using reader As New BinaryReader(context.Request.InputStream)
			Using writer As New BinaryWriter(File.Open(serverFileName, fileMode))
				Dim buffer(4095) As Byte
				Dim bytesRead As Integer
				bytesRead = reader.Read(buffer, 0, buffer.Length)
				Do While bytesRead <> 0
					writer.Write(buffer, 0, bytesRead)
					bytesRead = reader.Read(buffer, 0, buffer.Length)
				Loop
			End Using
			End Using
		End Sub

		' Gets the local path to the target file from the file and directory names provided by the client.
		Private Function GetServerPath(ByVal server As HttpServerUtility, _
										ByVal filePath As String, _
										ByVal fileName As String) As String
			Return server.MapPath(Path.Combine(filePath, Path.GetFileName(fileName)))
		End Function
	End Class
End Namespace
