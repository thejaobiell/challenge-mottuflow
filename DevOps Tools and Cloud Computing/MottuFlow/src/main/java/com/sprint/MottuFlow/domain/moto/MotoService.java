package com.sprint.MottuFlow.domain.moto;

import com.sprint.MottuFlow.infra.exception.RegraDeNegocioException;
import com.sprint.MottuFlow.domain.patio.Patio;
import com.sprint.MottuFlow.domain.patio.PatioRepository;
import jakarta.transaction.Transactional;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.stream.Collectors;

@Service
public class MotoService {
	
	private final MotoRepository mR;
	private final PatioRepository pR;
	
	public MotoService(MotoRepository mR, PatioRepository pR ) {
		this.mR = mR;
		this.pR = pR;
	}
	
	public List<Moto> listarMotos() {
		return mR.findAll();
	}
	
	public Moto buscarMotoPorId(Long id) {
		return mR.findById(id)
				.orElseThrow(() -> new RegraDeNegocioException("Moto não encontrada com id: " + id));
	}
	
	public List<Moto> buscarPorPlaca(String placa) {
		List<Moto> motos = mR.findByPlacaContaining(placa);
		if (motos.isEmpty()) {
			throw new RegraDeNegocioException(
					"Nenhuma moto encontrada com Placa contendo: " + placa
			);
		}
		return motos;
	}
	
	public List<Moto> buscarPorFabricante(String fabricante) {
		return mR.findByFabricante(fabricante);
	}
	
	public List<Moto> buscarPorModelo(String modelo) {
		List<Moto> motos = mR.findByModeloContainingIgnoreCase(modelo);
		if (motos.isEmpty()) {
			throw new RegraDeNegocioException(
					"Nenhuma moto encontrada com modelo contendo: " + modelo
			);
		}
		return motos;
	}
	
	
	public List<Moto> buscarPorPatioId(long idPatio) {
		return mR.findByPatioId(idPatio);
	}
	
	@Transactional
	public Moto cadastrarMoto(Moto moto) {
		if (moto.getPatio() == null || moto.getPatio().getIdPatio() == null) {
			throw new RegraDeNegocioException("Pátio é obrigatório!");
		}
		
		Patio patio = pR.findById(moto.getPatio().getIdPatio())
				.orElseThrow(() -> new RegraDeNegocioException(
						"Pátio não encontrado com id: " + moto.getPatio().getIdPatio()));
		moto.setPatio(patio);
		
		return mR.save(moto);
	}
	
	@Transactional
	public Moto editarMoto(Long id, Moto motoAtualizada) {
		Moto moto = buscarMotoPorId(id);
		
		if (motoAtualizada.getPlaca() != null && !motoAtualizada.getPlaca().isBlank()) {
			moto.setPlaca(motoAtualizada.getPlaca());
		}
		
		if (motoAtualizada.getModelo() != null && !motoAtualizada.getModelo().isBlank()) {
			moto.setModelo(motoAtualizada.getModelo());
		}
		
		if (motoAtualizada.getFabricante() != null && !motoAtualizada.getFabricante().isBlank()) {
			moto.setFabricante(motoAtualizada.getFabricante());
		}
		
		if (motoAtualizada.getAno() > 0) { // Atualiza somente se ano for válido
			moto.setAno(motoAtualizada.getAno());
		}
		
		if (motoAtualizada.getLocalizacaoAtual() != null && !motoAtualizada.getLocalizacaoAtual().isBlank()) {
			moto.setLocalizacaoAtual(motoAtualizada.getLocalizacaoAtual());
		}
		
		if (motoAtualizada.getPatio() != null && motoAtualizada.getPatio().getIdPatio() != null) {
			Patio patio = pR.findById(motoAtualizada.getPatio().getIdPatio())
					.orElseThrow(() -> new RegraDeNegocioException(
							"Pátio não encontrado com id: " + motoAtualizada.getPatio().getIdPatio()));
			moto.setPatio(patio);
		}
		
		return mR.save(moto);
	}
	
	@Transactional
	public void deletarMoto(Long id) {
		Moto moto = buscarMotoPorId(id);
		mR.delete(moto);
	}
	
	@Transactional
	public List<MotoComTagsDTO> listarMotosComTags() {
		List<Moto> motos = mR.findAll();
		
		return motos.stream().map(moto -> {
			List<MotoComTagsDTO.ArucoTagInfo> tags = moto.getArucoTags().stream()
					.map(tag -> new MotoComTagsDTO.ArucoTagInfo(tag.getIdTag(), tag.getCodigo(), tag.getStatus()))
					.collect( Collectors.toList());
			
			return new MotoComTagsDTO(
					moto.getIdMoto(),
					moto.getPlaca(),
					moto.getModelo(),
					moto.getFabricante(),
					moto.getAno(),
					moto.getLocalizacaoAtual(),
					moto.getPatio().getIdPatio(),
					tags
			);
		}).collect(Collectors.toList());
	}
}
