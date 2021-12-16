# altinn2-test-apiclient
Altinn 2 REST API test client

## About
This is a .NET Core 5.0 test application meant to show how a client can access and use Altinn REST resources.
It is written to be educational first, so we do not recommend that you use this code for production environments.

## Installation

### Prerequisite
In order to use this application to send REST requests to Maskinporten and Altinn's test environment, you need to following:
- Client ID from Maskinporten
- Subscription Key (API Key) from Altinn
- Enterprise Certificate from either Buypass or Commfides
- Username and Password for a Enterprise Certificate User in Altinn
- Test users and organization in Altinn's test environment

### Maskinporten
https://docs.digdir.no/maskinporten_guide_apikonsument.html#4-opprett-en-integrasjon-i-maskinporten

### Subscription Key (API Key)
https://altinn.github.io/docs/api/#api-key

### Enteprise Certificate and User in Altinn
https://www.altinn.no/hjelp/innlogging/alternativ-innlogging-i-altinn/virksomhetssertifikat/
- It is recommended to have a test enterprise certificate for all non-production uses.
- The application assumes that the Certificate is installed in ```LocalMachine\My``` certificate store.
- Once installed you can easily find the certificate's thumbprint by running the following command in PowerShell or Windows Terminal:
```dir Cert:\LocalMachine\My```

### Test users
https://www.altinndigital.no/produkter/altinn-api/kom-i-gang-med-altinn-api/
- See: step 7
- The test organization should have the same organization number as the entreprise certiifcate.

## User secrets
The application relies on Microsoft.Extensions.Configuration.UserSecrets to store secrets.
For Visual Studio Code we recommend using .NET Core User Secrets extension to easily access the secrets.json file.
This is the outline for the secrets.sjon file expected by the application.
Fill in the fields according to the data gotten from the previous steps taken in Prerequisite.

```jsonc
{
    "Altinn": {
        "ApiKey": "subscription key",
        "EnterpriseUser": {
            "Username": "username",
            "Password": "password"
        }
    },
    "Certificate": {
         "Thumbprint": "thumbprint"
    },
    "Maskinporten": {
        "ClientId": "client id"
    }
}
```