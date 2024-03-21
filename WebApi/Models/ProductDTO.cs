namespace WebApi.Models
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required int[] Ratings { get; set; }
    }
}
