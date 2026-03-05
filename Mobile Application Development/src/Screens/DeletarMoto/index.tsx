import React, { useState } from "react";
import {
	View,
	Text,
	TouchableOpacity,
	Alert,
	ActivityIndicator,
	ScrollView,
	Image,
} from "react-native";
import { Ionicons } from "@expo/vector-icons";
import { NativeStackScreenProps } from "@react-navigation/native-stack";
import { useTheme } from "../../Context/ThemeContext";
import { Telas } from "../../../types/Telas";
import { Moto } from "../../../types/Tabelas";
import { deletarMotoPorId } from "../../../types/Endpoints";
import { deletarMotoStyle } from "./styles";

type Props = NativeStackScreenProps<Telas, "DeletarMoto">;

const getImagem = (modelo: string) => {
	const img = modelo.toLowerCase();
	if (img.includes("sport")) return require("../../../images/mottuSport.png");
	if (img.includes("pop")) return require("../../../images/mottuPop.png");
	if (img.includes("e")) return require("../../../images/mottuE.png");
};

export default function DeletarMoto({ navigation, route }: Props) {
	const { colors } = useTheme();
	const styles = deletarMotoStyle(colors);
	const { moto } = route.params as { moto: Moto };
	const [loading, setLoading] = useState(false);

	const handleDelete = () => {
		Alert.alert(
			"⚠️ Confirmação de Exclusão",
			`Tem certeza que deseja deletar a moto ${moto.modelo} (${moto.placa})?\nEsta ação não pode ser desfeita.`,
			[
				{ text: "Cancelar", style: "cancel" },
				{
					text: "Deletar",
					style: "destructive",
					onPress: async () => {
						setLoading(true);
						try {
							await deletarMotoPorId(moto.idMoto!);
							Alert.alert(
								"Sucesso",
								"Moto deletada com sucesso!",
								[
									{
										text: "OK",
										onPress: () =>
											navigation.goBack()
									},
								]
							);
						} catch (err) {
							console.error(err);
							Alert.alert(
								"Erro",
								"Não foi possível deletar a moto. Verifique sua conexão."
							);
						} finally {
							setLoading(false);
						}
					},
				},
			]
		);
	};

	const InfoItem = ({
		label,
		value,
		icon,
	}: {
		label: string;
		value: string | number;
		icon?: string;
	}) => (
		<View style={styles.infoItemContainer}>
			{icon && (
				<Ionicons
					name={icon as any}
					size={16}
					color="black"
					style={styles.infoIcon}
				/>
			)}
			<Text style={styles.infoItemText}>
				<Text style={styles.infoLabel}>{label}: </Text>
				<Text style={styles.infoValue}>{value}</Text>
			</Text>
		</View>
	);

	return (
		<ScrollView
			contentContainerStyle={styles.container}
			showsVerticalScrollIndicator={false}>
			<View style={styles.header}>
				<Ionicons name="warning" size={24} color="#ff6b6b" />
				<Text style={styles.headerTitle}>Excluir Motocicleta</Text>
			</View>

			<View style={styles.mainCard}>
				<View style={styles.imageContainer}>
					<Image
						source={getImagem(moto.modelo)}
						style={styles.motoImage}
					/>
					<View style={styles.modeloBadge}>
						<Text style={styles.modeloText}>{moto.modelo}</Text>
					</View>
				</View>

				<View style={styles.mainInfo}>
					<View style={styles.placaContainer}>
						<Ionicons name="car" size={20} color="#fff" />
						<Text style={styles.placaText}>{moto.placa}</Text>
					</View>
					<Text style={styles.fabricanteText}>
						{moto.fabricante} • {moto.ano}
					</Text>
				</View>
			</View>

			<View style={styles.detailsCard}>
				<Text style={styles.titleText}>Informações Detalhadas</Text>
				<InfoItem
					label="ID da Moto"
					value={moto.idMoto || "N/A"}
					icon="barcode"
				/>
				<InfoItem
					label="Localização Atual"
					value={moto.localizacaoAtual}
					icon="location"
				/>
				<InfoItem
					label="ID do Pátio"
					value={moto.idPatio || "N/A"}
					icon="business"
				/>

				{moto.arucoTags?.length > 0 && (
					<View style={{ marginTop: 8 }}>
						<Text style={styles.infoLabel}>Tags ArUco:</Text>
						{moto.arucoTags.map((tag) => (
							<Text key={tag.idTag} style={styles.infoValue}>
								{tag.codigo} - {tag.status}
							</Text>
						))}
					</View>
				)}
			</View>

			<View style={styles.warningCard}>
				<Ionicons name="alert-circle" size={24} color="#fbbf24" />
				<View style={styles.warningContent}>
					<Text style={styles.warningTitle}>Atenção!</Text>
					<Text style={styles.warningText}>
						Esta ação irá remover permanentemente todos os dados
						desta motocicleta do sistema. Não pode ser desfeita.
					</Text>
				</View>
			</View>

			<TouchableOpacity
				style={[
					styles.deleteButton,
					loading && styles.deleteButtonDisabled,
				]}
				onPress={handleDelete}
				disabled={loading}>
				{loading ? (
					<ActivityIndicator size="small" color="#fff" />
				) : (
					<>
						<Ionicons name="trash" size={20} color="#fff" />
						<Text style={styles.deleteButtonText}>
							Confirmar Exclusão
						</Text>
					</>
				)}
			</TouchableOpacity>
		</ScrollView>
	);
}
