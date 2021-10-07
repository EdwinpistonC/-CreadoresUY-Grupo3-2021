
Para crear la migracion usar 

	dotnet ef --startup-project Api/Api.csproj migrations add prueba1 -p Persistence/Persistence.csproj

Para actualizar la base de dato con la migracion realizada 

	dotnet ef --startup-project Api/Api.csproj database update
