```shell
dotnet ef migrations add Init_ProductDb01
dotnet ef migrations add Init_CustomerDb01
dotnet ef database update
```

```shell
#Migration commands for Ordering API:
cd src/services/ordering
dotnet ef migrations add OrderingDbMigrate01 -p Ordering.Infrastructure --startup-project Ordering.API --output-dir Persistence/Migrations
dotnet ef migrations remove -p Ordering.Infrastructure --startup-project Ordering.API
dotnet ef database update -p Ordering.Infrastructure --startup-project Ordering.API
```