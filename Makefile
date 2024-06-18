start-app:
	dotnet run --project clean-arch.Api

migrate-start:
	dotnet ef migrations add InitialCreate --project clean-arch.Infrastructure/ --startup-project clean-arch.Api/ 

migrate-update:
	dotnet ef database update --project clean-arch.Infrastructure/ --startup-project clean-arch.Api/

.PHONY: migrate-start migrate-update start-app