# CometD for Salesforce Platform events
[![Build status](https://ci.appveyor.com/api/projects/status/95p5gcuv67v7vq7q?svg=true)](https://ci.appveyor.com/project/kdcllc/kdcllc-buildingblocks-salesforceeventbus)

This repo implementation of Event Bus for Salesforce platform events with a sample app.

- [CometD2.NetCore](https://github.com/kdcllc/CometD.NetCore) - [CometD.org](CometD.org) implementation, supports replay id.
- [CometD.NetCore2.Salesforce project](https://github.com/kdcllc/CometD.NetCore.Salesforce) - provides with implementation of this library.

## Install 

```cmd
    PM> Install-Package Bet.BuildingBlocks.SalesforceEventBus
```

## Usage
In the S`tartup.cs` or generic host please add the following registration:

```csharp
    services.AddSalesforceEventBus()
```

## TestApp
This sample application demonstrates usage of the library in light of Salesfore Platform event.
Please setup the required values in Azure Key Vault or User Secrets or appsettings.json.
Use dotnet cli `salesforce` tool to generate access and refresh tokens. 
Install latest dotnet cli `salesforce` tool by `dotnet tool install --global salesforce`. Read more on how to use [here](https://github.com/kdcllc/CometD.NetCore.Salesforce#salesforce-dotnet-cli-usage)
In order for this app code to log into SalesForce, please specify the following setup:

```json
  "Salesforce": {
    "ClientId": "",
    "ClientSecret": "",
    "RefreshToken": "",
    "AccessToken": ""
  }
````

## Previous 1.0.2 Version of the package was under a different name

```cmd
    PM> Install-Package KDCLLC.BuildingBlocks.SalesforceEventBus 
```

## Usage
In the startup.cs or generic host please add the following registration:

```csharp
    services.AddSalesforceEventBus()
```

## Configure Saleforce Developer instance
[Salesforce Platform Events - Video](https://www.youtube.com/watch?v=L6OWyCfQD6U)
1. Sing up for development sandbox with Saleforce: [https://developer.salesforce.com/signup](https://developer.salesforce.com/signup).
2. Create Connected App in Salesforce.
3. Create a Platform Event.

