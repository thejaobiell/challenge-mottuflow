package com.sprint.MottuFlow.domain.patio;

import jakarta.validation.constraints.Min;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

public class PatioDTO {

    private Long idPatio;
    
    @NotBlank
    @Size(max = 100)
    private String nome;

    @NotBlank
    @Size(max = 200)
    private String endereco;

    @NotNull
    @Min(1)
    private int capacidadeMaxima;

    public PatioDTO() {}

    public PatioDTO(Long idPatio, String nome, String endereco, int capacidadeMaxima) {
        this.idPatio = idPatio;
        this.nome = nome;
        this.endereco = endereco;
        this.capacidadeMaxima = capacidadeMaxima;
    }

	public Long getIdPatio() {
		return idPatio;
	}

	public void setIdPatio(Long idPatio) {
		this.idPatio = idPatio;
	}

	public String getNome() {
		return nome;
	}

	public void setNome(String nome) {
		this.nome = nome;
	}

	public String getEndereco() {
		return endereco;
	}

	public void setEndereco(String endereco) {
		this.endereco = endereco;
	}

	public int getCapacidadeMaxima() {
		return capacidadeMaxima;
	}

	public void setCapacidadeMaxima(int capacidadeMaxima) {
		this.capacidadeMaxima = capacidadeMaxima;
	}
}
