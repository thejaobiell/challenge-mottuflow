package com.sprint.MottuFlow.domain.status;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.time.LocalDateTime;
import java.util.List;
import java.util.Optional;

public interface StatusRepository extends JpaRepository<Status, Long> {
	
	@Query("SELECT s FROM Status s WHERE s.tipoStatus = :tipoStatus")
	List<Status> findByTipoStatus(@Param("tipoStatus") TipoStatus tipoStatus);
	
	@Query(value = "SELECT * FROM registro_status WHERE descricao LIKE %:descricao%", nativeQuery = true)
	List<Status> findByDescricao(@Param("descricao") String descricao);
	
	@Query("SELECT s FROM Status s WHERE s.dataStatus BETWEEN :inicio AND :fim")
	List<Status> findByPeriodo( @Param("inicio") LocalDateTime inicio, @Param("fim") LocalDateTime fim);
	
	@Query("SELECT s FROM Status s WHERE s.moto.idMoto = :idMoto ORDER BY s.dataStatus DESC")
	Optional<Status> findFirstByMotoIdOrderByDataStatusDesc( @Param("idMoto") Long idMoto);
}

