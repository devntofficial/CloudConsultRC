namespace CloudConsult.Common.Builders;
public class ApiResponseBuilder : IApiResponseBuilder, IApiResponse, IApiSuccessResponse, IApiErrorResponse
{
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public IEnumerable<string> Errors { get; set; }
    public IEnumerable<string> Messages { get; set; }

    public IApiResponse CreateErrorResponse(Action<IApiErrorResponse> action)
    {
        IsSuccess = false;
        action.Invoke(this);
        return this;
    }

    public IApiResponse CreateSuccessResponse(Action<IApiSuccessResponse> action)
    {
        IsSuccess = false;
        action.Invoke(this);
        return this;
    }

    public void WithErrorCode(int code)
    {
        StatusCode = code;
    }

    public void WithErrors(params string[] errors)
    {
        Errors = errors;
    }

    public void WithErrors(IEnumerable<string> errors)
    {
        Errors = errors;
    }

    public void WithMessages(params string[] messages)
    {
        Messages = messages;
    }

    public void WithMessages(IEnumerable<string> messages)
    {
        Messages = messages;
    }

    public void WithSuccessCode(int code)
    {
        StatusCode = code;
    }
}
public class ApiResponseBuilder<T> : IApiResponseBuilder<T>, IApiResponse<T>, IApiSuccessResponse, IApiErrorResponse
{
    public T Payload { get; set; }
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public IEnumerable<string> Errors { get; set; }
    public IEnumerable<string> Messages { get; set; }

    public IApiResponse<T> CreateErrorResponse(T data, Action<IApiErrorResponse> action)
    {
        Payload = data;
        IsSuccess = false;
        action.Invoke(this);
        return this;
    }

    public IApiResponse<T> CreateSuccessResponse(T data, Action<IApiSuccessResponse> action)
    {
        Payload = data;
        IsSuccess = true;
        action.Invoke(this);
        return this;
    }

    public void WithErrorCode(int code)
    {
        StatusCode = code;
    }

    public void WithErrors(params string[] errors)
    {
        Errors = errors;
    }

    public void WithErrors(IEnumerable<string> errors)
    {
        Errors = errors;
    }

    public void WithMessages(params string[] messages)
    {
        Messages = messages;
    }

    public void WithMessages(IEnumerable<string> messages)
    {
        Messages = messages;
    }

    public void WithSuccessCode(int code)
    {
        StatusCode = code;
    }
}