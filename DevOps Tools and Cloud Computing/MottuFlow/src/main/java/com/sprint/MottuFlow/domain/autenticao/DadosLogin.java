package com.sprint.MottuFlow.domain.autenticao;

import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotBlank;

public record DadosLogin( @NotBlank @Email String email, @NotBlank String senha ) {}