using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.SecurityToken.Model;
using Amazon.SecurityToken;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Net;
using UniwayBackend.Config;
using UniwayBackend.Models.Payloads.Core.Response.Storage;
using UniwayBackend.Services.interfaces;
using UniwayBackend.Helpers;

namespace UniwayBackend.Services.implements
{
    public class Aws3Service : IAws3Service
    {

        private readonly string _bucketName;
        private readonly IAmazonS3 _awsS3Client;

        public Aws3Service(AwsCredentialsManager credentialsManager)
        {
            // Obtiene los valores y los descompone directamente
            (_awsS3Client, _bucketName) = credentialsManager.GetS3ClientAsync().Result;
        }




        public async Task<bool> DeleteFileAsync(string fileName, string versionId = "")
        {
            DeleteObjectRequest request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };

            if (!string.IsNullOrEmpty(versionId))
                request.VersionId = versionId;

            await _awsS3Client.DeleteObjectAsync(request);

            return IsFileExists(fileName, versionId);
        }

        public async Task<byte[]> DownloadFileAsync(string file)
        {
            MemoryStream ms = null; // Permitirá almacenar los datos descargados temporalmente.

            try
            {
                // Crea una solicitud para obtener un objeto desde el bucket S3.
                GetObjectRequest getObjectRequest = new GetObjectRequest
                {
                    BucketName = _bucketName, 
                    Key = file,
                };

                // Envía la solicitud a AWS S3 y obtiene la respuesta.
                using (var response = await _awsS3Client.GetObjectAsync(getObjectRequest))
                {
                    // Verifica si la respuesta fue exitosa (código HTTP 200).
                    if (response.HttpStatusCode == HttpStatusCode.OK)
                    {
                        // Crea un nuevo MemoryStream para almacenar los datos descargados.
                        using (ms = new MemoryStream())
                        {
                            // Copia el contenido del archivo desde el ResponseStream al MemoryStream.
                            await response.ResponseStream.CopyToAsync(ms);
                        }
                    }
                }

                // Verifica si el MemoryStream está vacío o si el archivo descargado no contiene datos.
                if (ms is null || ms.ToArray().Length < 1)
                    throw new FileNotFoundException(string.Format("The document '{0}' is not found", file));
                // Lanza una excepción si el archivo no existe o no se pudo descargar correctamente.

                // Devuelve el contenido del archivo como un arreglo de bytes.
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, simplemente la vuelve a lanzar (mala práctica, ya que no agrega información adicional).
                throw;
            }
        }

        public async Task<List<FileResponse>> UploadFilesAsync(IEnumerable<IFormFile> files)
        {
            var fileResponses = new List<FileResponse>();

            try
            {
                foreach (var file in files)
                {
                    using (var newMemoryStream = new MemoryStream())
                    {
                        file.CopyTo(newMemoryStream);

                        string uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                        var uploadRequest = new TransferUtilityUploadRequest
                        {
                            InputStream = newMemoryStream,
                            Key = uniqueFileName,
                            BucketName = _bucketName,
                            ContentType = file.ContentType
                        };

                        var fileTransferUtility = new TransferUtility(_awsS3Client);

                        await fileTransferUtility.UploadAsync(uploadRequest);

                        fileResponses.Add(new FileResponse
                        {
                            Url = uniqueFileName,
                            OriginalName = file.FileName,
                            ExtensionType = Path.GetExtension(file.FileName),
                            ContentType = file.ContentType,
                        });
                    }
                }

                return fileResponses;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<FileResponse> UploadFileAsync(IFormFile file)
        {
            try
            {
                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);

                    string uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = uniqueFileName,
                        BucketName = _bucketName,
                        ContentType = file.ContentType
                    };

                    var fileTransferUtility = new TransferUtility(_awsS3Client);

                    await fileTransferUtility.UploadAsync(uploadRequest);

                    return new FileResponse
                    {
                        Url = uniqueFileName,
                        OriginalName = file.FileName,
                        ExtensionType = Path.GetExtension(file.FileName),
                        ContentType = file.ContentType,
                    };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        private bool IsFileExists(string fileName, string versionId)
        {
            try
            {
                GetObjectMetadataRequest request = new GetObjectMetadataRequest()
                {
                    BucketName = _bucketName,
                    Key = fileName,
                    VersionId = !string.IsNullOrEmpty(versionId) ? versionId : null
                };

                var response = _awsS3Client.GetObjectMetadataAsync(request).Result;

                return true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is AmazonS3Exception awsEx)
                {
                    if (string.Equals(awsEx.ErrorCode, "NoSuchBucket"))
                        return false;

                    else if (string.Equals(awsEx.ErrorCode, "NotFound"))
                        return false;
                }

                throw;
            }
        }
    }
}
