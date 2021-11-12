using System;
using System.Collections.Generic;

namespace CloudConsult.Common.Builders
{
    public interface IApiResponseBuilder<T>
    {
        IApiSuccessResponse<T> CreateSuccessResponse(T data, Action<IApiSuccessResponse> action);
        IApiErrorResponse<T> CreateErrorResponse(T data, Action<IApiErrorResponse> action);
    }

    public interface IApiResponse<T> : IApiResponse
    {
        public T Payload { get; set; }
    }

    public interface IApiErrorResponse<T> : IApiResponse<T>
    {
        void WithErrorCode(int code);
        void WithErrors(params string[] errors);
        void WithErrors(IEnumerable<string> errors);
    }

    public interface IApiSuccessResponse<T> : IApiResponse<T>
    {
        void WithSuccessCode(int code);
        void WithMessages(params string[] messages);
        void WithMessages(IEnumerable<string> messages);
    }

    public interface IApiResponse
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public IEnumerable<string> Messages { get; set; }
    }

    public interface IApiErrorResponse : IApiResponse
    {
        void WithErrorCode(int code);
        void WithErrors(params string[] errors);
        void WithErrors(IEnumerable<string> errors);
    }

    public interface IApiSuccessResponse : IApiResponse
    {
        void WithSuccessCode(int code);
        void WithMessages(params string[] messages);
        void WithMessages(IEnumerable<string> messages);
    }
}