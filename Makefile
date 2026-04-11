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
	ASPIRE_ALLOW_UNSECURED_TRANSPORT=true dotnet run --project apps/backend/src/Aspire/WeatherApp.Host

lint:
	biome check .
	dotnet tool run csharpier check .
	dotnet tool run roslynator analyze $(PROJECTS)

fix:
	biome check --write .
	dotnet tool run csharpier format .

generate:
	@ASPIRE_STARTED=false; \
	if ! curl -sf http://localhost:5092/openapi/v1.json > /dev/null 2>&1; then \
		echo "Backend not running, starting Aspire..."; \
		ASPIRE_ALLOW_UNSECURED_TRANSPORT=true dotnet run --project apps/backend/src/Aspire/WeatherApp.Host > /tmp/aspire-generate.log 2>&1 & \
		ASPIRE_PID=$$!; \
		ASPIRE_STARTED=true; \
		echo "Waiting for API to be ready..."; \
		until curl -sf http://localhost:5092/openapi/v1.json > /dev/null 2>&1; do sleep 2; done; \
		echo "API ready."; \
	fi; \
	cd apps/frontend && pnpm run generate; \
	if [ "$$ASPIRE_STARTED" = "true" ]; then \
		echo "Stopping Aspire..."; \
		kill $$ASPIRE_PID 2>/dev/null || true; \
	fi

migrate:
	dotnet ef migrations add $(name) \
		--project apps/backend/src/WeatherApp.Api \
		--startup-project apps/backend/src/WeatherApp.Migrator \
		--context AppDbContext \
		--output-dir Infrastructure/Persistence/Migrations

