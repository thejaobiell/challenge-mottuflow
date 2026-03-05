package com.sprint.MottuFlow.domain.moto;

import jakarta.validation.constraints.Min;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

public class MotoDTO {

	private Long idMoto;

    @NotBlank
    @Size(max = 10)
    private String placa;

    @NotBlank
    @Size(max = 50)
    private String modelo;

    @NotBlank
    @Size(max = 50)
    private String fabricante;

    @Min(1900)
    private int ano;

    @NotNull
    private Long idPatio;

    @NotBlank
    @Size(max = 100)
    private String localizacaoAtual;

    public MotoDTO() {}

    public MotoDTO(Long idMoto, String placa, String modelo, String fabricante, int ano, Long idPatio, String localizacaoAtual) {
        this.idMoto = idMoto;
        this.placa = placa;
        this.modelo = modelo;
        this.fabricante = fabricante;
        this.ano = ano;
        this.idPatio = idPatio;
        this.localizacaoAtual = localizacaoAtual;
    }

	public Long getIdMoto() {
		return idMoto;
	}

	public void setIdMoto(Long idMoto) {
		this.idMoto = idMoto;
	}

	public String getPlaca() {
		return placa;
	}

	public void setPlaca(String placa) {
		this.placa = placa;
	}

	public String getModelo() {
		return modelo;
	}

	public void setModelo(String modelo) {
		this.modelo = modelo;
	}

	public String getFabricante() {
		return fabricante;
	}

	public void setFabricante(String fabricante) {
		this.fabricante = fabricante;
	}

	public int getAno() {
		return ano;
	}

	public void setAno(int ano) {
		this.ano = ano;
	}

	public Long getIdPatio() {
		return idPatio;
	}

	public void setIdPatio(Long idPatio) {
		this.idPatio = idPatio;
	}

	public String getLocalizacaoAtual() {
		return localizacaoAtual;
	}

	public void setLocalizacaoAtual(String localizacaoAtual) {
		this.localizacaoAtual = localizacaoAtual;
	}
}
