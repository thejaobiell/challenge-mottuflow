BEGIN
    EXECUTE IMMEDIATE 'INSERT INTO "' || USER || '"."funcionario" ("nome", "cpf", "cargo", "telefone", "email", "senha")
                       VALUES (''Admin'', ''000.000.000-00'', ''Administrador'', ''(00)00000-0000'', ''admin@mottu.com'', ''$2a$12$5SqXdv5vuwXtJllVDEK//uOmjVQBTZI0VqwiQbiQ/bO2Jcu717Kly'')';

    EXECUTE IMMEDIATE 'INSERT INTO "' || USER || '"."patio" ("nome", "endereco", "capacidade_maxima")
                       VALUES (''Pátio Central'', ''Rua Exemplo, 123, Cidade'', 50)';

    EXECUTE IMMEDIATE 'INSERT INTO "' || USER || '"."camera" ("status_operacional", "localizacao_fisica", "id_patio")
                       VALUES (''Ativa'', ''Entrada Principal do Pátio'', 1)';

    EXECUTE IMMEDIATE 'INSERT INTO "' || USER || '"."moto" ("placa", "modelo", "fabricante", "ano", "id_patio", "localizacao_atual")
                       VALUES (''ABC-1234'', ''Mottu Pop'', ''Honda'', 2022, 1, ''Pátio Central'')';

    EXECUTE IMMEDIATE 'INSERT INTO "' || USER || '"."aruco_tag" ("codigo", "status", "id_moto")
                       VALUES (''TAG-001'', ''Ativa'', 1)';

    EXECUTE IMMEDIATE 'INSERT INTO "' || USER || '"."localidade" ("data_hora", "ponto_referencia", "id_moto", "id_patio", "id_camera")
                       VALUES (TO_TIMESTAMP(''2025-10-28 08:00:00'', ''YYYY-MM-DD HH24:MI:SS''), ''Entrada Principal'', 1, 1, 1)';

    EXECUTE IMMEDIATE 'INSERT INTO "' || USER || '"."registro_status" ("tipo_status", "descricao", "data_status", "id_moto", "id_funcionario")
                       VALUES (''Entrada'', ''Moto chegou ao pátio'', TO_TIMESTAMP(''2025-10-28 08:15:00'', ''YYYY-MM-DD HH24:MI:SS''), 1, 1)';

    COMMIT;
END;
/

