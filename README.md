# Quickstart: .NET ASP Razor Pages app with FusionAuth

This repository contains a .NET ASP Razor Pages app that works with a locally running instance of [FusionAuth](https://fusionauth.io/), the authentication and authorization platform.

## Setup

### Prerequisites

- [.NET 7 ](https://dotnet.microsoft.com/en-us/download/dotnet/7.0): This is the main .NET framework.
- [Docker](https://www.docker.com): The quickest way to stand up FusionAuth. (There are [other ways](https://fusionauth.io/docs/v1/tech/installation-guide/)).

This app has been tested with .NET 7. This example should work with other compatible versions of .NET.

Optionally, you can also install [Visual Studio](https://visualstudio.microsoft.com/) to make it easier to work with .NET through an IDE.


### FusionAuth Installation via Docker

The root of this project directory (next to this README) are two files [a Docker compose file](./docker-compose.yml) and an [environment variables configuration file](./.env). Assuming you have Docker installed on your machine, you can stand up FusionAuth up on your machine with:

```
docker compose up -d
```

The FusionAuth configuration files also make use of a unique feature of FusionAuth, called [Kickstart](https://fusionauth.io/docs/v1/tech/installation-guide/kickstart): when FusionAuth comes up for the first time, it will look at the [Kickstart file](./kickstart/kickstart.json) and mimic API calls to configure FusionAuth for use when it is first run. 

> **NOTE**: If you ever want to reset the FusionAuth system, delete the volumes created by docker-compose by executing `docker compose down -v`. 

FusionAuth will be initially configured with these settings:

* Your client Id is: `e9fdb985-9173-4e01-9d73-ac2d60d1dc8e`
* Your client secret is: `super-secret-secret-that-should-be-regenerated-for-production`
* Your example username is `richard@example.com` and your password is `password`.
* Your admin username is `admin@example.com` and your password is `password`.
* Your fusionAuthBaseUrl is 'http://localhost:9011/'

You can log into the [FusionAuth admin UI](http://localhost:9011/admin) and look around if you want, but with Docker/Kickstart you don't need to.

### .NET ASP Razor Pages complete-app

The `complete-app` directory contains a minimal .NET ASP Razor Pages app configured to authenticate with locally running FusionAuth.

First build the application. There are multiple ways of [deploying a .NET application](https://docs.microsoft.com/en-us/dotnet/core/deploying/), but publishing ensures your deployment process is repeatable. Here’s the command to publish a standalone executable for macOS:

```shell
cd complete-app/MyApp
dotnet publish -r osx-x64
```

To build for a different platform, modify the `-r` parameter to match the RID for your platform. You can find a [list of RIDs here](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog).

Start up the .NET application using this command:

```shell
ASPNETCORE_ENVIRONMENT=Development FusionAuth__ClientSecret=super-secret-secret-that-should-be-regenerated-for-production bin/Debug/net7.0/osx-x64/publish/MyApp
```

`ASPNETCORE_ENVIRONMENT` is set to `Development` to ensure the application uses the `appsettings.Development.json` file.

`FusionAuth__ClientSecret` is the client secret, which was defined by the [FusionAuth Kickstart Script](#run-fusionauth-via-docker) step. You don't want to commit secrets like this to version control, so use an environment variable.

You can also run the project from Visual Studio if you prefer.

You can now open up an incognito window and visit the .NET app at https://localhost:7028/ . Log in with the user credentials created with the Kickstart.json file, which should be `richard@example.com` and the password `password`.

You'll see the email of the user next to the log out button. You'll also see the user's `given_name` claim above their balance.

### Further Information

Visit https://fusionauth.io/quickstarts/quickstart-dotnet-razor-pages for a step by step guide on how to build this .NET ASP Razor Pages app integrated with FusionAuth by scratch.

### Troubleshooting

* I get `This site can’t be reached  localhost refused to connect.` when I click the Login button

Ensure FusionAuth is running in the Docker container.  You should be able to login as the admin user, `admin@example.com` with a password of `password` at [http://localhost:9011/admin](http://localhost:9011/admin).

* I get an error page when I click on the Login button with message of `"error_reason" : "invalid_client_id"`

Ensure the value for `ClientId` in the `FusionAuth` section of the `appsettings.json` matches the client Id configured in FusionAuth for the Example App Application at [http://localhost:9011/admin/application/](http://localhost:9011/admin/application/).

* I'm getting an error from .NET after logging in

```
OpenIdConnectProtocolException: Message contains error: 'invalid_client', error_description: 'Client authentication missing as Basic Authorization header or credentials in the body (or some combination of them).', error_uri: 'error_uri is null'.
```

This indicates that .NET OpenIDConnect is unable to call FusionAuth to validate the returned token.  It is likely caused because of an incorrect client secret, or the client secret has not been passed to the app.  Ensure the `FusionAuth__ClientSecret` environment variable used to start the .NET app matches the FusionAuth ExampleApp client secret. You can review that by logging in as the admin user and examining the Application at [http://localhost:9011/admin/application/](http://localhost:9011/admin/application/)
