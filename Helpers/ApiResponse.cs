using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sadkah.Backend.Helpers
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public object? Errors { get; set; }
        public object? Metadata { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string message = "", object? metadata = null)
            => new() { Success = true, Message = message, Data = data, Metadata = metadata };

        public static ApiResponse<T> FailResponse(string message, object? errors = null)
            => new() { Success = false, Message = message, Errors = errors };
    }
}