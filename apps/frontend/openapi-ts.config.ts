import { defineConfig } from "@hey-api/openapi-ts";

export default defineConfig({
	input: "http://localhost:5092/openapi/v1.json",
	output: {
		path: "src/client",
		postProcess: ["biome:format"],
	},
	plugins: [
		"@hey-api/typescript",
		{
			name: "@hey-api/client-fetch",
			runtimeConfigPath: "../hey-api.ts",
		},
		"@hey-api/sdk",
	],
});
