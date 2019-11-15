# Bet.Salesforce.TestApp

This project contains sample of the SalesForce models generated and Custom Salesforce `EventBus`.

## Configure Saleforce Developer instance

[Watch: Salesforce Platform Events - Video](https://www.youtube.com/watch?v=L6OWyCfQD6U)

1. Sing up for development sandbox with Saleforce: [https://developer.salesforce.com/signup](https://developer.salesforce.com/signup).
2. Create Connected App in Salesforce.
3. Create a Platform Event.

## NetCoreForce.ModelGenerator

SalesForce objects are generated with the command line utility `NetCoreForce.ModelGenerator` that is installed by the project.

It requires to have SalesForce admin account in order for the login to be successful and the model to be generated.

In addition specify all of the SF object that are desired to be generated in `modelgenerator_config.json` file.

[NetCoreForce.ModelGenerator](https://github.com/anthonyreilly/NetCoreForce/tree/master/src/NetCoreForce.ModelGenerator)

### Generate `Salesforce` models

If the following is not set the tool authentication won't work correctly.

> Connected App --> Manage -- Edit Policies --> Select Relax IP restrictions for active devices.

Run the following in the root of the project and provide with login credentials

```bash
    dotnet modelgenerator generate

    # or
    dotnet modelgenerator generate --client-id  --client-secret  
```
