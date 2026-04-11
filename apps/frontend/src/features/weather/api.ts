import useSWRImmutable from "swr/immutable";
import { getWeather } from "@/client/sdk.gen";
import type { WeatherResponse } from "@/client/types.gen";

export type { WeatherResponse };

async function fetchWeather(params: {
	lat: number;
	lon: number;
}): Promise<WeatherResponse> {
	const { data, error } = await getWeather({
		query: { Lat: params.lat, Lon: params.lon },
	});
	if (error || !data) throw error ?? new Error("No data");
	return {
		...data,
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
