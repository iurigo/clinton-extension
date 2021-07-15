# Clinton Extension Server

## How to start

### Create `.env` file, based on an example below.
```
ASPNETCORE_ENVIRONMENT=development
ASPNETCORE_URLS=http://0.0.0.0:4000
APP_VERSION=1.0.0
APP_DB=server=.;database=clinton-extension;uid=<!--db-login-->;pwd=<!--db-password-->
APP_JWT_KEY=<!--jwt_key-->
```

- `APP_JWT_KEY`: should be 32 characters long

<i>Note: </i>You can use `https://www.guidgenerator.com/` website to generate UUID without hyphens "-".