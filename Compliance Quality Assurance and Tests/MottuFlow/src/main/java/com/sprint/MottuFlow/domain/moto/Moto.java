package com.sprint.MottuFlow.domain.moto;

import com.sprint.MottuFlow.domain.arucotag.ArucoTag;
import com.sprint.MottuFlow.domain.localidade.Localidade;
import com.sprint.MottuFlow.domain.patio.Patio;
import com.sprint.MottuFlow.domain.status.Status;
import jakarta.persistence.*;
import lombok.*;

import java.util.List;

@Entity
@Table(name = "moto")
@Data
@NoArgsConstructor
@AllArgsConstructor
public class Moto {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long idMoto;

    @Column(nullable = false, length = 10)
    private String placa;

    @Column(nullable = false, length = 50)
    private String modelo;

    @Column(nullable = false, length = 50)
    private String fabricante;

    @Column(nullable = false)
    private int ano;

    @ManyToOne
    @JoinColumn(name = "id_patio", nullable = false)
    private Patio patio;

    @Column(nullable = false, length = 100)
    private String localizacaoAtual;

    @OneToMany(mappedBy = "moto", cascade = CascadeType.ALL, orphanRemoval = true)
    private List<ArucoTag> arucoTags;

    @OneToMany(mappedBy = "moto", cascade = CascadeType.ALL, orphanRemoval = true)
    private List<Status> estados;

    @OneToMany(mappedBy = "moto", cascade = CascadeType.ALL, orphanRemoval = true)
    private List<Localidade> localidades;

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

	public Patio getPatio() {
		return patio;
	}

	public void setPatio(Patio patio) {
		this.patio = patio;
	}

	public String getLocalizacaoAtual() {
		return localizacaoAtual;
	}

	public void setLocalizacaoAtual(String localizacaoAtual) {
		this.localizacaoAtual = localizacaoAtual;
	}

	public List<ArucoTag> getArucoTags() {
		return arucoTags;
	}

	public void setArucoTags(List<ArucoTag> arucoTags) {
		this.arucoTags = arucoTags;
	}

	public List<Status> getEstados() {
		return estados;
	}

	public void setEstados( List<Status> status ) {
		this.estados = estados;
	}

	public List<Localidade> getLocalidades() {
		return localidades;
	}

	public void setLocalidades(List<Localidade> localidades) {
		this.localidades = localidades;
	}
}