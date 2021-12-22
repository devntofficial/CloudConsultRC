namespace CloudConsult.UI.Data.Common
{
    public class ApiResponse : IApiResponse
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Messages { get; set; } = new List<string>();
    }
    public class ApiResponse<T> : IApiResponse<T> where T : new()
    {
        public T Payload { get; set; } = new T();
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Messages { get; set; } = new List<string>();
    }
}