package com.sprint.MottuFlow.domain.autenticao;

import jakarta.validation.constraints.NotBlank;

public record DadosRefreshToken( @NotBlank String refreshToken) {
}