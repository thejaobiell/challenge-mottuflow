package com.sprint.MottuFlow.domain.funcionario;

import java.util.Optional;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

public interface FuncionarioRepository extends JpaRepository<Funcionario, Long> {
	
	@Query( value = "SELECT * FROM funcionario WHERE cpf = :cpf", nativeQuery = true )
	Funcionario findByCpfNative( @Param( "cpf" ) String cpf );
	
	Optional<Funcionario> findByEmailIgnoreCase( String email );
	
	Optional<Funcionario> findByRefreshToken( String refreshToken );
}
