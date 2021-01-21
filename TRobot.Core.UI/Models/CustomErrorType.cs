namespace TRobot.Core.UI.Models
{
    public class CustomErrorType : Model
    {
        private string validationMessage;
        private ValidationErrorSeverity severity;

        public CustomErrorType(string validationMessage, ValidationErrorSeverity severity)
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

        public ValidationErrorSeverity Severity
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
