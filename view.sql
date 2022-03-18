create or alter view vwSelectTeste
as
select top 100  * from Aluno

select * from vwSelectTeste
--não pode ter um where na consulta da view


DECLATE @categoryID int(11) NOT NULL
SET @categoryID = (SELECT TOP 1 [Id] from Categoria)