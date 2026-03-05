package com.sprint.MottuFlow.domain.localidade;

import jakarta.validation.constraints.*;
import java.time.LocalDateTime;

public class LocalidadeDTO {

    private Long idLocalidade;

    @NotNull
    private LocalDateTime dataHora;

    @NotNull
    private Long idMoto;

    @NotNull
    private Long idPatio;

    @NotBlank
    @Size(max = 100)
    private String pontoReferencia;

    @NotNull
    private Long idCamera;

    public LocalidadeDTO() {}

    public LocalidadeDTO(Long idLocalidade, LocalDateTime dataHora, Long idMoto, Long idPatio, String pontoReferencia, Long idCamera) {
        this.idLocalidade = idLocalidade;
        this.dataHora = dataHora;
        this.idMoto = idMoto;
        this.idPatio = idPatio;
        this.pontoReferencia = pontoReferencia;
        this.idCamera = idCamera;
    }

	public Long getIdLocalidade() {
		return idLocalidade;
	}

	public void setIdLocalidade(Long idLocalidade) {
		this.idLocalidade = idLocalidade;
	}

	public LocalDateTime getDataHora() {
		return dataHora;
	}

	public void setDataHora(LocalDateTime dataHora) {
		this.dataHora = dataHora;
	}

	public Long getIdMoto() {
		return idMoto;
	}

	public void setIdMoto(Long idMoto) {
		this.idMoto = idMoto;
	}

	public Long getIdPatio() {
		return idPatio;
	}

	public void setIdPatio(Long idPatio) {
		this.idPatio = idPatio;
	}

	public String getPontoReferencia() {
		return pontoReferencia;
	}

	public void setPontoReferencia(String pontoReferencia) {
		this.pontoReferencia = pontoReferencia;
	}

	public Long getIdCamera() {
		return idCamera;
	}

	public void setIdCamera(Long idCamera) {
		this.idCamera = idCamera;
	}
}
