## Local Development Set Up
1. Set up user secrets

```bash
dotnet user-secrets init
```

2. Add the following to `secrets.json`

```json
{
  "Minio": {
    "Endpoint": "localhost:9000",
    "AccessKey": "*replace with your key*",
    "SecretKey": "*replace with your key*"
  },
  "Postgres": {
    "User": "*replace with your user*",
    "Password": "*replace with your password*",
    "Database": "travel"
  }
}
```

When the `Travel.Api` project is built, the `secrets.json` file will be copied and parsed to `.env` used by `compose.yml`.

