package com.sprint.MottuFlow.domain.arucotag;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.util.List;

public interface ArucoTagRepository extends JpaRepository<ArucoTag, Long> {

    @Query("SELECT a FROM ArucoTag a WHERE LOWER(a.status) = LOWER(:status)")
    List<ArucoTag> findByStatus(@Param("status") String status);

    @Query(value = "SELECT * FROM aruco_tag WHERE codigo = :codigo", nativeQuery = true)
    ArucoTag findByCodigoNative(@Param("codigo") String codigo);
}
