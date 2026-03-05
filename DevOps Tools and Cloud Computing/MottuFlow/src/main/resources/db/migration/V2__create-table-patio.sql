CREATE TABLE patio (
    id_patio BIGINT PRIMARY KEY AUTO_INCREMENT,
    nome VARCHAR(100) NOT NULL,
    endereco VARCHAR(200) NOT NULL,
    capacidade_maxima INT NOT NULL
);

INSERT INTO patio (nome, endereco, capacidade_maxima) VALUES
('Patio Central', 'Rua Principal, 1000', 50),
('Patio Norte', 'Av. Norte, 250', 30),
('Patio Sul', 'Av. Sul, 500', 40);
