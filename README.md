# IT Partners README.MD file

## Summary: 

Back-end and APIs to pull information. This is replacing the old people function app and the old contact support app.

## Production location:

There will be two locations, one for each project:
* https://directoryapi.itpartners.illinois.edu. This has a database connection and is only used to populate the faculty API. 
* https://directory.itpartners.illinois.edu. This also has a database connection and is a Blazor application. Because of the data warehouse, we need to restrict access to campus VPN.

## Development location: 

Currently, none. We do development on local machines.

## How to deploy to production/development: 

Using CI -- Github Action to a Function App. 

## How to set up locally: 

Download the code. Use NuGet to get the latest copy of the executables. 

The "local.settings.json" needs to be filled in with a proper Data Warehouse key and Experts API information, or add it to your local secrets file. 

The database uses EF to generate, and it will generate Rob and Bryan as admins automatically.

Note that to access the data warehouse, you need to be on VPN, even if you are testing locally. 

## Notes (error logging, external tools, links, etc.): 

Information about EF Core Tools:

``Add-Migration -Name {migration name} -Project uofi-itp-directory-data``

``Update-Database``

If you run into the issue "The certificate chain was issued by an authority that is not trusted.", then add **TrustServerCertificate=True** to the connection string. 