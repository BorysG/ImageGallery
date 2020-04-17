namespace ImageGallery.Search.Models
{
    public class ImageLocalModel
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Camera { get; set; }
        public string CroppedPicture { get; set; }
        public string FullPicture { get; set; }
        public string Tags { get; set; }
    }
}
