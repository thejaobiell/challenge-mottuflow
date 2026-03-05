CREATE TABLE camera (
    id_camera BIGINT PRIMARY KEY AUTO_INCREMENT,
    status_operacional VARCHAR(20) NOT NULL,
    localizacao_fisica VARCHAR(255) NOT NULL,
    id_patio BIGINT NOT NULL,
    FOREIGN KEY (id_patio) REFERENCES patio(id_patio)
        ON DELETE RESTRICT
        ON UPDATE CASCADE
);

INSERT INTO camera (status_operacional, localizacao_fisica, id_patio) VALUES
('OPERACIONAL', 'Entrada principal', 1),
('OPERACIONAL', 'Saída norte', 1),
('MANUTENCAO', 'Corredor lateral', 2),
('OPERACIONAL', 'Entrada sul', 3);