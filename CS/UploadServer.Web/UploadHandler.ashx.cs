using System;
using System.IO;
using System.Web;
using System.Web.Services;

namespace UploadServer.Web {
    public class UploadHandler : IHttpHandler {
        string FilePath { get; set; }
        string FileName { get; set; }
        int PackageCount { get; set; }
        int PackageNumber { get; set; }
        public bool IsReusable { get { return false; } }

        // Implements the IHttpHandler.ProcessRequest method.
        // Is called each time a new package is received from the client.
        public void ProcessRequest(HttpContext context) {
            ProcessQueryString(context);
            ProcessFile(context);
        }

        // Processes the query string to extract the parameters' values.
        void ProcessQueryString(HttpContext context) {
            var query = context.Request.QueryString;
            FilePath = Uri.UnescapeDataString(query["filePath"]);
            FileName = Uri.UnescapeDataString(query["fileName"]);
            PackageCount = int.Parse(query["packageCount"]);
            PackageNumber = int.Parse(query["packageNumber"]);
        }

        // Reads data from the input stream and writes it to the specified file.
        void ProcessFile(HttpContext context) {
            string serverFileName = GetServerPath(context.Server, FilePath, FileName);
            FileMode fileMode = File.Exists(serverFileName) && PackageNumber > 0 ?
                FileMode.Append :
                FileMode.Create;
            using (BinaryReader reader = new BinaryReader(context.Request.InputStream))
            using (BinaryWriter writer = new BinaryWriter(File.Open(serverFileName, fileMode))) {
                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) != 0)
                    writer.Write(buffer, 0, bytesRead);
            }
        }

        // Gets the local path to the target file from the file and directory names
        // provided by the client.
        string GetServerPath(HttpServerUtility server, string filePath, string fileName) {
            return server.MapPath(Path.Combine(filePath, Path.GetFileName(fileName)));
        }
    }
}
