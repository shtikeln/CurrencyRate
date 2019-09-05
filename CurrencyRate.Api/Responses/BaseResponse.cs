using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyRate.Api.Responses
{
    public class BaseResponse<T>
    {
        public BaseResponse()
        {
        }

        public BaseResponse(T result)
        {
            Data = result;
        }

        public BaseResponse(Exception ex)
        {
            IsSuccess = false;
            Error = ex.Message;
            Data = default(T);
        }

        public bool IsSuccess { get; set; } = true;
        public string Error { get; set; }
        public T Data { get; set; }
    }
}