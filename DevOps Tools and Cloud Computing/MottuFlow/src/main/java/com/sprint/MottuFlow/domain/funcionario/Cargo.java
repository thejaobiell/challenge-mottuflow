package com.sprint.MottuFlow.domain.funcionario;

public enum Cargo {
	MECANICO(1),
	GERENTE(2),
	ADMIN(3);
	
	private final int nivel;
	
	Cargo(int nivel) {
		this.nivel = nivel;
	}
	
	public int getNivel() {
		return nivel;
	}
}
