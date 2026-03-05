package com.sprint.MottuFlow.controller.rest;

import com.sprint.MottuFlow.domain.camera.CameraDTO;
import com.sprint.MottuFlow.domain.camera.Camera;
import com.sprint.MottuFlow.domain.patio.Patio;
import com.sprint.MottuFlow.domain.camera.CameraService;

import jakarta.validation.Valid;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.stream.Collectors;

@RestController
@RequestMapping("/api/cameras")
public class CameraRestController {
	
	private final CameraService cS;
	
	public CameraRestController(CameraService cS) {
		this.cS = cS;
	}
	
	private CameraDTO paraDTO(Camera camera) {
		return new CameraDTO(
				camera.getIdCamera(),
				camera.getStatusOperacional(),
				camera.getLocalizacaoFisica(),
				camera.getPatio().getIdPatio()
		);
	}
	
	private Camera paraEntity(CameraDTO dto) {
		Camera camera = new Camera();
		camera.setIdCamera(dto.getIdCamera());
		camera.setStatusOperacional(dto.getStatusOperacional());
		camera.setLocalizacaoFisica(dto.getLocalizacaoFisica());
		
		if (dto.getIdPatio() != 0) {
			Patio patio = new Patio();
			patio.setIdPatio(dto.getIdPatio());
			camera.setPatio(patio);
		}
		return camera;
	}
	
	@GetMapping("/listar")
	public List<CameraDTO> listarRest() {
		return cS.listarCameras().stream().map(this::paraDTO).collect(Collectors.toList());
	}
	
	@GetMapping("/buscar-por-id/{id}")
	public ResponseEntity<CameraDTO> buscarPorIdRest(@PathVariable Long id) {
		Camera camera = cS.buscarCameraPorId(id);
		return ResponseEntity.ok(paraDTO(camera));
	}
	
	@GetMapping("/buscar-por-status/{status}")
	public List<CameraDTO> buscarPorStatusRest(@PathVariable String status) {
		return cS.buscarPorStatusOperacional(status)
				.stream()
				.map(this::paraDTO)
				.collect(Collectors.toList());
	}
	
	@GetMapping("/buscar-por-localizacao/{localizacao}")
	public List<CameraDTO> buscarPorLocalizacaoRest(@PathVariable String localizacao) {
		return cS.buscarPorLocalizacaoFisica(localizacao)
				.stream()
				.map(this::paraDTO)
				.collect(Collectors.toList());
	}
	
	@PostMapping("/cadastrar")
	public ResponseEntity<CameraDTO> cadastrarRest(@RequestBody @Valid CameraDTO dto) {
		Camera camera = paraEntity(dto);
		Camera salvo = cS.cadastrarCamera(camera);
		return ResponseEntity.ok(paraDTO(salvo));
	}
	
	@PutMapping("/editar/{id}")
	public ResponseEntity<CameraDTO> editarRest(@PathVariable Long id, @RequestBody CameraDTO dto) {
		Camera camera = paraEntity(dto);
		Camera atualizado = cS.editarCamera(id, camera);
		return ResponseEntity.ok(paraDTO(atualizado));
	}
	
	@DeleteMapping("/deletar/{id}")
	public ResponseEntity<Void> deletarRest(@PathVariable Long id) {
		cS.deletarCamera(id);
		return ResponseEntity.noContent().build();
	}
}

