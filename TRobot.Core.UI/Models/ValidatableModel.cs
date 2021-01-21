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
                    validationErrors[propertyName].Add(new CustomErrorType(validationResult.ErrorMessage, Severity.ERROR));
                }
            }

            ErrorsCount = validationResults.Count;

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
                  (from validationAttribute in propertyInfo.GetCustomAttributes(true).OfType<ValidationAttribute>()
                   where !validationAttribute.IsValid(value)
                   select new CustomErrorType(validationAttribute.FormatErrorMessage(string.Empty), Severity.ERROR))
                   .ToList();

            validationErrors.Add(propertyName, validationErrorsList);
            ErrorsCount = validationErrorsList.Count;
            RaiseErrorsChanged(propertyName);
        }

        protected void ValidateModel()
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
                        validationErrors[property].Add(new CustomErrorType(validationResult.ErrorMessage, Severity.ERROR));
                    }
                    else
                    {
                        validationErrors.Add(property, new List<CustomErrorType> { new CustomErrorType(validationResult.ErrorMessage, Severity.ERROR) });
                    }
                }
            }

            ErrorsCount = validationResults.Count;

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
