import React from "react";
import { Provider as PaperProvider } from "react-native-paper";
import { AuthProvider } from "./src/Context/AuthContext";
import { ThemeProvider } from "./src/Context/ThemeContext";
import Navigation from "./src/Navigation/Navigation";
import * as Font from "expo-font";
import { useEffect, useState } from "react";
import { ActivityIndicator, View, Text } from "react-native";

export default function App() {
	const [fontesCarregadas, setFontesCarregadas] = useState(false);

	useEffect(() => {
		async function loadFonts() {
			await Font.loadAsync({
				"RobotoSlab-Thin": require("./assets/fonts/Roboto_Slab/RobotoSlab-Thin.ttf"),
				"RobotoSlab-Light": require("./assets/fonts/Roboto_Slab/RobotoSlab-Light.ttf"),
				"RobotoSlab-Regular": require("./assets/fonts/Roboto_Slab/RobotoSlab-Regular.ttf"),
				"RobotoSlab-Medium": require("./assets/fonts/Roboto_Slab/RobotoSlab-Medium.ttf"),
				"RobotoSlab-Bold": require("./assets/fonts/Roboto_Slab/RobotoSlab-Bold.ttf"),
				"RobotoSlab-Black": require("./assets/fonts/Roboto_Slab/RobotoSlab-Black.ttf"),

				"DMSans-Regular": require("./assets/fonts/DMSans/DMSans-Regular.ttf"),
				"DMSans-Medium": require("./assets/fonts/DMSans/DMSans-Medium.ttf"),
				"DMSans-Bold": require("./assets/fonts/DMSans/DMSans-Bold.ttf"),
			});
			setFontesCarregadas(true);
		}
		loadFonts();
	}, []);

	if (!fontesCarregadas) {
		return (
			<View>
				<ActivityIndicator size="large" color="#1A5D48" />
				<Text>CARREGANDO FONTES</Text>
				<Text>AGUARDE UM INSTANTE</Text>
			</View>
		);
	}

	return (
		<ThemeProvider>
			<AuthProvider>
				<PaperProvider>
					<Navigation />
				</PaperProvider>
			</AuthProvider>
		</ThemeProvider>
	);
}
