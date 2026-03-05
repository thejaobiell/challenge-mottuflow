import AsyncStorage from "@react-native-async-storage/async-storage";
import { User } from "./Tabelas";

export const AUTH_TOKEN_KEY = "@access_token";
export const REFRESH_TOKEN_KEY = "@refresh_token";
export const REFRESH_EXP_KEY = "@refresh_exp";
export const USUARIO_KEY = "@usuario_logado";

// JWT
export const setJWT = (token: string) =>
	AsyncStorage.setItem(AUTH_TOKEN_KEY, token);
export const getJWT = () => AsyncStorage.getItem(AUTH_TOKEN_KEY);
export const limparJWT = () => AsyncStorage.removeItem(AUTH_TOKEN_KEY);

// Refresh Token
export const setRefreshToken = (token: string) =>
	AsyncStorage.setItem(REFRESH_TOKEN_KEY, token);
export const getRefreshToken = () => AsyncStorage.getItem(REFRESH_TOKEN_KEY);
export const clearRefreshToken = () =>
	AsyncStorage.removeItem(REFRESH_TOKEN_KEY);

// Expiração
export const setRefreshExp = (exp: string) =>
	AsyncStorage.setItem(REFRESH_EXP_KEY, exp);
export const getRefreshExp = () => AsyncStorage.getItem(REFRESH_EXP_KEY);
export const clearRefreshExp = () => AsyncStorage.removeItem(REFRESH_EXP_KEY);

// Usuário
export const setUsuario = (user: User) =>
	AsyncStorage.setItem(USUARIO_KEY, JSON.stringify(user));
export const getUsuario = async (): Promise<User | null> => {
	const raw = await AsyncStorage.getItem(USUARIO_KEY);
	return raw ? JSON.parse(raw) : null;
};
export const clearUsuario = () => AsyncStorage.removeItem(USUARIO_KEY);

// Limpar tudo
export const clearAll = () =>
	Promise.all([
		limparJWT(),
		clearRefreshToken(),
		clearRefreshExp(),
		clearUsuario(),
	]);
