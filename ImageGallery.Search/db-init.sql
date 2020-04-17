IF db_id('ImageGallery') IS NULL
	Create database [ImageGallery];
Go

USE [ImageGallery]
GO

SET ANSI_NULLS ON
GO

Create table [Tag](
	[Id] INT IDENTITY (1, 1) NOT NULL,
	[Name] NVARCHAR(100) NOT NULL

	CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED ([Id] ASC)
);

Create table [Image](
	[Id] INT IDENTITY (1, 1) NOT NULL,
	[ExternalId] NVARCHAR(20) NOT NULL,
	[Author] NVARCHAR(100) NOT NULL,
	[Camera] NVARCHAR(100) NULL,
	[CroppedPicture] NVARCHAR(MAX) NOT NULL,
	[FullPicture] NVARCHAR(MAX) NOT NULL

	CONSTRAINT [PK_Image] PRIMARY KEY CLUSTERED ([Id] ASC)
);

Create table [ImageTag](
	[ImageId] INT NOT NULL,
	[TagId] INT NOT NULL

	CONSTRAINT [PK_ImageTag] PRIMARY KEY NONCLUSTERED ([ImageId], [TagId]),
	CONSTRAINT [FK_ImageTag_Image] FOREIGN KEY ([ImageId]) REFERENCES [Image]([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_ImageTag_Tag] FOREIGN KEY ([TagId]) REFERENCES [Tag]([Id]) ON DELETE CASCADE
);

CREATE UNIQUE INDEX Idx_TagName ON [Tag] ([Name]);
CREATE INDEX Idx_Author ON [Image] ([Author]);
CREATE INDEX Idx_ExternalId ON [Image] ([ExternalId]);
