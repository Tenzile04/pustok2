namespace Pustokk.CustomExceptions.SliderException
{
    public class InvalidNullReferanceException:Exception
    {
        public string PropertyName { get; set; }

        public InvalidNullReferanceException()
        {
                
        }
        public InvalidNullReferanceException(string propertyName, string message) : base(message)
        {
            PropertyName = propertyName;
        }
       
    }
}
