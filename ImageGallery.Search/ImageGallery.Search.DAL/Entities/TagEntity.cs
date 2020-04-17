using System.Collections.Generic;

namespace ImageGallery.Search.DAL.Entities
{
    public partial class TagEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ImageTagEntity> ImageTags { get; set; }
    }
}
