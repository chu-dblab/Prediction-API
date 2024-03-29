USE [Prediction]
GO
/****** Object:  Table [dbo].[CG]    Script Date: 2015/7/8 下午 10:22:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CG](
	[Cname] [nchar](10) NOT NULL,
	[Gname] [nchar](10) NOT NULL,
 CONSTRAINT [PK_CG] PRIMARY KEY CLUSTERED 
(
	[Cname] ASC,
	[Gname] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[D]    Script Date: 2015/7/8 下午 10:22:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[D](
	[DID] [int] NOT NULL,
	[UName] [nchar](20) NOT NULL,
	[UURL] [nchar](85) NOT NULL,
	[DName] [nchar](50) NOT NULL,
	[DURL] [nchar](85) NOT NULL,
	[Salary] [int] NOT NULL,
	[SalaryURL] [nchar](85) NOT NULL,
	[ELLevel] [nchar](1) NOT NULL,
	[MinScore] [float] NOT NULL,
	[TL1] [smallint] NOT NULL,
	[TL2] [smallint] NOT NULL,
	[TL3] [smallint] NOT NULL,
	[TL4] [smallint] NOT NULL,
	[TL5] [smallint] NOT NULL,
	[TL6] [smallint] NOT NULL,
	[EW1] [float] NOT NULL,
	[EW2] [float] NOT NULL,
	[EW3] [float] NOT NULL,
	[EW4] [float] NOT NULL,
	[EW5] [float] NOT NULL,
	[EW6] [float] NOT NULL,
	[EW7] [float] NOT NULL,
	[EW8] [float] NOT NULL,
	[EW9] [float] NOT NULL,
	[EW10] [float] NOT NULL,
 CONSTRAINT [PK_D] PRIMARY KEY CLUSTERED 
(
	[DID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DC]    Script Date: 2015/7/8 下午 10:22:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DC](
	[DID] [int] NOT NULL,
	[Cname] [nchar](10) NOT NULL,
 CONSTRAINT [PK_DC] PRIMARY KEY CLUSTERED 
(
	[DID] ASC,
	[Cname] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[E103]    Script Date: 2015/7/8 下午 10:22:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[E103](
	[Ename] [nchar](4) NOT NULL,
	[Score] [int] NOT NULL,
	[Percent] [float] NOT NULL,
 CONSTRAINT [PK_E103] PRIMARY KEY CLUSTERED 
(
	[Ename] ASC,
	[Score] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[E104]    Script Date: 2015/7/8 下午 10:22:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[E104](
	[Ename] [nchar](4) NOT NULL,
	[Score] [int] NOT NULL,
	[Percent] [float] NOT NULL,
 CONSTRAINT [PK_E104] PRIMARY KEY CLUSTERED 
(
	[Ename] ASC,
	[Score] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T]    Script Date: 2015/7/8 下午 10:22:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T](
	[TName] [nchar](4) NOT NULL,
	[Grade1] [int] NOT NULL,
	[Grade2] [int] NOT NULL,
	[Grade3] [int] NOT NULL,
	[Grade4] [int] NOT NULL,
	[Grade5] [int] NOT NULL,
 CONSTRAINT [PK_T] PRIMARY KEY CLUSTERED 
(
	[TName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO