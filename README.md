# Azure AD B2C ID Token Builder
This sample ASP.NET web application generates ID tokens and hosts the necessary metadata endpoints required to use the "id_token_hint" parameter in Azure AD B2C.

ID tokens are JSON Web Tokens (JWTs) and, in this application, are signed using RSA certificates. This application hosts an Open ID Connect metatdata endpoint and JSON Web Keys (JWKs) endpoint which are used by Azure AD B2C to validate the signature of the ID token.

In the near future, we will post a sample IEF policy which uses the id_token_hint parameter to pass claims into an Azure AD B2C custom policy.

## Disclaimer
The sample app is developed and managed by the open-source community in GitHub. The application is not part of Azure AD B2C product and it's not supported under any Microsoft standard support program or service. 
The app is provided AS IS without warranty of any kind.

### Creating a signing certificate
The sample application uses a self-signed certificate to sign the ID tokens. You can generate a valid self-signed certificate for this purpose and get the thumbprint using PowerShell *(note: Run as Administrator)*:
```Powershell
$cert = New-SelfSignedCertificate -Type Custom -Subject "CN=MySelfSignedCertificate" -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.3") -KeyUsage DigitalSignature -KeyAlgorithm RSA -KeyLength 2048 -NotAfter (Get-Date).AddYears(2) -CertStoreLocation "Cert:\CurrentUser\My"
$cert.Thumbprint
```

### Configuring the application
Update the *appSettings* values in **Web.config** with the information for your Azure AD B2C tenant and the signing certificate you just created.
1. **B2CTenant**: Your Azure AD B2C tenant name (without *.onmicrosoft.com*)
2. **B2CPolicy**: The policy which you'd like to send the id_token_hint
3. **B2CClientId**: The application ID for the Azure AD B2C app you'd like to redirect to
4. **B2CRedirectUri**: The target redirect URI for your application
5. **SigningCertThumbprint**: The thumbprint for the signing certificate you just created
6. **SigningCertAlgorithm**: The certificate algorithm (must be an RSA algorithm)

### Running the application
When you run the application, you'll be able to enter up to 8 custom claims to be included in the ID token. When you click on **Generate token and link**, the Azure AD B2C policy link and ID token will be displayed.

To inspect the generated token, copy and paste it into a tool like [JWT.ms](htttps://jwt.ms).

### Hosting the application in Azure App Service
If you publish the application to Azure App Service, you'll need to configure a valid certificate with a private key in Azure App Service.
1. First, export your certificate as a PFX file using the User Certificates management tool (or create a new one)
2. Upload your certificate in the **Private Certificates** tab of the **SSL Settings** blade of your Azure App Service
3. Under the App Service, Click **Configuration**
4. Click **Application Settings**, and then **New Application Setting**.
5. Enter Name: `WEBSITE_LOAD_CERTIFICATES`, and Value: `*`.