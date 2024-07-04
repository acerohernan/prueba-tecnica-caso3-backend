namespace MantenimientoSimple.Api.DTOs
{
    public class ErrorResponse
    {
        public string Status { get; set; } = "Error";

        public string Message { get; set; } = string.Empty;

        public ErrorResponse(string message)
        {
            Message = message;
        }
    }
}
