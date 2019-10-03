# RDPMonWebGUI
 
I created this project to have a web interface for the [great tool RdpMon by Cameyo](https://github.com/cameyo/rdpmon).
It offers the same functionality as the WinForms application provided by Cameyo. 

The site's build using .NET Core 3 and should be able to be run anywhere, but ofcourse since RdpMon is basically
a Windows focussed tool, it makes most sence to run it on Windows.

## Prerequisites

* .NET Core 3 SDK
* If using IIS: .NET Core: Runtime &amp; Hosting Bundle

## Installation

Currently no builds are provided so you will have to build the website yourself.

If the .NET Core 3 SDK is installed, this is possible to do with the following command in a Command Prompt or 
Powershell prompt (when you are inside the root of the project);

```
dotnet publish ./RDPMonWebGUI.sln
```

You are then able to host the compiled site on IIS or using the .NET Core builtin webserver Kestrel.

## Configuration

All the settings are defined in the file *appsettings.json*, which can be found in the root of this project.
Below you find a brief description per setting that is of significance.

* **ConnectionStrings**
    This is an essential setting and the value should point to the location of the RdpMon.db.

* **PageSize**
    This setting controls how many items per page are shown.