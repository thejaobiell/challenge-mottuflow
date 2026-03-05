import React, { useEffect, useState } from "react";
import { View, Text, ScrollView, ActivityIndicator } from "react-native";
import { useAuth } from "../../Context/AuthContext";
import {
	localidadesPorPeriodo,
	listarPatios,
	listarMotos,
	listarArucoTags,
	listarRegistrosStatusPorPeriodo,
} from "../../../types/Endpoints";
import { dashboardStyles } from "./styles";

export default function Dashboard() {
	const styles = dashboardStyles();
	const { user, loading: authLoading } = useAuth();
	const [loading, setLoading] = useState(true);

	const [movHoje, setMovHoje] = useState(0);
	const [movOntem, setMovOntem] = useState(0);
	const [mediaSemana, setMediaSemana] = useState(0);

	const [patiosAtivos, setPatiosAtivos] = useState<any[]>([]);
	const [totalMotos, setTotalMotos] = useState(0);
	const [totalTags, setTotalTags] = useState(0);

	const formatarData = (data: Date) => {
		const ano = data.getFullYear();
		const mes = String(data.getMonth() + 1).padStart(2, "0");
		const dia = String(data.getDate()).padStart(2, "0");
		const hora = String(data.getHours()).padStart(2, "0");
		const minuto = String(data.getMinutes()).padStart(2, "0");
		const segundo = String(data.getSeconds()).padStart(2, "0");
		return `${ano}-${mes}-${dia}T${hora}:${minuto}:${segundo}`;
	};

	useEffect(() => {
		const carregarKPIs = async () => {
			try {
				const hoje = new Date();
				const inicioHoje = new Date(hoje);
				inicioHoje.setHours(0, 0, 0, 0);
				const fimHoje = new Date(hoje);
				fimHoje.setHours(23, 59, 59, 0);

				const ontem = new Date(inicioHoje);
				ontem.setDate(ontem.getDate() - 1);
				const inicioOntem = new Date(ontem);
				const fimOntem = new Date(ontem);
				fimOntem.setHours(23, 59, 59, 0);

				// Movimentações Hoje
				const responseHoje = await localidadesPorPeriodo(
					formatarData(inicioHoje),
					formatarData(fimHoje)
				);
				const responseStatusHoje =
					await listarRegistrosStatusPorPeriodo?.(
						formatarData(inicioHoje),
						formatarData(fimHoje)
					);

				setMovHoje(
					(Array.isArray(responseHoje.data)
						? responseHoje.data.length
						: 0) +
						(Array.isArray(responseStatusHoje?.data)
							? responseStatusHoje.data.length
							: 0)
				);

				// Movimentações Ontem
				const responseOntem = await localidadesPorPeriodo(
					formatarData(inicioOntem),
					formatarData(fimOntem)
				);
				const responseStatusOntem =
					await listarRegistrosStatusPorPeriodo?.(
						formatarData(inicioOntem),
						formatarData(fimOntem)
					);

				setMovOntem(
					(Array.isArray(responseOntem.data)
						? responseOntem.data.length
						: 0) +
						(Array.isArray(responseStatusOntem?.data)
							? responseStatusOntem.data.length
							: 0)
				);

				// Média da semana (últimos 7 dias)
				let soma = 0;
				for (let i = 0; i < 7; i++) {
					const dia = new Date();
					dia.setDate(dia.getDate() - i);
					const inicioDia = new Date(dia);
					inicioDia.setHours(0, 0, 0, 0);
					const fimDia = new Date(dia);
					fimDia.setHours(23, 59, 59, 0);

					const resp = await localidadesPorPeriodo(
						formatarData(inicioDia),
						formatarData(fimDia)
					);
					const respStatus = await listarRegistrosStatusPorPeriodo?.(
						formatarData(inicioDia),
						formatarData(fimDia)
					);

					soma +=
						(Array.isArray(resp.data) ? resp.data.length : 0) +
						(Array.isArray(respStatus?.data)
							? respStatus.data.length
							: 0);
				}
				setMediaSemana(Math.round(soma / 7));

				// Patios ativos
				const patiosResp = await listarPatios();
				setPatiosAtivos(
					Array.isArray(patiosResp.data) ? patiosResp.data : []
				);

				// Total de Motos
				const motosResp = await listarMotos();
				setTotalMotos(
					Array.isArray(motosResp.data) ? motosResp.data.length : 0
				);

				// Total de Tags
				const tagsResp = await listarArucoTags();
				setTotalTags(
					Array.isArray(tagsResp.data) ? tagsResp.data.length : 0
				);
			} catch (err) {
				console.error("Erro ao carregar KPIs:", err);
			} finally {
				setLoading(false);
			}
		};

		carregarKPIs();
	}, []);

	if (loading || authLoading) {
		return (
			<View style={styles.loadingContainer}>
				<ActivityIndicator size="large" />
				<Text style={styles.loadingTexto}>Carregando dados...</Text>
			</View>
		);
	}

	return (
		<View
			style={{
				flex: 1,
				backgroundColor: styles.container.backgroundColor,
			}}>
			<ScrollView contentContainerStyle={{ padding: 16 }}>
				<Text style={styles.textoBemvindo}>
					Seja bem-vindo{user ? `, ${user.nome}` : ""}!
				</Text>

				<View
					style={{
						flexDirection: "row",
						flexWrap: "wrap",
						justifyContent: "space-between",
					}}>
					<View style={styles.estatisticaCard}>
						<Text style={styles.estatisticaNumber}>{movHoje}</Text>
						<Text style={styles.estatisticaLabel}>
							Movimentações Hoje
						</Text>
					</View>

					<View style={styles.estatisticaCard}>
						<Text style={styles.estatisticaNumber}>{movOntem}</Text>
						<Text style={styles.estatisticaLabel}>
							Movimentações Ontem
						</Text>
					</View>

					<View style={styles.estatisticaCard}>
						<Text style={styles.estatisticaNumber}>
							{mediaSemana}
						</Text>
						<Text style={styles.estatisticaLabel}>
							Média Semanal
						</Text>
					</View>

					<View style={styles.estatisticaCard}>
						<Text style={styles.estatisticaNumber}>
							{patiosAtivos.length}
						</Text>
						<Text style={styles.estatisticaLabel}>
							Patios Ativos
						</Text>
					</View>

					<View style={styles.estatisticaCard}>
						<Text style={styles.estatisticaNumber}>
							{totalMotos}
						</Text>
						<Text style={styles.estatisticaLabel}>
							Motos Cadastradas
						</Text>
					</View>

					<View style={styles.estatisticaCard}>
						<Text style={styles.estatisticaNumber}>
							{totalTags}
						</Text>
						<Text style={styles.estatisticaLabel}>Tags Ativas</Text>
					</View>
				</View>
			</ScrollView>
		</View>
	);
}
