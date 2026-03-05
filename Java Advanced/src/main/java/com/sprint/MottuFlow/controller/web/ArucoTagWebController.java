package com.sprint.MottuFlow.controller.web;

import com.sprint.MottuFlow.domain.arucotag.ArucoTag;
import com.sprint.MottuFlow.domain.arucotag.ArucoTagService;
import com.sprint.MottuFlow.domain.moto.MotoService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

@Controller
@RequestMapping( "/arucotags" )
public class ArucoTagWebController {
	
	private final ArucoTagService atS;
	private final MotoService mS;
	
	public ArucoTagWebController( ArucoTagService atS, MotoService mS ) {
		this.atS = atS;
		this.mS = mS;
	}
	
	@GetMapping( "/listar-cadastrar-delete" )
	public String listarTags( Model model ) {
		model.addAttribute( "tags", atS.listarArucoTags() );
		model.addAttribute( "motos", mS.listarMotos() );
		model.addAttribute( "novaTag", new ArucoTag() );
		return "arucotags/listar-cadastrar-delete";
	}
	
	@PostMapping( "/cadastrar" )
	public String cadastrarTag( @ModelAttribute( "novaTag" ) ArucoTag tag ) {
		atS.cadastrarTag( tag );
		return "redirect:/arucotags/listar-cadastrar-delete";
	}
	
	@GetMapping( "/editar/{id}" )
	public String editarTagForm( @PathVariable Long id, Model model ) {
		ArucoTag tag = atS.buscarTagPorId( id );
		model.addAttribute( "tag", tag );
		model.addAttribute( "motos", mS.listarMotos() );
		return "arucotags/editar";
	}
	
	@PostMapping( "/editar/{id}" )
	public String editarTag( @PathVariable Long id, @ModelAttribute ArucoTag tagAtualizada ) {
		atS.editarTag( id, tagAtualizada );
		return "redirect:/arucotags/listar-cadastrar-delete";
	}
	
	@PostMapping( "/deletar/{id}" )
	public String deletarTag( @PathVariable Long id ) {
		atS.deletarTag( id );
		return "redirect:/arucotags/listar-cadastrar-delete";
	}
}
