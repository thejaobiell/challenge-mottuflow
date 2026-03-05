import { StyleSheet } from "react-native";
import { ThemeContextData } from "../../Context/ThemeContext";
import { Fonts } from "../../../types/Fonts";

export const alterarsenhaStyles = (colors: ThemeContextData["colors"]) =>
	StyleSheet.create({
		container: {
			flex: 1,
			padding: 20,
			backgroundColor: colors.background,
		},
		label: {
			fontSize: 16,
			marginBottom: 4,
			fontFamily: Fonts.RobotoSlab.Bold,
			color: colors.texto,
		},
		inputContainer: {
			flexDirection: "row",
			alignItems: "center",
			borderWidth: 1,
			borderColor: "#ccc",
			borderRadius: 8,
			marginBottom: 16,
			backgroundColor:
				colors.background === "#FFFFFF" ? "#f5f5f5" : "#3A3A3A",
		},
		input: {
			flex: 1,
			padding: 12,
			fontFamily: Fonts.RobotoSlab.Regular,
			color: colors.texto,
		},
		botaoEye: {
			padding: 10,
		},
		botao: {
			marginTop: 20,
			backgroundColor: colors.header,
			paddingVertical: 12,
			borderRadius: 8,
			alignItems: "center",
		},
		textoBotao: {
			color: "#FFF",
			fontSize: 16,
			fontFamily: Fonts.RobotoSlab.Bold,
		},
	});
