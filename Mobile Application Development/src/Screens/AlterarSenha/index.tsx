import { useState } from "react";
import {
	View,
	Text,
	TextInput,
	Button,
	StyleSheet,
	Alert,
	TouchableOpacity,
} from "react-native";
import { NativeStackScreenProps } from "@react-navigation/native-stack";
import { Telas } from "../../../types/Telas";
import { alterarSenha } from "../../../types/Endpoints";
import { useAuth } from "../../Context/AuthContext";
import { Feather } from "@expo/vector-icons";
import { useTheme } from "../../Context/ThemeContext";
import { alterarsenhaStyles } from "./styles";

type Props = NativeStackScreenProps<Telas, "AlterarSenha">;

export default function AlterarSenha({ navigation }: Props) {
	const [senhaAtual, setSenhaAtual] = useState("");
	const [novaSenha, setNovaSenha] = useState("");
	const [confirmar, setConfirmar] = useState("");
	const [showSenhaAtual, setShowSenhaAtual] = useState(false);
	const [showNovaSenha, setShowNovaSenha] = useState(false);
	const [showConfirmar, setShowConfirmar] = useState(false);
	const { colors } = useTheme();
	const styles = alterarsenhaStyles(colors);

	const { user } = useAuth();

	const handleAlterar = async () => {
		if (!user || !user.email) {
			Alert.alert(
				"Erro",
				"Usuário não autenticado. Faça login novamente."
			);
			return;
		}

		if (novaSenha !== confirmar) {
			Alert.alert("Erro", "A nova senha e a confirmação não coincidem");
			return;
		}

		try {
			await alterarSenha(user.email, senhaAtual, novaSenha);
			Alert.alert("Sucesso", "Senha alterada com sucesso!");
			navigation.goBack();
		} catch (err: any) {
			console.error(err);
			Alert.alert(
				"Erro",
				err.response?.data?.message || "Falha ao alterar senha"
			);
		}
	};

	return (
		<View style={styles.container}>
			{/* Senha Atual */}
			<Text style={styles.label}>Senha Atual</Text>
			<View style={styles.inputContainer}>
				<TextInput
					secureTextEntry={!showSenhaAtual}
					style={styles.input}
					value={senhaAtual}
					onChangeText={setSenhaAtual}
				/>
				<TouchableOpacity
					onPress={() => setShowSenhaAtual(!showSenhaAtual)}
					style={styles.botaoEye}>
					<Feather
						name={showSenhaAtual ? "eye" : "eye-off"}
						size={20}
						color="#666"
					/>
				</TouchableOpacity>
			</View>

			{/* Nova Senha */}
			<Text style={styles.label}>Nova Senha</Text>
			<View style={styles.inputContainer}>
				<TextInput
					secureTextEntry={!showNovaSenha}
					style={styles.input}
					value={novaSenha}
					onChangeText={setNovaSenha}
				/>
				<TouchableOpacity
					onPress={() => setShowNovaSenha(!showNovaSenha)}
					style={styles.botaoEye}>
					<Feather
						name={showNovaSenha ? "eye" : "eye-off"}
						size={20}
						color="#666"
					/>
				</TouchableOpacity>
			</View>

			{/* Confirmar Nova Senha */}
			<Text style={styles.label}>Confirmar Nova Senha</Text>
			<View style={styles.inputContainer}>
				<TextInput
					secureTextEntry={!showConfirmar}
					style={styles.input}
					value={confirmar}
					onChangeText={setConfirmar}
				/>
				<TouchableOpacity
					onPress={() => setShowConfirmar(!showConfirmar)}
					style={styles.botaoEye}>
					<Feather
						name={showConfirmar ? "eye" : "eye-off"}
						size={20}
						color="#666"
					/>
				</TouchableOpacity>
			</View>

			<Button title="Alterar Senha" onPress={handleAlterar} />
		</View>
	);
}
