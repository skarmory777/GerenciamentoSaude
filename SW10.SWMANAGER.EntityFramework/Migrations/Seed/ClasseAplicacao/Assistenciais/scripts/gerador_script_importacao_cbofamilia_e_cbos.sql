/* ------------------------------------------------------------------------------------
  | FAMILIAS CBO
   ------------------------------------------------------------------------------------*/

--delete from SisCboFamilia
--DBCC CHECKIDENT (SisCboFamilia, RESEED, 0)

if NOT object_id('TempDB.dbo.#TB_TEMP_IMPORTA_FAMILIA_CBO') is null
begin
     DROP TABLE #TB_TEMP_IMPORTA_FAMILIA_CBO
end 

if NOT object_id('TempDB.dbo.#TB_TEMP_IMPORTA_FAMILIA_CBO_RESULTADO_DISPONIVEL') is null
begin
     DROP TABLE #TB_TEMP_IMPORTA_FAMILIA_CBO_RESULTADO_DISPONIVEL
end 

--SELECT COM TOP 1 APENAS PARA CRIAR UMA TABELA TEMPORARIA COM A ESTRUTURA DE SISCBOFAMILIA
SELECT TOP 0 [IsSistema],[Codigo],[Descricao],[IsDeleted],[DeleterUserId],[DeletionTime],[LastModificationTime],[LastModifierUserId],[CreationTime],[CreatorUserId]
INTO	#TB_TEMP_IMPORTA_FAMILIA_CBO
FROM	SisCboFamilia 

ALTER TABLE #TB_TEMP_IMPORTA_FAMILIA_CBO
ADD [id] [int] IDENTITY (1, 1) NOT NULL 


--LOAD NA TABELA TEMPORARIA POR UM ARQUIVO CSV
INSERT INTO #TB_TEMP_IMPORTA_FAMILIA_CBO ([IsSistema],[Codigo],[Descricao],[IsDeleted],[DeleterUserId],[DeletionTime],[LastModificationTime],[LastModifierUserId],[CreationTime],[CreatorUserId])
SELECT 1, Codigo, Descricao collate sql_latin1_general_cp1251_ci_as, 0, NULL, NULL, NULL, NULL, GETDATE(), 2 
FROM OPENROWSET(BULK N'C:\Users\SWDev\Source\Repos\SW10.SWMANAGER\SW10.SWMANAGER.EntityFramework\Migrations\Seed\ClasseAplicacao\Assistenciais\scripts\cbo_familia_v1.csv',
    FORMATFILE = N'C:\Users\SWDev\Source\Repos\SW10.SWMANAGER\SW10.SWMANAGER.EntityFramework\Migrations\Seed\ClasseAplicacao\Assistenciais\scripts\cbo_familia_v1.fmt'
    ,FIRSTROW=2
    ) AS cbo_familias


--SELECT id, [IsSistema],[Codigo],[Descricao],[IsDeleted],[DeleterUserId],[DeletionTime],[LastModificationTime],[LastModifierUserId],[CreationTime],[CreatorUserId]
--from	#TB_TEMP_IMPORTA_FAMILIA_CBO

