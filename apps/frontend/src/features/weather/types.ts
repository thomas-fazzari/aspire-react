export const CELSIUS = "C" as const;
export const FAHRENHEIT = "F" as const;

export type TemperatureUnit = typeof CELSIUS | typeof FAHRENHEIT;
