package com.sprint.MottuFlow.controller.web;

import com.sprint.MottuFlow.domain.moto.Moto;
import com.sprint.MottuFlow.domain.moto.MotoService;
import com.sprint.MottuFlow.domain.patio.PatioService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

@Controller
@RequestMapping( "/motos" )
public class MotoWebController {
	
	private final MotoService mS;
	private final PatioService pS;
	
	public MotoWebController( MotoService mS, PatioService pS ) {
		this.mS = mS;
		this.pS = pS;
	}
	
	@GetMapping( "/listar-cadastrar-delete" )
	public String listarMotos( Model model ) {
		model.addAttribute( "motos", mS.listarMotos() );
		model.addAttribute( "novaMoto", new Moto() );
		model.addAttribute( "patios", pS.listarPatios() );
		return "motos/listar-cadastrar-delete";
	}
	
	@PostMapping( "/cadastrar" )
	public String cadastrarMoto( @ModelAttribute( "novaMoto" ) Moto moto ) {
		mS.cadastrarMoto( moto );
		return "redirect:/motos/listar-cadastrar-delete";
	}
	
	@GetMapping( "/editar/{id}" )
	public String editarMotoForm( @PathVariable Long id, Model model ) {
		Moto moto = mS.buscarMotoPorId( id );
		model.addAttribute( "moto", moto );
		model.addAttribute( "patios", pS.listarPatios() );
		return "motos/editar";
	}
	
	@PostMapping( "/editar/{id}" )
	public String editarMoto( @PathVariable Long id, @ModelAttribute Moto motoAtualizada ) {
		mS.editarMoto( id, motoAtualizada );
		return "redirect:/motos/listar-cadastrar-delete";
	}
	
	@PostMapping( "/deletar/{id}" )
	public String deletarMoto( @PathVariable Long id ) {
		mS.deletarMoto( id );
		return "redirect:/motos/listar-cadastrar-delete";
	}
}
