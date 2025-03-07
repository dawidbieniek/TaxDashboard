<div align="center">
  <h3 align="center">TaxDashboard</h3>
  <p align="center">
    A medium application that streamlines client management and accounting tasks.
  </p>
</div>

<!-- ABOUT -->
## About
![Screenshot of application][app-screenshot]

TaxDashboard is an application developed to simplify client management for accounting purposes. Originally created for a friend, it brings together essential client data display, automated reminders, and backup routines in one convenient dashboard. The app uses [AppInstaller](https://learn.microsoft.com/en-us/windows/msix/app-installer/how-to-create-appinstaller-file) for auto-updates.

### Built With
[![.NET][dotnet-badge]][dotnet-url]

[![MAUI][maui-badge]][maui-url]

[![Blazor][blazor-badge]][blazor-url]

[![Bootstrap][bootstrap-badge]][bootstrap-url]

[![EF][ef-badge]][ef-url]

[![MailKit][mailkit-badge]][mailkit-url]

### Features
* **Auto-updates:** The application checks for updates automatically using AppInstaller
* **Client Dashboard:** Displays key client data with interactive charts and summaries
* **Client Management:** Easily add, edit, and view client details, keeping your records up-to-date.
* **Comprehensive Client List:** Offers a full list of clients for quick reference and efficient management.
* **Database Backups:** Automatically creates periodic backups with the option for manual backup when needed.
* **Reminders:** Notifies you when deadlines are near or when certain income thresholds are reached or missed.
* **Email Integration:** Enables sending emails directly to clients via Gmail. Additionally allows for email templates to save time on writing the same messages.
* **CI/CD Workflows:** Includes automated scripts for versioning and releasing updates.

<!-- GETTING STARTED -->
## Getting Started

### Prerequisites
* **.NET 9.0 Desktop Runtime:** Required to run the desktop application. The app will prompt you to install it if missing.
* **Self-Signed Certificate:** You must install self-signed certificate into the Trusted People storage. The certificate can be downloaded from **Releases**.
* **Internet Connection:** Required for OAuth functionality, to install using AppInstaller and to receive auto-updates.

.NET SDK: Usually includes the runtime. If you can build the app, you should be able to run it as well.

### Installation
> [!NOTE]
> When using **Releases** build, OAuth will not work since it's configured for test environments - only allowed email addresses are permitted to use it. In order to use your own Google OAuth Client you need to build the app yourself.

#### Using AppInstaller (with auto-updates)
1. **Download certificate and AppInstaller:** Download necessary files from **Releases**.
2. **Install Certificate:** Add the self-signed certificate to your Trusted People storage to avoid security error.
3. **Run AppInstaller:** Launch the AppInstaller file and follow the prompts. This installation method supports auto-updates.

#### Using the MSIX Package (without auto-updates)
If you prefer, you can install the app by opening the MSIX package directly instead of AppInstaller. Note that this method will not support automatic updates.

#### Building from Source
1. **Clone the Repository:**
```
git clone https://github.com/dawidbieniek/TaxDashboard.git
```

2. **Create your self-signed certificate:**

Packing app into MSIX requires giving it some certificate. The easiest way is to create self-signed certificate. Whole process is described [here](https://learn.microsoft.com/en-us/windows/msix/package/create-certificate-package-signing).

2. **Setup OAuth:**

For Google OAuth to work you need to create [Google OAuth Client](https://developers.google.com/identity/protocols/oauth2) and setup project _appsettings.json_ to connect to the client.

3. **Build the Application:**

Project is setup to produce MSIX package. You should use `dotnet publish` instead of `dotnet build`. 

Navigate to the project folder and run (replace **<YOUR_CERTIFICATE_THUMBPRINT>** with your certificate thumbprint): 
```
dotnet publish -c Release -f:net9.0-windows10.0.19041.0 /p:GenerateAppxPackageOnBuild=true /p:AppxPackageSigningEnabled=true /p:PackageCertificateThumbprint=<YOUR_CERTIFICATE_THUMBPRINT>
```

4. **Install the app:**

After successfull build, packed app should be created inside `/bin/Release/net9.0-windows10.0.19041.0/win10-x64/AppPackages/TaxDashboard_<PROJECT_VERSION>.0_Test/`. To install it just follow steps of [installing the app from releases](https://github.com/dawidbieniek/TaxDashboard/new/readme?filename=README.md#installation)


## Screenshots
### Views
![Data page][ss-dataView]
![List page][ss-listView]
![Settings page][ss-settingsView]
![Other pages][ss-otherViews]

### Emails
![Emails][ss-email]


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[app-screenshot]: img/appScreenshot.png
[dotnet-badge]: https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white
[dotnet-url]: https://dotnet.microsoft.com/en-us/
[bootstrap-badge]: https://img.shields.io/badge/Bootstrap-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white
[bootstrap-url]: https://getbootstrap.com
[blazor-badge]: https://img.shields.io/badge/Blazor-512BD4?style=for-the-badge&logo=blazor&logoColor=white
[blazor-url]: https://learn.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-9.0
[ef-badge]: https://img.shields.io/badge/EntityFramework-512BD4?style=for-the-badge
[ef-url]: https://www.nuget.org/packages/microsoft.entityframeworkcore
[maui-badge]: https://img.shields.io/badge/MAUI-512BD4?style=for-the-badge
[maui-url]: https://github.com/dotnet/maui
[mailkit-badge]: https://img.shields.io/badge/MailKit-3498db?style=for-the-badge
[mailkit-url]: https://github.com/jstedfast/MailKit

[ss-dataView]: img/data.png
[ss-listView]: list/data.png
[ss-settingsView]: img/settings.png
[ss-otherViews]: img/otherViews.png
[ss-email]: img/email.png
