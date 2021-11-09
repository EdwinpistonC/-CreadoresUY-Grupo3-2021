using System.IO;

namespace Share.Dtos
{
    public class ImageDto
    {
        public string Image {  get; set; }
        public string ImageName {  get; set; }
        public string FolderName {  get; set; }

        public ImageDto(string image, string imageName, string folderName)
        {
            Image = image;
            ImageName = imageName;
            FolderName = folderName;
        }
    }

}
