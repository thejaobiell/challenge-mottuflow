import React from "react";
import { View, Text, TouchableOpacity, Image } from "react-native";
import { MotoCardProps } from "../../types/PropsTypes";
import { motoCardStyles } from "./MotoCardStyles";

const mottuPop = require("../../images/mottuPop.png");
const mottuSport = require("../../images/mottuSport.png");
const mottuE = require("../../images/mottuE.png");

export default function MotoCard({ moto, onPressEdit, onPressDelete }: MotoCardProps) {
	const styles = motoCardStyles();

	const getMotoImage = (modelo?: string) => {
		if (!modelo) return null;
		const nome = modelo.toLowerCase();

		if (nome.includes("pop")) return mottuPop;
		if (nome.includes("sport")) return mottuSport;
		if (nome.includes("e")) return mottuE;

		return null;
	};

	const imagem = getMotoImage(moto.modelo);

	return (
		<View style={styles.card}>
			<View style={styles.infoContainer}>
				{moto.idMoto !== undefined && (
					<Text style={styles.label}>
						ID: <Text style={styles.textoInfo}>{moto.idMoto}</Text>
					</Text>
				)}
				{moto.placa && (
					<Text style={styles.label}>
						Placa:{" "}
						<Text style={styles.textoInfo}>{moto.placa}</Text>
					</Text>
				)}
				{moto.fabricante && (
					<Text style={styles.label}>
						Fabricante:{" "}
						<Text style={styles.textoInfo}>{moto.fabricante}</Text>
					</Text>
				)}
				{moto.ano !== undefined && (
					<Text style={styles.label}>
						Ano: <Text style={styles.textoInfo}>{moto.ano}</Text>
					</Text>
				)}
				{moto.localizacaoAtual && (
					<Text style={styles.label}>
						Localização:{" "}
						<Text style={styles.textoInfo}>
							{moto.localizacaoAtual}
						</Text>
					</Text>
				)}

				{moto.arucoTags && moto.arucoTags.length > 0 && (
					<View style={{ marginTop: 4 }}>
						<Text style={styles.label}>Código do ArUco:</Text>
						{moto.arucoTags.map((tag) => (
							<Text key={tag.idTag} style={styles.textoInfo}>
								{tag.codigo} ({tag.status})
							</Text>
						))}
					</View>
				)}

				{(onPressEdit || onPressDelete) && (
					<View style={{ flexDirection: "row", marginTop: 8 }}>
						{onPressEdit && (
							<TouchableOpacity
								style={styles.botaoEditar}
								onPress={onPressEdit}>
								<Text style={styles.textoBotaoEditar}>
									Editar
								</Text>
							</TouchableOpacity>
						)}
						{onPressDelete && (
							<TouchableOpacity
								style={styles.botaoDeletar}
								onPress={onPressDelete}>
								<Text style={styles.textoBotaoEditar}>
									Deletar
								</Text>
							</TouchableOpacity>
						)}
					</View>
				)}
			</View>

			<View style={styles.imagemContainer}>
				{imagem && <Image source={imagem} style={styles.imagem} />}
				<Text style={styles.modeloTexto}>{moto.modelo}</Text>
			</View>
		</View>
	);
}
