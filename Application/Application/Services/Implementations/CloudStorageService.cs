using Application.Services.Interfaces;
using Application.Settings;
using Common.Helpers;
using Google;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using System.Web;

namespace Application.Services.Implementations;

public class CloudStorageService : ICloudStorageService
{
    private static readonly StorageClient Storage;
    private readonly AppSettings _settings;

    static CloudStorageService()
    {
        Storage = CloudStorageHelper.GetStorage();
    }

    public CloudStorageService(IOptions<AppSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task<string> Upload(Guid id, string contentType, Stream stream)
    {
        try
        {
            await Storage.UploadObjectAsync(
                _settings.Bucket,
                $"{_settings.Folder}/{id}",
                contentType,
                stream,
                null,
                CancellationToken.None);
            var url = "https://firebasestorage.googleapis.com/v0/b/plant-detection-7a03e.appspot.com/o/images%2F" + id + "?alt=media";
            return url;
            //return CloudStorageHelper.GenerateV4UploadSignedUrl(
            //    _settings.Bucket,
            //    $"{_settings.Folder}/{id}");
            //return GetMediaLink(id);
        }
        catch
        {
            throw;
        }
    }

    // Delete an object, IsSuccess if deleted successfully or not found
    public async Task<string> Delete(Guid id)
    {
        try
        {
            await Storage.DeleteObjectAsync(
                _settings.Bucket,
                $"{_settings.Folder}/{id}",
                null,
                CancellationToken.None
            );
            return "Success";
        }
        catch (GoogleApiException e)
        {
            return e.HttpStatusCode.ToString();
        }
        catch
        {
            throw;
        }
    }

    // Object url
    public string GetMediaLink(Guid id)
    {
        return CloudStorageHelper.GenerateV4UploadSignedUrl(
            HttpUtility.UrlEncode(_settings.Bucket),
            _settings.Folder + '/' + id);
    }
}