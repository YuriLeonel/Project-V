namespace API.Models.Responses
{
    public class ResponseDefault<T>
    {
        public T? Data { get; set; }
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class ResponsePaginationDefault<T> : ResponseDefault<T>
    {
        public Pagination Pagination { get; set; } = new Pagination();
    }
}