SELECT 'INSERT INTO SisCboFamilia ([IsSistema],[Codigo],[Descricao],[IsDeleted],[DeleterUserId],[DeletionTime],[LastModificationTime],[LastModifierUserId],[CreationTime],[CreatorUserId]) '
+ ' VALUES ('
+ isnull(cast([IsSistema] as varchar), 'NULL') 
+ ', ''' + isnull([Codigo], 'NULL') + ''''
+ ', ''' + isnull([Descricao], 'NULL') + ''''
+ ', ' + isnull(cast([IsDeleted] as varchar), 'NULL')
+ ', ' + isnull(cast([DeleterUserId] as varchar), 'NULL')
+ ', ' + isnull(cast([DeletionTime] as varchar), 'NULL')
+ ', ' + isnull(cast([LastModificationTime] as varchar), 'NULL')
+ ', ' + isnull(cast([LastModifierUserId] as varchar), 'NULL')
+ ', GETDATE()' + 
+ ', ' + isnull(cast([CreatorUserId] as varchar), 'NULL')
+ ')' CAMPO_EXPORTACAO
INTO #TB_TEMP_IMPORTA_FAMILIA_CBO_RESULTADO_DISPONIVEL 
from #TB_TEMP_IMPORTA_FAMILIA_CBO

SELECT * FROM #TB_TEMP_IMPORTA_FAMILIA_CBO_RESULTADO_DISPONIVEL


/* ------------------------------------------------------------------------------------
  | CBOS
   ------------------------------------------------------------------------------------*/

----delete from siscbo
----DBCC CHECKIDENT (siscbo, RESEED, 0)

--DESTROI TABELAS TEMPORARIAS
if NOT object_id('TempDB.dbo.#TB_TEMP_IMPORTA_CBO') is null
begin
     DROP TABLE #TB_TEMP_IMPORTA_CBO
end 

if NOT object_id('TempDB.dbo.#TB_TEMP_IMPORTA_CBO_RESULTADO_DISPONIVEL') is null
begin
     DROP TABLE #TB_TEMP_IMPORTA_CBO_RESULTADO_DISPONIVEL
end 

--SELECT COM TOP 1 APENAS PARA CRIAR UMA TABELA TEMPORARIA COM A ESTRUTURA DE SISCBO
SELECT TOP 0 [IsSistema],[Codigo],[Descricao],[IsDeleted],[DeleterUserId],[DeletionTime],[LastModificationTime],[LastModifierUserId],[CreationTime],[CreatorUserId],[SisCboFamiliaId],[SisCboTipoId]
INTO	#TB_TEMP_IMPORTA_CBO
FROM	SisCbo

--LOAD NA TABELA TEMPORARIA POR UM ARQUIVO CSV
INSERT INTO #TB_TEMP_IMPORTA_CBO ([IsSistema],[Codigo],[Descricao],[IsDeleted],[DeleterUserId],[DeletionTime],[LastModificationTime],[LastModifierUserId],[CreationTime],[CreatorUserId],[SisCboFamiliaId],[SisCboTipoId])
SELECT b.[IsSistema],b.[Codigo],upper(b.[Descricao]),b.[IsDeleted],b.[DeleterUserId],b.[DeletionTime],b.[LastModificationTime],b.[LastModifierUserId],b.[CreationTime],b.[CreatorUserId],a.id [SisCboFamiliaId],b.[SisCboTipoId]
FROM	#TB_TEMP_IMPORTA_FAMILIA_CBO a
JOIN   (SELECT 1 [IsSistema], Codigo, Descricao collate sql_latin1_general_cp1251_ci_as Descricao, 0 [IsDeleted], NULL [DeleterUserId], NULL [DeletionTime], NULL [LastModificationTime], NULL [LastModifierUserId], GETDATE() [CreationTime], 2 [CreatorUserId], SUBSTRING(cbos.codigo, 1, 4) codigo_familia, TIPOID [SisCboTipoId]
		FROM OPENROWSET(BULK N'C:\Users\SWDev\Source\Repos\SW10.SWMANAGER\SW10.SWMANAGER.EntityFramework\Migrations\Seed\ClasseAplicacao\Assistenciais\scripts\cbo_v1.csv',
			FORMATFILE = N'C:\Users\SWDev\Source\Repos\SW10.SWMANAGER\SW10.SWMANAGER.EntityFramework\Migrations\Seed\ClasseAplicacao\Assistenciais\scripts\cbo_v1.fmt'
			,FIRSTROW=2
			) AS cbos
		) b
ON		a.Codigo = SUBSTRING(b.codigo, 1, 4)
ORDER BY a.id


SELECT 'INSERT INTO SisCbo ([IsSistema],[Codigo],[Descricao],[IsDeleted],[DeleterUserId],[DeletionTime],[LastModificationTime],[LastModifierUserId],[CreationTime],[CreatorUserId],[SisCboFamiliaId],[SisCboTipoId]) '
		+ ' VALUES ('
		+ isnull(cast([IsSistema] as varchar), 'NULL') 
		+ ', ''' + isnull([Codigo], 'NULL') + ''''
		+ ', ''' + isnull([Descricao], 'NULL') + ''''
		+ ', ' + isnull(cast([IsDeleted] as varchar), 'NULL')
		+ ', ' + isnull(cast([DeleterUserId] as varchar), 'NULL')
		+ ', ' + isnull(cast([DeletionTime] as varchar), 'NULL')
		+ ', ' + isnull(cast([LastModificationTime] as varchar), 'NULL')
		+ ', ' + isnull(cast([LastModifierUserId] as varchar), 'NULL')
		+ ', GETDATE()' + 
		+ ', ' + isnull(cast([CreatorUserId] as varchar), 'NULL')
		+ ', ' + isnull(cast([SisCboFamiliaId] as varchar), 'NULL')
		+ ', ' + isnull(cast([SisCboTipoId] as varchar), 'NULL')
		+ ')' CAMPO_EXPORTACAO,
		[SisCboFamiliaId]
INTO #TB_TEMP_IMPORTA_CBO_RESULTADO_DISPONIVEL
FROM #TB_TEMP_IMPORTA_CBO


SELECT CAMPO_EXPORTACAO 
FROM #TB_TEMP_IMPORTA_CBO_RESULTADO_DISPONIVEL 
ORDER BY [SisCboFamiliaId]

/*-------------------------------------------------------------------------------*/


--DESTROI TABELAS TEMPORARIAS
if NOT object_id('TempDB.dbo.#TB_TEMP_IMPORTA_FAMILIA_CBO') is null
begin
     DROP TABLE #TB_TEMP_IMPORTA_FAMILIA_CBO
end 

if NOT object_id('TempDB.dbo.#TB_TEMP_IMPORTA_FAMILIA_CBO_RESULTADO_DISPONIVEL') is null
begin
     DROP TABLE #TB_TEMP_IMPORTA_FAMILIA_CBO_RESULTADO_DISPONIVEL
end 


if NOT object_id('TempDB.dbo.#TB_TEMP_IMPORTA_CBO') is null
begin
     DROP TABLE #TB_TEMP_IMPORTA_CBO
end 

if NOT object_id('TempDB.dbo.#TB_TEMP_IMPORTA_CBO_RESULTADO_DISPONIVEL') is null
begin
     DROP TABLE #TB_TEMP_IMPORTA_CBO_RESULTADO_DISPONIVEL
end
