namespace ProvaPub.Models
{
    public class PagedList<T> where T : class
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public bool HasNext { get; set; }
    }
}
