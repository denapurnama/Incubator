USE [db_logistic]
GO
/****** Object:  Table [dbo].[tb_org]    Script Date: 19/03/2021 13:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_org](
	[org_code] [varchar](16) NOT NULL,
	[org_name] [varchar](64) NOT NULL,
	[create_by] [varchar](16) NOT NULL,
	[create_on] [datetime] NOT NULL,
	[update_by] [varchar](16) NULL,
	[update_on] [datetime] NULL,
 CONSTRAINT [tb_org_pk] PRIMARY KEY CLUSTERED 
(
	[org_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[tb_org] ([org_code], [org_name], [create_by], [create_on], [update_by], [update_on]) VALUES (N'ORG001', N'Finance', N'admin', CAST(N'2016-09-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_org] ([org_code], [org_name], [create_by], [create_on], [update_by], [update_on]) VALUES (N'ORG002', N'Human Resource', N'admin', CAST(N'2016-09-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_org] ([org_code], [org_name], [create_by], [create_on], [update_by], [update_on]) VALUES (N'ORG003', N'Warehouse', N'admin', CAST(N'2016-09-01T00:00:00.000' AS DateTime), NULL, NULL)
