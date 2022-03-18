--Constraint - é uma regra


NOT NULL -- não pode ser nulla de
UNIQUE -- é UNICO


alter table [Aluno]
alter column [Nome] NVARCHAR not null

alter table [Aluno]
alter column [Nome] NVARCHAR UNIQUE

PRIMARY KEY (ID)

-- PK COMPOSTA
PRIMARY KEY(ID, EMAIL)
