package com.sprint.MottuFlow.domain.patio;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.util.List;

public interface PatioRepository extends JpaRepository<Patio, Long> {
}
