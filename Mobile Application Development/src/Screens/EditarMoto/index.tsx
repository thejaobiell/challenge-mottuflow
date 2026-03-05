import React, { useEffect, useState } from "react";
import {
	ScrollView,
	Text,
	TextInput,
	TouchableOpacity,
	Alert,
	KeyboardAvoidingView,
	View,
	Switch,
} from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";
import { NativeStackScreenProps } from "@react-navigation/native-stack";
import { Telas } from "../../../types/Telas";
import { editarmotoStyles } from "./styles";
import { Picker } from "@react-native-picker/picker";
import { useTheme } from "../../Context/ThemeContext";
import {
	Moto,
	Patio,
	Localidade,
	TipoStatus,
	ArucoTag,
	RegistroStatus,
} from "../../../types/Tabelas";
import {
	listarPatios,
	localidadesBuscarPorPatio,
	editarMoto,
	editarArucoTag,
	editarRegistroStatus,
	buscarRegistroStatusPorMoto,
} from "../../../types/Endpoints";
import MaskInput from "react-native-mask-input";
import { useAuth } from "../../Context/AuthContext";

type Props = NativeStackScreenProps<Telas, "EditarMoto">;

export default function EditarMoto({ route, navigation }: Props) {
	const { moto } = route.params;
	const { colors } = useTheme();
	const { user } = useAuth();
	const styles = editarmotoStyles();

	const modelosMotos = [
		{ nome: "Mottu Sport", fabricante: "TVS", ano: "2022" },
		{ nome: "Mottu E", fabricante: "Mottu", ano: "2022" },
		{ nome: "Mottu Pop", fabricante: "Honda", ano: "2025" },
	];

	const [editarMotoAtivo, setEditarMotoAtivo] = useState(false);
	const [editarArucoAtivo, setEditarArucoAtivo] = useState(false);
	const [editarStatusAtivo, setEditarStatusAtivo] = useState(false);
	const [placa, setPlaca] = useState(moto.placa);
	const [modelo, setModelo] = useState(moto.modelo);
	const [fabricante, setFabricante] = useState(moto.fabricante);
	const [ano, setAno] = useState(moto.ano.toString());
	const [idPatio, setIdPatio] = useState(moto.idPatio.toString());
	const [idLocalidade, setIdLocalidade] = useState(moto.localizacaoAtual.toString());
	const [motoPredefinida, setMotoPredefinida] = useState<string>("");
	const [codigoAruco, setCodigoAruco] = useState(
		moto.arucoTags?.[0]?.codigo || ""
	);
	const [statusAruco, setStatusAruco] = useState(
		moto.arucoTags?.[0]?.status || "ATIVO"
	);

	const [registroStatus, setRegistroStatus] = useState<RegistroStatus | null>(
		null
	);
	const [tipoStatus, setTipoStatus] = useState<TipoStatus>("DISPONIVEL");
	const [descricaoStatus, setDescricaoStatus] = useState("");

	const [patios, setPatios] = useState<Patio[]>([]);
	const [localidades, setLocalidades] = useState<Localidade[]>([]);

	useEffect(() => {
		listarPatios()
			.then(({ data }) => setPatios(data))
			.catch(() => setPatios([]));
	}, []);

	useEffect(() => {
		if (!idPatio) return setLocalidades([]);
		localidadesBuscarPorPatio(Number(idPatio))
			.then(({ data }) => setLocalidades(data))
			.catch(() => setLocalidades([]));
	}, [idPatio]);

	useEffect(() => {
		const fetchStatus = async () => {
			try {
				const { data } = await buscarRegistroStatusPorMoto(moto.idMoto);
				if (data) {
					setRegistroStatus(data);
					setTipoStatus(data.tipoStatus);
					setDescricaoStatus(data.descricao || "");
				}
			} catch (err) {
				console.error("Erro ao buscar registroStatus:", err);
			}
		};
		fetchStatus();
	}, [moto.idMoto]);

	const salvarAlteracoes = async () => {
		try {
			let alteracoes: string[] = [];
			const motoAlterada: Partial<Moto> = {
				placa: placa.toUpperCase(),
				modelo,
				fabricante,
				ano: Number(ano),
				idPatio: Number(idPatio),
				localizacaoAtual: idLocalidade,
			};
			if (editarMotoAtivo) {
				if (placa !== moto.placa) {
					motoAlterada.placa = placa.toUpperCase();
					alteracoes.push(
						`Placa: ${moto.placa} → ${placa.toUpperCase()}`
					);
				}
				if (modelo !== moto.modelo) {
					motoAlterada.modelo = modelo;
					alteracoes.push(`Modelo: ${moto.modelo} → ${modelo}`);
				}
				if (fabricante !== moto.fabricante) {
					motoAlterada.fabricante = fabricante;
					alteracoes.push(
						`Fabricante: ${moto.fabricante} → ${fabricante}`
					);
				}
				if (Number(ano) !== moto.ano) {
					motoAlterada.ano = Number(ano);
					alteracoes.push(`Ano: ${moto.ano} → ${ano}`);
				}
				if (Number(idPatio) !== moto.idPatio) {
					motoAlterada.idPatio = Number(idPatio);
					const nomeAntigo =
						patios.find((p) => p.idPatio === moto.idPatio)?.nome ||
						`ID ${moto.idPatio}`;
					const nomeNovo =
						patios.find((p) => p.idPatio === Number(idPatio))
							?.nome || `ID ${idPatio}`;
					alteracoes.push(`Pátio: ${nomeAntigo} → ${nomeNovo}`);
				}
				if (idLocalidade !== moto.localizacaoAtual) {
					motoAlterada.localizacaoAtual = idLocalidade;
					const nomeAntigo =
						localidades.find(
							(l) =>
								l.idLocalidade.toString() ===
								moto.localizacaoAtual
						)?.pontoReferencia || `ID ${moto.localizacaoAtual}`;
					const nomeNovo =
						localidades.find(
							(l) => l.idLocalidade.toString() === idLocalidade
						)?.pontoReferencia || `ID ${idLocalidade}`;
					alteracoes.push(`Localidade: ${nomeAntigo} → ${nomeNovo}`);
				}
			}

			const aruco = moto.arucoTags?.[0];
			const arucoAlterado: Partial<ArucoTag> = {
				idTag: aruco?.idTag,
				idMoto: moto.idMoto,
			};
			if (editarArucoAtivo && aruco) {
				if (codigoAruco !== aruco.codigo) {
					arucoAlterado.codigo = codigoAruco;
					alteracoes.push(
						`Código Aruco: ${aruco.codigo} → ${codigoAruco}`
					);
				}
				if (statusAruco !== aruco.status) {
					arucoAlterado.status = statusAruco;
					alteracoes.push(
						`Status Aruco: ${aruco.status} → ${statusAruco}`
					);
				}
			}

			const registroAlterado: Partial<RegistroStatus> = {
				idMoto: moto.idMoto,
				idFuncionario: user?.id,
			};
			if (editarStatusAtivo && registroStatus) {
				if (tipoStatus) registroAlterado.tipoStatus = tipoStatus;
				if (descricaoStatus)
					registroAlterado.descricao = descricaoStatus;
				if (tipoStatus || descricaoStatus) {
					alteracoes.push(
						`Status alterado: ${tipoStatus || ""} ${
							descricaoStatus || ""
						}`
					);
				}
			}

			if (alteracoes.length === 0) {
				Alert.alert(
					"Nenhuma alteração",
					"Você não modificou nenhum campo."
				);
				return;
			}

			Alert.alert("Confirma alterações?", alteracoes.join("\n"), [
				{ text: "Cancelar", style: "cancel" },
				{
					text: "Salvar",
					onPress: async () => {
						if (Object.keys(motoAlterada).length > 1) {
							await editarMoto(moto.idMoto, motoAlterada);
						}

						if (editarArucoAtivo && aruco) {
							await editarArucoTag(aruco.idTag, arucoAlterado);
						}

						if (editarStatusAtivo && registroStatus?.idStatus) {
							await editarRegistroStatus(
								registroStatus.idStatus,
								registroAlterado
							);
						}

						Alert.alert("Sucesso", "Alterações salvas!");
						navigation.goBack();
					},
				},
			]);
		} catch (error) {
			console.error(error);
			Alert.alert("Erro", "Falha ao atualizar moto.");
		}
	};

	return (
		<SafeAreaView style={{ flex: 1, backgroundColor: colors.background }}>
			<KeyboardAvoidingView style={{ flex: 1 }}>
				<ScrollView
					contentContainerStyle={{ paddingBottom: 200 }}
					keyboardShouldPersistTaps="handled">
					<View
						style={{
							flexDirection: "row",
							justifyContent: "space-between",
							alignItems: "center",
						}}>
						<Text style={styles.subtitulo}>
							Editar Informações da Moto
						</Text>
						<Switch
							value={editarMotoAtivo}
							onValueChange={setEditarMotoAtivo}
						/>
					</View>
					{editarMotoAtivo && (
						<>
							<Text style={styles.subtitulo}>Placa</Text>
							<MaskInput
								style={styles.input}
								value={placa}
								onChangeText={setPlaca}
								mask={[
									/[A-Z]/,
									/[A-Z]/,
									/[A-Z]/,
									"-",
									/\d/,
									/\d/,
									/\d/,
									/\d/,
								]}
								autoCapitalize="characters"
							/>
							<Text style={styles.subtitulo}>Modelo</Text>
							<View style={styles.pickerContainer}>
								<Picker
									selectedValue={motoPredefinida}
									onValueChange={(valor) => {
										setMotoPredefinida(valor);
										const motoSel = modelosMotos.find(
											(m) => m.nome === valor
										);
										if (motoSel) {
											setModelo(motoSel.nome);
											setFabricante(motoSel.fabricante);
											setAno(motoSel.ano);
										}
									}}
									style={{
										backgroundColor: colors.tabBar,
										color: colors.texto,
									}}>
									<Picker.Item
										label="Selecione um modelo"
										value=""
									/>
									{modelosMotos.map((moto) => (
										<Picker.Item
											key={moto.nome}
											label={`${moto.nome}`}
											value={moto.nome}
										/>
									))}
								</Picker>
							</View>

							<Text style={styles.subtitulo}>Fabricante</Text>
							<TextInput
								style={styles.input}
								value={fabricante}
								editable={false}
							/>

							<Text style={styles.subtitulo}>Ano</Text>
							<TextInput
								style={styles.input}
								value={ano}
								editable={false}
							/>
							<Text style={styles.subtitulo}>Pátio</Text>
							<View style={styles.pickerContainer}>
								<Picker
									selectedValue={idPatio}
									onValueChange={setIdPatio}
									style={{
										backgroundColor: colors.tabBar,
										color: colors.texto,
									}}>
									<Picker.Item
										label="Selecione um pátio"
										value=""
									/>
									{patios.map((p) => (
										<Picker.Item
											key={p.idPatio}
											label={` ${p.idPatio} - ${p.nome} - ${p.endereco}`}
											value={p.idPatio.toString()}
										/>
									))}
								</Picker>
							</View>
							<Text style={styles.subtitulo}>Localidade</Text>
							<View style={styles.pickerContainer}>
								<Picker
									selectedValue={idLocalidade}
									onValueChange={setIdLocalidade}
									style={{
										backgroundColor: colors.tabBar,
										color: colors.texto,
									}}>
									<Picker.Item
										label="Selecione uma localidade"
										value=""
									/>
									{localidades.map((loc) => (
										<Picker.Item
											key={loc.idLocalidade}
											label={loc.pontoReferencia}
											value={loc.idLocalidade.toString()}
										/>
									))}
								</Picker>
							</View>
						</>
					)}

					<View style={styles.separador} />

					<View style={{ marginVertical: 10 }}>
						<View
							style={{
								flexDirection: "row",
								justifyContent: "space-between",
								alignItems: "center",
							}}>
							<Text style={styles.subtitulo}>
								Editar Informações do Aruco Tag
							</Text>
							<Switch
								value={editarArucoAtivo}
								onValueChange={setEditarArucoAtivo}
							/>
						</View>
						{editarArucoAtivo && (
							<>
								<Text style={styles.subtitulo}>
									Código ArucoTag
								</Text>
								<TextInput
									style={styles.input}
									value={codigoAruco.replace("TAG", "")}
									onChangeText={(num) =>
										setCodigoAruco(`TAG${num}`)
									}
									keyboardType="numeric"
									maxLength={3}
								/>
								<Text style={styles.subtitulo}>
									Status ArucoTag
								</Text>
								<View style={styles.pickerContainer}>
									<Picker
										selectedValue={statusAruco}
										onValueChange={(itemValue) =>
											setStatusAruco(itemValue)
										}
										style={{
											backgroundColor: colors.tabBar,
											color: colors.texto,
										}}>
										<Picker.Item
											label="ATIVO"
											value="ATIVO"
										/>
										<Picker.Item
											label="INATIVO"
											value="INATIVO"
										/>
									</Picker>
								</View>
							</>
						)}
					</View>

					<View style={styles.separador} />

					<View style={{ marginVertical: 10 }}>
						<View
							style={{
								flexDirection: "row",
								justifyContent: "space-between",
								alignItems: "center",
							}}>
							<Text style={styles.subtitulo}>
								Editar Informações da Status
							</Text>
							<Switch
								value={editarStatusAtivo}
								onValueChange={setEditarStatusAtivo}
							/>
						</View>
						{editarStatusAtivo && (
							<>
								<Text style={styles.subtitulo}>
									Tipo de Status
								</Text>
								<View style={styles.pickerContainer}>
									<Picker
										selectedValue={tipoStatus}
										onValueChange={(v) =>
											setTipoStatus(v as TipoStatus)
										}
										style={{
											backgroundColor: colors.tabBar,
											color: colors.texto,
										}}>
										<Picker.Item
											label="Disponível"
											value="DISPONIVEL"
										/>
										<Picker.Item
											label="Manutenção"
											value="MANUTENCAO"
										/>
										<Picker.Item
											label="Reservado"
											value="RESERVADO"
										/>
										<Picker.Item
											label="Baixa por Boletim de Ocorrência"
											value="BAIXA_BOLETIM_OCORRENCIA"
										/>
									</Picker>
								</View>
								<Text style={styles.subtitulo}>
									Descrição Status
								</Text>
								<TextInput
									style={styles.input}
									value={descricaoStatus}
									onChangeText={setDescricaoStatus}
								/>
							</>
						)}
					</View>

					<TouchableOpacity
						style={styles.botaoSalvar}
						onPress={salvarAlteracoes}>
						<Text style={styles.botaoSalvarTexto}>Salvar</Text>
					</TouchableOpacity>
				</ScrollView>
			</KeyboardAvoidingView>
		</SafeAreaView>
	);
}