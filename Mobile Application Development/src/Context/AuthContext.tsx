import React, {
	createContext,
	useState,
	useEffect,
	ReactNode,
	useContext,
} from "react";
import {
	getJWT,
	setJWT,
	getUsuario,
	setUsuario,
	clearAll,
	setRefreshToken,
} from "../../types/AuthStorage";
import { buscarFuncionarioPorEmail, login } from "../../types/Endpoints";
import { User } from "../../types/Tabelas";
import { Alert } from "react-native";

type AuthContextData = {
	user: User | null;
	token: string | null;
	loading: boolean;
	signIn: (email: string, senha: string) => Promise<void>;
	signOut: (voluntario?: boolean) => Promise<void>;
};

export const AuthContext = createContext<AuthContextData>(
	{} as AuthContextData
);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
	const [user, setUser] = useState<User | null>(null);
	const [token, setToken] = useState<string | null>(null);
	const [loading, setLoading] = useState(true);

	useEffect(() => {
		async function carregarAsyncStorage() {
			const JWTSalvo = await getJWT();
			const UserSalvo = await getUsuario();

			if (JWTSalvo && UserSalvo) {
				setToken(JWTSalvo);
				setUser(UserSalvo);
			}
			setLoading(false);
		}

		carregarAsyncStorage();
	}, []);

	/* Função para o login
	1- Usa o endpoint 'login' para obter o JWT e o refreshToken.
	2- Salva o JWT no AsyncStorage e no estado (useState) para que o interceptor possa enviar o token nas próximas requisições.
	3- Usa o email para buscar as informações completas do usuário com o endpoint 'buscarFuncionarioPorEmail'.
	4- Salva os dados do usuário no estado (useState) e no AsyncStorage para persistência da sessão.
	5- Salva também o refreshToken no AsyncStorage. */
	const signIn = async (email: string, senha: string) => {
		try {
			const resposta = await login(email, senha);
			const { tokenAcesso: JWT, refreshToken } = resposta.data;

			setToken(JWT);
			await setJWT(JWT);
			await setRefreshToken(refreshToken);

			const dadosUsuario: User = (await buscarFuncionarioPorEmail(email))
				.data;

			setUser(dadosUsuario);
			await setUsuario(dadosUsuario);
		} catch (erro: any) {
			console.log("deu erro no Login:", erro.message);
			throw erro;
		}
	};

	/*Função para logout
		1- LIMPA o asyncStorage.
		2- coloca o setUser e setToken (que são usados universalmente no app) como nulos.
		3- CASO o logout for involuntário, ele informa que a sessão expirou.
	*/
	const signOut = async (voluntario = true) => {
		await clearAll();
		setUser(null);
		setToken(null);

		if (!voluntario) {
			Alert.alert(
				"Sessão expirada",
				"Sua sessão expirou. Faça login novamente."
			);
		}
	};

	return (
		<AuthContext.Provider value={{ user, token, loading, signIn, signOut }}>
			{children}
		</AuthContext.Provider>
	);
};

export const useAuth = () => useContext(AuthContext);