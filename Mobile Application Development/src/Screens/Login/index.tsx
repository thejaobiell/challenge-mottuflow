import React, { useEffect, useState } from "react";
import {
	View,
	Text,
	Image,
	TextInput,
	TouchableOpacity,
	Alert,
	StatusBar,
	ActivityIndicator,
} from "react-native";
import { KeyboardAwareScrollView } from "react-native-keyboard-aware-scroll-view";
import { SafeAreaView } from "react-native-safe-area-context";
import { NativeStackScreenProps } from "@react-navigation/native-stack";
import { Telas } from "../../../types/Telas";
import { loginStyles as styles } from "./styles";
import { Feather } from "@expo/vector-icons";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { useAuth } from "../../Context/AuthContext";

type Props = NativeStackScreenProps<Telas, "Login">;

export default function Login({ navigation }: Props) {
	const { signIn } = useAuth();
	const [showPassword, setShowPassword] = useState(false);
	const [email, setEmail] = useState("");
	const [senha, setSenha] = useState("");
	const [loading, setLoading] = useState(false);

	const handleLogin = async () => {
		if (!email || !senha) {
			Alert.alert("Campos vazios", "Preencha todos os campos.");
			return;
		}
		setLoading(true);
		try {
			await signIn(email, senha);
		} catch (erro: any) {
			console.error(erro);
			if (
				erro.response?.status === 401 ||
				erro.response?.status === 500
			) {
				Alert.alert("Erro", "Email ou senha inválidos");
			} else {
				Alert.alert("Erro", "Erro ao conectar com o servidor");
			}
		} finally {
			setLoading(false);
		}
	};

	useEffect(() => {
		const verificarUsuarioLogado = async () => {
			try {
				const usuarioSalvo = await AsyncStorage.getItem(
					"@usuario_logado"
				);
				if (usuarioSalvo) {
					const user = JSON.parse(usuarioSalvo);
					navigation.replace("BottomTabs");
				}
			} catch (error) {
				console.log("Erro ao verificar usuário logado:", error);
			}
		};
		verificarUsuarioLogado();
	}, []);

	return (
		<SafeAreaView style={styles.container} edges={["top", "bottom"]}>
			<StatusBar barStyle="dark-content" />
			<KeyboardAwareScrollView
				contentContainerStyle={styles.content}
				enableOnAndroid={true}
				keyboardShouldPersistTaps="handled">
				<View style={styles.cabecalho}>
					<View style={styles.logoContainer}>
						<Image
							source={require("../../../images/logo.png")}
							style={styles.logo}
						/>
						<Text style={styles.bemvindo}>Bem-vindo</Text>
						<Text style={styles.subtitulo}>Entre na sua conta</Text>
					</View>
				</View>

				<View style={styles.formContainer}>
					<View style={styles.card}>
						<View style={styles.inputGroup}>
							<Text style={styles.inputLabel}>Email</Text>
							<View style={styles.inputContainer}>
								<Feather
									name="mail"
									size={20}
									style={styles.inputIcon}
								/>
								<TextInput
									placeholder="seu@email.com"
									placeholderTextColor="#999"
									style={styles.textInput}
									value={email}
									onChangeText={setEmail}
									keyboardType="email-address"
									autoCapitalize="none"
									selectionHandleColor={"#1A5D48"}
								/>
							</View>
						</View>

						<View style={styles.inputGroup}>
							<Text style={styles.inputLabel}>Senha</Text>
							<View style={styles.inputContainer}>
								<Feather
									name="lock"
									size={20}
									style={styles.inputIcon}
								/>
								<TextInput
									placeholder="••••••••"
									placeholderTextColor="#999"
									secureTextEntry={!showPassword}
									style={[styles.textInput, { flex: 1 }]}
									value={senha}
									onChangeText={setSenha}
									selectionHandleColor={"#1A5D48"}
								/>
								<TouchableOpacity
									onPress={() =>
										setShowPassword(!showPassword)
									}
									style={styles.eyeButton}>
									<Feather
										name={showPassword ? "eye" : "eye-off"}
										size={20}
										color="#666"
									/>
								</TouchableOpacity>
							</View>
						</View>

						<TouchableOpacity
							style={[
								styles.loginButton,
								loading && styles.loginButtonDisabled,
							]}
							onPress={handleLogin}
							disabled={loading}
							activeOpacity={0.8}>
							{loading ? (
								<ActivityIndicator size="small" color="#FFF" />
							) : (
								<>
									<Text style={styles.loginButtonText}>
										Entrar
									</Text>
									<Feather
										name="arrow-right"
										size={20}
										color="#FFF"
									/>
								</>
							)}
						</TouchableOpacity>
					</View>
				</View>
			</KeyboardAwareScrollView>
		</SafeAreaView>
	);
}