using System;
using System.Collections.Generic;

namespace CloudConsult.UI.Helpers
{
    public interface IApiResponseBuilder
    {
        IApiResponse CreateSuccessResponse(Action<IApiSuccessResponse> action);
        IApiResponse CreateErrorResponse(Action<IApiErrorResponse> action);
    }

    public interface IApiResponseBuilder<T>
    {
        IApiResponse<T> CreateSuccessResponse(T data, Action<IApiSuccessResponse> action);
        IApiResponse<T> CreateErrorResponse(T data, Action<IApiErrorResponse> action);
    }

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

    public interface IApiErrorResponse
    {
        void WithErrorCode(int code);
        void WithErrors(params string[] errors);
        void WithErrors(IEnumerable<string> errors);
    }

    public interface IApiSuccessResponse
    {
        void WithSuccessCode(int code);
        void WithMessages(params string[] messages);
        void WithMessages(IEnumerable<string> messages);
    }
}