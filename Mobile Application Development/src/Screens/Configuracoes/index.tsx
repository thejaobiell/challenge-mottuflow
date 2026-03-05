import React, { useState } from "react";
import {
	View,
	Text,
	Switch,
	TouchableOpacity,
	Modal,
	ScrollView,
	Linking,
} from "react-native";
import { NativeStackScreenProps } from "@react-navigation/native-stack";
import { Telas } from "../../../types/Telas";
import { configuracoesStyles } from "./styles";
import { useTheme } from "../../Context/ThemeContext";
import { useAuth } from "../../Context/AuthContext";

type Props = NativeStackScreenProps<Telas, "Configuracoes">;

export default function Configuracoes({ navigation }: Props) {
	const { dark, toggleTheme, colors } = useTheme();
	const { signOut } = useAuth();
	const [modalVisible, setModalVisible] = useState(false);

	const styles = configuracoesStyles(colors);

	const abrirLink = (url: string) => Linking.openURL(url);

	const handleLogout = async () => {
		await signOut();
	};

	return (
		<View
			style={[styles.container, { backgroundColor: colors.background }]}>
			<View style={styles.opcao}>
				<Text style={[styles.label, { color: colors.texto }]}>
					Tema escuro
				</Text>
				<Switch value={dark} onValueChange={toggleTheme} />
			</View>

			<View style={styles.opcao}>
				<Text style={[styles.label, { color: colors.texto }]}>
					Sobre o app
				</Text>
				<TouchableOpacity onPress={() => setModalVisible(true)}>
					<Text style={{ color: colors.header }}>Ver detalhes</Text>
				</TouchableOpacity>
			</View>

			<TouchableOpacity
				style={[styles.botao, { backgroundColor: colors.header }]}
				onPress={() => navigation.navigate("Perfil")}>
				<Text style={styles.textoBotao}>Ver Perfil</Text>
			</TouchableOpacity>

			<View style={styles.footer}>
				<TouchableOpacity
					style={[
						styles.botao,
						{ backgroundColor: "red", marginTop: 10 },
					]}
					onPress={handleLogout}>
					<Text style={styles.textoBotao}>Sair</Text>
				</TouchableOpacity>
			</View>

			<Modal
				animationType="slide"
				transparent={true}
				visible={modalVisible}
				onRequestClose={() => setModalVisible(false)}>
				<View
					style={[
						styles.modalContainer,
						{ backgroundColor: dark ? "#2E2E2E" : "#fff" },
					]}>
					<ScrollView style={{ padding: 20 }}>
						<Text
							style={[
								styles.modalTitulo,
								{ color: colors.texto },
							]}>
							Equipe do App
						</Text>

						<TouchableOpacity
							onPress={() =>
								abrirLink("https://github.com/thejaobiell")
							}>
							<Text
								style={[
									styles.modalTexto,
									{ color: colors.header },
								]}>
								* João Gabriel Boaventura Marques e Silva |
								RM554874 | 2TDSB-2025 – Desenvolvedor
							</Text>
						</TouchableOpacity>

						<TouchableOpacity
							onPress={() =>
								abrirLink("https://github.com/leomotalima")
							}>
							<Text
								style={[
									styles.modalTexto,
									{ color: colors.header },
								]}>
								* Léo Mota Lima | RM557851 | 2TDSB-2025 –
								Desenvolvedor
							</Text>
						</TouchableOpacity>

						<TouchableOpacity
							onPress={() =>
								abrirLink("https://github.com/LucasLDC")
							}>
							<Text
								style={[
									styles.modalTexto,
									{ color: colors.header },
								]}>
								* Lucas Leal das Chagas | RM551124 | 2TDSB-2025
								– Desenvolvedor
							</Text>
						</TouchableOpacity>

						<TouchableOpacity
							style={[
								styles.botao,
								{
									backgroundColor: colors.header,
									marginTop: 20,
								},
							]}
							onPress={() => setModalVisible(false)}>
							<Text style={styles.textoBotao}>Fechar</Text>
						</TouchableOpacity>
					</ScrollView>
				</View>
			</Modal>
		</View>
	);
}
