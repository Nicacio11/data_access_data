-- create database
create database NameDatabase


-- script para apagar todos os processos e poder dropar a tabela
DECLARE @kill varchar(8000) = '';
select @kill = @kill + 'kill ' + CONVERT(varchar(5), session_id)
from sys.dm_exec_sessions
where database_id = db_id('NameDatabase')

exec(@kill)




-- dropando Database 
drop database NameDatabase



create table [nametable]

é bom utilizar  [] cochetes em caso de ter palavras igual as reservadas

use [database]

create table [Aluno](
    [Id] INT IDENTITY PRIMARY KEY,
    [Nome] NVARCHAR(100),
    [Nascimento] DateTime,
    [Active] bit
)
GO