namespace TiendaOnline.Common.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public bool IsStarred { get; set; }

        //public Category Category { get; set; }\
        //public ICollection<ProductImage> ProductImages { get; set; }

        //public int ProductImagesNumber => ProductImages == null ? 0 : ProductImages.Count;

        //public string ImageFullPath => ProductImages == null || ProductImages.Count == 0
        //? $"https://tiendaonline.azurewebsites.net/images/noimage.png"
        //: ProductImages.FirstOrDefault().ImageFullPath;
    }
}
