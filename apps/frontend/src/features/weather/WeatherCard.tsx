import { useMemo } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import { FLAGS } from "@/lib/flags";
import { useWeather } from "./api";
import type { City } from "./citiesApi";
import { FAHRENHEIT, type TemperatureUnit } from "./types";

interface WeatherCardProps {
	capital: City;
	unit: TemperatureUnit;
}

export function WeatherCard({ capital, unit }: WeatherCardProps) {
	const { data, error, isLoading } = useWeather(capital.lat, capital.lon);

	const FlagComponent = FLAGS[capital.countryCode];

	const formattedTemp = useMemo(() => {
		if (!data) return null;
		const c = data.current.temperature;
		const val = unit === FAHRENHEIT ? c * 1.8 + 32 : c;
		return Math.round(val * 10) / 10;
	}, [data, unit]);

	return (
		<Card className="overflow-hidden transition-all hover:bg-muted/50">
			<CardHeader className="pb-2">
				<CardTitle className="flex items-center gap-2 text-sm font-medium text-muted-foreground">
					{FlagComponent && <FlagComponent className="h-4 w-auto rounded-sm" />}
					{capital.name}
				</CardTitle>
			</CardHeader>
			<CardContent>
				{isLoading && <Skeleton className="mt-2 h-12 w-24" />}

				{error && (
					<p className="mt-2 text-sm text-destructive">Failed to load</p>
				)}

				{data && (
					<>
						<div className="mt-1 flex items-baseline">
							<span className="text-5xl font-bold tracking-tighter">
								{formattedTemp}
							</span>
							<span className="ml-1 text-2xl font-medium text-muted-foreground">
								°{unit}
							</span>
						</div>
						<div className="mt-6 flex flex-col gap-2 border-t pt-4">
							<div className="flex items-center gap-2 text-sm">
								<span className="w-12 text-muted-foreground">Wind</span>
								<span className="font-medium">
									{data.current.windSpeed} km/h
								</span>
							</div>
							<div className="flex items-center gap-2 text-sm">
								<span className="w-12 text-muted-foreground">Code</span>
								<span className="font-medium">{data.current.weatherCode}</span>
							</div>
						</div>
					</>
				)}
			</CardContent>
		</Card>
	);
}
