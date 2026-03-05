import React, { useEffect, useState } from "react";
import { View, Text, TextInput, Alert, Image } from "react-native";
import { Button } from "react-native-paper";
import MaskInput from "react-native-mask-input";
import { Picker } from "@react-native-picker/picker";
import { useTheme } from "../../Context/ThemeContext";
import { cadastromotoStyle } from "./styles";
import { SafeAreaView } from "react-native-safe-area-context";
import {
	listarPatios,
	cadastrarMoto,
	cadastrarArucoTag,
	cadastrarRegistroStatus,
	localidadesBuscarPorPatio,
} from "../../../types/Endpoints";
import { KeyboardAwareScrollView } from "react-native-keyboard-aware-scroll-view";
import { MaterialCommunityIcons } from "@expo/vector-icons";
import { useAuth } from "../../Context/AuthContext";
import { Patio, Localidade, Moto, TipoStatus } from "../../../types/Tabelas";
import { NativeStackScreenProps } from "@react-navigation/native-stack";
import { Telas } from "../../../types/Telas";

type Props = NativeStackScreenProps<Telas, "CadastroMoto">;

export default function CadastroMoto({ navigation }: Props) {
	const styles = cadastromotoStyle();
	const { colors } = useTheme();
	const { user } = useAuth();

	const [placa, setPlaca] = useState("");
	const [modelo, setModelo] = useState("");
	const [fabricante, setFabricante] = useState("");
	const [ano, setAno] = useState("");
	const [idPatio, setIdPatio] = useState("");
	const [idLocalidade, setIdLocalidade] = useState("");
	const [motoPredefinida, setMotoPredefinida] = useState<string>("");
	const [codigoAruco, setCodigoAruco] = useState("");
	const [tipoStatus, setTipoStatus] = useState<TipoStatus | "">("");
	const [descricaoStatus, setDescricaoStatus] = useState("");

	const [patios, setPatios] = useState<Patio[]>([]);
	const [localidades, setLocalidades] = useState<Localidade[]>([]);

	const modelosMotos = [
		{ nome: "Mottu Sport", fabricante: "TVS", ano: "2022" },
		{ nome: "Mottu E", fabricante: "Mottu", ano: "2022" },
		{ nome: "Mottu Pop", fabricante: "Honda", ano: "2025" },
	];

	const imagensMotos = [
		{
			nome: modelosMotos[0].nome,
			imagem: require("../../../images/mottuPop.png"),
		},
		{
			nome: modelosMotos[1].nome,
			imagem: require("../../../images/mottuSport.png"),
		},
		{
			nome: modelosMotos[2].nome,
			imagem: require("../../../images/mottuE.png"),
		},
	];

	const imagemSelecionada = imagensMotos.find(
		(m) => m.nome === motoPredefinida
	)?.imagem;

	useEffect(() => {
		listarPatios()
			.then((res) => setPatios(Array.isArray(res.data) ? res.data : []))
			.catch((error) => {
				if (error.response?.status !== 401) setPatios([]);
			});
	}, []);

	useEffect(() => {
		if (!idPatio) {
			setLocalidades([]);
			setIdLocalidade("");
			return;
		}
		localidadesBuscarPorPatio(Number(idPatio))
			.then(({ data }) => setLocalidades(Array.isArray(data) ? data : []))
			.catch((error) => {
				if (error.response?.status !== 401) {
					setLocalidades([]);
					setIdLocalidade("");
				}
			});
	}, [idPatio]);

	const enviarPost = async () => {
		if (
			!placa ||
			!modelo ||
			!fabricante ||
			!ano ||
			!idPatio ||
			!idLocalidade ||
			!codigoAruco ||
			!tipoStatus
		) {
			Alert.alert("Erro", "Preencha todos os campos obrigatórios.");
			return;
		}
		if (!user) {
			Alert.alert("Erro", "Usuário não autenticado.");
			return;
		}

		try {
			const novaMoto: Omit<Moto, "idMoto" | "arucoTags"> = {
				placa: placa.toUpperCase(),
				modelo,
				fabricante,
				ano: Number(ano),
				idPatio: Number(idPatio),
				localizacaoAtual: idLocalidade,
			};

			const respostaMoto = await cadastrarMoto(novaMoto);
			const motoCadastrada = respostaMoto.data as Moto;

			await cadastrarArucoTag({
				codigo: codigoAruco,
				idMoto: motoCadastrada.idMoto,
				status: "ATIVO",
			});

			await cadastrarRegistroStatus({
				tipoStatus,
				descricao: descricaoStatus,
				idMoto: motoCadastrada.idMoto,
				idFuncionario: user.id,
			});

			Alert.alert("Sucesso", "Moto, ArucoTag e Status registrados!");
			navigation.navigate("PesquisarMoto");

			setPlaca("");
			setModelo("");
			setFabricante("");
			setAno("");
			setIdPatio("");
			setIdLocalidade("");
			setMotoPredefinida("");
			setCodigoAruco("");
			setTipoStatus("");
			setDescricaoStatus("");
		} catch (error) {
			Alert.alert(
				"Erro",
				"Erro ao registrar moto, ArucoTag ou Status. Tente novamente."
			);
			console.error(error);
		}
	};

	return (
		<SafeAreaView
			style={[styles.container, { backgroundColor: colors.background }]}>
			<KeyboardAwareScrollView
				contentContainerStyle={{ padding: 20, flexGrow: 1 }}
				enableOnAndroid
				keyboardShouldPersistTaps="handled"
				showsVerticalScrollIndicator={false}>
				<View style={styles.viewDaMoto}>
					{imagemSelecionada ? (
						<Image
							source={imagemSelecionada}
							style={styles.imagem}
						/>
					) : (
						<View style={styles.viewMotoNull}>
							<MaterialCommunityIcons
								name="help-circle-outline"
								size={80}
								color={colors.texto}
							/>
						</View>
					)}
					<View style={styles.infoMoto}>
						<Text style={styles.infoMotoTexto}>
							Modelo: {modelo || "—"}
						</Text>
						<Text style={styles.infoMotoTexto}>
							Fabricante: {fabricante || "—"}
						</Text>
						<Text style={styles.infoMotoTexto}>
							Ano: {ano || "—"}
						</Text>
					</View>
				</View>

				<View style={styles.separador} />

				<Text style={styles.tituloSecao}>
					Selecione o Modelo da Moto
				</Text>
				<View style={styles.pickerContainer}>
					<Picker
						selectedValue={motoPredefinida}
						onValueChange={(valor) => {
							setMotoPredefinida(valor);
							const moto = modelosMotos.find(
								(m) => m.nome === valor
							);
							if (moto) {
								setModelo(moto.nome);
								setFabricante(moto.fabricante);
								setAno(moto.ano);
							} else {
								setModelo("");
								setFabricante("");
								setAno("");
							}
						}}
						style={{
							backgroundColor: colors.tabBar,
							color: colors.texto,
						}}>
						<Picker.Item label="Nenhuma" value="" />
						{modelosMotos.map((moto) => (
							<Picker.Item
								key={moto.nome}
								label={`${moto.nome} - ${moto.fabricante} - ${moto.ano}`}
								value={moto.nome}
							/>
						))}
					</Picker>
				</View>

				<Text style={styles.tituloSecao}>
					Placa <Text style={styles.obrigatorio}>*</Text>
				</Text>
				<MaskInput
					style={styles.input}
					placeholder="Ex: ABC-1234"
					autoCapitalize="characters"
					value={placa}
					onChangeText={setPlaca}
					mask={[
						/[A-Za-z]/,
						/[A-Za-z]/,
						/[A-Za-z]/,
						"-",
						/\d/,
						/\d/,
						/\d/,
						/\d/,
					]}
					placeholderTextColor={colors.texto}
				/>

				<Text style={styles.tituloSecao}>
					ID Pátio <Text style={styles.obrigatorio}>*</Text>
				</Text>
				<View style={styles.pickerContainer}>
					<Picker
						selectedValue={idPatio}
						onValueChange={setIdPatio}
						style={{
							backgroundColor: colors.tabBar,
							color: colors.texto,
						}}>
						<Picker.Item label="Selecione um pátio" value="" />
						{patios.map((p) => (
							<Picker.Item
								key={p.idPatio}
								label={`${p.idPatio} - ${p.nome} - ${p.endereco}`}
								value={p.idPatio.toString()}
							/>
						))}
					</Picker>
				</View>

				<Text style={styles.tituloSecao}>
					Localização Atual <Text style={styles.obrigatorio}>*</Text>
				</Text>
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

				<Text style={styles.tituloSecao}>
					Código da ArucoTag <Text style={styles.obrigatorio}>*</Text>
				</Text>
				<TextInput
					style={styles.input}
					placeholder="Ex: 001"
					value={codigoAruco.replace("TAG", "")}
					onChangeText={(num) => setCodigoAruco(`TAG${num}`)}
					keyboardType="numeric"
					placeholderTextColor={colors.texto}
					maxLength={3}
				/>

				<Text style={styles.tituloSecao}>
					Tipo de Status <Text style={styles.obrigatorio}>*</Text>
				</Text>
				<View style={styles.pickerContainer}>
					<Picker<TipoStatus | "">
						selectedValue={tipoStatus}
						onValueChange={(valor) =>
							setTipoStatus(valor as TipoStatus | "")
						}
						style={{
							backgroundColor: colors.tabBar,
							color: colors.texto,
						}}>
						<Picker.Item label="Selecione um status" value="" />
						<Picker.Item label="Disponível" value="DISPONIVEL" />
						<Picker.Item label="Manutenção" value="MANUTENCAO" />
						<Picker.Item label="Reservado" value="RESERVADO" />
						<Picker.Item
							label="Baixa por Boletim de Ocorrência"
							value="BAIXA_BOLETIM_OCORRENCIA"
						/>
					</Picker>
				</View>

				<Text style={styles.tituloSecao}>Descrição do Status</Text>
				<TextInput
					style={styles.input}
					placeholder="Ex: Moto disponível para uso"
					value={descricaoStatus}
					onChangeText={setDescricaoStatus}
					placeholderTextColor={colors.texto}
				/>

				<Button
					mode="contained"
					onPress={enviarPost}
					style={styles.botaoRegistrar}>
					Registrar
				</Button>
			</KeyboardAwareScrollView>
		</SafeAreaView>
	);
}