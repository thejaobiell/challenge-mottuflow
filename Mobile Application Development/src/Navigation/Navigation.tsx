import React from "react";
import { StyleSheet } from "react-native";
import {NavigationContainer, getFocusedRouteNameFromRoute} from "@react-navigation/native";
import { createNativeStackNavigator } from "@react-navigation/native-stack";

import Login from "../Screens/Login";
import EditarMoto from "../Screens/EditarMoto";
import BottomTabs from "./BottomTabs";
import Perfil from "../Screens/Perfil";
import AlterarSenha from "../Screens/AlterarSenha";

import { Telas } from "../../types/Telas";
import { useAuth } from "../Context/AuthContext";
import { Fonts } from "../../types/Fonts";
import DeletarMoto from "../Screens/DeletarMoto";

const Lista = createNativeStackNavigator<Telas>();

export default function Navigation() {
	const { user, loading } = useAuth();

	if (loading) {
		return null;
	}

	return (
		<NavigationContainer>
			<Lista.Navigator initialRouteName={!user ? "Login" : "BottomTabs"}>
				{!user ? (
					<Lista.Screen
						name="Login"
						component={Login}
						options={{ headerShown: false }}
					/>
				) : (
					<>
						<Lista.Screen
							name="BottomTabs"
							component={BottomTabs}
							options={({ route }) => {
								const nomeRota =
									getFocusedRouteNameFromRoute(route) ??
									"Dashboard";

								let tituloHeader = "";
								switch (nomeRota) {
									case "Dashboard":
										tituloHeader = "Dashboard";
										break;
									case "PesquisarMoto":
										tituloHeader = "Pesquisar Moto";
										break;
									case "CadastroMoto":
										tituloHeader = "Cadastro de Moto";
										break;
									case "Configuracoes":
										tituloHeader = "Configurações";
										break;
									default:
										tituloHeader = "";
								}

								return {
									headerTitle: tituloHeader,
									headerStyle: styles.header,
									headerTitleStyle: styles.titulo,
								};
							}}
						/>
						<Lista.Screen
							name="EditarMoto"
							component={EditarMoto}
							options={{
								headerStyle: styles.header,
								headerTintColor: "white",
								headerTitleStyle: styles.titulo,
								title: "Editar Informações da Moto",
							}}
						/>
						<Lista.Screen
							name="DeletarMoto"
							component={DeletarMoto}
							options={{
								headerStyle: styles.header,
								headerTintColor: "white",
								headerTitleStyle: styles.titulo,
								title: "Deletar Moto",
							}}
						/>
						<Lista.Screen
							name="Perfil"
							component={Perfil}
							options={{
								headerStyle: styles.header,
								headerTintColor: "white",
								headerTitleStyle: styles.titulo,
								title: "Seu Perfil",
							}}
						/>
						<Lista.Screen
							name="AlterarSenha"
							component={AlterarSenha}
							options={{
								headerStyle: styles.header,
								headerTintColor: "white",
								headerTitleStyle: styles.titulo,
								title: "Alterar Senha",
							}}
						/>
					</>
				)}
			</Lista.Navigator>
		</NavigationContainer>
	);
}

const styles = StyleSheet.create({
	header: {
		backgroundColor: "#1A5D48",
	},
	titulo: {
		fontFamily: Fonts.RobotoSlab.Black,
		fontSize: 16,
		color: "white",
	},
});