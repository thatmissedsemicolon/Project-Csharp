namespace PP.Library.Utilities
{
    public class QueryMessage
    {
        private string? query;
        public string Query { get => query ?? string.Empty;
            set
            {
                query = value;
            }
        } 
    }
}
