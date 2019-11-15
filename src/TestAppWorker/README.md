# TestAppWorker for Bet.BuildingBlocks.SalesforceEventBus

This is a test application running as Generic Host app DotNetCore 3.0 Worker template.

In addition, this sample app can be run in Docker container. 
If used with Azure Key Vault then use [`appauthentication` dotnet tool](https://github.com/kdcllc/Bet.AspNetCore/tree/master/src/AppAuthentication) to authenticate inside of Docker Container.

## Usage

This sample application demonstrates usage of the library in light of Salesforce Platform event.

Required authentication values are:

```json
  "Salesforce": {
    "ClientId": "",
    "ClientSecret": "",
    "RefreshToken": "",
    "AccessToken": ""
  }
```

Use [salesforce dotnet cli tool](https://github.com/kdcllc/CometD.NetCore.Salesforce/tree/master/src/AuthApp) to generate `RefreshToken` and `AccessToken` values.
Install latest dotnet cli `salesforce` tool by `dotnet tool install --global salesforce`.

Please setup the required values in:

- Azure Key Vault  (recommended)
- Or User Secrets
- Or appsettings.json.
