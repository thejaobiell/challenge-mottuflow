import { StyleSheet } from "react-native";
import { ThemeContextData } from "../../Context/ThemeContext";
import { Fonts } from "../../../types/Fonts";

export const deletarMotoStyle = (colors: ThemeContextData["colors"]) =>
	StyleSheet.create({
		container: {
			flexGrow: 1,
			padding: 16,
			backgroundColor: colors.background,
		},

		header: {
			flexDirection: "row",
			alignItems: "center",
			justifyContent: "center",
			marginBottom: 20,
		},

		headerTitle: {
			fontSize: 20,
			fontWeight: "bold",
			color: colors.texto,
			marginLeft: 10,
		},

		mainCard: {
			backgroundColor: colors.header,
			borderRadius: 16,
			marginBottom: 16,
			overflow: "hidden",
		},

		imageContainer: {
			alignItems: "center",
			paddingVertical: 16,
			backgroundColor: "#f0f0f0",
		},

		motoImage: {
			height: 150,
			aspectRatio: 1.5,
			resizeMode: "contain",
		},

		modeloBadge: {
			position: "absolute",
			top: 12,
			right: 12,
			backgroundColor: "rgba(0,0,0,0.8)",
			paddingHorizontal: 12,
			paddingVertical: 6,
			borderRadius: 20,
		},

		modeloText: {
			color: "#fff",
			fontSize: 12,
			fontFamily: Fonts.DMSans.Regular,
		},

		mainInfo: {
			padding: 16,
			backgroundColor: colors.header,
		},

		placaContainer: {
			flexDirection: "row",
			alignItems: "center",
			marginBottom: 6,
		},

		placaText: {
			color: "white",
			fontSize: 24,
			fontFamily: Fonts.RobotoSlab.Black,
			marginLeft: 8,
			letterSpacing: 1,
		},

		fabricanteText: {
			color: "white",
			fontSize: 16,
			fontFamily: Fonts.RobotoSlab.Black,
		},

		detailsCard: {
			backgroundColor: colors.iconeAtivo,
			borderRadius: 16,
			padding: 16,
			marginBottom: 16,
		},

		titleText: {
			fontSize: 18,
			fontFamily: Fonts.RobotoSlab.Black,
			color: colors.texto,
			marginBottom: 12,
		},

		infoItemContainer: {
			flexDirection: "row",
			alignItems: "center",
			marginBottom: 8,
		},

		infoIcon: {
			marginRight: 6,
		},

		infoItemText: {
			fontSize: 16,
		},

		infoLabel: {
			color: "black",
			fontFamily: Fonts.RobotoSlab.Bold,
		},

		infoValue: {
			color: "white",
			fontFamily: Fonts.DMSans.Regular,
		},

		warningCard: {
			flexDirection: "row",
			backgroundColor: "#fff4e5",
			padding: 16,
			borderRadius: 16,
			marginBottom: 16,
			borderLeftWidth: 4,
			borderLeftColor: "#fbbf24",
		},

		warningContent: {
			flex: 1,
			marginLeft: 12,
		},

		warningTitle: {
			fontSize: 16,
			fontFamily: Fonts.RobotoSlab.Black,
			color: "#92400e",
			marginBottom: 4,
		},

		warningText: {
			fontSize: 14,
			color: "#92400e",
			fontFamily: Fonts.DMSans.Bold,
			lineHeight: 20,
		},

		deleteButton: {
			backgroundColor: "#ef4444",
			flexDirection: "row",
			alignItems: "center",
			justifyContent: "center",
			padding: 16,
			borderRadius: 16,
			marginBottom: 30,
		},

		deleteButtonDisabled: {
			backgroundColor: "#9ca3af",
		},

		deleteButtonText: {
			color: "#fff",
			fontSize: 16,
			fontWeight: "bold",
			marginLeft: 8,
		},
	});
