start-app:
	dotnet run --project clean-arch.Api

migrate-start:
	dotnet ef migrations add AddedComputedColumn --project clean-arch.Infrastructure/ --startup-project clean-arch.Api/ 

migrate-update:
	dotnet ef database update --project clean-arch.Infrastructure/ --startup-project clean-arch.Api/

migrate-drop:
	dotnet ef database drop --project clean-arch.Infrastructure/ --startup-project clean-arch.Api/

.PHONY: migrate-start migrate-update start-app