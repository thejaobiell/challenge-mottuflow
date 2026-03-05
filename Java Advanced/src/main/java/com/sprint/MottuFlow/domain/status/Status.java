package com.sprint.MottuFlow.domain.status;

import com.sprint.MottuFlow.domain.funcionario.Funcionario;
import com.sprint.MottuFlow.domain.moto.Moto;
import jakarta.persistence.*;
import lombok.*;

import java.time.LocalDateTime;

@Entity
@Table( name = "registro_status" )
@Data
@NoArgsConstructor
@AllArgsConstructor
public class Status {
	
	@Id
	@GeneratedValue( strategy = GenerationType.IDENTITY )
	private Long idStatus;
	
	@Enumerated(EnumType.STRING)
	@Column(nullable = false, length = 50)
	private TipoStatus tipoStatus;
	
	@Column( length = 255 )
	private String descricao;
	
	@Column( columnDefinition = "TIMESTAMP DEFAULT CURRENT_TIMESTAMP" )
	private LocalDateTime dataStatus;
	
	@ManyToOne
	@JoinColumn( name = "id_moto", nullable = false )
	private Moto moto;
	
	@ManyToOne
	@JoinColumn( name = "id_funcionario", nullable = false )
	private Funcionario funcionario;
	
	@PrePersist
	public void prePersist() {
		if ( dataStatus == null ) {
			dataStatus = LocalDateTime.now();
		}
	}
	
	public Long getIdStatus() {
		return idStatus;
	}
	
	public void setIdStatus( Long idStatus ) {
		this.idStatus = idStatus;
	}
	
	public Moto getMoto() {
		return moto;
	}
	
	public void setMoto( Moto moto ) {
		this.moto = moto;
	}
	
	public TipoStatus getTipoStatus() {
		return tipoStatus;
	}
	
	public void setTipoStatus( TipoStatus tipoStatus ) {
		this.tipoStatus = tipoStatus;
	}
	
	public String getDescricao() {
		return descricao;
	}
	
	public void setDescricao( String descricao ) {
		this.descricao = descricao;
	}
	
	public LocalDateTime getDataStatus() {
		return dataStatus;
	}
	
	public void setDataStatus( LocalDateTime dataStatus ) {
		this.dataStatus = dataStatus;
	}
	
	public Funcionario getFuncionario() {
		return funcionario;
	}
	
	public void setFuncionario( Funcionario funcionario ) {
		this.funcionario = funcionario;
	}
}
