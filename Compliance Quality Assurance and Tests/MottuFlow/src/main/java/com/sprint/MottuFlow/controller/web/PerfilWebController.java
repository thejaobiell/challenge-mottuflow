package com.sprint.MottuFlow.controller.web;

import com.sprint.MottuFlow.domain.funcionario.Funcionario;
import com.sprint.MottuFlow.domain.funcionario.FuncionarioService;
import org.springframework.security.core.annotation.AuthenticationPrincipal;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.servlet.mvc.support.RedirectAttributes;

import java.security.Principal;

@Controller
public class PerfilWebController {
	
	private final FuncionarioService fS;
	
	public PerfilWebController( FuncionarioService fS ) {
		this.fS = fS;
	}
	
	@GetMapping( "/perfil" )
	public String mostrarPerfil( Model model, @AuthenticationPrincipal UserDetails userDetails ) {
		Funcionario usuario = fS.buscarFuncionarioPorEmail( userDetails.getUsername() );
		model.addAttribute( "usuario", usuario );
		return "perfil";
	}
	
	@PostMapping("/alterar-senha")
	public String alterarSenha(@RequestParam String senhaAtual,
	                           @RequestParam String novaSenha,
	                           @RequestParam String confirmarSenha,
	                           Principal principal,
	                           RedirectAttributes redirectAttributes) {
		
		if (!novaSenha.equals(confirmarSenha)) {
			redirectAttributes.addFlashAttribute("erro", "As senhas não coincidem!");
			return "redirect:/perfil";
		}
		
		try {
			fS.alterarSenha(principal.getName(), senhaAtual, novaSenha);
			redirectAttributes.addFlashAttribute("sucesso", "Senha alterada com sucesso!");
		} catch (Exception e) {
			redirectAttributes.addFlashAttribute("erro", e.getMessage());
		}
		
		return "redirect:/perfil";
	}
}