import useSWRImmutable from "swr/immutable";
import { getCities } from "@/client/sdk.gen";
import type { CityResponse } from "@/client/types.gen";
import type { CountryCode } from "@/lib/flags";

export type City = Omit<CityResponse, "countryCode" | "lat" | "lon"> & {
	countryCode: CountryCode;
	lat: number;
	lon: number;
};

async function fetchCities(): Promise<City[]> {
	const { data, error } = await getCities();
	if (error || !data) throw error ?? new Error("No data");
	return data.map((c) => ({
		...c,
		lat: Number(c.lat),
		lon: Number(c.lon),
	})) as City[];
}

export function useCities() {
	return useSWRImmutable("cities", fetchCities);
}
