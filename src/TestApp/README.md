# TestApp for Bet.BuildingBlocks.SalesforceEventBus

This is a test application running as Generic Host app DotNetCore 2.2

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
