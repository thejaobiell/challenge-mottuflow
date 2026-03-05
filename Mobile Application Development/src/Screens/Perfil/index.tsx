import React from "react";
import {
	View,
	Text,
	TouchableOpacity,
	ActivityIndicator,
	ScrollView,
} from "react-native";
import { NativeStackScreenProps } from "@react-navigation/native-stack";
import { Telas } from "../../../types/Telas";
import { perfilStyles as styles } from "./styles";
import { useAuth } from "../../Context/AuthContext";
import { useTheme } from "../../Context/ThemeContext";

type Props = NativeStackScreenProps<Telas, "Perfil">;

export default function Perfil({ navigation }: Props) {
	const { user, loading } = useAuth();
	const { dark, colors } = useTheme();

	if (loading) {
		return (
			<View
				style={[
					styles.container,
					{ backgroundColor: colors.background },
				]}>
				<ActivityIndicator size="large" color={colors.header} />
			</View>
		);
	}

	const irParaAlterarSenha = () => {
		navigation.navigate("AlterarSenha");
	}

	if (!user) {
		return (
			<View
				style={[
					styles.container,
					{ backgroundColor: colors.background },
				]}>
				<Text style={[styles.titulo, { color: colors.texto }]}>
					Usuário não encontrado
				</Text>
				<TouchableOpacity
					style={[styles.botao, { backgroundColor: colors.header }]}
					onPress={() => navigation.replace("Login")}>
					<Text style={styles.textoBotao}>Voltar para Login</Text>
				</TouchableOpacity>
			</View>
		);
	}

	return (
		<ScrollView
			style={[styles.container, { backgroundColor: colors.background }]}
			contentContainerStyle={{ paddingVertical: 15 }}>
			<Text style={[styles.titulo, { color: colors.texto }]}>
				Perfil do Funcionário
			</Text>

			{[
				{ label: "Nome", valor: user.nome },
				{ label: "Email", valor: user.email },
				{ label: "CPF", valor: user.cpf },
				{ label: "Cargo", valor: user.cargo },
				{ label: "Telefone", valor: user.telefone },
			].map((item, campo) => (
				<View
					key={campo}
					style={[
						styles.itemContainer,
						{ borderBottomColor: dark ? "#888" : "#555" },
					]}>
					<Text style={[styles.label, { color: colors.texto }]}>
						{item.label}
					</Text>
					<Text style={[styles.valor, { color: colors.texto }]}>
						{item.valor}
					</Text>
				</View>
			))}
			<TouchableOpacity onPress={irParaAlterarSenha} style={[styles.botao, { backgroundColor: colors.header }]}>
				<Text style={styles.textoBotao}>Alterar Senha</Text>
			</TouchableOpacity>
		</ScrollView>
	);
}