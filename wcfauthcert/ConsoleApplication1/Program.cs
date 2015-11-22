using System;
using System.ServiceModel.Security;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new ServiceReference1.Service1Client();
            client.ClientCredentials.UserName.UserName = "Tony";
            client.ClientCredentials.UserName.Password = "abc";
            client.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            var value = client.GetData(77);
            Console.WriteLine(value);
            Console.ReadLine();
        }
    }
}
