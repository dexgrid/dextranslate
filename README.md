## DexTranslate

DexTranslate is a microservice that simplifies dealing with localization in asp.net core projects.
Setting up localization is always a hassle, but outsourcing the boilerplate logic to this handy microservice will hopefuly save you some time!

To get started add the dextranslate api client to your asp.net core project, add the service url to the configuration and host the dextranslate service in docker or a webserver.

To see how to configure your asp.net core application have a look at the [example application](https://github.com/dexgrid/dextranslate-example).

### Features
- import and export translations in csv format
- support for multiple databases (sqlite, pgsql, mssql)
- Caching on both the server side and the client side


