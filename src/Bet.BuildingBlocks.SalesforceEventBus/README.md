# Bet.BuildingBlocks.SalesforceEventBus

[![Build status](https://ci.appveyor.com/api/projects/status/95p5gcuv67v7vq7q?svg=true)](https://ci.appveyor.com/project/kdcllc/kdcllc-buildingblocks-salesforceeventbus)
[![NuGet](https://img.shields.io/nuget/v/Bet.BuildingBlocks.SalesforceEventBus.svg)](https://www.nuget.org/packages?q=Bet.BuildingBlocks.SalesforceEventBus)

Add the following to the project

```csharp
    dotnet add package Bet.BuildingBlocks.SalesforceEventBus
```

## Usage

In the `Startup.cs` or generic host please add the following registration:

```csharp
    services.AddSalesforceEventBus(context.Configuration);
```

## Sample Projects

- [TestApp](../../src/TestApp/README.md) - DotNetCore 2.2 worker implementation of `Bet.Salesforce.TestApp`
- [TestAppWorker](../../src/TestAppWorker/README.md) - DotNetCore 3.0 worker implementation of `Bet.Salesforce.TestApp` with Docker support.
