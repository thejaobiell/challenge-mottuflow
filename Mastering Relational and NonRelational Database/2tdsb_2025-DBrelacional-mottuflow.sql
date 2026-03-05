    
/* ============================================================
                     MOTTU-FLOW
Joao Gabriel Boaventura Marques e Silva | RM554874 | 2TDSB-2025
Leo Motta Lima                          | RM557851 | 2TDSB-2025
Lucas Leal das Chagas                   | RM551124 | 2TDSB-2025
============================================================ */

SET SERVEROUTPUT ON;
SET VERIFY OFF;

-- ============================================================
-- DROPS
-- ============================================================
DROP TABLE fato_motos_status CASCADE CONSTRAINTS;
DROP TABLE auditoria CASCADE CONSTRAINTS;
DROP TABLE registro_status CASCADE CONSTRAINTS;
DROP TABLE localidade CASCADE CONSTRAINTS;
DROP TABLE aruco_tag CASCADE CONSTRAINTS;
DROP TABLE camera CASCADE CONSTRAINTS;
DROP TABLE moto CASCADE CONSTRAINTS;
DROP TABLE patio CASCADE CONSTRAINTS;
DROP TABLE funcionario CASCADE CONSTRAINTS;

-- ============================================================
-- TABELAS
-- ============================================================

CREATE TABLE funcionario (
    id_funcionario          NUMBER(10) PRIMARY KEY,
    nome                    VARCHAR2(100) NOT NULL,
    cpf                     VARCHAR2(14) NOT NULL UNIQUE,
    cargo                   VARCHAR2(50) NOT NULL,
    telefone                VARCHAR2(20) NOT NULL,
    email                   VARCHAR2(50) NOT NULL UNIQUE,
    senha                   VARCHAR2(255) NOT NULL,
    refresh_token           VARCHAR2(255) DEFAULT NULL,
    expiracao_refresh_token TIMESTAMP DEFAULT NULL
);

CREATE TABLE patio (
    id_patio         NUMBER(10) PRIMARY KEY,
    nome             VARCHAR2(100) NOT NULL,
    endereco         VARCHAR2(200) NOT NULL,
    capacidade_maxima NUMBER NOT NULL
);

CREATE TABLE moto (
    id_moto            NUMBER(10) PRIMARY KEY,
    placa              VARCHAR2(10) NOT NULL,
    modelo             VARCHAR2(50) NOT NULL,
    fabricante         VARCHAR2(50) NOT NULL,
    ano                NUMBER(4) NOT NULL,
    id_patio           NUMBER(10) NOT NULL,
    localizacao_atual  VARCHAR2(100) NOT NULL,
    CONSTRAINT fk_moto_patio FOREIGN KEY (id_patio)
        REFERENCES patio(id_patio)
);

CREATE TABLE camera (
    id_camera          NUMBER(10) PRIMARY KEY,
    status_operacional VARCHAR2(20) NOT NULL,
    localizacao_fisica VARCHAR2(255) NOT NULL,
    id_patio           NUMBER(10) NOT NULL,
    CONSTRAINT fk_camera_patio FOREIGN KEY (id_patio)
        REFERENCES patio(id_patio)
);

CREATE TABLE aruco_tag (
    id_tag   NUMBER(10) PRIMARY KEY,
    codigo   VARCHAR2(50) NOT NULL,
    status   VARCHAR2(20) NOT NULL,
    id_moto  NUMBER(10) NOT NULL,
    CONSTRAINT fk_aruco_moto FOREIGN KEY (id_moto)
        REFERENCES moto(id_moto)
);

CREATE TABLE localidade (
    id_localidade  NUMBER(10) PRIMARY KEY,
    data_hora      TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
    ponto_referencia VARCHAR2(100) NOT NULL,
    id_moto        NUMBER(10) NOT NULL,
    id_patio       NUMBER(10) NOT NULL,
    id_camera      NUMBER(10) NOT NULL,
    CONSTRAINT fk_localidade_moto FOREIGN KEY (id_moto) REFERENCES moto(id_moto),
    CONSTRAINT fk_localidade_patio FOREIGN KEY (id_patio) REFERENCES patio(id_patio),
    CONSTRAINT fk_localidade_camera FOREIGN KEY (id_camera) REFERENCES camera(id_camera)
);

