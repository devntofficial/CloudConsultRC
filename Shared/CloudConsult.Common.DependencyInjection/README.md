# Extension Method Required Configurations
```json5
{
  "JwtConfiguration": {
    "SecretKey": "some_secret_key_here"
  },
  "KafkaConfiguration": {
    "BootstrapServers": "localhost:9092",
    "Acks": "All", //Possible Values: All, Leader, None
    "MessageSendMaxRetries": 3,
    "RetryBackoffMs": 500,
    "MessageTimeoutMs": 3000,
    "EnableIdempotence": true
  },
  "EmailServiceConfiguration": {
    "HostName": "localhost",
    "Port": 587,
    "UseSSL": true,
    "Username": "",
    "Password": ""
  }
}
```