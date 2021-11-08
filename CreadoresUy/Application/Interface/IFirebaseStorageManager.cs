using Share.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IFirebaseStorageManager
    {
        string ApiKey { get; set; }
        string Bucket { get; set; }
        string AuthEmail { get; set; }
        string AuthPassword { get; set; }

        abstract StreamContent ConvertBase64ToStream(string image);
        abstract Task<string> SubirImagen(Stream stream, ImageDto image);

    }
}
