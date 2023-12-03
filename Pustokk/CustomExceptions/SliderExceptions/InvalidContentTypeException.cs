namespace Pustokk.CustomExceptions.SliderException
{
    public class InvalidContentTypeException:Exception
    {
        public string PropertyName { get; set; }
        public InvalidContentTypeException()
        {
                
        }
        public InvalidContentTypeException(string propertyName, string message) : base(message)
        {
            PropertyName = propertyName;

        }
    }
}
