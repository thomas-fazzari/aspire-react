import useSWRImmutable from "swr/immutable";
import { getCities } from "@/client/sdk.gen";
import type { CityResponse } from "@/client/types.gen";
import type { CountryCode } from "@/lib/flags";

export type City = Omit<CityResponse, "countryCode"> & {
	countryCode: CountryCode;
};

async function fetchCities(): Promise<City[]> {
	const { data, error } = await getCities();
	if (error || !data) throw error ?? new Error("No data");
	return data as City[];
}

export function useCities() {
	return useSWRImmutable("cities", fetchCities);
}
