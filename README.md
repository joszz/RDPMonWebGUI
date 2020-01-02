# RDPMonWebGUI
&copy; 2020, Jos Nienhuis
 
I created this project to have a web interface for the [great tool RdpMon by Cameyo](https://github.com/cameyo/rdpmon).
It offers the same functionality as the WinForms application provided by Cameyo. 

The site's build using .NET Core 3 and should be able to be run anywhere, but ofcourse since RdpMon is basically
a Windows focussed tool, it makes most sence to run it on Windows.

## Prerequisites

* **[.NET Core 3 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.0)**
* If using IIS: **.NET Core: Runtime &amp; Hosting Bundle** (see link above)
* **[Libman](https://github.com/aspnet/LibraryManager/wiki/Using-LibMan-CLI#get-the-libman-global-tool)**

    Run the following in a command Prompt or Powershell prompt
    ```
    dotnet tool install -g Microsoft.Web.LibraryManager.CLI
    ```

## Installation

Currently no builds are provided so you will have to build the website yourself.

First you will need to restore the client side library dependencies. This can be done by running libman in the root of 
this project's directory;

```
libman restore
```

Next we can build/publish the project. This can be done with the following command in a Command or 
Powershell prompt (when you are inside the root of the project);

```
dotnet publish ./RDPMonWebGUI.sln --configuration Release
```

You are then able to host the compiled site on IIS or using the .NET Core built-in webserver Kestrel.

## Configuration

All the settings are defined in the file *appsettings.json*, which can be found in the root of this project.
Below you find a brief description per setting that is of significance.

* **ConnectionStrings**

    This is an essential setting and the value should point to the location of the RdpMon.db.

* **PageSize**

    This setting controls how many items per page are shown.

* **Password**

    This is an optional setting where you can provide a password. If this is not included, no credentials need to
    be provided to access the webapplication.
    
    After changing this value, the first time you hit the application, it will hash this value automatically.
    It will create an additional setting "PasswordSalt" as well. On authentication it will hash the provided password 
    and compare it with the stored hash in appsettings.

    If you want to change the password, simply do so by replacing the hashed password with a plaintext one.
    Also remove the "PasswordSalt" entirely from the appsettings.

    Upon next visit, the application will rehash this value again and create a new "PasswordSalt".