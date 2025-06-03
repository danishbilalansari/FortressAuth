# Fortress Auth

A .Net 9 based project that contains the implementation of multi factor authentication (mfa).

## Project Structure

<pre lang="text"><code>
FortressAuth/                
├── FortressAuth.sln
│
├── IdentityServer/
│   ├── Connected Services/
│   ├── Dependencies/
│   ├── Properties/
│   ├── wwwroot/
│   ├── Controllers/
│   │   ├── AccountController.cs
│   │   └── MfaController.cs
│   ├── Data/
│   │   └── ApplicationDbContext.cs
│   ├── Extensions/
│   │   ├── ApplicationUserExtensions.cs
│   │   └── MfaRecoveryCodeExtensions.cs
│   ├── Models/
│   │   ├── AccountViewModels/
│   │   │   ├── LoginViewModel.cs
│   │   │   └── RegisterViewModel.cs
│   │   └── MfaViewModels/
│   │       ├── MfaSetupViewModel.cs
│   │       └── VerifyViewModel.cs
│   ├── Services/
│   │   ├── CustomProfileService.cs
│   │   ├── EmailSender.cs
│   │   ├── MfaRecoveryCodeService.cs
│   │   └── TOTPService.cs
│   ├── Views/
│   │   ├── Account/
│   │   │   ├── _ViewImports.cshtml
│   │   │   ├── Login.cshtml
│   │   │   ├── Profile.cshtml
│   │   │   └── Register.cshtml
│   │   ├── Mfa/
│   │   │   ├── RecoveryCodes.cshtml
│   │   │   ├── Setup.cshtml
│   │   │   └── Verify.cshtml
│   │   └── Shared/
│   │       ├── _Layout.cshtml
│   │       └── _ValidationScriptsPartial.cshtml
│   ├── appsettings.json
│   └── Program.cs
│   
├── Shared/
│   ├── Dependencies/
│   ├── Models/
│   │   ├── ApplicationUser.cs
│   │   ├── MfaRecoveryCode.cs
│   │   ├── UseRecoveryCodeRequest.cs
│   │   └── VerifyMfaRequest.cs
│   └── Services/
│       └── Base32Encoding.cs
│   	
├── WebAPI/
│   ├── Connected Services/
│   ├── Dependencies/
│   ├── Properties/
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── PublicController.cs
│   │   └── SecureController.cs
│   ├── Services/
│   │   ├── ITokenService.cs
│   │   └── TokenService.cs
│   ├── appsettings.json
│   ├── Program.cs
│   └── WebAPI.http
│   
├── WebApp/
│   ├── Connected Services/
│   ├── Dependencies/
│   ├── Properties/
│   ├── wwwroot/
│   │   ├── css/
│   │   │   └── site.css
│   │   ├── js/
│   │   ├── lib/
│   │   └── favicon.ico
│   ├── Controllers/
│   │   ├── AccountController.cs
│   │   └── HomeController.cs
│   ├── Models/
│   │   ├── ErrorViewModel.cs
│   │   └── ProfileViewModel.cs
│   ├── Services/
│   │   ├── ApiClientService.cs
│   │   └── IApiClientService.cs
│   ├── Views/
│   │   ├── Account/
│   │   ├── Home/
│   │   │   └── Index.cshtml
│   │   └── Shared/
│   │       ├── _Layout.cshtml
│   │       ├── _LoginPartial.cshtml
│   │       ├── _ValidationScriptsPartial.cshtml
│   │       ├── Error.cshtml
│   │   ├── _ViewImports.cshtml
│   │   └── _ViewStart.cshtml
│   ├── appsettings.json
│   └── Program.cs
</code></pre>
