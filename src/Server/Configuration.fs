module Castos.Configuration

type AuthConfig = {
    Secret: string
    Issuer: string
}

type Configuration =
    { Auth: AuthConfig
      Url: string
      ClientBaseUrl: string
      Port: uint16
      CorsUrls: string array }
