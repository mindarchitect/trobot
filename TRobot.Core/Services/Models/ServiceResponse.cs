namespace TRobot.Core.Services.Models
{
    public class ServiceResponse
    {
        public string ErrorMessage { get; set; }
        public string ErrorDescription { get; set; }

        public object Result { get; set; }
    }
}
