using Toyota.Common.Credential;
using Toyota.Common.Web.Platform;

namespace PakAnas
{
    public class MvcApplication : WebApplication
    {
        public MvcApplication()
        {
            ApplicationSettings.Instance.Name = "PakAnas TRAINING";
            ApplicationSettings.Instance.Alias = "ADAPTRAINING";
            ApplicationSettings.Instance.OwnerName = "WhiteOpen Teknologi";
            ApplicationSettings.Instance.OwnerAlias = "WOT";
            ApplicationSettings.Instance.OwnerEmail = "PakAnas@whiteopen.com";
            ApplicationSettings.Instance.Menu.Enabled = true;
            ApplicationSettings.Instance.Runtime.HomeController = "Home"; //uncomment this to setting default page

            ApplicationSettings.Instance.Security.EnableAuthentication = true;

            BypassLogin(true);
        }

        private void BypassLogin(bool isBypass)
        {
            if (isBypass)
            {
                ApplicationSettings.Instance.Security.IgnoreAuthorization = true;
                ApplicationSettings.Instance.Security.SimulateAuthenticatedSession = true;
                ApplicationSettings.Instance.Security.EnableSingleSignOn = false;

                ApplicationSettings.Instance.Security.SimulatedAuthenticatedUser = new User()
                {
                    Username = "dummy",
                    Password = "toyota",
                    FirstName = "ISTD",
                    LastName = "User",
                    //RegistrationNumber = "09003220"
                    //RegistrationNumber = "01222322"
                    RegistrationNumber = "09709493"
                    //RegistrationNumber =  "00010052"
                };
            }
            else
            {
                ApplicationSettings.Instance.Security.IgnoreAuthorization = true;
                ApplicationSettings.Instance.Security.SimulateAuthenticatedSession = false;
                ApplicationSettings.Instance.Security.EnableSingleSignOn = false;
            }
        }

        protected override void Startup()
        {
            ProviderRegistry.Instance.Register<IUserProvider>(typeof(UserProvider), DatabaseManager.Instance, "SecurityCenter");
            
            //ProviderRegistry.Instance.Register<UILayout>(typeof(BootstrapLayout));
            //BootstrapLayout layout = (BootstrapLayout)ApplicationSettings.Instance.UI.GetLayout();          
        }
    }
}