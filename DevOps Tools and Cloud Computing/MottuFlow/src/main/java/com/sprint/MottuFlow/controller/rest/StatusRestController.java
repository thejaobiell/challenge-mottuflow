package com.sprint.MottuFlow.controller.rest;

import com.sprint.MottuFlow.domain.status.StatusDTO;
import com.sprint.MottuFlow.domain.status.Status;
import com.sprint.MottuFlow.domain.status.StatusService;
import com.sprint.MottuFlow.domain.status.TipoStatus;
import com.sprint.MottuFlow.domain.funcionario.Funcionario;
import com.sprint.MottuFlow.domain.moto.Moto;

import jakarta.validation.Valid;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

import java.time.LocalDateTime;
import java.util.List;
import java.util.stream.Collectors;

@RestController
@RequestMapping("/api/status")
public class StatusRestController {
	
	private final StatusService sS;
	
	public StatusRestController(StatusService sS ) {
		this.sS = sS;
	}
	
	private StatusDTO paraDTO(Status status) {
		return new StatusDTO(
				status.getIdStatus(),
				status.getMoto().getIdMoto(),
				status.getTipoStatus(),
				status.getDescricao(),
				status.getDataStatus(),
				status.getFuncionario().getId_funcionario()
		);
	}
	
	private Status paraEntity(StatusDTO dto) {
		Status status = new Status();
		status.setIdStatus(dto.getIdStatus());
		status.setTipoStatus(dto.getTipoStatus());
		status.setDescricao(dto.getDescricao());
		status.setDataStatus(dto.getDataStatus());
		
		if (dto.getIdMoto() != 0) {
			Moto moto = new Moto();
			moto.setIdMoto(dto.getIdMoto());
			status.setMoto(moto);
		}
		
		if (dto.getIdFuncionario() != 0) {
			Funcionario funcionario = new Funcionario();
			funcionario.setId_funcionario(dto.getIdFuncionario());
			status.setFuncionario(funcionario);
		}
		
		return status;
	}
	
	@GetMapping("/listar")
	public List<StatusDTO> listarRest() {
		return sS.listarStatus()
				.stream().map(this::paraDTO)
				.collect(Collectors.toList());
	}
	
	@GetMapping("/buscar-por-id/{id}")
	public ResponseEntity<StatusDTO> buscarPorIdRest(@PathVariable Long id) {
		Status status = sS.buscarStatusPorId(id);
		return ResponseEntity.ok(paraDTO(status));
	}
	
	@GetMapping("/buscar-por-tipo")
	public List<StatusDTO> buscarPorTipoRest(@RequestParam String tipoStatus) {
		TipoStatus tipo;
		try {
			tipo = TipoStatus.valueOf(tipoStatus.toUpperCase().replace(" ", "_"));
		} catch (IllegalArgumentException e) {
			throw new ResponseStatusException(HttpStatus.BAD_REQUEST, "TipoStatus inválido: " + tipoStatus);
		}
		return sS.buscarPorTipoStatus(tipo)
				.stream().map(this::paraDTO)
				.collect(Collectors.toList());
	}
	
	@GetMapping("/buscar-por-descricao")
	public List<StatusDTO> buscarPorDescricaoRest(@RequestParam String descricao) {
		return sS.buscarPorDescricaoStatus(descricao)
				.stream().map(this::paraDTO)
				.collect(Collectors.toList());
	}
	
	@GetMapping("/buscar-por-periodo")
	public List<StatusDTO> buscarPorPeriodo(@RequestParam String inicio, @RequestParam String fim) {
		LocalDateTime dataInicio = LocalDateTime.parse(inicio);
		LocalDateTime dataFim = LocalDateTime.parse(fim);
		
		return sS.buscarPorPeriodo(dataInicio, dataFim)
				.stream().map(this::paraDTO)
				.collect(Collectors.toList());
	}
	
	
	@GetMapping("/buscar-por-moto/{idMoto}")
	public ResponseEntity<StatusDTO> buscarPorMoto(@PathVariable Long idMoto) {
		Status status = sS.buscarPorMoto(idMoto);
		return ResponseEntity.ok(paraDTO(status));
	}
	
	@PostMapping("/cadastrar")
	public ResponseEntity<StatusDTO> cadastrarRest(@RequestBody @Valid StatusDTO dto) {
		Status status = paraEntity(dto);
		Status salvo = sS.cadastrarStatus(status);
		return ResponseEntity.status(HttpStatus.CREATED).body(paraDTO(salvo));
	}
	
	@PutMapping("/editar/{id}")
	public ResponseEntity<StatusDTO> editarRest(@PathVariable Long id, @RequestBody @Valid StatusDTO dto) {
		Status atualizado = paraEntity(dto);
		Status updated = sS.editarStatus(id, atualizado);
		return ResponseEntity.ok(paraDTO(updated));
	}
	
	@DeleteMapping("/deletar/{id}")
	public ResponseEntity<Void> deletarRest(@PathVariable Long id) {
		sS.deletarStatus(id);
		return ResponseEntity.noContent().build();
	}
}
