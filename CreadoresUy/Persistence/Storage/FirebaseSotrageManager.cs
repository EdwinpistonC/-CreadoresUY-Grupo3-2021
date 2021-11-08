using Application.Interface;
using Firebase.Auth;
using Firebase.Storage;
using Share.Dtos;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Storage
{
    public class FirebaseSotrageManager : IFirebaseStorageManager
    {

        public string ApiKey { get; set; }
        public string Bucket { get; set; }
        public string AuthEmail { get; set; }
        public string AuthPassword { get; set; }

    public StreamContent ConvertBase64ToStream(string image)
        {
            byte[] imageStringToBase64 = Convert.FromBase64String(image);
            StreamContent stream = new(new MemoryStream(imageStringToBase64));
            return stream;
        }

        public async Task<string> SubirImagen(Stream stream, ImageDto image)
        {
            string imageFromFirebase = "";
            FirebaseAuthProvider firebaseConf = new( new FirebaseConfig(ApiKey));

            FirebaseAuthLink authConf = await firebaseConf.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
            //devuelve un token 
            Console.WriteLine(authConf.FirebaseToken);
            CancellationTokenSource cancellationToken = new();

            FirebaseStorageTask storageManager = new FirebaseStorage(Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(authConf.FirebaseToken),
                    ThrowOnCancel = true
                }).Child(image.FolderName)
                .Child(image.ImageName).
                PutAsync(stream, cancellationToken.Token);

            try
            {
                imageFromFirebase = await storageManager;
            }
            catch
            {
            }
            Console.WriteLine(imageFromFirebase);
            return imageFromFirebase;
        }

        
    }

    
}
/*
ApiKey = "AIzaSyBSR8jmmqA4CvbFS73P8pC0o2fjTa0163s"; //clave api del proy en firebase
Bucket = "creadoresuy-674c1.appspot.com";  // dir del bucket que te asigna firebase
AuthEmail = "creadoresuy21@gmail.com"; // user y pass configurados 
AuthPassword = "creadoresuy2021";
*/