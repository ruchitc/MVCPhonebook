namespace PhonebookClient.ViewModels
{
    public class PaginationServiceResponse<T>
    {
        public T Data { get; set; }
        public int Total { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
