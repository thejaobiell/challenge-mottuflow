import { StyleSheet } from "react-native";
import { Fonts } from "../../../types/Fonts";
import { ThemeContextData } from "../../Context/ThemeContext";

export const configuracoesStyles = (colors: ThemeContextData["colors"]) =>
	StyleSheet.create({
		container: {
			flex: 1,
			padding: 20,
			backgroundColor: colors.background,
		},
		titulo: {
			fontSize: 40,
			fontFamily: Fonts.RobotoSlab.Bold,
			marginBottom: 20,
			color: colors.texto,
		},
		opcao: {
			flexDirection: "row",
			alignItems: "center",
			justifyContent: "space-between",
			marginBottom: 20,
		},
		label: {
			fontSize: 18,
			fontFamily: Fonts.RobotoSlab.Regular,
			color: colors.texto,
		},
		footer: {
			marginTop: "auto",
			marginBottom: 20,
		},
		botao: {
			padding: 15,
			borderRadius: 8,
			alignItems: "center",
			backgroundColor: colors.header,
		},
		textoBotao: {
			color: "#fff",
			fontSize: 16,
			fontFamily: Fonts.RobotoSlab.Bold,
		},

		modalContainer: {
			flex: 1,
			marginTop: 80,
			marginHorizontal: 20,
			borderRadius: 12,
			padding: 15,
			elevation: 5,
			backgroundColor: colors.background,
		},
		modalTitulo: {
			fontSize: 20,
			fontFamily: Fonts.RobotoSlab.Bold,
			marginBottom: 15,
			color: colors.texto,
		},
		modalTexto: {
			fontSize: 16,
			fontFamily: Fonts.RobotoSlab.Regular,
			marginBottom: 10,
			color: colors.header,
		},
	});