CREATE TABLE registro_status (
    id_status     NUMBER(10) PRIMARY KEY,
    tipo_status   VARCHAR2(50) NOT NULL,
    descricao     VARCHAR2(255),
    data_status   TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    id_moto       NUMBER(10) NOT NULL,
    id_funcionario NUMBER(10) NOT NULL,
    CONSTRAINT fk_status_moto FOREIGN KEY (id_moto) REFERENCES moto(id_moto),
    CONSTRAINT fk_status_funcionario FOREIGN KEY (id_funcionario) REFERENCES funcionario(id_funcionario)
);

CREATE TABLE auditoria (
    id_auditoria      NUMBER(10) PRIMARY KEY,
    nome_usuario      VARCHAR2(50),
    tipo_operacao     VARCHAR2(20),
    data_hora         TIMESTAMP,
    valores_anteriores CLOB,
    valores_novos      CLOB
);

CREATE TABLE fato_motos_status (
    id_fato     NUMBER(10) PRIMARY KEY,
    id_patio    NUMBER(10) NOT NULL,
    tipo_status VARCHAR2(50),
    quantidade  NUMBER,
    CONSTRAINT fk_patio FOREIGN KEY (id_patio) REFERENCES patio(id_patio)
);


-- ============================================================
-- SEQUENCES
-- ============================================================
DROP SEQUENCE seq_registro_status;
DROP SEQUENCE seq_localidade;
DROP SEQUENCE auditoria_seq;

CREATE SEQUENCE seq_registro_status START WITH 6 INCREMENT BY 1;
CREATE SEQUENCE seq_localidade START WITH 6 INCREMENT BY 1;
CREATE SEQUENCE auditoria_seq START WITH 1 INCREMENT BY 1;

-- ============================================================
-- INSERTS
-- ============================================================

INSERT INTO funcionario VALUES (1, 'CONTA ADMIN', '00000000000', 'ADMIN', '00000000000', 'admin@email.com', '$2a$12$e6QJwFNdnau90pQN/3OkFeelAPVi8nCsJped.YQYxEy/573E1nR1G', NULL, NULL);
INSERT INTO funcionario VALUES (2, 'Joao Silva', '12345678900', 'Gerente', '11999990001', 'joao.silva@email.com', '$2a$12$P7BetbmT3nap8KkA/sD4aOHD8SI2JHSgkUrCuuzljXtd.wtpbEFHC', NULL, NULL);
INSERT INTO funcionario VALUES (3, 'Maria Souza', '22345678900', 'Tecnico', '11999990002', 'maria.souza@email.com', '$2a$12$P7BetbmT3nap8KkA/sD4aOHD8SI2JHSgkUrCuuzljXtd.wtpbEFHC', NULL, NULL);
INSERT INTO funcionario VALUES (4, 'Carlos Lima', '32345678900', 'Mecânico', '11999990003', 'carlos.lima@email.com', '$2a$12$P7BetbmT3nap8KkA/sD4aOHD8SI2JHSgkUrCuuzljXtd.wtpbEFHC', NULL, NULL);
INSERT INTO funcionario VALUES (5, 'Ana Costa', '42345678900', 'Supervisor', '11999990004', 'ana.costa@email.com', 'senha123', NULL, NULL);
--Senha dos funcionarios: senha123
--Senha do ADMIN: adminmottu
--Utilizei o Bcrypt Hash para encriptar a senha
--(Rounds/Cost Factor 12)   

INSERT INTO patio VALUES (1, 'Patio Central', 'Rua A 123', 50);
INSERT INTO patio VALUES (2, 'Patio Norte', 'Rua B 456', 30);
INSERT INTO patio VALUES (3, 'Patio Sul', 'Rua C 789', 40);
INSERT INTO patio VALUES (4, 'Patio Leste', 'Rua D 321', 20);
INSERT INTO patio VALUES (5, 'Patio Oeste', 'Rua E 654', 25);

INSERT INTO moto VALUES (1, 'ABC1234', 'Mottu Pop', 'Honda', 2020, 1, 'Rua A');
INSERT INTO moto VALUES (2, 'DEF5678', 'Mottu Sport', 'TVS', 2021, 2, 'Rua B');
INSERT INTO moto VALUES (3, 'GHI9012', 'Mottu E', 'Yadea', 2019, 3, 'Rua C');
INSERT INTO moto VALUES (4, 'JKL3456', 'Mottu Pop', 'Honda', 2022, 4, 'Rua D');
INSERT INTO moto VALUES (5, 'MNO7890', 'Mottu Pop', 'Honda', 2023, 5, 'Rua E');

