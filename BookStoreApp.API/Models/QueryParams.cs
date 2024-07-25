namespace BookStoreApp.API.Models
{
    public class QueryParams
    {
        private int _pageSize = 10;
        public int StartIndex { get; set; }
        public int PageSize { 
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }
    }
}
