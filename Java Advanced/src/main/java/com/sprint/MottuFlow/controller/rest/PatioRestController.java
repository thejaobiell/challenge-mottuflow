package com.sprint.MottuFlow.controller.rest;

import com.sprint.MottuFlow.domain.patio.PatioDTO;
import com.sprint.MottuFlow.domain.patio.Patio;
import com.sprint.MottuFlow.domain.patio.PatioService;

import jakarta.validation.Valid;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.stream.Collectors;

@RestController
@RequestMapping("/api/patios")
public class PatioRestController {
	
	private final PatioService pS;
	
	public PatioRestController(PatioService pS ) {
		this.pS = pS;
	}
	
	private PatioDTO paraDTO(Patio patio) {
		return new PatioDTO(
				patio.getIdPatio(),
				patio.getNome(),
				patio.getEndereco(),
				patio.getCapacidadeMaxima()
		);
	}
	
	private Patio paraEntity(PatioDTO dto) {
		Patio patio = new Patio();
		patio.setIdPatio(dto.getIdPatio());
		patio.setNome(dto.getNome());
		patio.setEndereco(dto.getEndereco());
		patio.setCapacidadeMaxima(dto.getCapacidadeMaxima());
		return patio;
	}
	
	@GetMapping("/listar")
	public List<PatioDTO> listarRest() {
		return pS.listarPatios().stream().map(this::paraDTO).collect(Collectors.toList());
	}
	
	@GetMapping("/buscar-por-id/{id}")
	public ResponseEntity<PatioDTO> buscarPorIdRest(@PathVariable Long id) {
		Patio patio = pS.buscarPatioPorId(id);
		return ResponseEntity.ok(paraDTO(patio));
	}
	
	@PostMapping("/cadastrar")
	public ResponseEntity<PatioDTO> cadastrarRest(@RequestBody @Valid PatioDTO dto) {
		Patio patio = paraEntity(dto);
		Patio salvo = pS.cadastrarPatio(patio);
		return ResponseEntity.ok(paraDTO(salvo));
	}
	
	@PutMapping("/editar/{id}")
	public ResponseEntity<PatioDTO> editarRest(@PathVariable Long id, @RequestBody PatioDTO dto) {
		Patio patio = paraEntity(dto);
		Patio atualizado = pS.editarPatio(id, patio);
		return ResponseEntity.ok(paraDTO(atualizado));
	}
	
	@DeleteMapping("/deletar/{id}")
	public ResponseEntity<Void> deletarRest(@PathVariable Long id) {
		pS.deletarPatio(id);
		return ResponseEntity.noContent().build();
	}
}