# CometD for Salesforce Platform events
[![Build status](https://ci.appveyor.com/api/projects/status/95p5gcuv67v7vq7q?svg=true)](https://ci.appveyor.com/project/kdcllc/kdcllc-buildingblocks-salesforceeventbus)

This repo contains the CometD .NET Core implementation of the Java ported code.
- [CometD2.NetCore](https://github.com/kdcllc/CometD.NetCore) - [CometD.org](CometD.org) implementation, supports replay id.
- [CometD.NetCore2.Salesforce project](https://github.com/kdcllc/CometD.NetCore.Salesforce) - provides with implementation of this library.


## Nuget Packages
``` 
PM> Install-Package KDCLLC.BuildingBlocks.SalesforceEventBus 
```
## Enable with your project
```
    services.AddSalesforceEventBus()
```

## Configure Saleforce Developer instance
[Salesforce Platform Events - Video](https://www.youtube.com/watch?v=L6OWyCfQD6U)
1. Sing up for development sandbox with Saleforce: [https://developer.salesforce.com/signup](https://developer.salesforce.com/signup).
2. Create Connected App in Salesforce.
3. Create a Platform Event.

