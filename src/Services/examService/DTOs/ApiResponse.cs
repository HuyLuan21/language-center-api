namespace examService.DTOs
{
    public class ApiResponse<TData, TMeta>(bool success, string message, TData? data, TMeta? metaData = default)
    {
        public bool Success { get; set; } = success;
        public string Message { get; set; } = message;
        public TData? Data { get; set; } = data;
        public TMeta? Meta { get; set; } = metaData;
    }
}
