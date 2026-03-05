package com.sprint.MottuFlow.domain.camera;

import com.sprint.MottuFlow.infra.exception.RegraDeNegocioException;
import com.sprint.MottuFlow.domain.patio.Patio;
import com.sprint.MottuFlow.domain.patio.PatioRepository;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class CameraService {
	
	private final CameraRepository cR;
	
	private final PatioRepository pR;
	
	public CameraService( CameraRepository cR, PatioRepository pR ) {
		this.cR = cR;
		this.pR = pR;
	}
	
	public List<Camera> listarCameras() {
		return cR.findAll();
	}
	
	public Camera buscarCameraPorId( Long id ) {
		return cR.findById( id ).orElseThrow( () -> new RegraDeNegocioException( "Camera não encontrada com id: " + id ) );
	}
	
	public List<Camera> buscarPorStatusOperacional( String status ) {
		return cR.findByStatusOperacional( status );
	}
	
	public List<Camera> buscarPorLocalizacaoFisica( String localizacao ) {
		return cR.findByLocalizacaoFisica( localizacao );
	}
	
	public Camera cadastrarCamera( Camera camera ) {
		Patio patio = pR.findById( camera.getPatio().getIdPatio() ).orElseThrow( () -> new RegraDeNegocioException( "Patio não encontrado com id: " + camera.getPatio().getIdPatio() ) );
		camera.setPatio( patio );
		
		return cR.save( camera );
	}
	
	public Camera editarCamera( Long id, Camera cameraAtualizada ) {
		Camera camera = buscarCameraPorId( id );
		
		if ( cameraAtualizada.getStatusOperacional() != null && !cameraAtualizada.getStatusOperacional().isBlank() ) {
			camera.setStatusOperacional( cameraAtualizada.getStatusOperacional() );
		}
		
		if ( cameraAtualizada.getLocalizacaoFisica() != null && !cameraAtualizada.getLocalizacaoFisica().isBlank() ) {
			camera.setLocalizacaoFisica( cameraAtualizada.getLocalizacaoFisica() );
		}
		
		if ( cameraAtualizada.getPatio() != null && cameraAtualizada.getPatio().getIdPatio() != null ) {
			Patio patio = pR.findById( cameraAtualizada.getPatio().getIdPatio() ).orElseThrow( () -> new RegraDeNegocioException( "Pátio não encontrado com id: " + cameraAtualizada.getPatio().getIdPatio() ) );
			camera.setPatio( patio );
		}
		
		return cR.save( camera );
	}
	
	public void deletarCamera( Long id ) {
		Camera camera = buscarCameraPorId( id );
		cR.delete( camera );
	}
}
