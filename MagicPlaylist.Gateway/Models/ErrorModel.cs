namespace MagicPlaylist.Gateway.Models
{
    public class ErrorModel
    {
        public readonly string errorType;
        public readonly string message;
        public readonly string stackTrace;

        public ErrorModel(string errorType, string message, string stackTrace)
        {
            this.errorType = errorType;
            this.message = message;
            this.stackTrace = stackTrace;
        }
    }
}
