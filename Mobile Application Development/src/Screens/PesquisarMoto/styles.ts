import { StyleSheet } from "react-native";
import { ThemeContextData } from "../../Context/ThemeContext";
import { Fonts } from "../../../types/Fonts";

export const pesquisarmotosStyle = (colors: ThemeContextData["colors"]) =>
	StyleSheet.create({
		container: {
			flex: 1,
			backgroundColor: colors.background,
		},
		scrollContainer: {
			padding: 16,
		},
		label: {
			fontSize: 16,
			fontFamily: Fonts.RobotoSlab.Black,
			marginBottom: 8,
			color: colors.texto,
		},
		radioGroup: {
			flexDirection: "row",
			marginBottom: 16,
		},
		opcoesRadio: {
			flexDirection: "row",
			alignItems: "center",
			marginRight: 16,
		},
		textoRadio: {
			marginLeft: 4,
			fontSize: 14,
			color: colors.texto,
			textTransform: "capitalize",
			fontFamily: Fonts.RobotoSlab.Regular,
		},
		inputRow: {
			flexDirection: "row",
			alignItems: "center",
			marginBottom: 16,
		},
		input: {
			flex: 1,
			borderWidth: 1,
			borderColor: colors.texto + "40",
			borderRadius: 6,
			paddingHorizontal: 12,
			paddingVertical: 12,
			fontSize: 14,
			color: colors.texto,
			backgroundColor: colors.background,
			fontFamily: Fonts.DMSans.Regular,
		},
		iconeBotao: {
			marginLeft: 8,
			padding: 8,
		},
		botaoRow: {
			flexDirection: "row",
			gap: 10,
			marginBottom: 16,
		},
		botao: {
			flex: 1,
			backgroundColor: colors.header,
			padding: 12,
			borderRadius: 6,
			flexDirection: "row",
			alignItems: "center",
			justifyContent: "center",
		},
		botaoSecundario: {
			backgroundColor: colors.iconeInativo,
		},
		textoBotao: {
			color: "#fff",
			fontWeight: "bold",
			marginLeft: 8,
			fontFamily: Fonts.DMSans.Bold,
			fontSize: 14,
		},
		containerResultado: {
			marginTop: 16,
		},
		contadorResultado: {
			fontSize: 16,
			fontFamily: Fonts.DMSans.Bold,
			color: colors.texto,
			marginBottom: 8,
		},
		containerVazio: {
			alignItems: "center",
			justifyContent: "center",
			paddingVertical: 40,
			paddingHorizontal: 20,
		},
		iconeVazio: {
			marginBottom: 16,
		},
		tituloVazio: {
			fontSize: 18,
			fontFamily: Fonts.RobotoSlab.Black,
			color: colors.texto,
			marginBottom: 8,
			textAlign: "center",
		},
		mensagemVazio: {
			fontSize: 14,
			fontFamily: Fonts.RobotoSlab.Black,
			color: colors.iconeInativo,
			textAlign: "center",
			marginBottom: 20,
			lineHeight: 20,
		},
		botaoVazio: {
			backgroundColor: colors.header,
			paddingHorizontal: 20,
			paddingVertical: 10,
			borderRadius: 6,
		},
		textoBotaoVazio: {
			color: "#fff",
			fontFamily: Fonts.RobotoSlab.Bold,
			fontSize: 14,
		},
	});