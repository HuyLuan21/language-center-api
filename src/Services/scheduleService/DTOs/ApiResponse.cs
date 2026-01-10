namespace scheduleService.DTOs
{
    public class ApiResponse<TData, TMetadata>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public TData? Data { get; set; }
        public TMetadata? MetaData { get; set; }

        public ApiResponse(bool success, string message, TData? data, TMetadata? metaData = default)
        {
            Success = success;
            Message = message;
            Data = data;
            MetaData = metaData;
        }
    }
}
