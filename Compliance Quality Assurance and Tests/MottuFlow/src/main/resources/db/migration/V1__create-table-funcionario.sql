CREATE TABLE funcionario (
    id_funcionario BIGINT PRIMARY KEY AUTO_INCREMENT,
    nome VARCHAR(100) NOT NULL,
    cpf VARCHAR(14) NOT NULL UNIQUE,
    cargo VARCHAR(50) NOT NULL,
    telefone VARCHAR(20) NOT NULL,
    email VARCHAR(50) NOT NULL UNIQUE,
    senha VARCHAR(255) NOT NULL,
    refresh_token VARCHAR(255) DEFAULT NULL,
    expiracao_refresh_token DATETIME DEFAULT NULL
);

INSERT INTO funcionario (
    nome,
    cpf,
    cargo,
    telefone,
    email,
    senha,
    refresh_token,
    expiracao_refresh_token
) VALUES
('CONTA ADMIN', '000.000.000-00', 'ADMIN', '(00) 00000-0000', 'admin@email.com', '$2a$12$HkHTbCOCrUW55EXH8MjZfO.8MpjpyKWsVd.4oM1xCbceqtCpaqOFK', NULL, NULL),
('João Mecânico', '111.111.111-11', 'MECANICO', '(11) 11111-1111', 'joao@email.com', '$2a$12$WvSaeLKOeMhnaT65b1cHaeHQKyOz5M0cNDDNZ0eDbdmeLqoYbnFhi', NULL, NULL),
('Maria Gerente', '222.222.222-22', 'GERENTE', '(22) 22222-2222', 'maria@email.com', '$2a$12$AymGyslp9tPClu/sIsfaNOVcAgUEdWXfrqG8liodWVIczQA9XYJ/e', NULL, NULL);