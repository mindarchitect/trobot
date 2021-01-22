using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace TRobot.Core.UI.Models
{
    public class ValidatableModel : Model, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, ICollection<CustomErrorType>> validationErrors = new Dictionary<string, ICollection<CustomErrorType>>();

        private bool valid;
        private int errorsCount;
        
        public int ErrorsCount
        {
            get
            {
                return errorsCount;
            }
            set
            {
                errorsCount = value;
                OnPropertyChanged("ErrorsCount");
            }
        }

        public bool Valid
        {
            get
            {
                return valid;
            }
            set
            {
                valid = value;
                OnPropertyChanged("Valid");
            }
        }

        protected void ValidateModelProperty(object value, string propertyName)
        {
            if (validationErrors.ContainsKey(propertyName))
            {
                validationErrors.Remove(propertyName);
            }

            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext validationContext = new ValidationContext(this, null, null) { MemberName = propertyName };
            if (!Validator.TryValidateProperty(value, validationContext, validationResults))
            {
                validationErrors.Add(propertyName, new List<CustomErrorType>());
                foreach (ValidationResult validationResult in validationResults)
                {
                    validationErrors[propertyName].Add(new CustomErrorType(validationResult.ErrorMessage, ValidationErrorSeverity.ERROR));
                }
            }
            
            ErrorsCount = validationResults.Count;
            Valid = ErrorsCount > 0;

            RaiseErrorsChanged(propertyName);
        }

        /* Alternative solution using LINQ */
        protected void ValidateModelPropertyLinq(object value, string propertyName)
        {
            if (validationErrors.ContainsKey(propertyName))
            {
                validationErrors.Remove(propertyName);
            }

            PropertyInfo propertyInfo = GetType().GetProperty(propertyName);

            ICollection<CustomErrorType> validationErrorsList =
                  (from validationAttribute in propertyInfo.GetCustomAttributes().OfType<ValidationAttribute>()
                   where !validationAttribute.IsValid(value)
                   select new CustomErrorType(validationAttribute.FormatErrorMessage(string.Empty), ValidationErrorSeverity.ERROR))
                   .ToList();

            validationErrors.Add(propertyName, validationErrorsList);

            ErrorsCount = validationErrorsList.Count;
            Valid = ErrorsCount > 0;

            RaiseErrorsChanged(propertyName);
        }

        public void ValidateModel()
        {
            validationErrors.Clear();
            ICollection<ValidationResult> validationResults = new List<ValidationResult>();

            ValidationContext validationContext = new ValidationContext(this, null, null);
            if (!Validator.TryValidateObject(this, validationContext, validationResults, true))
            {
                foreach (ValidationResult validationResult in validationResults)
                {
                    string property = validationResult.MemberNames.ElementAt(0);
                    if (validationErrors.ContainsKey(property))
                    {
                        validationErrors[property].Add(new CustomErrorType(validationResult.ErrorMessage, ValidationErrorSeverity.ERROR));
                    }
                    else
                    {
                        validationErrors.Add(property, new List<CustomErrorType> { new CustomErrorType(validationResult.ErrorMessage, ValidationErrorSeverity.ERROR) });
                    }
                }
            }

            ErrorsCount = validationResults.Count;
            Valid = ErrorsCount > 0;

            //Raise the ErrorsChanged for all properties explicitly
            RaiseErrorsChanged("UserName");
        }

        #region INotifyDataErrorInfo members

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void RaiseErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !validationErrors.ContainsKey(propertyName))
            {
                return null;
            }
                
            var customErrorType = validationErrors[propertyName];
            return customErrorType;
        }

        public bool HasErrors
        {
            get { return validationErrors.Count > 0; }
        }

        #endregion
    }
}
