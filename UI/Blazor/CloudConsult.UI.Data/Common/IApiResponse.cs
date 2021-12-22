namespace CloudConsult.UI.Data.Common
{
    public interface IApiResponse<T> : IApiResponse
    {
        public T Payload { get; }
    }

    public interface IApiResponse
    {
        public bool IsSuccess { get; }
        public int StatusCode { get; }
        public List<string> Errors { get; }
        public List<string> Messages { get; }
    }
}