INSERT INTO camera VALUES (1, 'Operacional', 'Entrada Patio Central', 1);
INSERT INTO camera VALUES (2, 'Manutencao', 'Saida Patio Norte', 2);
INSERT INTO camera VALUES (3, 'Operacional', 'Corredor Patio Sul', 3);
INSERT INTO camera VALUES (4, 'Inoperante', 'Portao Patio Leste', 4);
INSERT INTO camera VALUES (5, 'Operacional', 'Garagem Patio Oeste', 5);

INSERT INTO aruco_tag VALUES (1, 'TAG001', 'Ativo', 1);
INSERT INTO aruco_tag VALUES (2, 'TAG002', 'Ativo', 2);
INSERT INTO aruco_tag VALUES (3, 'TAG003', 'Inativo', 3);
INSERT INTO aruco_tag VALUES (4, 'TAG004', 'Ativo', 4);
INSERT INTO aruco_tag VALUES (5, 'TAG005', 'Manutencao', 5);

INSERT INTO localidade VALUES (1, TIMESTAMP '2025-05-16 08:00:00', 'Portao A', 1, 1, 1);
INSERT INTO localidade VALUES (2, TIMESTAMP '2025-05-16 08:10:00', 'Portao B', 2, 2, 2);
INSERT INTO localidade VALUES (3, TIMESTAMP '2025-05-16 08:20:00', 'Portao C', 3, 3, 3);
INSERT INTO localidade VALUES (4, TIMESTAMP '2025-05-16 08:30:00', 'Portao D', 4, 4, 4);
INSERT INTO localidade VALUES (5, TIMESTAMP '2025-05-16 08:40:00', 'Portao E', 5, 5, 5);

INSERT INTO registro_status VALUES (1, 'DISPONIVEL', 'Moto pronta para uso.', DEFAULT, 1, 1);
INSERT INTO registro_status VALUES (2, 'MANUTENCAO', 'Aguardando documentacao.', DEFAULT, 2, 3);
INSERT INTO registro_status VALUES (3, 'MANUTENCAO', 'Revisao geral.', DEFAULT, 3, 2);
INSERT INTO registro_status VALUES (4, 'RESERVADO', 'Reservada via app.', DEFAULT, 4, 4);
INSERT INTO registro_status VALUES (5, 'BAIXA_BOLETIM_OCORRENCIA', 'Moto furtada.', DEFAULT, 5, 1);

INSERT INTO fato_motos_status VALUES (1, 1, 'Disponivel', 10);
INSERT INTO fato_motos_status VALUES (2, 1, 'Manutencao', 5);
INSERT INTO fato_motos_status VALUES (3, 2, 'Disponivel', 8);
INSERT INTO fato_motos_status VALUES (4, 2, 'Reservado', 3);
INSERT INTO fato_motos_status VALUES (5, 3, 'Disponivel', 7);
INSERT INTO fato_motos_status VALUES (6, 3, 'Manutencao', 2);
INSERT INTO fato_motos_status VALUES (7, 4, 'Reservado', 4);
INSERT INTO fato_motos_status VALUES (8, 4, 'Disponivel', 6);
INSERT INTO fato_motos_status VALUES (9, 5, 'Manutencao', 1);
INSERT INTO fato_motos_status VALUES (10, 5, 'Disponivel', 9);

COMMIT;

-- ============================================================
-- TRIGGER DE AUDITORIA
-- ============================================================

CREATE OR REPLACE TRIGGER auditoria_moto_mottuflow
AFTER INSERT OR UPDATE OR DELETE ON moto
FOR EACH ROW
DECLARE
    v_tipo VARCHAR2(10);
    v_old  CLOB := NULL;
    v_new  CLOB := NULL;
BEGIN
    IF INSERTING THEN
        v_tipo := 'INSERT';
        v_new  := TO_CLOB('{"id_moto":' || :NEW.id_moto || ',"placa":"' || NVL(:NEW.placa, '') || '"}');
        DBMS_OUTPUT.PUT_LINE('Trigger: inserindo moto ID ' || :NEW.id_moto || ' com placa ' || :NEW.placa);
    ELSIF UPDATING THEN
        v_tipo := 'UPDATE';
        v_old  := TO_CLOB('{"placa_antiga":"' || NVL(:OLD.placa, '') || '"}');
        v_new  := TO_CLOB('{"placa_nova":"' || NVL(:NEW.placa, '') || '"}');
        DBMS_OUTPUT.PUT_LINE('Trigger: atualizando moto ID ' || :OLD.id_moto || ' de ' || :OLD.placa || ' para ' || :NEW.placa);
    ELSIF DELETING THEN
        v_tipo := 'DELETE';
        v_old  := TO_CLOB('{"id_moto":' || :OLD.id_moto || '}');
        DBMS_OUTPUT.PUT_LINE('Trigger: removendo moto ID ' || :OLD.id_moto);
    END IF;

    INSERT INTO auditoria
    VALUES (auditoria_seq.NEXTVAL, USER, v_tipo, SYSTIMESTAMP, v_old, v_new);
