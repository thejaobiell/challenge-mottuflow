import React, {
	createContext,
	useState,
	useContext,
	ReactNode,
	useEffect,
} from "react";
import AsyncStorage from "@react-native-async-storage/async-storage";

export type ThemeContextData = {
	dark: boolean;
	toggleTheme: () => void;
	colors: {
		background: string;
		texto: string;
		header: string;
		tabBar: string;
		iconeAtivo: string;
		iconeInativo: string;
	};
};

const lightColors = {
	background: "#FFFFFF",
	texto: "#000000",
	header: "#1A5D48",
	tabBar: "#e2e2e2ff",
	iconeAtivo: "#1A5D48",
	iconeInativo: "#888888",
};

const darkColors = {
	background: "#2E2E2E",
	texto: "#FFFFFF",
	header: "#2FA36F",
	tabBar: "#3A3A3A",
	iconeAtivo: "#2FA36F",
	iconeInativo: "#AAAAAA",
};

const ThemeContext = createContext<ThemeContextData>({} as ThemeContextData);

export const ThemeProvider = ({ children }: { children: ReactNode }) => {
	const [dark, setDark] = useState(false);

	useEffect(() => {
		(async () => {
			const valor = await AsyncStorage.getItem("@dark_theme");
			if (valor !== null) {
				setDark(valor === "1");
			}
		})();
	}, []);

	const toggleTheme = async () => {
		const newDark = !dark;
		setDark(newDark);
		await AsyncStorage.setItem("@dark_theme", newDark ? "1" : "0");
	};

	const colors = dark ? darkColors : lightColors;

	return (
		<ThemeContext.Provider value={{ dark, toggleTheme, colors }}>
			{children}
		</ThemeContext.Provider>
	);
};

export const useTheme = () => useContext(ThemeContext);