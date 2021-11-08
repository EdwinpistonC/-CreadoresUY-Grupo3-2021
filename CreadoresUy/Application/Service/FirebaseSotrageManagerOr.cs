using Firebase.Auth;
using Firebase.Storage;
using Share.Dtos;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Service
{
    public class FirebaseSotrageManagerOr
    {
        private static readonly string ApiKey = "AIzaSyBSR8jmmqA4CvbFS73P8pC0o2fjTa0163s"; //clave api del proy en firebase
        private static readonly string Bucket = "creadoresuy-674c1.appspot.com";  // dir del bucket que te asigna firebase
        private static readonly string AuthEmail = "creadoresuy21@gmail.com"; // user y pass configurados 
        private static readonly string AuthPassword= "creadoresuy2021";
        public static StreamContent convertBase64ToStream(string image)
        {
            byte[] imageStringToBase64 = Convert.FromBase64String(image);
            StreamContent stream = new(new MemoryStream(imageStringToBase64));
            return stream;
        }

        public static async Task<string> SubirImagen(Stream stream, ImageDto image)
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
