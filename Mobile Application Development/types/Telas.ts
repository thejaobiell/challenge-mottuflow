import { Moto } from "./Tabelas";

export type Telas = {
	Login: undefined;
	BottomTabs: undefined;
	Dashboard: undefined;
	PesquisarMoto: undefined;
	CadastroMoto: undefined;
	Perfil: undefined;
	VerCameras: undefined;
	MenuEditar: undefined;
	EditarMoto: { moto: Moto };
	DeletarMoto: { moto: Moto };
	AlterarSenha: undefined;
	Configuracoes: undefined;
};
