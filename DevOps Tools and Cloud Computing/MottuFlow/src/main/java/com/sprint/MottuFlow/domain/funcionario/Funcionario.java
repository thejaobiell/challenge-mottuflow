package com.sprint.MottuFlow.domain.funcionario;

import com.sprint.MottuFlow.domain.status.Status;
import jakarta.persistence.*;
import lombok.*;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import java.util.UUID;

import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.core.userdetails.UserDetails;

@Entity
@Table(name = "funcionario")
@Data
@NoArgsConstructor
@AllArgsConstructor
public class Funcionario implements UserDetails{

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id_funcionario;

    @Column(nullable = false, length = 100)
    private String nome;

    @Column(nullable = false, length = 14)
    private String cpf;
	
	@Enumerated(EnumType.STRING)
	@Column(nullable = false, length = 50)
	private Cargo cargo;

    @Column(nullable = false, length = 20)
    private String telefone;

    @Column(nullable = false, length = 50)
    private String email;

    @Column(nullable = false)
    private String senha;
	
	@Column(name = "refresh_token", nullable = true)
	private String refreshToken;
	
	@Column(name = "expiracao_refresh_token", nullable = true)
	private LocalDateTime expiracaoRefreshToken;

    @OneToMany(mappedBy = "funcionario", cascade = CascadeType.ALL, orphanRemoval = true)
    private List<Status> estadoList;
	
	public Long getId_funcionario() {
		return id_funcionario;
	}
	
	public void setId_funcionario( Long id_funcionario ) {
		this.id_funcionario = id_funcionario;
	}
	
	public String getNome() {
		return nome;
	}
	
	public void setNome( String nome ) {
		this.nome = nome;
	}
	
	public String getCpf() {
		return cpf;
	}
	
	public void setCpf( String cpf ) {
		this.cpf = cpf;
	}
	
	public Cargo getCargo() {
		return cargo;
	}
	
	public void setCargo( Cargo cargo ) {
		this.cargo = cargo;
	}
	
	public String getTelefone() {
		return telefone;
	}
	
	public void setTelefone( String telefone ) {
		this.telefone = telefone;
	}
	
	public String getEmail() {
		return email;
	}
	
	public void setEmail( String email ) {
		this.email = email;
	}
	
	public String getSenha() {
		return senha;
	}
	
	public void setSenha( String senha ) {
		this.senha = senha;
	}
	
	public String getRefreshToken() {
		return refreshToken;
	}
	
	public void setRefreshToken( String refreshToken ) {
		this.refreshToken = refreshToken;
	}
	
	public LocalDateTime getExpiracaoRefreshToken() {
		return expiracaoRefreshToken;
	}
	
	public void setExpiracaoRefreshToken( LocalDateTime expiracaoRefreshToken ) {
		this.expiracaoRefreshToken = expiracaoRefreshToken;
	}
	
	@Override
	public Collection<? extends GrantedAuthority> getAuthorities() {
		return List.of(new SimpleGrantedAuthority("ROLE_" + this.cargo.name()));
	}
	
	@Override
	public String getPassword() {
		return senha;
	}

	@Override
	public String getUsername() {
		return email;
	}
	
	public String novoRefreshToken() {
		this.refreshToken = UUID.randomUUID().toString();
		this.expiracaoRefreshToken = LocalDateTime.now().plusMinutes( 120 );
		return refreshToken;
	}
	
	public boolean refreshTokenExpirado() {
		return expiracaoRefreshToken.isBefore( LocalDateTime.now() );
	}
}