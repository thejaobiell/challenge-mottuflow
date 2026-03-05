import React, { useState, useEffect, useCallback } from "react";
import {
	View,
	Text,
	TextInput,
	TouchableOpacity,
	KeyboardAvoidingView,
	TouchableWithoutFeedback,
	ActivityIndicator,
	Alert,
	ScrollView,
	RefreshControl,
} from "react-native";
import { RadioButton } from "react-native-paper";
import { Ionicons } from "@expo/vector-icons";
import { useTheme } from "../../Context/ThemeContext";
import MotoCard from "../../Components/MotoCard";
import {
	listarMotosTags,
	buscarMotosPorPlaca,
	buscarArucoPorCodigo,
	buscarMotoPorId,
	buscarMotosPorModelo,
} from "../../../types/Endpoints";
import { ArucoTag, Moto } from "../../../types/Tabelas";
import { pesquisarmotosStyle } from "./styles";
import { NativeStackScreenProps } from "@react-navigation/native-stack";
import { useIsFocused } from "@react-navigation/native";
import { Telas } from "../../../types/Telas";

type opcoes = "placa" | "aruco" | "modelo";
type Props = NativeStackScreenProps<Telas, "PesquisarMoto">;

export default function PesquisarMoto({ navigation }: Props) {
	const { colors } = useTheme();
	const styles = pesquisarmotosStyle(colors);

	const [opcaoSelecionada, setOpcaoSelecionada] = useState<opcoes>("placa");
	const [textoPesquisado, setTextoPesquisado] = useState<string>("");
	const [taCarregando, setTaCarregando] = useState<boolean>(false);
	const [motos, setMotos] = useState<Moto[]>([]);
	const [foiPesquisado, setFoiPesquisado] = useState<boolean>(false);
	const [refreshing, setRefreshing] = useState(false);
	const isFocused = useIsFocused();

	useEffect(() => {
		if (isFocused) {
			carregarTodasMotos();
		}
	}, [isFocused]);

	const carregarTodasMotos = async () => {
		setTaCarregando(true);
		try {
			const response = await listarMotosTags();
			const motosData: Moto[] = Array.isArray(response.data)
				? response.data
				: [response.data];
			setMotos(motosData);
		} catch (error) {
			console.error("Erro ao carregar motos:", error);
			Alert.alert("Erro", "Não foi possível carregar as motos");
			setMotos([]);
		} finally {
			setTaCarregando(false);
			setRefreshing(false);
		}
	};

	const getPlaceholder = (option: opcoes) => {
		switch (option) {
			case "placa":
				return "Digite a placa";
			case "aruco":
				return "Digite o código ArUco";
			case "modelo":
				return "Digite o modelo da moto";
		}
	};

	const limparCampos = () => {
		setTextoPesquisado("");
		setFoiPesquisado(false);
		carregarTodasMotos();
	};

	const handlePesquisa = async () => {
		if (!textoPesquisado.trim()) {
			Alert.alert("Atenção", "Digite algo para pesquisar");
			return;
		}

		setTaCarregando(true);
		setFoiPesquisado(true);

		try {
			let resultados: Moto[] = [];

			switch (opcaoSelecionada) {
				case "placa": {
					try {
						const { data } = await buscarMotosPorPlaca(
							textoPesquisado.trim()
						);
						const arr = Array.isArray(data) ? data : [data];
						resultados = arr.filter(
							(item): item is Moto =>
								"placa" in item &&
								"modelo" in item &&
								"fabricante" in item &&
								"ano" in item
						);
					} catch (err: any) {
						if (
							err.response?.status === 404 ||
							err.response?.data?.message?.includes(
								"não encontrada"
							)
						) {
							resultados = [];
						} else {
							throw err;
						}
					}
					break;
				}

				case "modelo": {
					try {
						const { data } = await buscarMotosPorModelo(
							textoPesquisado.trim()
						);
						const arr = Array.isArray(data) ? data : [data];
						resultados = arr.filter(
							(item): item is Moto =>
								"placa" in item &&
								"modelo" in item &&
								"fabricante" in item &&
								"ano" in item
						);
					} catch (err: any) {
						if (
							err.response?.status === 404 ||
							err.response?.data?.message?.includes(
								"não encontrada"
							)
						) {
							resultados = [];
						} else {
							throw err;
						}
					}
					break;
				}

				case "aruco": {
					const codigoAruco = textoPesquisado.trim().startsWith("TAG")
						? textoPesquisado.trim()
						: `TAG${textoPesquisado.trim()}`;

					try {
						const { data: tag } = await buscarArucoPorCodigo(
							codigoAruco
						);
						const { data: moto } = await buscarMotoPorId(
							tag.idMoto
						);
						resultados = [moto];
					} catch (err: any) {
						if (
							err.response?.status === 404 ||
							err.response?.data?.message?.includes(
								"ArucoTag não encontrada"
							)
						) {
							resultados = [];
						} else {
							throw err;
						}
					}
					break;
				}
			}

			setMotos(resultados);
		} catch (error: any) {
			console.error("Erro na pesquisa:", error);
			Alert.alert("Erro", "Erro de conexão ou servidor");
			setMotos([]);
		} finally {
			setTaCarregando(false);
		}
	};

	const handleEditarMoto = (moto: Moto) => {
		navigation.navigate("EditarMoto", { moto });
	};

	const handleDeletarMoto= (moto: Moto) => {
		navigation.navigate("DeletarMoto", { moto });
	};

	const renderVazio = () => (
		<View style={styles.containerVazio}>
			<Ionicons
				name="search"
				size={80}
				color={colors.iconeInativo}
				style={styles.iconeVazio}
			/>
			<Text style={styles.tituloVazio}>Nenhuma moto encontrada</Text>
			<Text style={styles.mensagemVazio}>
				Não foram encontradas motos com os critérios de pesquisa
				utilizados.
			</Text>
			<TouchableOpacity style={styles.botaoVazio} onPress={limparCampos}>
				<Text style={styles.textoBotaoVazio}>Ver todas as motos</Text>
			</TouchableOpacity>
		</View>
	);

	const onRefresh = useCallback(() => {
		setRefreshing(true);
		carregarTodasMotos();
	}, []);

	return (
		<KeyboardAvoidingView behavior="height" style={styles.container}>
			<TouchableWithoutFeedback>
				<ScrollView
					contentContainerStyle={{ flexGrow: 1, paddingBottom: 40 }}
					refreshControl={
						<RefreshControl
							refreshing={refreshing}
							onRefresh={onRefresh}
							colors={[colors.header]}
						/>
					}>
					<Text style={styles.label}>Pesquisar por:</Text>
					<View style={styles.radioGroup}>
						{(["placa", "aruco", "modelo"] as opcoes[]).map(
							(option) => (
								<View key={option} style={styles.opcoesRadio}>
									<RadioButton
										value={option}
										status={
											opcaoSelecionada === option
												? "checked"
												: "unchecked"
										}
										onPress={() =>
											setOpcaoSelecionada(option)
										}
										theme={{
											colors: {
												primary: colors.iconeAtivo,
											},
										}}
									/>
									<Text style={styles.textoRadio}>
										{option}
									</Text>
								</View>
							)
						)}
					</View>

					<View style={styles.inputRow}>
						<TextInput
							style={styles.input}
							placeholder={getPlaceholder(opcaoSelecionada)}
							placeholderTextColor={colors.iconeInativo}
							returnKeyType="search"
							value={textoPesquisado}
							onChangeText={setTextoPesquisado}
							onSubmitEditing={handlePesquisa}
						/>
						{textoPesquisado.length > 0 && (
							<TouchableOpacity
								style={styles.iconeBotao}
								onPress={limparCampos}>
								<Ionicons
									name="close-circle"
									size={24}
									color={colors.iconeInativo}
								/>
							</TouchableOpacity>
						)}
					</View>

					<View style={styles.botaoRow}>
						<TouchableOpacity
							style={styles.botao}
							onPress={handlePesquisa}
							disabled={taCarregando}>
							{taCarregando && (
								<ActivityIndicator size="small" color="#fff" />
							)}
							<Ionicons name="search" size={20} color="#fff" />
							<Text style={styles.textoBotao}>Buscar</Text>
						</TouchableOpacity>
					</View>

					<View style={styles.containerResultado}>
						{motos.length > 0 && (
							<Text style={styles.contadorResultado}>
								{motos.length}{" "}
								{motos.length === 1
									? "moto encontrada"
									: "motos encontradas"}
							</Text>
						)}

						{taCarregando && (
							<View
								style={{
									alignItems: "center",
									paddingVertical: 20,
								}}>
								<ActivityIndicator
									size="large"
									color={colors.header}
								/>
							</View>
						)}

						{motos.length === 0 &&
							!taCarregando &&
							foiPesquisado &&
							renderVazio()}

						{motos.length > 0 &&
							motos.map((moto, index) => (
								<MotoCard
									key={moto.idMoto ?? index}
									moto={moto}
									onPressEdit={() => handleEditarMoto(moto)}
									onPressDelete={() => handleDeletarMoto(moto)} 
								/>
							))}
					</View>
				</ScrollView>
			</TouchableWithoutFeedback>
		</KeyboardAvoidingView>
	);
}