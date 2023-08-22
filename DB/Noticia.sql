USE db_noticias;
GO
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Noticia' and xtype='U') 
	CREATE TABLE Noticia (
	    Id INT PRIMARY KEY IDENTITY,
	    Titulo NVARCHAR(255) NOT NULL,
	    Descricao NVARCHAR(MAX) NOT NULL,
	    DataPublicacao DATETIME NOT NULL,
	    Autor NVARCHAR(255) NOT NULL
	);