USE [TiendaLinea]
GO
/****** Object:  Table [dbo].[Pedidos]    Script Date: 04/12/2024 13:21:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pedidos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[IdProducto] [int] NOT NULL,
	[CantCompra] [int] NOT NULL,
	[PrecioTotal] [decimal](18, 2) NOT NULL,
	[Estado] [nvarchar](50) NOT NULL,
	[Fecha] [datetime2](7) NOT NULL,
	[NumeroPedido] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Pedidos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Productos]    Script Date: 04/12/2024 13:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Productos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Descripcion] [nvarchar](225) NOT NULL,
	[Precio] [decimal](10, 2) NOT NULL,
	[Categoria] [nvarchar](50) NOT NULL,
	[Stock] [int] NOT NULL,
	[IconoFontAwesome] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Productos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 04/12/2024 13:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Contraseña] [nvarchar](50) NOT NULL,
	[FechaNacimiento] [datetime2](7) NOT NULL,
	[DireccionExacta] [nvarchar](250) NOT NULL,
	[Telefono] [nvarchar](50) NOT NULL,
	[FechaRegistro] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Pedidos]  WITH CHECK ADD  CONSTRAINT [FK_Pedidos_Productos] FOREIGN KEY([IdProducto])
REFERENCES [dbo].[Productos] ([Id])
GO
ALTER TABLE [dbo].[Pedidos] CHECK CONSTRAINT [FK_Pedidos_Productos]
GO
ALTER TABLE [dbo].[Pedidos]  WITH CHECK ADD  CONSTRAINT [FK_Pedidos_Usuarios] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([Id])
GO
ALTER TABLE [dbo].[Pedidos] CHECK CONSTRAINT [FK_Pedidos_Usuarios]
GO
