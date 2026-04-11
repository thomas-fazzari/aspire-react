import useSWRImmutable from "swr/immutable";
import { getCurrentWeather } from "@/client/sdk.gen";

interface TypedCurrentConditions {
	temperature: number;
	windSpeed: number;
	weatherCode: number;
}

export interface TypedWeatherResponse {
	lat: number;
	lon: number;
	current: TypedCurrentConditions;
}

async function fetchWeather(params: {
	lat: number;
	lon: number;
}): Promise<TypedWeatherResponse> {
	const { data, error } = await getCurrentWeather({
		query: { Lat: params.lat, Lon: params.lon },
	});
	if (error || !data) throw error ?? new Error("No data");
	return {
		lat: Number(data.lat),
		lon: Number(data.lon),
		current: {
			temperature: Number(data.current.temperature),
			windSpeed: Number(data.current.windSpeed),
			weatherCode: Number(data.current.weatherCode),
		},
	};
}

export function useWeather(lat: number, lon: number) {
	return useSWRImmutable({ lat, lon }, fetchWeather, {
		onErrorRetry: (_error, _key, _config, revalidate, { retryCount }) => {
			if (retryCount >= 3) return;
			setTimeout(() => revalidate({ retryCount }), 5000);
		},
	});
}
