/*
    ============================================================
    SCRIPT DE CRIAÇÃO E POPULAÇÃO DO BANCO DE DADOS - MOTTUFLOW
    ============================================================
    Banco: MySQL 8.x
    Observação: Esse script contém as DDLs + alguns INSERTs
    obrigatórios para testes do CRUD e da aplicação.
*/

-- =========================
-- Tabela: FUNCIONARIO
-- Armazena os dados dos funcionários que atuam no sistema,
-- incluindo mecânicos, gerentes e administradores.
-- =========================
DROP DATABASE IF EXISTS mottuflow;
CREATE DATABASE mottuflow;
USE mottuflow;

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

INSERT INTO funcionario (nome, cpf, cargo, telefone, email, senha) VALUES
('CONTA ADMIN', '000.000.000-00', 'ADMIN', '(00) 00000-0000', 'admin@email.com', '$2a$12$HkHTbCOCrUW55EXH8MjZfO.8MpjpyKWsVd.4oM1xCbceqtCpaqOFK'),
('João Mecânico', '111.111.111-11', 'MECANICO', '(11) 11111-1111', 'joao@email.com', '$2a$12$WvSaeLKOeMhnaT65b1cHaeHQKyOz5M0cNDDNZ0eDbdmeLqoYbnFhi'),
('Maria Gerente', '222.222.222-22', 'GERENTE', '(22) 22222-2222', 'maria@email.com', '$2a$12$AymGyslp9tPClu/sIsfaNOVcAgUEdWXfrqG8liodWVIczQA9XYJ/e');

-- =========================
-- Tabela: PATIO
-- Representa os pátios físicos onde as motos ficam estacionadas.
-- =========================
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

-- =========================
-- Tabela: MOTO
-- Cadastro das motos, associadas a um pátio.
-- =========================
CREATE TABLE moto (
    id_moto BIGINT PRIMARY KEY AUTO_INCREMENT,
    placa VARCHAR(10) NOT NULL,
    modelo VARCHAR(50) NOT NULL,
    fabricante VARCHAR(50) NOT NULL,
    ano INT NOT NULL,
    id_patio BIGINT NOT NULL,
    localizacao_atual VARCHAR(100) NOT NULL,
    FOREIGN KEY (id_patio) REFERENCES patio(id_patio)
        ON DELETE RESTRICT
        ON UPDATE CASCADE
);

INSERT INTO moto (placa, modelo, fabricante, ano, id_patio, localizacao_atual) VALUES
('ABC-1234', 'Mottu Pop', 'Honda', 2022, 1, 'Vaga 01'),
('DEF-5678', 'Mottu Sport', 'TVS', 2021, 1, 'Vaga 02'),
('GHI-9012', 'Mottu E', 'Yadea', 2023, 2, 'Vaga 01'),
('JKL-3456', 'Mottu Pop', 'Honda', 2022, 3, 'Vaga 03');

-- =========================
-- Tabela: CAMERA
-- Representa câmeras de vigilância vinculadas a um pátio.
-- =========================
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

-- =========================
-- Tabela: ARUCO_TAG
-- Identificação por tags (ex: QRCode, RFID) associadas às motos.
-- =========================
CREATE TABLE aruco_tag (
    id_tag BIGINT PRIMARY KEY AUTO_INCREMENT,
    codigo VARCHAR(50) NOT NULL,
    status VARCHAR(20) NOT NULL,
    id_moto BIGINT NOT NULL,
    FOREIGN KEY (id_moto) REFERENCES moto(id_moto)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

INSERT INTO aruco_tag (codigo, status, id_moto) VALUES
('TAG001', 'ATIVO', 1),
('TAG002', 'ATIVO', 2),
('TAG003', 'INATIVO', 3),
('TAG004', 'ATIVO', 4);

-- =========================
-- Tabela: LOCALIDADE
-- Histórico de localizações das motos nos pátios, 
-- cruzando moto, pátio e câmera.
-- =========================
CREATE TABLE localidade (
    id_localidade BIGINT PRIMARY KEY AUTO_INCREMENT,
    data_hora DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ponto_referencia VARCHAR(100) NOT NULL,
    id_moto BIGINT NOT NULL,
    id_patio BIGINT NOT NULL,
    id_camera BIGINT NOT NULL,
    FOREIGN KEY (id_moto) REFERENCES moto(id_moto)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    FOREIGN KEY (id_patio) REFERENCES patio(id_patio)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    FOREIGN KEY (id_camera) REFERENCES camera(id_camera)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

INSERT INTO localidade (ponto_referencia, id_moto, id_patio, id_camera) VALUES
('Entrada principal', 1, 1, 1),
('Saída norte', 2, 1, 2),
('Corredor lateral', 3, 2, 3),
('Vaga sul', 4, 3, 4);

-- =========================
-- Tabela: REGISTRO_STATUS
-- Registra o status de cada moto (disponível, manutenção, etc.)
-- vinculado a um funcionário responsável.
-- =========================
CREATE TABLE registro_status (
    id_status BIGINT PRIMARY KEY AUTO_INCREMENT,
    tipo_status VARCHAR(50) NOT NULL,
    descricao VARCHAR(255),
    data_status DATETIME DEFAULT CURRENT_TIMESTAMP,
    id_moto BIGINT NOT NULL,
    id_funcionario BIGINT NOT NULL,
    FOREIGN KEY (id_moto) REFERENCES moto(id_moto)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    FOREIGN KEY (id_funcionario) REFERENCES funcionario(id_funcionario)
        ON DELETE RESTRICT
        ON UPDATE CASCADE
);

INSERT INTO registro_status (tipo_status, descricao, id_moto, id_funcionario) VALUES
('DISPONIVEL', 'Moto pronta para uso', 1, 1),
('MANUTENCAO', 'Troca de óleo', 2, 2),
('RESERVADO', 'Reservada para cliente', 3, 2),
('BAIXA_BOLETIM_OCORRENCIA', 'Perda por BO', 4, 1);

