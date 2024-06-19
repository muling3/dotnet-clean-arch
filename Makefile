start-app:
	dotnet run --project clean-arch.Api

migrate-auth:
	dotnet ef migrations add AddedIdentity --project clean-arch.Infrastructure/ --startup-project clean-arch.Api/ --context AuthDbContext 

migrate-app:
	dotnet ef migrations add InitialSetup --project clean-arch.Infrastructure/ --startup-project clean-arch.Api/ --context ApplicationDbContext

update-auth:
	dotnet ef database update --project clean-arch.Infrastructure/ --startup-project clean-arch.Api/ --context AuthDbContext

update-app:
	dotnet ef database update --project clean-arch.Infrastructure/ --startup-project clean-arch.Api/ --context ApplicationDbContext

revert-auth:
	dotnet ef database update AddedIdentity --project clean-arch.Infrastructure/ --startup-project clean-arch.Api/ --context AuthDbContext

revert-app:
	dotnet ef database update --project clean-arch.Infrastructure/ --startup-project clean-arch.Api/ --context ApplicationDbContext

drop-auth:
	dotnet ef database drop --project clean-arch.Infrastructure/ --startup-project clean-arch.Api/ --context AuthDbContext

drop-app:
	dotnet ef database drop --project clean-arch.Infrastructure/ --startup-project clean-arch.Api/ --context ApplicationDbContext

remove-auth:
	dotnet ef migrations remove --project clean-arch.Infrastructure/ --startup-project clean-arch.Api/ --context AuthDbContext

remove-app:
	dotnet ef migrations remove --project clean-arch.Infrastructure/ --startup-project clean-arch.Api/ --context ApplicationDbContext

.PHONY: migrate-auth migrate-uapp update-auth update-app drop-auth drop-app remove-auth remove-app