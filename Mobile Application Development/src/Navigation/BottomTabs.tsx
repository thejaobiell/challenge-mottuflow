import React from "react";
import { createBottomTabNavigator } from "@react-navigation/bottom-tabs";
import { Feather } from "@expo/vector-icons";

import Dashboard from "../Screens/Dashboard";
import PesquisarMoto from "../Screens/PesquisarMoto";
import CadastroMoto from "../Screens/CadastroMoto";
import Configuracoes from "../Screens/Configuracoes";
import { Telas } from "../../types/Telas";
import { useTheme } from "../Context/ThemeContext";

const Rodape = createBottomTabNavigator<Telas>();

export default function BottomTabs() {
	const { colors } = useTheme();

	return (
		<Rodape.Navigator
			initialRouteName="Dashboard"
			screenOptions={({ route }) => ({
				headerShown: false,
				tabBarActiveTintColor: colors.iconeAtivo,
				tabBarInactiveTintColor: colors.iconeInativo,
				tabBarStyle: {
					height: 95,
					backgroundColor: colors.tabBar,
				},
				tabBarLabelStyle: {
					fontSize: 12,
				},
				tabBarIcon: ({ color, size }) => {
					let iconName: string = "home";
					switch (route.name) {
						case "Dashboard":
							iconName = "file-text";
							break;
						case "PesquisarMoto":
							iconName = "search";
							break;
						case "CadastroMoto":
							iconName = "plus-circle";
							break;
						case "Configuracoes":
							iconName = "settings";
							break;
					}
					return (
						<Feather
							name={iconName as any}
							size={size}
							color={color}
						/>
					);
				},
			})}>
			<Rodape.Screen
				name="Dashboard"
				component={Dashboard}
				options={{ title: "Dashboard" }}
			/>
			<Rodape.Screen
				name="CadastroMoto"
				component={CadastroMoto}
				options={{ title: "Cadastrar" }}
			/>
			<Rodape.Screen
				name="PesquisarMoto"
				component={PesquisarMoto}
				options={{ title: "Pesquisar Moto" }}
			/>
			<Rodape.Screen
				name="Configuracoes"
				component={Configuracoes}
				options={{ title: "Configurações" }}
			/>
		</Rodape.Navigator>
	);
}
