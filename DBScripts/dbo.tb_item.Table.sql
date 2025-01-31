USE [db_logistic]
GO
/****** Object:  Table [dbo].[tb_item]    Script Date: 19/03/2021 13:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_item](
	[item_code] [varchar](16) NOT NULL,
	[item_name] [varchar](64) NOT NULL,
	[item_um] [varchar](8) NOT NULL,
	[create_by] [varchar](16) NOT NULL,
	[create_on] [datetime] NOT NULL,
	[update_by] [varchar](16) NULL,
	[update_on] [datetime] NULL,
 CONSTRAINT [tb_item_pk] PRIMARY KEY CLUSTERED 
(
	[item_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[tb_item] ([item_code], [item_name], [item_um], [create_by], [create_on], [update_by], [update_on]) VALUES (N'I001', N'Kertas A4', N'Rim', N'admin', CAST(N'2016-09-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_item] ([item_code], [item_name], [item_um], [create_by], [create_on], [update_by], [update_on]) VALUES (N'I002', N'Kertas HVS', N'Rim', N'admin', CAST(N'2016-09-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_item] ([item_code], [item_name], [item_um], [create_by], [create_on], [update_by], [update_on]) VALUES (N'I003', N'Cartridge Epson', N'Unit', N'admin', CAST(N'2016-09-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_item] ([item_code], [item_name], [item_um], [create_by], [create_on], [update_by], [update_on]) VALUES (N'I004', N'Cartridge Canon', N'Unit', N'admin', CAST(N'2016-09-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_item] ([item_code], [item_name], [item_um], [create_by], [create_on], [update_by], [update_on]) VALUES (N'I005', N'Laptop Acer', N'Unit', N'admin', CAST(N'2016-09-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_item] ([item_code], [item_name], [item_um], [create_by], [create_on], [update_by], [update_on]) VALUES (N'I006', N'Laptop Asus', N'Unit', N'admin', CAST(N'2016-09-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_item] ([item_code], [item_name], [item_um], [create_by], [create_on], [update_by], [update_on]) VALUES (N'I007', N'Laptop Lenovo', N'Unit', N'admin', CAST(N'2016-09-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_item] ([item_code], [item_name], [item_um], [create_by], [create_on], [update_by], [update_on]) VALUES (N'I008', N'Projector', N'Unit', N'admin', CAST(N'2016-09-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_item] ([item_code], [item_name], [item_um], [create_by], [create_on], [update_by], [update_on]) VALUES (N'I009', N'Meja Kerja', N'Unit', N'admin', CAST(N'2016-09-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_item] ([item_code], [item_name], [item_um], [create_by], [create_on], [update_by], [update_on]) VALUES (N'I010', N'Kursi Kerja', N'Unit', N'admin', CAST(N'2016-09-01T00:00:00.000' AS DateTime), NULL, NULL)
