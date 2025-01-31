USE [db_logistic]
GO
/****** Object:  Table [dbo].[tb_supplier]    Script Date: 19/03/2021 13:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_supplier](
	[supplier_code] [varchar](16) NOT NULL,
	[supplier_name] [varchar](64) NOT NULL,
	[address] [varchar](128) NOT NULL,
	[city] [varchar](32) NOT NULL,
	[province] [varchar](32) NOT NULL,
	[create_by] [varchar](16) NOT NULL,
	[create_on] [datetime] NOT NULL,
	[update_by] [varchar](16) NULL,
	[update_on] [datetime] NULL,
 CONSTRAINT [tb_supplier_pk] PRIMARY KEY CLUSTERED 
(
	[supplier_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[tb_supplier] ([supplier_code], [supplier_name], [address], [city], [province], [create_by], [create_on], [update_by], [update_on]) VALUES (N'S001', N'Sinar Jaya', N'Jl. Bandengan Raya No.11', N'Jakarta Barat', N'DKI Jakarta', N'admin', CAST(N'2016-09-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_supplier] ([supplier_code], [supplier_name], [address], [city], [province], [create_by], [create_on], [update_by], [update_on]) VALUES (N'S002', N'PD Sejati', N'Jl. Mangga Besar No.3', N'Jakarta Utara', N'DKI Jakarta', N'admin', CAST(N'2016-09-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_supplier] ([supplier_code], [supplier_name], [address], [city], [province], [create_by], [create_on], [update_by], [update_on]) VALUES (N'S003', N'Kawan Lama', N'Jl. Tanjung Duren No.9', N'Jakarta Barat', N'DKI Jakarta', N'admin', CAST(N'2016-09-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_supplier] ([supplier_code], [supplier_name], [address], [city], [province], [create_by], [create_on], [update_by], [update_on]) VALUES (N'S004', N'Sumber Rejeki', N'Jl. Daan Mogot No.30', N'Jakarta Barat', N'DKI Jakarta', N'admin', CAST(N'2016-09-01T00:00:00.000' AS DateTime), NULL, NULL)
