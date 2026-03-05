import { Moto } from "./Tabelas";

export type MotoCardProps = {
	moto: Moto;
	onPressEdit?: () => void;
	onPressDelete?: () => void;
};
