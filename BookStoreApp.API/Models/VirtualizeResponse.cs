namespace BookStoreApp.API.Models
{
    public class VirtualizeResponse<T>
    {
        public List<T> Results { get; set; }
        public int TotalSize { get; set; }
    }
}
