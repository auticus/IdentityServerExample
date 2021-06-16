# IdentityServerExample
This project implements an Identity server, api, and consuming mvc project to demonstrate how Identity server can work to protect our resources in an overall architecture

Movies.API - a simple rest API used to query and modify a database.  The database used here works with EntityFramework and this project is using a SQL Express database.  
This can be configured however you wish, just modify the connection string in the appsettings.json

IdentityServer - the identity server itself.  This has a UI Diagnostics project added to it.  To install it, go into powershell in the directory that the IdentityServer project resides and command:
iex ((New-Object System.Net.WebClient).DownloadString('https://raw.githubusercontent.com/IdentityServer/IdentityServer4.Quickstart.UI/main/getmain.ps1'))

Movies.Client - a sample web app that needs authentication to access the api.
