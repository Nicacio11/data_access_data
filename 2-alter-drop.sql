alter table [Aluno]
add [Document] NVARCHAR(11)
GO


alter table [Aluno]
DROP COLUMN [Document]


alter table [Aluno]
Alter COLUMN [Document] CHAR(11)