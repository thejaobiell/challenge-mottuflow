package com.sprint.MottuFlow.domain.camera;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.util.List;

public interface CameraRepository extends JpaRepository<Camera, Long> {

    @Query("SELECT c FROM Camera c WHERE LOWER(c.statusOperacional) = LOWER(:status)")
    List<Camera> findByStatusOperacional(@Param("status") String status);

    @Query(value = "SELECT * FROM camera WHERE localizacao_fisica LIKE %:localizacao%", nativeQuery = true)
    List<Camera> findByLocalizacaoFisica(@Param("localizacao") String localizacao);
}
