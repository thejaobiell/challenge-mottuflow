package com.sprint.MottuFlow.controller.rest;

import com.sprint.MottuFlow.domain.localidade.LocalidadeDTO;
import com.sprint.MottuFlow.domain.camera.Camera;
import com.sprint.MottuFlow.domain.localidade.Localidade;
import com.sprint.MottuFlow.domain.moto.Moto;
import com.sprint.MottuFlow.domain.patio.Patio;
import com.sprint.MottuFlow.domain.localidade.LocalidadeService;

import jakarta.validation.Valid;

import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDateTime;
import java.util.List;
import java.util.stream.Collectors;

@RestController
@RequestMapping("/api/localidades")
public class LocalidadeRestController {
	
	private final LocalidadeService lS;
	
	public LocalidadeRestController(LocalidadeService lS ) {
		this.lS = lS;
	}
	
	private LocalidadeDTO paraDTO(Localidade localidade) {
		return new LocalidadeDTO(
				localidade.getIdLocalidade(),
				localidade.getDataHora(),
				localidade.getMoto().getIdMoto(),
				localidade.getPatio().getIdPatio(),
				localidade.getPontoReferencia(),
				localidade.getCamera().getIdCamera()
		);
	}
	
	private Localidade paraEntity(LocalidadeDTO dto) {
		Localidade localidade = new Localidade();
		localidade.setIdLocalidade(dto.getIdLocalidade());
		localidade.setDataHora(dto.getDataHora());
		localidade.setPontoReferencia(dto.getPontoReferencia());
		
		if (dto.getIdMoto() != 0) {
			Moto moto = new Moto();
			moto.setIdMoto(dto.getIdMoto());
			localidade.setMoto(moto);
		}
		if (dto.getIdPatio() != 0) {
			Patio patio = new Patio();
			patio.setIdPatio(dto.getIdPatio());
			localidade.setPatio(patio);
		}
		if (dto.getIdCamera() != 0) {
			Camera camera = new Camera();
			camera.setIdCamera(dto.getIdCamera());
			localidade.setCamera(camera);
		}
		
		return localidade;
	}
	
	@GetMapping("/listar")
	public List<LocalidadeDTO> listarRest() {
		return lS.listarLocalidades().stream().map(this::paraDTO).collect(Collectors.toList());
	}
	
	@GetMapping("/buscar-por-id/{id}")
	public ResponseEntity<LocalidadeDTO> buscarPorIdRest(@PathVariable Long id) {
		Localidade localidade = lS.buscarLocalidadePorId(id);
		return ResponseEntity.ok(paraDTO(localidade));
	}
	
	@GetMapping("/buscar-por-ponto-referencia/{ponto}")
	public List<LocalidadeDTO> buscarPorPontoReferenciaRest(@PathVariable String ponto) {
		return lS.buscarPorPontoReferencia(ponto).stream().map(this::paraDTO).collect(Collectors.toList());
	}
	
	@GetMapping("/buscar-por-periodo")
	public List<LocalidadeDTO> buscarPorPeriodoRest(
			@RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime dataInicio,
			@RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime dataFim) {
		return lS.buscarPorIntervaloData(dataInicio, dataFim).stream().map(this::paraDTO).collect(Collectors.toList());
	}
	
	@GetMapping("/buscar-por-patio/{idPatio}")
	public List<LocalidadeDTO> buscarPorPatioRest(@PathVariable Long idPatio) {
		return lS.buscarPorPatio(idPatio)
				.stream()
				.map(this::paraDTO)
				.collect(Collectors.toList());
	}
	
	@PostMapping("/cadastrar")
	public ResponseEntity<LocalidadeDTO> cadastrarRest(@RequestBody @Valid LocalidadeDTO dto) {
		Localidade localidade = paraEntity(dto);
		Localidade salvo = lS.cadastrarLocalidade(localidade);
		return ResponseEntity.ok(paraDTO(salvo));
	}
	
	@PutMapping("/editar/{id}")
	public ResponseEntity<LocalidadeDTO> editarRest(@PathVariable Long id, @RequestBody LocalidadeDTO dto) {
		Localidade localidade = paraEntity(dto);
		Localidade atualizado = lS.editarLocalidade(id, localidade);
		return ResponseEntity.ok(paraDTO(atualizado));
	}
	
	@DeleteMapping("/deletar/{id}")
	public ResponseEntity<Void> deletarRest(@PathVariable Long id) {
		lS.deletarLocalidade(id);
		return ResponseEntity.noContent().build();
	}
}
