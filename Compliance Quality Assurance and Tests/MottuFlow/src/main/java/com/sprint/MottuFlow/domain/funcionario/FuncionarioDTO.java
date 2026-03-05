package com.sprint.MottuFlow.domain.funcionario;

import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

public class FuncionarioDTO {

    private Long id;
    
    @NotBlank(message = "Nome é obrigatório")
    @Size(max = 100)
    private String nome;

    @NotBlank(message = "CPF é obrigatório")
    @Size(min = 11, max = 14, message = "CPF deve ter entre 11 e 14 caracteres")
    private String cpf;
	
	@NotNull(message = "Cargo não pode ser nulo")
	private Cargo cargo;
	
	@NotBlank(message = "Telefone é obrigatório")
    @Size(max = 20)
    private String telefone;

    @NotBlank(message = "Email é obrigatório")
    @Email(message = "Isso não é um email")
    private String email;

    @NotBlank(message = "Senha é obrigatória")
    @Size(min = 6, message = "Senha deve ter no minímo 6 caracteres")
    private String senha;

    public FuncionarioDTO() {}

    public FuncionarioDTO(Long id, String nome, String cpf, Cargo cargo, String telefone, String email, String senha) {
        this.id = id;
        this.nome = nome;
        this.cpf = cpf;
        this.cargo = cargo;
        this.telefone = telefone;
        this.email = email;
        this.senha = senha;
    }

	public Long getId() {
		return id;
	}

	public void setId(Long id) {
		this.id = id;
	}

	public String getNome() {
		return nome;
	}

	public void setNome(String nome) {
		this.nome = nome;
	}

	public String getCpf() {
		return cpf;
	}

	public void setCpf(String cpf) {
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

	public void setTelefone(String telefone) {
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

	public void setSenha(String senha) {
		this.senha = senha;
	}
}