END;
/

INSERT INTO moto (id_moto, placa, modelo, fabricante, ano, id_patio, localizacao_atual)
VALUES (10, 'ZZZ9999', 'Mottu Pop', 'Honda', 2025, 1, 'Rua Teste');

UPDATE moto SET placa = 'ZZZ0000' WHERE id_moto = 10;

DELETE FROM moto WHERE id_moto = 10;

-- ============================================================
-- PACOTE MOTTU-FLOW
-- ============================================================
DROP PACKAGE pct_mottuflow;

CREATE OR REPLACE PACKAGE pct_mottuflow AS
    PROCEDURE relatorio_funcionario_moto_status(p_id_funcionario IN NUMBER, p_json OUT CLOB);
    PROCEDURE soma_fato_motos;
    FUNCTION moto_to_json(p_id_moto IN NUMBER) RETURN CLOB;
    FUNCTION validar_senha(p_id_funcionario IN NUMBER, p_senha IN VARCHAR2) RETURN VARCHAR2;
END pct_mottuflow;
/

CREATE OR REPLACE PACKAGE BODY pct_mottuflow AS

        PROCEDURE relatorio_funcionario_moto_status(
        p_id_funcionario IN NUMBER,
        p_json OUT CLOB
    ) IS
    BEGIN
        SELECT JSON_OBJECT(
                   'id_funcionario' VALUE f.id_funcionario,
                   'nome' VALUE f.nome,
                   'cargo' VALUE f.cargo,
                   'motos' VALUE COALESCE(
                       JSON_ARRAYAGG(
                           JSON_OBJECT(
                               'id_moto' VALUE m.id_moto,
                               'placa' VALUE m.placa,
                               'status' VALUE rs.tipo_status
                           )
                       ),
                       JSON_ARRAY()
                   )
               )
        INTO p_json
        FROM funcionario f
        LEFT JOIN registro_status rs ON f.id_funcionario = rs.id_funcionario
        LEFT JOIN moto m ON rs.id_moto = m.id_moto
        WHERE f.id_funcionario = p_id_funcionario
        GROUP BY f.id_funcionario, f.nome, f.cargo;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN p_json := '{"erro":"Funcionario nao encontrado"}';
        WHEN OTHERS THEN p_json := '{"erro":"' || SQLERRM || '"}';
    END;

    FUNCTION validar_senha(p_id_funcionario IN NUMBER, p_senha IN VARCHAR2)
    RETURN VARCHAR2 IS
    v_senha_bd VARCHAR2(255);
    BEGIN
        SELECT senha INTO v_senha_bd
        FROM funcionario
        WHERE id_funcionario = p_id_funcionario;
    
        IF v_senha_bd = p_senha THEN
            RETURN 'Valido';
        ELSE
            RETURN 'Invalido';
        END IF;
    
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            RETURN 'Funcionario nao encontrado';
        WHEN TOO_MANY_ROWS THEN
            RETURN 'Mais de um funcionario retornado';
        WHEN OTHERS THEN
            RETURN 'Erro inesperado: ' || SQLERRM;
    END;

    PROCEDURE soma_fato_motos IS
        v_total NUMBER;
    BEGIN
        SELECT COUNT(*) INTO v_total FROM moto;
        DBMS_OUTPUT.PUT_LINE('Total de motos: ' || v_total);
    END;


    FUNCTION moto_to_json(p_id_moto IN NUMBER) RETURN CLOB IS
        v_json CLOB;
    BEGIN
        SELECT JSON_OBJECT(
                   'id_moto' VALUE m.id_moto,
                   'placa' VALUE m.placa,
                   'modelo' VALUE m.modelo,
                   'fabricante' VALUE m.fabricante,
                   'ano' VALUE m.ano,
                   'localizacao_atual' VALUE m.localizacao_atual
               )
        INTO v_json
        FROM moto m
        WHERE m.id_moto = p_id_moto;

        RETURN v_json;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN RETURN '{"erro":"Moto nao encontrada"}';
        WHEN OTHERS THEN RETURN '{"erro":"' || SQLERRM || '"}';
    END;

END pct_mottuflow;
/

-- ============================================================
-- TESTES
-- ============================================================

DECLARE
    v_json   CLOB;
    v_status VARCHAR2(50);
