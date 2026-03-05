package com.sprint.MottuFlow.domain.moto;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import java.util.List;

//metodo para exibir as motos + seus aruCos tags
@Data
@NoArgsConstructor
@AllArgsConstructor
public class MotoComTagsDTO {
	private Long idMoto;
	private String placa;
	private String modelo;
	private String fabricante;
	private int ano;
	private String localizacaoAtual;
	private Long idPatio;
	private List<ArucoTagInfo> arucoTags;
	
	@Data
	@NoArgsConstructor
	@AllArgsConstructor
	public static class ArucoTagInfo {
		private Long idTag;
		private String codigo;
		private String status;
	}
}
