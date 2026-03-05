package com.sprint.MottuFlow.domain.arucotag;

import com.sprint.MottuFlow.domain.moto.Moto;
import jakarta.persistence.*;
import lombok.*;

@Entity
@Table(name = "aruco_tag")
@Data
@NoArgsConstructor
@AllArgsConstructor
public class ArucoTag {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long idTag;

    @Column(nullable = false, length = 50)
    private String codigo;

    @Column(nullable = false, length = 20)
    private String status;

    @ManyToOne
    @JoinColumn(name = "id_moto", nullable = false)
    private Moto moto;

	public Long getIdTag() {
		return idTag;
	}

	public void setIdTag(Long idTag) {
		this.idTag = idTag;
	}

	public String getCodigo() {
		return codigo;
	}

	public void setCodigo(String codigo) {
		this.codigo = codigo;
	}

	public Moto getMoto() {
		return moto;
	}

	public void setMoto(Moto moto) {
		this.moto = moto;
	}

	public String getStatus() {
		return status;
	}

	public void setStatus(String status) {
		this.status = status;
	}
}
