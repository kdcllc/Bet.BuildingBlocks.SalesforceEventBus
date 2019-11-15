# Bet.BuildingBlocks.SalesforceEventBus

[![Build status](https://ci.appveyor.com/api/projects/status/95p5gcuv67v7vq7q?svg=true)](https://ci.appveyor.com/project/kdcllc/kdcllc-buildingblocks-salesforceeventbus)
[![NuGet](https://img.shields.io/nuget/v/Bet.BuildingBlocks.SalesforceEventBus.svg)](https://www.nuget.org/packages?q=Bet.BuildingBlocks.SalesforceEventBus)

This repo is the implementation of Event Bus for Salesforce platform events with a sample app.
It demonstrates how to interact with CometD for Salesforce Platform events.

- [Bet.BuildingBlocks.SalesforceEventBus](./src/Bet.BuildingBlocks.SalesforceEventBus/README.md) - the reusable library.
- [Bet.Salesforce.TestApp](./src/Bet.Salesforce.TestApp/README.md) - the project that contains `IEventBus` sample implementation.
- [TestApp](./src/TestApp/README.md) - DotNetCore 2.2 worker implementation of `Bet.Salesforce.TestApp`
- [TestAppWorker](./src/TestAppWorker/README.md) - DotNetCore 3.0 worker implementation of `Bet.Salesforce.TestApp` with Docker support.

## Referenced Projects

- [CometD2.NetCore](https://github.com/kdcllc/CometD.NetCore) - [CometD.org](CometD.org) implementation, supports replay id.
- [CometD.NetCore2.Salesforce project](https://github.com/kdcllc/CometD.NetCore.Salesforce) - provides with implementation of this library.


