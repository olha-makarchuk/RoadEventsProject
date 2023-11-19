using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using Xabe.FFmpeg;


public class GoogleDrive
{
    private const string PathToService = @"C:\Users\olama\OneDrive\Desktop\roadevents-405216-3aec149490dd.json";

    private DriveService service;

    public GoogleDrive()
    {
        service = GetDriveService();
    }

    public DriveService GetDriveService()
    {
        var credential = GoogleCredential.FromFile(PathToService)
            .CreateScoped(new[] { DriveService.ScopeConstants.Drive });

        return new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential
        });
    }

public async Task<string> UploadAsync(string nameFile, IFormFile photo, string format, string contentType)
    {
        var fileMetadata = new Google.Apis.Drive.v3.Data.File()
        {
            Name = $"{nameFile}{format}",
            Parents = new List<string> { "11Ly-K7WH1uQvncCNnbx_FxcU66LAdMPg" }
        };

        using (var memoryStream = new MemoryStream())
        {
            // Compress file content
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                var entry = archive.CreateEntry($"{nameFile}{format}", CompressionLevel.Optimal);
                using (var entryStream = entry.Open())
                {
                    await photo.CopyToAsync(entryStream);
                }
            }

            // Reset the memory stream position to beginning
            memoryStream.Seek(0, SeekOrigin.Begin);

            var request = service.Files.Create(fileMetadata, memoryStream, contentType);
            request.Fields = "id";
            var results = await request.UploadAsync(CancellationToken.None);

            if (results != null)
            {
                // Retrieve uploaded file details
                var listRequest = service.Files.List();
                listRequest.Q = $"name = '{nameFile}{format}'";
                var files = await listRequest.ExecuteAsync();

                if (files.Files.Count > 0)
                {
                    var file = files.Files[0];

                    // Set file permission
                    var permissionRequest = service.Permissions.Create(new Google.Apis.Drive.v3.Data.Permission
                    {
                        Type = "anyone",
                        Role = "reader"
                    }, file.Id);
                    permissionRequest.Fields = "id";
                    var permission = await permissionRequest.ExecuteAsync();

                    return $"https://drive.google.com/uc?id={file.Id}";
                }
            }
        }

        throw new FileNotFoundException("File was not saved.");
    }
}
