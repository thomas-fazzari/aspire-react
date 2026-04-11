import { useCallback, useEffect, useRef, useState } from "react";

const KEY = "theme";

function getInitial(): boolean {
	const stored = localStorage.getItem(KEY);
	if (stored) return stored === "dark";
	return window.matchMedia("(prefers-color-scheme: dark)").matches;
}

export function useTheme() {
	const [isDark, setIsDark] = useState(getInitial);
	const mounted = useRef(false);

	useEffect(() => {
		document.documentElement.classList.toggle("dark", isDark);
		if (mounted.current) {
			localStorage.setItem(KEY, isDark ? "dark" : "light");
		} else {
			mounted.current = true;
		}
	}, [isDark]);

	const toggle = useCallback(() => setIsDark((v) => !v), []);

	return { isDark, toggle };
}
