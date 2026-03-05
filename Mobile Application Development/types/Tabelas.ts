export type User = {
	id: number;
	nome: string;
	cpf: string;
	cargo: string;
	telefone: string;
	email: string;
	senha: string;
};

export type ArucoTag = {
	idTag: number;
	codigo: string;
	status: string;
	idMoto: number;
};

export type Moto = {
	idMoto: number;
	placa: string;
	modelo: string;
	fabricante: string;
	ano: number;
	localizacaoAtual: string;
	idPatio: number;
	arucoTags: ArucoTag[];
};

export type Patio = {
	idPatio: number;
	nome: string;
	endereco: string;
	capacidade: number;
};

export type TipoStatus =
	| "DISPONIVEL"
	| "MANUTENCAO"
	| "RESERVADO"
	| "BAIXA_BOLETIM_OCORRENCIA";

export type RegistroStatus = {
	idStatus?: number;
	tipoStatus: TipoStatus;
	descricao?: string;
	idMoto: number;
	idFuncionario: number;
};

export type Localidade = {
	idLocalidade: number;
	dataHora: string;
	idMoto: number;
	idPatio: number;
	pontoReferencia: string;
	idCamera: number;
};
