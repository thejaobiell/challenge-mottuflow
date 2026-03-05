import { StyleSheet } from "react-native";
import { Fonts } from "../../types/Fonts";
import { useTheme } from "../Context/ThemeContext";

export const motoCardStyles = () => {
	const { colors } = useTheme();

	return StyleSheet.create({
		card: {
			flexDirection: "row",
			justifyContent: "space-between",
			alignItems: "center",
			backgroundColor: colors.background,
			padding: 12,
			borderRadius: 8,
			borderColor: "#fff",
			borderWidth: 1,
			marginBottom: 12,
			elevation: 3,
		},

		infoContainer: {
			flex: 1,
			paddingRight: 12,
		},

		label: {
			fontSize: 16,
			fontFamily: Fonts.DMSans.Bold,
			color: colors.texto,
			marginTop: 4,
		},

		textoInfo: {
			fontSize: 16,
			fontFamily: Fonts.DMSans.Regular,
			color: colors.texto,
		},

		botaoEditar: {
			marginTop: 8,
			paddingVertical: 6,
			paddingHorizontal: 20,
			backgroundColor: colors.header,
			borderRadius: 6,
			alignSelf: "flex-start",
			marginRight: 8,
		},

		botaoDeletar: {
			marginTop: 8,
			paddingVertical: 6,
			paddingHorizontal: 20,
			backgroundColor: "red",
			borderRadius: 6,
			alignSelf: "flex-start",
		},

		textoBotaoEditar: {
			color: "#fff",
			fontSize: 20,
			fontFamily: Fonts.DMSans.Bold,
		},

		imagemContainer: {
			alignItems: "center",
			justifyContent: "center",
			flexShrink: 0,
		},

		imagem: {
			width: 120,
			height: 120,
			resizeMode: "contain",
		},

		modeloTexto: {
			fontSize: 20,
			fontFamily: Fonts.RobotoSlab.Bold,
			color: colors.header,
			marginTop: 4,
		},
	});
};
