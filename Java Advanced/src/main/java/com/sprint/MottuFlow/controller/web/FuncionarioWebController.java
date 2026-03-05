package com.sprint.MottuFlow.controller.web;

import com.sprint.MottuFlow.domain.funcionario.Funcionario;
import com.sprint.MottuFlow.domain.funcionario.FuncionarioService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

@Controller
@RequestMapping( "/funcionarios" )
public class FuncionarioWebController {
	
	private final FuncionarioService fS;
	
	public FuncionarioWebController( FuncionarioService fS ) {
		this.fS = fS;
	}
	
	@GetMapping( "/listar-cadastrar-delete" )
	public String listarFuncionarios( Model model ) {
		model.addAttribute( "funcionarios", fS.listarFuncionarios() );
		model.addAttribute( "novoFuncionario", new Funcionario() );
		return "funcionarios/listar-cadastrar-delete";
	}
	
	@PostMapping( "/cadastrar" )
	public String cadastrarFuncionario( @ModelAttribute( "novoFuncionario" ) Funcionario funcionario ) {
		fS.cadastrarFuncionario( funcionario );
		return "redirect:/funcionarios/listar-cadastrar-delete";
	}
	
	@GetMapping( "/editar/{id}" )
	public String editarFuncionarioForm( @PathVariable Long id, Model model ) {
		Funcionario funcionario = fS.buscarFuncionarioPorId( id );
		model.addAttribute( "funcionario", funcionario );
		return "funcionarios/editar";
	}
	
	@PostMapping( "/editar/{id}" )
	public String editarFuncionario( @PathVariable Long id, @ModelAttribute Funcionario funcionarioAtualizado ) {
		fS.editarFuncionario( id, funcionarioAtualizado );
		return "redirect:/funcionarios/listar-cadastrar-delete";
	}
	
	@PostMapping( "/deletar/{id}" )
	public String deletarFuncionario( @PathVariable Long id ) {
		fS.deletarFuncionario( id );
		return "redirect:/funcionarios/listar-cadastrar-delete";
	}
}
