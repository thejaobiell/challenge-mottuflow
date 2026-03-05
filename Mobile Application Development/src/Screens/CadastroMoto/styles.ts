import { StyleSheet } from "react-native";
import { useTheme } from "../../Context/ThemeContext";
import { Fonts } from "../../../types/Fonts";

export const cadastromotoStyle = () => {
	const { colors } = useTheme();

	return StyleSheet.create({
		container: {
			flex: 1,
		},

		label: {
			marginTop: 10,
			fontFamily: Fonts.RobotoSlab.Bold,
			fontSize: 16,
			color: colors.texto,
		},

		viewDaMoto: {
			flexDirection: "row",
			alignItems: "center",
			marginBottom: 20,
		},

		viewMotoNull: {
			width: 150,
			height: 150,
			justifyContent: "center",
			alignItems: "center",
			marginRight: 20,
			borderWidth: 1,
			borderColor: colors.texto,
			borderRadius: 10,
		},

		imagem: {
			width: 150,
			height: 150,
			resizeMode: "contain",
			marginRight: 20,
		},

		input: {
			borderWidth: 1,
			borderColor: "#ccc",
			borderRadius: 5,
			padding: 10,
			marginTop: 5,
			backgroundColor: colors.tabBar,
			fontFamily: Fonts.DMSans.Regular,
			fontSize: 16,
			color: colors.texto,
		},

		botaoRegistrar: {
			marginTop: 30,
			backgroundColor: colors.header,
			padding: 5,
			borderRadius: 5,
		},

		obrigatorio: {
			color: "#ff0000ff",
		},

		pickerContainer: {
			borderWidth: 1,
			borderColor: colors.texto,
			marginTop: 5,
			backgroundColor: colors.tabBar,
		},

		infoMoto: {
			justifyContent: "center",
			backgroundColor: colors.tabBar,
			padding: 10,
			borderRadius: 10,
			flex: 1,
		},

		infoMotoTexto: {
			fontSize: 16,
			color: colors.texto,
			fontFamily: Fonts.RobotoSlab.Black,
		},

		infoMotoTitulo: {
			fontSize: 18,
			fontFamily: Fonts.RobotoSlab.Medium,
			color: colors.texto,
		},

		separador: {
			height: 1,
			backgroundColor: colors.texto,
			marginVertical: 10,
		},

		tituloSecao: {
			fontSize: 20,
			fontFamily: Fonts.RobotoSlab.Bold,
			color: colors.header,
			marginBottom: 10,
			marginTop: 10,
		},
	});
};
