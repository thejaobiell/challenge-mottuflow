package com.sprint.MottuFlow.domain.autenticao;
import java.time.LocalDateTime;

public record DadosToken(String tokenAcesso, String refreshToken, LocalDateTime expiracaoRefreshToken ) {}
