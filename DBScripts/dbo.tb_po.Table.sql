USE [db_logistic]
GO
/****** Object:  Table [dbo].[tb_po]    Script Date: 19/03/2021 13:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_po](
	[po_no] [varchar](16) NOT NULL,
	[pr_no] [varchar](16) NOT NULL,
	[po_date] [date] NOT NULL,
	[supplier_code] [varchar](16) NOT NULL,
	[create_by] [varchar](16) NOT NULL,
	[create_on] [datetime] NOT NULL,
	[update_by] [varchar](16) NULL,
	[update_on] [datetime] NULL,
 CONSTRAINT [tb_po_pk] PRIMARY KEY CLUSTERED 
(
	[po_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[tb_po] ([po_no], [pr_no], [po_date], [supplier_code], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PO001', N'PR001', CAST(N'2016-09-02' AS Date), N'S002', N'admin', CAST(N'2016-09-02T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_po] ([po_no], [pr_no], [po_date], [supplier_code], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PO002', N'PR002', CAST(N'2016-09-06' AS Date), N'S002', N'admin', CAST(N'2016-09-06T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_po] ([po_no], [pr_no], [po_date], [supplier_code], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PO003', N'PR003', CAST(N'2016-09-07' AS Date), N'S001', N'admin', CAST(N'2016-09-07T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_po] ([po_no], [pr_no], [po_date], [supplier_code], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PO004', N'PR004', CAST(N'2016-09-12' AS Date), N'S001', N'admin', CAST(N'2016-09-12T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_po] ([po_no], [pr_no], [po_date], [supplier_code], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PO005', N'PR005', CAST(N'2016-09-15' AS Date), N'S004', N'admin', CAST(N'2016-09-15T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_po] ([po_no], [pr_no], [po_date], [supplier_code], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PO006', N'PR006', CAST(N'2016-09-20' AS Date), N'S004', N'admin', CAST(N'2016-09-20T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_po] ([po_no], [pr_no], [po_date], [supplier_code], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PO007', N'PR007', CAST(N'2016-09-22' AS Date), N'S004', N'admin', CAST(N'2016-09-22T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_po] ([po_no], [pr_no], [po_date], [supplier_code], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PO008', N'PR008', CAST(N'2016-09-26' AS Date), N'S004', N'admin', CAST(N'2016-09-26T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_po] ([po_no], [pr_no], [po_date], [supplier_code], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PO009', N'PR009', CAST(N'2016-09-28' AS Date), N'S003', N'admin', CAST(N'2016-09-28T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_po] ([po_no], [pr_no], [po_date], [supplier_code], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PO010', N'PR010', CAST(N'2016-10-04' AS Date), N'S003', N'admin', CAST(N'2016-10-04T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_po] ([po_no], [pr_no], [po_date], [supplier_code], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PO011', N'PR011', CAST(N'2016-10-05' AS Date), N'S003', N'admin', CAST(N'2016-10-05T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_po] ([po_no], [pr_no], [po_date], [supplier_code], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PO012', N'PR012', CAST(N'2016-10-05' AS Date), N'S003', N'admin', CAST(N'2016-10-05T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_po] ([po_no], [pr_no], [po_date], [supplier_code], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PO013', N'PR013', CAST(N'2016-10-06' AS Date), N'S004', N'admin', CAST(N'2016-10-06T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_po] ([po_no], [pr_no], [po_date], [supplier_code], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PO014', N'PR014', CAST(N'2016-10-07' AS Date), N'S004', N'admin', CAST(N'2016-10-07T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_po] ([po_no], [pr_no], [po_date], [supplier_code], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PO015', N'PR015', CAST(N'2016-10-10' AS Date), N'S004', N'admin', CAST(N'2016-10-10T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_po] ([po_no], [pr_no], [po_date], [supplier_code], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PO016', N'PR016', CAST(N'2016-10-12' AS Date), N'S004', N'admin', CAST(N'2016-10-12T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_po] ([po_no], [pr_no], [po_date], [supplier_code], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PO017', N'PR017', CAST(N'2016-10-13' AS Date), N'S001', N'admin', CAST(N'2016-10-13T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_po] ([po_no], [pr_no], [po_date], [supplier_code], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PO018', N'PR018', CAST(N'2016-10-14' AS Date), N'S001', N'admin', CAST(N'2016-10-14T00:00:00.000' AS DateTime), NULL, NULL)
ALTER TABLE [dbo].[tb_po]  WITH CHECK ADD  CONSTRAINT [tb_po_fk1] FOREIGN KEY([pr_no])
REFERENCES [dbo].[tb_pr] ([pr_no])
GO
ALTER TABLE [dbo].[tb_po] CHECK CONSTRAINT [tb_po_fk1]
GO
ALTER TABLE [dbo].[tb_po]  WITH CHECK ADD  CONSTRAINT [tb_po_fk2] FOREIGN KEY([supplier_code])
REFERENCES [dbo].[tb_supplier] ([supplier_code])
GO
ALTER TABLE [dbo].[tb_po] CHECK CONSTRAINT [tb_po_fk2]
GO
