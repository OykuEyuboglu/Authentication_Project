namespace authentication_project.Common
{
    public class ResultT<T> : Result
    {
        public T? Data { get; set; }

        public static ResultT<T> SuccessResult(T data, string? message = null, int? statusCode = 200)
            => new ResultT<T> 
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };

        public static ResultT<T> FailureResult(T data, string message, string? Error = null, int? statusCode = 400)
            => new ResultT<T>
            {
                Success = false,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };

        public static ResultT<T> ValidationFailure(List<string> errors)
            => new ResultT<T>
            {
                Success = false,
                Message = "Validation error",
                ValidationErrors = errors,
                StatusCode = 400
            };
    }
}
