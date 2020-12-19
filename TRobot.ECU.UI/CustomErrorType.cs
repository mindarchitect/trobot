using TRobot.Core.UI.Models;

namespace TRobot.ECU.UI
{
    public class CustomErrorType : Model
    {
        private string validationMessage;
        private Severity severity;

        public CustomErrorType(string validationMessage, Severity severity)
        {
            ValidationMessage = validationMessage;
            Severity = severity;
        }

        public string ValidationMessage
        {
            get
            {
                return validationMessage;
            }
            set
            {
                validationMessage = value;
                OnPropertyChanged("ValidationMessage");
            }
        }

        public Severity Severity
        {
            get
            {
                return severity;
            }
            set
            {
                severity = value;
                OnPropertyChanged("Severity");
            }
        }
    }
}
