package com.sprint.MottuFlow.domain.moto;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.util.List;

public interface MotoRepository extends JpaRepository<Moto, Long> {

    @Query("SELECT m FROM Moto m WHERE LOWER(m.fabricante) = LOWER(:fabricante)")
    List<Moto> findByFabricante(@Param("fabricante") String fabricante);
    
    @Query(value = "SELECT * FROM moto WHERE id_patio = :idPatio", nativeQuery = true)
    List<Moto> findByPatioId(@Param("idPatio") long idPatio);
	
	@Query("SELECT m FROM Moto m WHERE LOWER(m.placa) LIKE LOWER(CONCAT('%', :placa, '%'))")
	List<Moto> findByPlacaContaining(@Param("placa") String placa);
	
	@Query("SELECT m FROM Moto m WHERE LOWER(m.modelo) LIKE LOWER(CONCAT('%', :modelo, '%'))")
	List<Moto> findByModeloContainingIgnoreCase(@Param("modelo") String modelo);
}