namespace authentication_project.Common
{
    public class Result
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int? StatusCode { get; set; }
        // API durum kodlarını tasıma - return StatusCode(result.StatusCode ?? 200, result);
        public bool IsFailure => !Success;
        public DateTime? TimeStamp { get; set; } = DateTime.UtcNow;
        public string Error { get; set; }
        public List<string> ValidationErrors { get; set; } // modelstate hataları    olsun mu sor
        protected Result(bool success, string error)
        {
            if (success && error != string.Empty)
                throw new InvalidOperationException();

            if(!success && error != string.Empty)
                throw new InvalidOperationException();
            Success = success;
            Error = error;
        }




        //public static Result SuccessResult(string? message = null)
        //    => new Result { Success = true, Message = message, StatusCode = 200 };
        //public static Result FailureResult(string message, string? Error = null, int? statusCode = 400)
        //    => new Result { Success = false, Message = message, StatusCode = statusCode };

        //public static Result ValidationFailure(List<string> errors)
        //    => new Result { Success = false, Message = "Validation error", ValidationErrors = errors, StatusCode = 400 };

        //public static Result Fail(string message)
        //{
        //    return new Result { Success = false, Message = message };
        //}
    }
}
