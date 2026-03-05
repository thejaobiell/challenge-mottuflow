import { StyleSheet } from "react-native";
import { useTheme } from "../../Context/ThemeContext";
import { Fonts } from "../../../types/Fonts";

export const editarmotoStyles = () => {
	const { colors } = useTheme();

	return StyleSheet.create({
		container: {
			flex: 1,
			padding: 20,
			backgroundColor: colors.background,
		},
		subtitulo: {
			fontSize: 18,
			fontFamily: Fonts.RobotoSlab.Bold,
			marginTop: 20,
			marginBottom: 10,
			color: colors.texto,
		},
		checkboxRow: {
			flexDirection: "row",
			alignItems: "center",
			marginVertical: 5,
		},
		input: {
			width: "100%",
			borderWidth: 1,
			borderColor: colors.texto,
			borderRadius: 5,
			padding: 10,
			marginVertical: 10,
			backgroundColor: colors.tabBar,
			fontFamily: Fonts.DMSans.Regular,
			fontSize: 16,
			color: colors.texto,
		},
		botaoSalvar: {
			backgroundColor: colors.header,
			padding: 12,
			marginTop: 20,
			alignItems: "center",
			borderRadius: 8,
		},
		botaoSalvarTexto: {
			color: colors.texto,
			fontSize: 16,
			fontFamily: Fonts.RobotoSlab.Bold,
		},
		pickerContainer: {
			borderWidth: 1,
			borderColor: colors.texto,
			marginTop: 5,
			backgroundColor: colors.tabBar,
		},
		separador: {
			height: 1,
			backgroundColor: colors.texto,
			marginVertical: 20,
		},
	});
};
