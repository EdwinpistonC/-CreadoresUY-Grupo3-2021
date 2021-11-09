using Application.Interface;
using Share.Dtos;
using System.Threading.Tasks;


namespace Application.Service
{
    public class ImagePostService
    {
         private readonly IFirebaseStorageManager _storageManager;
          public ImagePostService(IFirebaseStorageManager storageManager)
          {
              _storageManager = storageManager;
          }

        public ImagePostService()
        {
        }
        public async Task<string> postImage(ImageDto imageDto)
        {/*
            var imageFromBase64ToStream = FirebaseSotrageManager.convertBase64ToStream(imageDto.Image); //convierto la imagen
            var imageStream = imageFromBase64ToStream.ReadAsStream();

            string imageFromFirebase = await FirebaseSotrageManager.SubirImagen(imageStream, imageDto);
            return imageFromFirebase; 
           */
            var imageFromBase64ToStream = _storageManager.ConvertBase64ToStream(imageDto.Image);
            var imageStream = imageFromBase64ToStream.ReadAsStream();
            string imageFromFirebase = await _storageManager.SubirImagen(imageStream, imageDto);
            return imageFromFirebase; //link de la imagen obtenido del post en firbase con el token para poder obt
        }
    }
}
