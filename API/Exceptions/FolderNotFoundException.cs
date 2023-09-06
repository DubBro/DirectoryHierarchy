namespace DirectoryHierarchy.Exceptions
{
    public class FolderNotFoundException : Exception
    {
        public FolderNotFoundException()
            : base()
        {
        }

        public FolderNotFoundException(string message)
            : base(message)
        {
        }

        public FolderNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
