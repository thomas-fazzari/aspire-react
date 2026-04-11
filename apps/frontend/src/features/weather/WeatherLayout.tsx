import { useState } from "react";
import { Card, CardContent, CardHeader } from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import { Switch } from "@/components/ui/switch";
import { Tabs, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { useTheme } from "@/lib/useTheme";
import { useCities } from "./citiesApi";
import { CELSIUS, FAHRENHEIT, type TemperatureUnit } from "./types";
import { WeatherCard } from "./WeatherCard";

const triggerCls =
	"rounded-md px-4 text-sm font-medium transition-all data-[state=active]:bg-background data-[state=active]:text-foreground data-[state=active]:shadow-sm dark:data-[state=active]:bg-background";

const LOADING_CARD_COUNT = 5;
const loadingCardKeys = Array.from(
	{ length: LOADING_CARD_COUNT },
	(_, index) => `city-skeleton-${index + 1}`,
);

export function WeatherLayout() {
	const { data: cities = [], isLoading: citiesLoading } = useCities();
	const { isDark, toggle } = useTheme();
	const [unit, setUnit] = useState<TemperatureUnit>(CELSIUS);

	return (
		<div className="mx-auto max-w-375 flex flex-col gap-10 p-8 md:px-12 md:py-16">
			<div className="flex items-center justify-between">
				<h1 className="text-4xl font-bold tracking-tight">Weather Dashboard</h1>
				<div className="flex items-center gap-6">
					<Tabs
						value={unit}
						onValueChange={(v) => {
							if (v === CELSIUS || v === FAHRENHEIT) setUnit(v);
						}}
						className="w-auto"
					>
						<TabsList className="h-10 rounded-lg bg-muted p-1">
							<TabsTrigger value={CELSIUS} className={triggerCls}>
								°C
							</TabsTrigger>
							<TabsTrigger value={FAHRENHEIT} className={triggerCls}>
								°F
							</TabsTrigger>
						</TabsList>
					</Tabs>

					<div className="flex items-center gap-3 text-sm font-medium text-muted-foreground">
						<span>Light</span>
						<Switch checked={isDark} onCheckedChange={toggle} />
						<span>Dark</span>
					</div>
				</div>
			</div>

			<div className="grid gap-5 grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-5">
				{citiesLoading
					? loadingCardKeys.map((loadingCardKey) => (
							<Card key={loadingCardKey} className="overflow-hidden">
								<CardHeader className="pb-2">
									<Skeleton className="h-4 w-24" />
								</CardHeader>
								<CardContent>
									<Skeleton className="mt-2 h-12 w-24" />
								</CardContent>
							</Card>
						))
					: cities.map((city) => (
							<WeatherCard key={city.name} capital={city} unit={unit} />
						))}
			</div>
		</div>
	);
}
