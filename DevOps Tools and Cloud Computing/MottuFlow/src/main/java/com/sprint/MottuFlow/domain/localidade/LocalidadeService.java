package com.sprint.MottuFlow.domain.localidade;

import com.sprint.MottuFlow.infra.exception.RegraDeNegocioException;
import com.sprint.MottuFlow.domain.camera.Camera;
import com.sprint.MottuFlow.domain.camera.CameraRepository;
import com.sprint.MottuFlow.domain.moto.Moto;
import com.sprint.MottuFlow.domain.moto.MotoRepository;
import com.sprint.MottuFlow.domain.patio.Patio;
import com.sprint.MottuFlow.domain.patio.PatioRepository;
import jakarta.transaction.Transactional;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.List;

@Service
public class LocalidadeService {
	
	private final LocalidadeRepository lR;
	private final MotoRepository mR;
	private final PatioRepository pR;
	private final CameraRepository cR;
	
	public LocalidadeService( LocalidadeRepository lR, MotoRepository mR,
	                          PatioRepository pR, CameraRepository cR ) {
		this.lR = lR;
		this.mR = mR;
		this.pR = pR;
		this.cR = cR;
	}
	
	public List<Localidade> listarLocalidades() {
		return lR.findAll();
	}
	
	public Localidade buscarLocalidadePorId(Long id) {
		return lR.findById(id)
				.orElseThrow(() -> new RegraDeNegocioException("Localidade não encontrada com id: " + id));
	}
	
	public List<Localidade> buscarPorPontoReferencia(String ponto) {
		return lR.findByPontoReferencia(ponto);
	}
	
	public List<Localidade> buscarPorIntervaloData(LocalDateTime dataInicio, LocalDateTime dataFim) {
		return lR.findDatas(dataInicio, dataFim);
	}
	
	public List<Localidade> buscarPorPatio(Long idPatio) {
		return lR.findByPatio(idPatio);
	}
	
	
	@Transactional
	public Localidade cadastrarLocalidade(Localidade localidade) {
		Moto moto = mR.findById(localidade.getMoto().getIdMoto())
				.orElseThrow(() -> new RegraDeNegocioException("Moto não encontrada com id: " + localidade.getMoto().getIdMoto()));
		
		Patio patio = pR.findById(localidade.getPatio().getIdPatio())
				.orElseThrow(() -> new RegraDeNegocioException("Patio não encontrado com id: " + localidade.getPatio().getIdPatio()));
		
		Camera camera = cR.findById(localidade.getCamera().getIdCamera())
				.orElseThrow(() -> new RegraDeNegocioException("Camera não encontrada com id: " + localidade.getCamera().getIdCamera()));
		
		localidade.setMoto(moto);
		localidade.setPatio(patio);
		localidade.setCamera(camera);
		
		return lR.save(localidade);
	}
	
	@Transactional
	public Localidade editarLocalidade(Long id, Localidade localidadeAtualizada) {
		Localidade localidade = buscarLocalidadePorId(id);
		
		if (localidadeAtualizada.getDataHora() != null) {
			localidade.setDataHora(localidadeAtualizada.getDataHora());
		}
		
		if (localidadeAtualizada.getPontoReferencia() != null && !localidadeAtualizada.getPontoReferencia().isBlank()) {
			localidade.setPontoReferencia(localidadeAtualizada.getPontoReferencia());
		}
		
		if (localidadeAtualizada.getMoto() != null && localidadeAtualizada.getMoto().getIdMoto() != null) {
			Moto moto = mR.findById(localidadeAtualizada.getMoto().getIdMoto())
					.orElseThrow(() -> new RegraDeNegocioException(
							"Moto não encontrada com id: " + localidadeAtualizada.getMoto().getIdMoto()));
			localidade.setMoto(moto);
		}
		
		if (localidadeAtualizada.getPatio() != null && localidadeAtualizada.getPatio().getIdPatio() != null) {
			Patio patio = pR.findById(localidadeAtualizada.getPatio().getIdPatio())
					.orElseThrow(() -> new RegraDeNegocioException(
							"Patio não encontrado com id: " + localidadeAtualizada.getPatio().getIdPatio()));
			localidade.setPatio(patio);
		}
		
		if (localidadeAtualizada.getCamera() != null && localidadeAtualizada.getCamera().getIdCamera() != null) {
			Camera camera = cR.findById(localidadeAtualizada.getCamera().getIdCamera())
					.orElseThrow(() -> new RegraDeNegocioException(
							"Camera não encontrada com id: " + localidadeAtualizada.getCamera().getIdCamera()));
			localidade.setCamera(camera);
		}
		
		return lR.save(localidade);
	}
	
	@Transactional
	public void deletarLocalidade(Long id) {
		Localidade localidade = buscarLocalidadePorId(id);
		lR.delete(localidade);
	}
}
