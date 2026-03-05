import axios from "axios";
import { getJWT } from "./AuthStorage";

const api = axios.create({
	baseURL: "http://192.168.0.74:8080/api",
	headers: { "Content-Type": "application/json" },
});

/* INTERCEPTADOR PARA PEGAR JWT DO ASYNC
	1- Pega o JWT utilizando a função getJWT()
	2- Verifica se existe token
	3- Se houver token, adiciona no cabeçalho Authorization da requisição
	4- Retorna a configuração atualizada da requisição para o Axios
	5- Permite que todas as requisições enviem automaticamente o JWT sem precisar setar manualmente */
api.interceptors.request.use(async (config) => {
	const token = await getJWT();
	if (token && config.headers) {
		config.headers.set("Authorization", `Bearer ${token}`);
	}
	return config;
});

export default api;