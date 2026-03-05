package com.sprint.MottuFlow.domain.status;

import jakarta.validation.constraints.*;
import java.time.LocalDateTime;

public class StatusDTO {
	
	private Long idStatus;
	
	@NotNull
	private Long idMoto;
	
	@NotNull
	private TipoStatus tipoStatus;
	
	@Size(max = 255)
	private String descricao;
	
	private LocalDateTime dataStatus;
	
	@NotNull
	private Long idFuncionario;
	
	public StatusDTO() {}
	
	public StatusDTO(Long idStatus, Long idMoto, TipoStatus tipoStatus, String descricao, LocalDateTime dataStatus, Long idFuncionario) {
		this.idStatus = idStatus;
		this.idMoto = idMoto;
		this.tipoStatus = tipoStatus;
		this.descricao = descricao;
		this.dataStatus = dataStatus;
		this.idFuncionario = idFuncionario;
	}
	
	public Long getIdStatus() {
		return idStatus;
	}
	
	public void setIdStatus(Long idStatus) {
		this.idStatus = idStatus;
	}
	
	public Long getIdMoto() {
		return idMoto;
	}
	
	public void setIdMoto(Long idMoto) {
		this.idMoto = idMoto;
	}
	
	public TipoStatus getTipoStatus() {
		return tipoStatus;
	}
	
	public void setTipoStatus(TipoStatus tipoStatus) {
		this.tipoStatus = tipoStatus;
	}
	
	public String getDescricao() {
		return descricao;
	}
	
	public void setDescricao(String descricao) {
		this.descricao = descricao;
	}
	
	public LocalDateTime getDataStatus() {
		return dataStatus;
	}
	
	public void setDataStatus(LocalDateTime dataStatus) {
		this.dataStatus = dataStatus;
	}
	
	public Long getIdFuncionario() {
		return idFuncionario;
	}
	
	public void setIdFuncionario(Long idFuncionario) {
		this.idFuncionario = idFuncionario;
	}
}