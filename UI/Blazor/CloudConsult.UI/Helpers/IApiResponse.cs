namespace CloudConsult.UI.Helpers
{
    public interface IApiResponse<T> : IApiResponse
    {
        public T Payload { get; }
    }

    public interface IApiResponse
    {
        public bool IsSuccess { get; }
        public int StatusCode { get; }
        public IEnumerable<string> Errors { get; }
        public IEnumerable<string> Messages { get; }
    }
}