package com.sprint.MottuFlow.controller.web;

import com.sprint.MottuFlow.domain.status.Status;
import com.sprint.MottuFlow.domain.status.StatusService;
import com.sprint.MottuFlow.domain.moto.MotoService;
import com.sprint.MottuFlow.domain.funcionario.FuncionarioService;
import com.sprint.MottuFlow.domain.status.TipoStatus;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

@Controller
@RequestMapping("/status")
public class StatusWebController {
	
	private final StatusService sS;
	private final MotoService mS;
	private final FuncionarioService fS;
	
	public StatusWebController( StatusService sS, MotoService mS, FuncionarioService fS ) {
		this.sS = sS;
		this.mS = mS;
		this.fS = fS;
	}
	
	@GetMapping("/listar-cadastrar-delete")
	public String listarStatus(Model model) {
		model.addAttribute("statusList", sS.listarStatus());
		model.addAttribute("novoStatus", new Status());
		model.addAttribute("motos", mS.listarMotos());
		model.addAttribute("funcionarios", fS.listarFuncionarios());
		model.addAttribute("tiposStatus", TipoStatus.values());
		return "status/listar-cadastrar-delete";
	}
	
	@PostMapping("/cadastrar")
	public String cadastrarStatus(@ModelAttribute("novoStatus") Status status) {
		sS.cadastrarStatus(status);
		return "redirect:/status/listar-cadastrar-delete";
	}
	
	@GetMapping("/editar/{id}")
	public String editarStatusForm(@PathVariable Long id, Model model) {
		Status status = sS.buscarStatusPorId(id);
		model.addAttribute("status", status);
		model.addAttribute("motos", mS.listarMotos());
		model.addAttribute("funcionarios", fS.listarFuncionarios());
		model.addAttribute("tiposStatus", TipoStatus.values());
		return "status/editar";
	}
	
	@PostMapping("/editar/{id}")
	public String editarStatus(@PathVariable Long id, @ModelAttribute Status statusAtualizado) {
		sS.editarStatus(id, statusAtualizado);
		return "redirect:/status/listar-cadastrar-delete";
	}
	
	@PostMapping("/deletar/{id}")
	public String deletarStatus(@PathVariable Long id) {
		sS.deletarStatus(id);
		return "redirect:/status/listar-cadastrar-delete";
	}
}
