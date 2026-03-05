import { StyleSheet } from "react-native";
import { Fonts } from "../../../types/Fonts";

export const perfilStyles = StyleSheet.create({
	container: {
		flex: 1,
		paddingHorizontal: 20,
	},
	titulo: {
		fontSize: 34,
		fontFamily: Fonts.RobotoSlab.Bold,
		textAlign: "center",
		marginBottom: 30,
	},
	itemContainer: {
		marginBottom: 30,
		borderBottomWidth: 1,
		paddingBottom: 10,
	},
	label: {
		fontSize: 20,
		fontFamily: Fonts.RobotoSlab.Black,
		marginBottom: 5,
	},
	valor: {
		fontSize: 26,
		fontFamily: Fonts.RobotoSlab.Regular,
	},
	botao: {
		padding: 14,
		borderRadius: 25,
		marginTop: 20,
		alignSelf: "center",
		width: "60%",
	},
	textoBotao: {
		fontSize: 20,
		fontFamily: Fonts.RobotoSlab.Bold,
		color: "#fff",
		textAlign: "center",
	},
});