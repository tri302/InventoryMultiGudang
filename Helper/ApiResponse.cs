namespace InventoryMultiGudang.Helper
{
    /// <summary>
    /// Standardisasi format response JSON untuk pertukaran data BE ke FE.
    /// </summary>
    public class ApiResponse<T>
    {
        public string Status { get; set; } = "Success"; // Success, Failed, Error, NotFound
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public string RedirectUrl { get; set; } = string.Empty;

        public static ApiResponse<T> SuccessResult(string message, string redirectUrl = "", T? data = default)
        {
            return new ApiResponse<T>
            {
                Status = "Success",
                Message = message,
                RedirectUrl = redirectUrl,
                Data = data
            };
        }

        public static ApiResponse<T> FailedResult(string message)
        {
            return new ApiResponse<T>
            {
                Status = "Failed",
                Message = message
            };
        }

        public static ApiResponse<T> NotFoundResult(string message)
        {
            return new ApiResponse<T>
            {
                Status = "NotFound",
                Message = message
            };
        }

        public static ApiResponse<T> ErrorResult(string message)
        {
            return new ApiResponse<T>
            {
                Status = "Error",
                Message = message
            };
        }
    }
}