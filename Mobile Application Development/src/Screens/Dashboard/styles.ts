import { StyleSheet } from "react-native";
import { useTheme } from "../../Context/ThemeContext";
import { Fonts } from "../../../types/Fonts";

export const dashboardStyles = () => {
	const { colors } = useTheme();

	return StyleSheet.create({
		container: {
			flex: 1,
			backgroundColor: colors.background,
			padding: 16,
		},
		texto: {
			fontSize: 20,
			fontFamily: Fonts.RobotoSlab.Bold,
			color: colors.texto,
		},
		nome: {
			fontSize: 30,
			fontFamily: Fonts.RobotoSlab.Black,
			color: colors.header,
		},
		botaoContainer: {
			marginTop: 50,
			flexDirection: "row",
			flexWrap: "wrap",
			justifyContent: "center",
			alignContent: "center",
		},
		loadingContainer: {
			flex: 1,
			justifyContent: "center",
			alignItems: "center",
			backgroundColor: colors.background,
		},
		loadingTexto: {
			marginTop: 10,
			fontSize: 16,
			fontFamily: Fonts.RobotoSlab.Regular,
			color: colors.texto,
		},
		textoBemvindo: {
			fontSize: 18,
			fontFamily: Fonts.RobotoSlab.Bold,
			color: colors.texto,
			marginBottom: 20,
			textAlign: "center",
		},
		estatisticaContainer: {
			flexDirection: "column",
			alignItems: "center",
			marginBottom: 24,
		},
		estatisticaCard: {
			width: "100%",
			backgroundColor: colors.background,
			padding: 16,
			borderRadius: 12,
			alignItems: "center",
			elevation: 5,
			marginBottom: 30,
		},
		estatisticaNumber: {
			fontSize: 24,
			fontFamily: Fonts.RobotoSlab.Black,
			color: colors.texto,
			marginTop: 8,
		},
		estatisticaLabel: {
			fontSize: 12,
			fontFamily: Fonts.RobotoSlab.Regular,
			color: colors.texto,
			marginTop: 4,
			textAlign: "center",
		},
		movimentacaoTexto: {
			fontSize: 12,
			fontFamily: Fonts.DMSans.Regular,
			color: colors.texto,
			marginTop: 2,
			textAlign: "center",
		},
	});
};