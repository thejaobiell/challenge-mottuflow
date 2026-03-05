import api from "./Api";
import { ArucoTag, Patio, RegistroStatus, Moto, Localidade } from "./Tabelas";

// ------------------- AUTH -------------------
export const login = (email: string, senha: string) =>
	api.post("/login", { email, senha });

export const verificarJWT = (tokenAcesso: string) =>
	api.post("/verificar-jwt", { tokenAcesso });

export const atualizarToken = (refreshToken: string) =>
	api.post("/atualizar-token", { refreshToken });

// ------------------- FUNCIONÁRIO -------------------
export const buscarFuncionarioPorEmail = (email: string) =>
	api.get(`/funcionario/buscar-por-email/${encodeURIComponent(email)}`);

export const alterarSenha = (
	email: string,
	senhaAtual: string,
	novaSenha: string
) => api.patch("/funcionario/alterar-senha", { email, senhaAtual, novaSenha });

// ------------------- MOTOS -------------------
export const listarMotos = () => api.get<Moto[]>("/motos/listar");

export const listarMotosTags = () => api.get("/motos/motos-com-tags");

export const cadastrarMoto = (moto: Omit<Moto, "idMoto" | "arucoTags">) =>
	api.post("/motos/cadastrar", moto);

export const buscarMotoPorId = (idMoto: number) =>
	api.get<Moto>(`/motos/buscar-por-id/${idMoto}`);

export const buscarMotosPorPlaca = (placa: string) =>
	api.get<Moto>(`/motos/buscar-por-placa/${placa}`);

export const buscarMotosPorModelo = (modelo: string) =>
	api.get<Moto>(`/motos/buscar-por-modelo/${modelo}`);

export const buscarMotosPorPatio = (idPatio: number) =>
	api.get<Moto[]>(`/motos/buscar-por-patio/${idPatio}`);

export const editarMoto = (idMoto: number, moto: Partial<Moto>) =>
	api.put(`/motos/editar/${idMoto}`, moto);

export const deletarMotoPorId = async (idMoto: number) => {
	api.delete(`/motos/deletar/${idMoto}`);
};

// ------------------- PATIOS -------------------
export const listarPatios = () => api.get<Patio[]>("/patios/listar");

// ------------------- LOCALIDADES -------------------
export const localidadesPorPeriodo = (dataInicio: string, dataFim: string) =>
	api.get<Localidade[]>("/localidades/buscar-por-periodo", {
		params: { dataInicio, dataFim },
	});

export const localidadesBuscarPorPatio = (idPatio: number) =>
	api.get<Localidade[]>(`/localidades/buscar-por-patio/${idPatio}`);

export const editarLocalidade = (
	idLocalidade: number,
	localidade: Partial<Localidade>
) => api.put(`/localidades/editar/${idLocalidade}`, localidade);

// ------------------- ARUCO TAGS -------------------
export const cadastrarArucoTag = (arucoTag: Omit<ArucoTag, "idTag">) =>
	api.post("/aruco-tags/cadastrar", arucoTag);

export const listarArucoTags = () => api.get<ArucoTag[]>("/aruco-tags/listar");

export const buscarArucoPorCodigo = (codigo: string) =>
	api.get<ArucoTag>(`/aruco-tags/buscar-por-codigo/${codigo}`);

export const editarArucoTag = (idTag: number, arucoTag: Partial<ArucoTag>) =>
	api.put(`/aruco-tags/editar/${idTag}`, arucoTag);

// ------------------- REGISTRO STATUS -------------------
export const cadastrarRegistroStatus = (registroStatus: RegistroStatus) =>
	api.post("/status/cadastrar", registroStatus);

export const editarRegistroStatus = (
	idStatus: number,
	registroStatus: Partial<RegistroStatus>
) => api.put(`/status/editar/${idStatus}`, registroStatus);

export const listarRegistrosStatusPorPeriodo = (
	dataInicio: string,
	dataFim: string
) =>
	api.get<RegistroStatus[]>("/status/buscar-por-periodo", {
		params: { inicio: dataInicio, fim: dataFim },
	});

export const buscarRegistroStatusPorMoto = (idMoto: number) =>
	api.get<RegistroStatus>(`/status/buscar-por-moto/${idMoto}`);
