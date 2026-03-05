package com.sprint.MottuFlow.domain.status;

import com.sprint.MottuFlow.infra.exception.RegraDeNegocioException;
import com.sprint.MottuFlow.domain.funcionario.Funcionario;
import com.sprint.MottuFlow.domain.funcionario.FuncionarioRepository;
import com.sprint.MottuFlow.domain.moto.Moto;
import com.sprint.MottuFlow.domain.moto.MotoRepository;
import jakarta.transaction.Transactional;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.List;

@Service
public class StatusService {
	
	private final StatusRepository sR;
	private final MotoRepository mR;
	private final FuncionarioRepository fR;
	
	public StatusService( StatusRepository sR, MotoRepository mR, FuncionarioRepository fR ) {
		this.sR = sR;
		this.mR = mR;
		this.fR = fR;
	}
	
	public List<Status> listarStatus() {
		return sR.findAll();
	}
	
	public Status buscarStatusPorId(Long id) {
		return sR.findById(id)
				.orElseThrow(() -> new RegraDeNegocioException("Status não encontrado com id: " + id));
	}
	
	public List<Status> buscarPorTipoStatus(TipoStatus tipoStatus) {
		return sR.findByTipoStatus(tipoStatus);
	}
	
	public List<Status> buscarPorDescricaoStatus(String descricao) {
		return sR.findByDescricao(descricao);
	}
	
	public List<Status> buscarPorPeriodo(LocalDateTime inicio, LocalDateTime fim) {
		return sR.findByPeriodo(inicio, fim);
	}
	
	public Status buscarPorMoto(Long idMoto) {
		return sR.findFirstByMotoIdOrderByDataStatusDesc(idMoto)
				.orElseThrow(() -> new RegraDeNegocioException("Nenhum registro de status encontrado para a moto com id: " + idMoto));
	}
	
	@Transactional
	public Status cadastrarStatus(Status status) {
		if (status.getMoto() == null || status.getMoto().getIdMoto() == null) {
			throw new RegraDeNegocioException("Moto é obrigatória!");
		}
		if (status.getFuncionario() == null || status.getFuncionario().getId_funcionario() == null) {
			throw new RegraDeNegocioException("Funcionario é obrigatório!");
		}
		
		Moto moto = mR.findById(status.getMoto().getIdMoto())
				.orElseThrow(() -> new RegraDeNegocioException("Moto não encontrada com id: " + status.getMoto().getIdMoto()));
		
		Funcionario funcionario = fR.findById(status.getFuncionario().getId_funcionario())
				.orElseThrow(() -> new RegraDeNegocioException("Funcionario não encontrado com id: " + status.getFuncionario().getId_funcionario()));
		
		status.setMoto(moto);
		status.setFuncionario(funcionario);
		
		return sR.save(status);
	}
	
	@Transactional
	public Status editarStatus(Long id, Status statusAtualizado) {
		Status status = buscarStatusPorId(id);
		
		if (statusAtualizado.getTipoStatus() != null) {
			status.setTipoStatus(statusAtualizado.getTipoStatus());
		}
		
		if (statusAtualizado.getDescricao() != null && !statusAtualizado.getDescricao().isBlank()) {
			status.setDescricao(statusAtualizado.getDescricao());
		}
		
		if (statusAtualizado.getDataStatus() != null) {
			status.setDataStatus(statusAtualizado.getDataStatus());
		}
		
		if (statusAtualizado.getMoto() != null && statusAtualizado.getMoto().getIdMoto() != null) {
			Moto moto = mR.findById(statusAtualizado.getMoto().getIdMoto())
					.orElseThrow(() -> new RegraDeNegocioException("Moto não encontrada com id: " + statusAtualizado.getMoto().getIdMoto()));
			status.setMoto(moto);
		}
		
		if (statusAtualizado.getFuncionario() != null && statusAtualizado.getFuncionario().getId_funcionario() != null) {
			Funcionario funcionario = fR.findById(statusAtualizado.getFuncionario().getId_funcionario())
					.orElseThrow(() -> new RegraDeNegocioException("Funcionario não encontrado com id: " + statusAtualizado.getFuncionario().getId_funcionario()));
			status.setFuncionario(funcionario);
		}
		
		return sR.save(status);
	}
	
	@Transactional
	public void deletarStatus(Long id) {
		Status status = buscarStatusPorId(id);
		sR.delete(status);
	}
}
