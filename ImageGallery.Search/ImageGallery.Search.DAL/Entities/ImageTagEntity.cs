namespace ImageGallery.Search.DAL.Entities
{
    public partial class ImageTagEntity
    {
        public int ImageId { get; set; }
        public int TagId { get; set; }

        public virtual ImageEntity Image { get; set; }
        public virtual TagEntity Tag { get; set; }
    }
}