BEGIN
    DBMS_OUTPUT.PUT_LINE('--- TESTE relatorio_funcionario_moto_status ---');
    
    -- captura o JSON retornado
    pct_mottuflow.relatorio_funcionario_moto_status(1, v_json);
    DBMS_OUTPUT.PUT_LINE('Funcionario 1: ' || v_json);

    pct_mottuflow.relatorio_funcionario_moto_status(999, v_json);
    DBMS_OUTPUT.PUT_LINE('Funcionario 999: ' || v_json);

    DBMS_OUTPUT.PUT_LINE(CHR(10) || '--- TESTE validar_senha ---');
    v_status := pct_mottuflow.validar_senha(5, 'senha123');
    DBMS_OUTPUT.PUT_LINE('Resultado senha correta: ' || v_status);
    
    v_status := pct_mottuflow.validar_senha(1, 'senha_errada');
    DBMS_OUTPUT.PUT_LINE('Resultado senha incorreta: ' || v_status);
    
    v_status := pct_mottuflow.validar_senha(999, 'qualquer');
    DBMS_OUTPUT.PUT_LINE('Funcionario inexistente: ' || v_status);

    DBMS_OUTPUT.PUT_LINE(CHR(10) || '--- TESTE moto_to_json ---');
    v_json := pct_mottuflow.moto_to_json(1);
    DBMS_OUTPUT.PUT_LINE('Moto existente: ' || v_json);
    
    v_json := pct_mottuflow.moto_to_json(999);
    DBMS_OUTPUT.PUT_LINE('Moto inexistente: ' || v_json);

    DBMS_OUTPUT.PUT_LINE(CHR(10) || '--- TESTE soma_fato_motos ---');
    pct_mottuflow.soma_fato_motos;
END;
/

-- ============================================================
-- Procedure para gerar os arquivos .json para o MONGODB
-- ============================================================

CREATE OR REPLACE PROCEDURE exportar_tabela_json(p_tabela IN VARCHAR2) IS
    v_sql        VARCHAR2(4000);
    v_cols       VARCHAR2(4000);
    v_json       CLOB;
    v_nome_arquivo VARCHAR2(200);
BEGIN
SELECT LISTAGG(
        CASE WHEN ROWNUM = 1 THEN 
            '''_id'' VALUE ' || column_name
            ELSE '''' || column_name || ''' VALUE ' || column_name
        END, ', '
    ) WITHIN GROUP (ORDER BY column_id)
INTO v_cols
FROM user_tab_columns
WHERE table_name = UPPER(p_tabela);

    v_sql := 'SELECT JSON_ARRAYAGG(JSON_OBJECT(' || v_cols || ' RETURNING CLOB)) FROM ' || p_tabela;
    EXECUTE IMMEDIATE v_sql INTO v_json;

    v_nome_arquivo := LOWER(p_tabela) || '.json';
    DBMS_OUTPUT.PUT_LINE(v_json);

EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Erro ao exportar tabela ' || p_tabela || ': ' || SQLERRM);
END;
/

/* 
Para exportar as tabelas corretamente 
execute selecione essa sequência de SET
*/
SET SQLPROMPT '' 
SET FEEDBACK OFF 
SET HEADING OFF 
SET VERIFY OFF 
SET ECHO OFF 
SET TERMOUT OFF
SET SERVEROUTPUT ON
    
SPOOL funcionario.json
EXEC exportar_tabela_json('FUNCIONARIO');
SPOOL OFF;
    
SPOOL patio.json
EXEC exportar_tabela_json('PATIO');
SPOOL OFF;
    
SPOOL moto.json
EXEC exportar_tabela_json('MOTO');
SPOOL OFF;
    
SPOOL camera.json
EXEC exportar_tabela_json('CAMERA');
SPOOL OFF;
    
SPOOL aruco_tag.json
EXEC exportar_tabela_json('ARUCO_TAG');
SPOOL OFF;
    
SPOOL localidade.json
EXEC exportar_tabela_json('LOCALIDADE');
SPOOL OFF;
    
SPOOL registro_status.json
EXEC exportar_tabela_json('REGISTRO_STATUS');
SPOOL OFF;

SPOOL auditoria.json
EXEC exportar_tabela_json('AUDITORIA');
SPOOL OFF;
    
SPOOL fato_motos_status.json
EXEC exportar_tabela_json('FATO_MOTOS_STATUS');
SPOOL OFF;
    
-- PARA O OUTPUT VOLTAR AO NORMAL, APENAS RECONECTE A SUA CONTA
