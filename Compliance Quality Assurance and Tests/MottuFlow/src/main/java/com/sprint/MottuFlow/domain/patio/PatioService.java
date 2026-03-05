package com.sprint.MottuFlow.domain.patio;

import com.sprint.MottuFlow.infra.exception.RegraDeNegocioException;
import jakarta.transaction.Transactional;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class PatioService {
	
	private final PatioRepository pR;
	
	public PatioService(PatioRepository pR ) {
		this.pR = pR;
	}
	
	public List<Patio> listarPatios() {
		return pR.findAll();
	}
	
	public Patio buscarPatioPorId(Long id) {
		return pR.findById(id)
				.orElseThrow(() -> new RegraDeNegocioException("Pátio não encontrado com id: " + id));
	}
	
	@Transactional
	public Patio cadastrarPatio(Patio patio) {
		return pR.save(patio);
	}
	
	@Transactional
	public Patio editarPatio(Long id, Patio patioAtualizado) {
		Patio patio = buscarPatioPorId(id);
		
		patio.setNome(patioAtualizado.getNome());
		patio.setEndereco(patioAtualizado.getEndereco());
		patio.setCapacidadeMaxima(patioAtualizado.getCapacidadeMaxima());
		
		return pR.save(patio);
	}
	
	@Transactional
	public void deletarPatio(Long id) {
		Patio patio = buscarPatioPorId(id);
		pR.delete(patio);
	}
}
