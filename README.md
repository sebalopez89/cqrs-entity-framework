# cqrs-entity-framework

run install for cli instalation
dotnet tool install --global dotnet-ef

run to create a new migration
dotnet ef migrations add InitialCreate --project Database --startup-project API

run to apply latest migrations
dotnet ef database update --project Database --startup-project API

install Kafka CLI from here
https://docs.confluent.io/confluent-cli/current/install.html

open cmd and create Kafka broker
confluent local kafka start

update brokerurl in appsettings with "plaintext ports" value

create Kafka topic
confluent local kafka topic create permisssions

build api
docker build -t api:latest .

run compose
docker compose up -d