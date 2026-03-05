package com.sprint.MottuFlow.domain.funcionario;

import com.sprint.MottuFlow.infra.exception.RegraDeNegocioException;
import jakarta.transaction.Transactional;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.List;

@Service
public class FuncionarioService implements UserDetailsService {
	
	private final FuncionarioRepository repository;
	private final PasswordEncoder encoder;
	
	public FuncionarioService(FuncionarioRepository repository, PasswordEncoder encoder) {
		this.repository = repository;
		this.encoder = encoder;
	}
	
	public List<Funcionario> listarFuncionarios() {
		return repository.findAll();
	}
	
	public Funcionario buscarFuncionarioPorId(Long id) {
		return repository.findById(id)
				.orElseThrow(() -> new RegraDeNegocioException("Funcionário não encontrado com id: " + id));
	}
	
	public Funcionario buscarFuncionarioPorEmail(String email) {
		return repository.findByEmailIgnoreCase(email)
				.orElseThrow(() -> new RegraDeNegocioException("Funcionário não encontrado com email: " + email));
	}
	
	@Transactional
	public Funcionario buscarFuncionarioPorCPF(String cpf) {
		return repository.findByCpfNative(cpf);
	}
	
	@Transactional
	public Funcionario cadastrarFuncionario(Funcionario funcionario) {
		if (funcionario.getCargo() == null) {
			throw new RegraDeNegocioException("Cargo não pode ser nulo!");
		}
		funcionario.setSenha(encoder.encode(funcionario.getSenha()));
		return repository.save(funcionario);
	}
	
	@Transactional
	public Funcionario editarFuncionario(Long id, Funcionario funcionarioAtualizado) {
		Funcionario funcionario = buscarFuncionarioPorId(id);
		
		if (funcionarioAtualizado.getNome() != null && !funcionarioAtualizado.getNome().isBlank())
			funcionario.setNome(funcionarioAtualizado.getNome());
		
		if (funcionarioAtualizado.getCpf() != null && !funcionarioAtualizado.getCpf().isBlank())
			funcionario.setCpf(funcionarioAtualizado.getCpf());
		
		if (funcionarioAtualizado.getCargo() != null)
			funcionario.setCargo(funcionarioAtualizado.getCargo());
		
		if (funcionarioAtualizado.getTelefone() != null && !funcionarioAtualizado.getTelefone().isBlank())
			funcionario.setTelefone(funcionarioAtualizado.getTelefone());
		
		if (funcionarioAtualizado.getEmail() != null && !funcionarioAtualizado.getEmail().isBlank())
			funcionario.setEmail(funcionarioAtualizado.getEmail());
		
		if (funcionarioAtualizado.getSenha() != null && !funcionarioAtualizado.getSenha().isBlank())
			funcionario.setSenha(encoder.encode(funcionarioAtualizado.getSenha()));
		
		return repository.save(funcionario);
	}
	
	@Transactional
	public void deletarFuncionario(Long id) {
		Funcionario funcionario = buscarFuncionarioPorId(id);
		repository.delete(funcionario);
	}
	
	@Override
	public UserDetails loadUserByUsername(String email) throws UsernameNotFoundException {
		return repository.findByEmailIgnoreCase(email)
				.orElseThrow(() -> new UsernameNotFoundException("Usuário não encontrado!"));
	}
	
	@Transactional
	public void atualizarRefreshTokenFuncionario(Funcionario funcionario, String refreshToken, LocalDateTime expiracao) {
		funcionario.setRefreshToken(refreshToken);
		funcionario.setExpiracaoRefreshToken(expiracao);
		repository.save(funcionario);
	}
	
	@Transactional
	public Funcionario validarRefreshTokenFuncionario(String refreshToken) {
		Funcionario funcionario = repository.findByRefreshToken(refreshToken)
				.orElseThrow(() -> new RegraDeNegocioException("Refresh token inválido!"));
		
		if (funcionario.getExpiracaoRefreshToken().isBefore(LocalDateTime.now())) {
			throw new RegraDeNegocioException("Refresh token expirado!");
		}
		
		return funcionario;
	}
	
	@Transactional
	public void alterarSenha(String email, String senhaAtual, String novaSenha) {
		Funcionario funcionario = buscarFuncionarioPorEmail(email);
		
		if (!encoder.matches(senhaAtual, funcionario.getSenha())) {
			throw new RegraDeNegocioException("Senha atual incorreta!");
		}
		
		funcionario.setSenha(encoder.encode(novaSenha));
		repository.save(funcionario);
	}
}
