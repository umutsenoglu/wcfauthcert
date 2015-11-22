# wcfauthcert
It sends client credentials to wcf server using certificate and wshttpbinding. You can idendify the client. Authentication and authorisation.

Welcome to the wcfauthcert wiki!

## 1. Create a wcf server
Click Visual Studio > File > New > Project > WCF Service Library

Solution name : wcfauthcert 

![](http://gdurl.com/qLSY)

You can test it by running. You will see the WCF test client.

## 2. Create a client console application
Right click on the solution in the solution explorer

Click Add > New Project

Select Console Application

![](http://gdurl.com/GluW)

## 3. Add service referance to the Console Application
Right click on the Referances in the Console Application in the solution explorer

Click Add Service Referance

Click Discover button . It will discover the WCF service that we created

![](http://gdurl.com/BBdG)

Click Ok 

### Let's test the client

Open Program.cs in the Console Application

In Main Void write this codeblock 

>             var client = new ServiceReference1.Service1Client();
>             var value = client.GetData(77);
>             Console.WriteLine(value);
>             Console.ReadLine();


Firstly Run the WCF Server Library project

( Right click the project > Debug > Start new instance )

Second Run the Console Application

You will see "You Entered 77"

## 4. Create a certificate in server

Open Command Prompt

Go to "c:\Program Files (x86)\Windows Kits\<version>\bin\x64>"

If the path doesnt exist , you must install Windows SDK. For Windows 10 : https://dev.windows.com/en-us/downloads/windows-10-sdk

Run this command : 
> makecert -r -pe -b 01/01/2013 -e 01/01/2030 -eku 1.3.6.1.5.5.7.3.1 -ss My -n CN=wcfauthcert -sky exchange

You will see "Succeeded"

## 5. Set WCF config

Open App.config file in the WCF Server Library project

Add new behavior below :

>         <behavior name="wcfauthcert">
>           <serviceMetadata httpGetEnabled="true"/>
>           <serviceDebug includeExceptionDetailInFaults="true"/>
>           <serviceCredentials>
>             <serviceCertificate findValue="wcfauthcert"
>             storeLocation="CurrentUser"
>             storeName="My"
>             x509FindType="FindBySubjectName" />
>             <userNameAuthentication userNamePasswordValidationMode="Custom"           customUserNamePasswordValidatorType="WcfServiceLibrary1.CustomUserNameValidator,WcfServiceLibrary1"/>
>           </serviceCredentials>
>         </behavior>

Add new tag in system.serviceModel :

>     <bindings>
>       <wsHttpBinding>
>         <binding name="bindws">
>           <security mode="Message">
>             <message clientCredentialType="UserName"/>
>           </security>
>         </binding>
>       </wsHttpBinding>
>     </bindings>

Add new attribute to service tag :

> behaviorConfiguration="wcfauthcert"

Change the endpoint binding attribute :

> binding="wsHttpBinding" 

Add new attribute to endpoint tag

> bindingConfiguration="bindws"

Remove this block below :

![](http://gdurl.com/35ja)

## 6. Create CustomUserNameValidator

Add new class to WCF Server Library project (Right click the project > Add > Class)
Name it CustomUserNameValidator
Right click References in the WCF Server Library project
Click Add reference
Select System.IdentityModel and System.IdentityModel.Selectors

![](http://gdurl.com/yQxQ)

Click Ok

The file must be below :

> using System.IdentityModel.Selectors;

> namespace WcfServiceLibrary1
> {

>     class CustomUserNameValidator : UserNamePasswordValidator
>     {
>         public override void Validate(string userName, string password)
>         {

>         }
>     }
> }

## 7. Refresh Console Application service referance

![](http://gdurl.com/iAOs)

## 8. Add credentials code to client

Open Program.cs in the Console Application

In Main Void must be below 

>             var client = new ServiceReference1.Service1Client();
>             client.ClientCredentials.UserName.UserName = "Tony";
>             client.ClientCredentials.UserName.Password = "abc";
>             client.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
>             var value = client.GetData(77);
>             Console.WriteLine(value);
>             Console.ReadLine();

Firstly Run the WCF Server Library project

( Right click the project > Debug > Start new instance )

Second Run the Console Application

You will see "You Entered 77"

## 9. Read credentials in the server

Open Service1.cs in the WCF Server Library project

Add codes below to GetData method

> var userName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

> object[] args = new object[] { userName, value };

> return string.Format("Hello {0}. You entered: {1}", args);

### Let's test the client

Firstly Run the WCF Server Library project

( Right click the project > Debug > Start new instance )

Second Run the Console Application

You will see "Hello Tony. You Entered 77"

## 10. Add auth controls to server

Open CustomUserNameValidator.cs file that we added

Add codes below to Validate function

> if (userName != "Tony")
>    throw new FaultException("Wrong user name!");
