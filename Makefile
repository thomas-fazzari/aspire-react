ASPIRE_HOST    = apps/backend/src/Aspire/WeatherApp.Host
API            = apps/backend/src/WeatherApp.Api
MIGRATOR       = apps/backend/src/WeatherApp.Migrator
FRONTEND       = apps/frontend
API_URL        = http://localhost:5092/openapi/v1.json

PROJECTS = \
	$(API)/WeatherApp.Api.csproj \
	$(MIGRATOR)/WeatherApp.Migrator.csproj \
	apps/backend/src/Aspire/WeatherApp.ServiceDefaults/WeatherApp.ServiceDefaults.csproj

.PHONY: install
install:
	pnpm install
	cd $(FRONTEND) && pnpm install
	dotnet restore WeatherApp.slnx
	dotnet tool restore

.PHONY: setup
setup:
	dotnet user-secrets set "Parameters:postgres-password" "DevPassword123!" --project $(ASPIRE_HOST)

.PHONY: dev
dev:
	@ASPIRE_ALLOW_UNSECURED_TRANSPORT=true dotnet run --project $(ASPIRE_HOST) & \
	ASPIRE_PID=$$!; \
	trap "kill $$ASPIRE_PID 2>/dev/null" INT TERM; \
	echo "Waiting for API to be ready..."; \
	until curl -sf $(API_URL) > /dev/null 2>&1; do sleep 2; done; \
	cd $(FRONTEND) && pnpm run generate; \
	wait $$ASPIRE_PID

.PHONY: test
test:
	dotnet test WeatherApp.slnx

.PHONY: lint
lint:
	biome check .
	cd $(FRONTEND) && pnpm exec tsc -b
	dotnet tool run csharpier check .
	dotnet tool run roslynator analyze $(PROJECTS)

.PHONY: fix
fix:
	biome check --write .
	dotnet tool run csharpier format .

.PHONY: migrate
migrate:
	dotnet ef migrations add $(name) \
		--project $(API) \
		--startup-project $(MIGRATOR) \
		--context AppDbContext \
		--output-dir Infrastructure/Persistence/Migrations
