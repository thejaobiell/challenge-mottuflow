package com.sprint.MottuFlow.infra.exception;

public class RegraDeNegocioException extends RuntimeException {
    public RegraDeNegocioException( String message) {
        super(message);
    }
}
