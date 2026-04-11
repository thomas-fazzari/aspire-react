PROJECTS = \
	apps/backend/src/WeatherApp.Api/WeatherApp.Api.csproj \
	apps/backend/src/WeatherApp.Migrator/WeatherApp.Migrator.csproj \
	apps/backend/src/Aspire/WeatherApp.ServiceDefaults/WeatherApp.ServiceDefaults.csproj

install:
	pnpm install
	cd apps/frontend && pnpm install
	dotnet restore WeatherApp.slnx
	dotnet tool restore

setup:
	dotnet user-secrets set "Parameters:postgres-password" "DevPassword123!" \
		--project apps/backend/src/Aspire/WeatherApp.Host

dev:
	@ASPIRE_ALLOW_UNSECURED_TRANSPORT=true dotnet run --project apps/backend/src/Aspire/WeatherApp.Host & \
	ASPIRE_PID=$$!; \
	trap "kill $$ASPIRE_PID 2>/dev/null" INT TERM; \
	echo "Waiting for API to be ready..."; \
	until curl -sf http://localhost:5092/openapi/v1.json > /dev/null 2>&1; do sleep 2; done; \
	cd apps/frontend && pnpm run generate; \
	wait $$ASPIRE_PID

test:
	dotnet test WeatherApp.slnx

lint:
	biome check .
	cd apps/frontend && pnpm exec tsc -b
	dotnet tool run csharpier check .
	dotnet tool run roslynator analyze $(PROJECTS)

fix:
	biome check --write .
	dotnet tool run csharpier format .

migrate:
	dotnet ef migrations add $(name) \
		--project apps/backend/src/WeatherApp.Api \
		--startup-project apps/backend/src/WeatherApp.Migrator \
		--context AppDbContext \
		--output-dir Infrastructure/Persistence/Migrations
