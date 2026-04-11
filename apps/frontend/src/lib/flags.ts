import {
	AE,
	AR,
	AU,
	BR,
	CA,
	CN,
	DE,
	EG,
	FR,
	GB,
	IN,
	IS,
	JP,
	KE,
	KR,
	MX,
	NG,
	RU,
	TR,
	US,
} from "country-flag-icons/react/3x2";

export type FlagComponent = typeof FR;

export const FLAGS = {
	AE,
	AR,
	AU,
	BR,
	CA,
	CN,
	DE,
	EG,
	FR,
	GB,
	IN,
	IS,
	JP,
	KE,
	KR,
	MX,
	NG,
	RU,
	TR,
	US,
} satisfies Record<string, FlagComponent>;

export type CountryCode = keyof typeof FLAGS;
