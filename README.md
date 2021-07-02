# IdentityServerExample
This project implements an Identity server, api, and consuming mvc project to demonstrate how Identity server can work to protect our resources in an overall architecture

Movies.API - a simple rest API used to query and modify a database.  The database used here works with EntityFramework and this project is using a SQL Express database.  
This can be configured however you wish, just modify the connection string in the appsettings.json

IdentityServer - the identity server itself.  This has a UI Diagnostics project added to it.  To install it, go into powershell in the directory that the IdentityServer project resides and command:
iex ((New-Object System.Net.WebClient).DownloadString('https://raw.githubusercontent.com/IdentityServer/IdentityServer4.Quickstart.UI/main/getmain.ps1'))

Movies.Client - a sample web app that needs authentication to access the api.
---> Nuget package Microsoft.AspNetCore.Authentication.OpenIdConnect -> middleware that enables an application to support the OpenID Connect authentication workflow
---> Nuget package IdentityModel -> OpenID Connect and OAuth2.0 client library

Hybrid Flow 
===============
https://code-maze.com/hybrid-flow-securing-aspnetcore-web-application/
We are using the Hybrid Flow when we want to acquire our tokens over the front and back channels. When we receive a token via /authorization endpoint over URI or Form POST, we are talking about the front-channel. When we receive a token via /token endpoint, we are talking about the back-channel. The Hybrid flow uses both channels in the process
This is the recommended flow for native applications that want to retrieve access tokens (and possibly refresh tokens as well) and is used for server-side web applications and native desktop/mobile applications.

Client Credential 
===================
This is the simplest grant type and is used for server to server communication - tokens are always requested on behalf of a client, not a user.

With this grant type you send a token request to the token endpoint, and get an access token back that represents the client. The client typically has to authenticate with the token endpoint using its client ID and secret.

Implicit 
=========
The implicit grant type is optimized for browser-based applications. Either for user authentication-only (both server-side and JavaScript applications), or authentication and access token requests (JavaScript applications).

In the implicit flow, all tokens are transmitted via the browser, and advanced features like refresh tokens are thus not allowed.

Authorization
=============
Authorization code flow was originally specified by OAuth 2, and provides a way to retrieve tokens on a back-channel as opposed to the browser front-channel. It also support client authentication.

While this grant type is supported on its own, it is generally recommended you combine that with identity tokens which turns it into the so called hybrid flow. Hybrid flow gives you important extra features like signed protocol responses.

Ocelot Gateway
==============
The OcelotApiGateway project uses the Ocelot package to unify point of entry into the system.
Also uses the Microsoft.AspNetCore.Authentication.JwtBearer package
