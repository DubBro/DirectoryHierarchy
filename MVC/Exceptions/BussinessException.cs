namespace MVC.Exceptions
{
    public class BussinessException : Exception
    {
        public BussinessException()
            : base()
        {
        }

        public BussinessException(string message)
            : base(message)
        {
        }

        public BussinessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
