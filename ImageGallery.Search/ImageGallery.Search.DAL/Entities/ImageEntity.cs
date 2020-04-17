using System.Collections.Generic;

namespace ImageGallery.Search.DAL.Entities
{
    public partial class ImageEntity
    {
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public string Author { get; set; }
        public string Camera { get; set; }
        public string CroppedPicture { get; set; }
        public string FullPicture { get; set; }

        public virtual ICollection<ImageTagEntity> ImageTags { get; set; }
    }
}
