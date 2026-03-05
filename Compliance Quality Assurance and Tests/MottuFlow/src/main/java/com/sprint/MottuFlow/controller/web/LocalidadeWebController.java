package com.sprint.MottuFlow.controller.web;

import com.sprint.MottuFlow.domain.localidade.Localidade;
import com.sprint.MottuFlow.domain.localidade.LocalidadeService;
import com.sprint.MottuFlow.domain.moto.Moto;
import com.sprint.MottuFlow.domain.moto.MotoService;
import com.sprint.MottuFlow.domain.patio.Patio;
import com.sprint.MottuFlow.domain.patio.PatioService;
import com.sprint.MottuFlow.domain.camera.Camera;
import com.sprint.MottuFlow.domain.camera.CameraService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@Controller
@RequestMapping( "/localidades" )
public class LocalidadeWebController {
	
	private final LocalidadeService lS;
	private final MotoService mS;
	private final PatioService pS;
	private final CameraService cS;
	
	public LocalidadeWebController( LocalidadeService lS, MotoService mS, PatioService patioService, CameraService cS ) {
		this.lS = lS;
		this.mS = mS;
		this.pS = patioService;
		this.cS = cS;
	}
	
	@GetMapping( "/listar-cadastrar-delete" )
	public String listarLocalidades( Model model ) {
		List<Localidade> localidades = lS.listarLocalidades();
		List<Moto> motos = mS.listarMotos();
		List<Patio> patios = pS.listarPatios();
		List<Camera> cameras = cS.listarCameras();
		
		String dataHoraAtual = java.time.LocalDateTime.now().format( java.time.format.DateTimeFormatter.ofPattern( "yyyy-MM-dd'T'HH:mm" ) );
		
		model.addAttribute( "localidades", localidades );
		model.addAttribute( "motos", motos );
		model.addAttribute( "patios", patios );
		model.addAttribute( "cameras", cameras );
		model.addAttribute( "novaLocalidade", new Localidade() );
		model.addAttribute( "dataHoraAtual", dataHoraAtual ); // adiciona no Model
		
		return "localidades/listar-cadastrar-delete";
	}
	
	
	@PostMapping( "/cadastrar" )
	public String cadastrarLocalidade( @ModelAttribute( "novaLocalidade" ) Localidade localidade ) {
		lS.cadastrarLocalidade( localidade );
		return "redirect:/localidades/listar-cadastrar-delete";
	}
	
	@GetMapping( "/editar/{id}" )
	public String editarLocalidadeForm( @PathVariable Long id, Model model ) {
		Localidade localidade = lS.buscarLocalidadePorId( id );
		List<Moto> motos = mS.listarMotos();
		List<Patio> patios = pS.listarPatios();
		List<Camera> cameras = cS.listarCameras();
		
		model.addAttribute( "localidade", localidade );
		model.addAttribute( "motos", motos );
		model.addAttribute( "patios", patios );
		model.addAttribute( "cameras", cameras );
		return "localidades/editar";
	}
	
	@PostMapping( "/editar/{id}" )
	public String editarLocalidade( @PathVariable Long id, @ModelAttribute Localidade localidadeAtualizada ) {
		lS.editarLocalidade( id, localidadeAtualizada );
		return "redirect:/localidades/listar-cadastrar-delete";
	}
	
	@PostMapping( "/deletar/{id}" )
	public String deletarLocalidade( @PathVariable Long id ) {
		lS.deletarLocalidade( id );
		return "redirect:/localidades/listar-cadastrar-delete";
	}
}
