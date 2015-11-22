using System;
using System.IdentityModel.Selectors;
using System.ServiceModel;

namespace WcfServiceLibrary1
{

    class CustomUserNameValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {

            if (userName != "Tony")
                throw new FaultException("Wrong user name!");

        }
    }
}
