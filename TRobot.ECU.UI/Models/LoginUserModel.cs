using System.ComponentModel.DataAnnotations;
using TRobot.Core.UI.Models;

namespace TRobot.ECU.UI.ViewModels
{
    class LoginUserModel : ValidatableModel
    {
        private string userName;
        private string password;

        [Required(ErrorMessage = "You must enter a username")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "The username must be between 4 and 10 characters long")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "The username must only contain letters and numbers")]
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
                //ValidateModelPropertyLinq(userName, "UserName");
                ValidateModelProperty(userName, "UserName");
                OnPropertyChanged("UserName"); 
            }
        }
  
        internal string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }
    }
}
