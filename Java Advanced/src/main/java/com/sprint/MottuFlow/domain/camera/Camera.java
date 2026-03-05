package com.sprint.MottuFlow.domain.camera;

import com.sprint.MottuFlow.domain.localidade.Localidade;
import com.sprint.MottuFlow.domain.patio.Patio;
import jakarta.persistence.*;
import lombok.*;

import java.util.List;

@Entity
@Table(name = "camera")
@Data
@NoArgsConstructor
@AllArgsConstructor
public class Camera {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long idCamera;

    @Column(nullable = false, length = 20)
    private String statusOperacional;

    @Column(nullable = false, length = 255)
    private String localizacaoFisica;

    @ManyToOne
    @JoinColumn(name = "id_patio", nullable = false)
    private Patio patio;

    @OneToMany(mappedBy = "camera", cascade = CascadeType.ALL, orphanRemoval = true)
    private List<Localidade> localidades;

	public Long getIdCamera() {
		return idCamera;
	}

	public void setIdCamera(Long idCamera) {
		this.idCamera = idCamera;
	}

	public String getStatusOperacional() {
		return statusOperacional;
	}

	public void setStatusOperacional(String statusOperacional) {
		this.statusOperacional = statusOperacional;
	}

	public String getLocalizacaoFisica() {
		return localizacaoFisica;
	}

	public void setLocalizacaoFisica(String localizacaoFisica) {
		this.localizacaoFisica = localizacaoFisica;
	}

	public Patio getPatio() {
		return patio;
	}

	public void setPatio(Patio patio) {
		this.patio = patio;
	}

	public List<Localidade> getLocalidades() {
		return localidades;
	}

	public void setLocalidades(List<Localidade> localidades) {
		this.localidades = localidades;
	}
}
