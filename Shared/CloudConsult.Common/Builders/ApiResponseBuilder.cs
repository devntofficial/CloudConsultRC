using System;
using System.Collections.Generic;

namespace CloudConsult.Common.Builders
{
    public class ApiResponseBuilder<T> : IApiResponseBuilder<T>, IApiSuccessResponse<T>,
        IApiErrorResponse<T>, IApiSuccessResponse, IApiErrorResponse
    {
        public T Payload { get; set; }
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public IEnumerable<string> Messages { get; set; }

        public IApiErrorResponse<T> CreateErrorResponse(T data, Action<IApiErrorResponse> action)
        {
            this.Payload = data;
            this.IsSuccess = false;
            action.Invoke(this);
            return this;
        }

        public IApiSuccessResponse<T> CreateSuccessResponse(T data, Action<IApiSuccessResponse> action)
        {
            this.Payload = data;
            this.IsSuccess = true;
            action.Invoke(this);
            return this;
        }

        public void WithErrorCode(int code)
        {
            this.StatusCode = code;
        }

        public void WithErrors(params string[] errors)
        {
            this.Errors = errors;
        }

        public void WithErrors(IEnumerable<string> errors)
        {
            this.Errors = errors;
        }

        public void WithMessages(params string[] messages)
        {
            this.Messages = messages;
        }

        public void WithMessages(IEnumerable<string> messages)
        {
            this.Messages = messages;
        }

        public void WithSuccessCode(int code)
        {
            this.StatusCode = code;
        }
    }
}