package com.sprint.MottuFlow.controller.rest;

import com.sprint.MottuFlow.domain.funcionario.AlterarSenhaDTO;
import com.sprint.MottuFlow.domain.funcionario.FuncionarioDTO;
import com.sprint.MottuFlow.domain.funcionario.Funcionario;
import com.sprint.MottuFlow.domain.funcionario.FuncionarioService;

import jakarta.validation.Valid;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.stream.Collectors;

@RestController
@RequestMapping( "/api/funcionario" )
public class FuncionarioRestController {
	
	private final FuncionarioService fS;
	
	public FuncionarioRestController( FuncionarioService fS ) {
		this.fS = fS;
	}
	
	private FuncionarioDTO paraDTO( Funcionario funcionario ) {
		return new FuncionarioDTO( funcionario.getId_funcionario(), funcionario.getNome(), funcionario.getCpf(), funcionario.getCargo(), funcionario.getTelefone(), funcionario.getEmail(), funcionario.getSenha() );
	}
	
	private FuncionarioDTO paraDTOSemSenha( Funcionario funcionario ) {
		return new FuncionarioDTO( funcionario.getId_funcionario(), funcionario.getNome(), funcionario.getCpf(), funcionario.getCargo(), funcionario.getTelefone(), funcionario.getEmail(), null );
	}
	
	private Funcionario paraEntity( FuncionarioDTO dto ) {
		Funcionario funcionario = new Funcionario();
		funcionario.setId_funcionario( dto.getId() );
		funcionario.setNome( dto.getNome() );
		funcionario.setCpf( dto.getCpf() );
		funcionario.setCargo( dto.getCargo() );
		funcionario.setTelefone( dto.getTelefone() );
		funcionario.setEmail( dto.getEmail() );
		funcionario.setSenha( dto.getSenha() );
		return funcionario;
	}
	
	@GetMapping( "/listar" )
	public List<FuncionarioDTO> listarRest() {
		return fS.listarFuncionarios().stream().map( this::paraDTO ).collect( Collectors.toList() );
	}
	
	@GetMapping( "/buscar-por-id/{id}" )
	public ResponseEntity<FuncionarioDTO> buscarPorIdRest( @PathVariable Long id ) {
		Funcionario funcionario = fS.buscarFuncionarioPorId( id );
		return ResponseEntity.ok( paraDTO( funcionario ) );
	}
	
	@GetMapping( "/buscar-por-cpf/{cpf}" )
	public ResponseEntity<FuncionarioDTO> buscarPorCpfRest( @PathVariable String cpf ) {
		Funcionario funcionario = fS.buscarFuncionarioPorCPF( cpf );
		if ( funcionario == null ) {
			return ResponseEntity.notFound().build();
		}
		return ResponseEntity.ok( paraDTO( funcionario ) );
	}
	
	@GetMapping( "/buscar-por-email/{email}" )
	public ResponseEntity<FuncionarioDTO> buscarPorEmail( @PathVariable String email ) {
		Funcionario funcionario = fS.buscarFuncionarioPorEmail( email );
		if ( funcionario == null ) {
			return ResponseEntity.notFound().build();
		}
		return ResponseEntity.ok( paraDTOSemSenha( funcionario ) );
	}
	
	@PostMapping( "/cadastrar" )
	public ResponseEntity<FuncionarioDTO> cadastrarRest( @RequestBody @Valid FuncionarioDTO dto ) {
		Funcionario funcionario = paraEntity( dto );
		Funcionario salvo = fS.cadastrarFuncionario( funcionario );
		return ResponseEntity.ok( paraDTO( salvo ) );
	}
	
	@PutMapping( "/editar/{id}" )
	public ResponseEntity<FuncionarioDTO> editarRest( @PathVariable Long id, @RequestBody FuncionarioDTO dto ) {
		Funcionario funcionario = paraEntity( dto );
		Funcionario atualizado = fS.editarFuncionario( id, funcionario );
		return ResponseEntity.ok( paraDTO( atualizado ) );
	}
	
	@PatchMapping("/alterar-senha")
	public ResponseEntity<Void> alterarSenha(@RequestBody @Valid AlterarSenhaDTO dto) {
		fS.alterarSenha(dto.email(), dto.senhaAtual(), dto.novaSenha());
		return ResponseEntity.noContent().build();
	}
	
	@DeleteMapping( "/deletar/{id}" )
	public ResponseEntity<Void> deletarRest( @PathVariable Long id ) {
		fS.deletarFuncionario( id );
		return ResponseEntity.noContent().build();
	}
}
