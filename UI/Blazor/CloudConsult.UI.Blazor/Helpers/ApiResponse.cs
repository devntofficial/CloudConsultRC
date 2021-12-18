namespace CloudConsult.UI.Blazor.Helpers
{
    public class ApiResponse : IApiResponse
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();
        public IEnumerable<string> Messages { get; set; } = new List<string>();
    }
    public class ApiResponse<T> : IApiResponse<T> where T : new()
    {
        public T Payload { get; set; } = new T();
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();
        public IEnumerable<string> Messages { get; set; } = new List<string>();
    }
}