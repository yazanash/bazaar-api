namespace Bazaar.app.Exceptions
{
    public class InvalidFileFormatException:Exception
    {
        public string? FileName { get; }

        public InvalidFileFormatException() : base("The requested file path is invalid.") { }

        public InvalidFileFormatException(string message) : base(message) { }

        public InvalidFileFormatException(string message, Exception innerException)
            : base(message, innerException) { }

        public InvalidFileFormatException(string fileName, string message) : base(message)
        {
            FileName = fileName;
        }
    }
}
