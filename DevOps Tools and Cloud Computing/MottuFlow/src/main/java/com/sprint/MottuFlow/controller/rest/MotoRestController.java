package com.sprint.MottuFlow.controller.rest;

import com.sprint.MottuFlow.domain.moto.MotoComTagsDTO;
import com.sprint.MottuFlow.domain.moto.MotoDTO;
import com.sprint.MottuFlow.domain.moto.Moto;
import com.sprint.MottuFlow.domain.patio.Patio;
import com.sprint.MottuFlow.domain.moto.MotoService;

import jakarta.validation.Valid;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.stream.Collectors;

@RestController
@RequestMapping( "/api/motos" )
public class MotoRestController {
	
	private final MotoService mS;
	
	public MotoRestController( MotoService mS ) {
		this.mS = mS;
	}
	
	private MotoDTO paraDTO( Moto moto ) {
		return new MotoDTO( moto.getIdMoto(), moto.getPlaca(), moto.getModelo(), moto.getFabricante(), moto.getAno(), moto.getPatio().getIdPatio(), moto.getLocalizacaoAtual() );
	}
	
	private Moto paraEntity( MotoDTO dto ) {
		Moto moto = new Moto();
		moto.setIdMoto( dto.getIdMoto() );
		moto.setPlaca( dto.getPlaca() );
		moto.setModelo( dto.getModelo() );
		moto.setFabricante( dto.getFabricante() );
		moto.setAno( dto.getAno() );
		if ( dto.getIdPatio() != 0 ) {
			Patio patio = new Patio();
			patio.setIdPatio( dto.getIdPatio() );
			moto.setPatio( patio );
		}
		moto.setLocalizacaoAtual( dto.getLocalizacaoAtual() );
		return moto;
	}
	
	@GetMapping( "/listar" )
	public List<MotoDTO> listarRest() {
		return mS.listarMotos().stream().map( this::paraDTO ).collect( Collectors.toList() );
	}
	
	@GetMapping( "/buscar-por-id/{id}" )
	public ResponseEntity<MotoDTO> buscarPorIdRest( @PathVariable Long id ) {
		Moto moto = mS.buscarMotoPorId( id );
		return ResponseEntity.ok( paraDTO( moto ) );
	}
	
	@GetMapping( "/buscar-por-placa/{placa}" )
	public ResponseEntity<?> buscarPorPlacaRest( @PathVariable String placa ) {
		List<Moto> motoList = mS.buscarPorPlaca( placa );
		if ( motoList.isEmpty() ) {
			return ResponseEntity.notFound().build();
		} else if ( motoList.size() == 1 ) {
			return ResponseEntity.ok( paraDTO( motoList.get( 0 ) ) );
		} else {
			List<MotoDTO> dtos = motoList.stream().map( this::paraDTO ).collect( Collectors.toList() );
			return ResponseEntity.ok( dtos );
		}
	}
	
	@GetMapping( "/buscar-por-fabricante" )
	public ResponseEntity<?> buscarPorFabricanteRest( @RequestParam String fabricante ) {
		List<Moto> motoList = mS.buscarPorFabricante( fabricante );
		
		if ( motoList.isEmpty() ) {
			return ResponseEntity.notFound().build();
		} else if ( motoList.size() == 1 ) {
			return ResponseEntity.ok( paraDTO( motoList.get( 0 ) ) );
		} else {
			List<MotoDTO> dtos = motoList.stream().map( this::paraDTO ).collect( Collectors.toList() );
			return ResponseEntity.ok( dtos );
		}
	}
	
	@GetMapping("/buscar-por-modelo/{modelo}")
	public ResponseEntity<?> buscarPorModeloRest(@PathVariable String modelo) {
		List<Moto> motoList = mS.buscarPorModelo(modelo);
		
		if (motoList.isEmpty()) {
			return ResponseEntity.notFound().build();
		} else if (motoList.size() == 1) {
			return ResponseEntity.ok(paraDTO(motoList.get(0)));
		} else {
			List<MotoDTO> dtos = motoList.stream()
					.map(this::paraDTO)
					.collect(Collectors.toList());
			return ResponseEntity.ok(dtos);
		}
	}
	
	@GetMapping( "/buscar-por-patio/{idPatio}" )
	public ResponseEntity<?> buscarPorPatioRest( @PathVariable long idPatio ) {
		List<Moto> motoList = mS.buscarPorPatioId( idPatio );
		
		if ( motoList.isEmpty() ) {
			return ResponseEntity.notFound().build();
		} else if ( motoList.size() == 1 ) {
			return ResponseEntity.ok( paraDTO( motoList.get( 0 ) ) );
		} else {
			List<MotoDTO> dtos = motoList.stream().map( this::paraDTO ).collect( Collectors.toList() );
			return ResponseEntity.ok( dtos );
		}
	}
	
	@PostMapping( "/cadastrar" )
	public ResponseEntity<MotoDTO> cadastrarRest( @RequestBody @Valid MotoDTO dto ) {
		Moto moto = paraEntity( dto );
		Moto salvo = mS.cadastrarMoto( moto );
		return ResponseEntity.ok( paraDTO( salvo ) );
	}
	
	@PutMapping( "/editar/{id}" )
	public ResponseEntity<MotoDTO> editarRest( @PathVariable Long id, @RequestBody MotoDTO dto ) {
		Moto moto = paraEntity( dto );
		Moto atualizado = mS.editarMoto( id, moto );
		return ResponseEntity.ok( paraDTO( atualizado ) );
	}
	
	@DeleteMapping( "/deletar/{id}" )
	public ResponseEntity<Void> deletarRest( @PathVariable Long id ) {
		mS.deletarMoto( id );
		return ResponseEntity.noContent().build();
	}
	
	@GetMapping( "/motos-com-tags" )
	public List<MotoComTagsDTO> listarMotosComTags() {
		return mS.listarMotosComTags();
	}
}
