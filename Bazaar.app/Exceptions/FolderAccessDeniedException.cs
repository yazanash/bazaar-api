namespace Bazaar.app.Exceptions
{
    public class FolderAccessDeniedException:Exception
    {
        public string? FolderName { get; }

        public FolderAccessDeniedException() : base("The requested file path is invalid.") { }

        public FolderAccessDeniedException(string message) : base(message) { }

        public FolderAccessDeniedException(string message, Exception innerException)
            : base(message, innerException) { }

        public FolderAccessDeniedException(string folderName, string message) : base(message)
        {
            FolderName = folderName;
        }
    }
}
