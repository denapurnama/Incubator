USE [db_logistic]
GO
/****** Object:  Table [dbo].[tb_pr_dtl]    Script Date: 19/03/2021 13:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_pr_dtl](
	[pr_no] [varchar](16) NOT NULL,
	[item_code] [varchar](16) NOT NULL,
	[request_qty] [numeric](8, 0) NOT NULL,
	[create_by] [varchar](16) NOT NULL,
	[create_on] [datetime] NOT NULL,
	[update_by] [varchar](16) NULL,
	[update_on] [datetime] NULL,
 CONSTRAINT [tb_pr_dtl_pk] PRIMARY KEY CLUSTERED 
(
	[pr_no] ASC,
	[item_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[tb_pr_dtl] ([pr_no], [item_code], [request_qty], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR001', N'I001', CAST(2 AS Numeric(8, 0)), N'admin', CAST(N'2016-09-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr_dtl] ([pr_no], [item_code], [request_qty], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR002', N'I002', CAST(2 AS Numeric(8, 0)), N'admin', CAST(N'2016-09-02T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr_dtl] ([pr_no], [item_code], [request_qty], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR003', N'I003', CAST(1 AS Numeric(8, 0)), N'admin', CAST(N'2016-09-06T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr_dtl] ([pr_no], [item_code], [request_qty], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR004', N'I004', CAST(3 AS Numeric(8, 0)), N'admin', CAST(N'2016-09-07T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr_dtl] ([pr_no], [item_code], [request_qty], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR005', N'I005', CAST(3 AS Numeric(8, 0)), N'admin', CAST(N'2016-09-12T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr_dtl] ([pr_no], [item_code], [request_qty], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR006', N'I006', CAST(1 AS Numeric(8, 0)), N'admin', CAST(N'2016-09-15T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr_dtl] ([pr_no], [item_code], [request_qty], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR007', N'I007', CAST(1 AS Numeric(8, 0)), N'admin', CAST(N'2016-09-20T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr_dtl] ([pr_no], [item_code], [request_qty], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR008', N'I008', CAST(1 AS Numeric(8, 0)), N'admin', CAST(N'2016-09-22T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr_dtl] ([pr_no], [item_code], [request_qty], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR009', N'I009', CAST(3 AS Numeric(8, 0)), N'admin', CAST(N'2016-09-26T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr_dtl] ([pr_no], [item_code], [request_qty], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR010', N'I010', CAST(3 AS Numeric(8, 0)), N'admin', CAST(N'2016-09-28T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr_dtl] ([pr_no], [item_code], [request_qty], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR011', N'I010', CAST(2 AS Numeric(8, 0)), N'admin', CAST(N'2016-10-04T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr_dtl] ([pr_no], [item_code], [request_qty], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR012', N'I009', CAST(1 AS Numeric(8, 0)), N'admin', CAST(N'2016-10-05T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr_dtl] ([pr_no], [item_code], [request_qty], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR013', N'I008', CAST(3 AS Numeric(8, 0)), N'admin', CAST(N'2016-10-05T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr_dtl] ([pr_no], [item_code], [request_qty], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR014', N'I007', CAST(3 AS Numeric(8, 0)), N'admin', CAST(N'2016-10-06T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr_dtl] ([pr_no], [item_code], [request_qty], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR015', N'I006', CAST(3 AS Numeric(8, 0)), N'admin', CAST(N'2016-10-07T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr_dtl] ([pr_no], [item_code], [request_qty], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR016', N'I005', CAST(1 AS Numeric(8, 0)), N'admin', CAST(N'2016-10-10T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr_dtl] ([pr_no], [item_code], [request_qty], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR017', N'I004', CAST(1 AS Numeric(8, 0)), N'admin', CAST(N'2016-10-12T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr_dtl] ([pr_no], [item_code], [request_qty], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR018', N'I003', CAST(3 AS Numeric(8, 0)), N'admin', CAST(N'2016-10-13T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr_dtl] ([pr_no], [item_code], [request_qty], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR019', N'I002', CAST(3 AS Numeric(8, 0)), N'admin', CAST(N'2016-10-14T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr_dtl] ([pr_no], [item_code], [request_qty], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR020', N'I001', CAST(3 AS Numeric(8, 0)), N'admin', CAST(N'2016-10-17T00:00:00.000' AS DateTime), NULL, NULL)
ALTER TABLE [dbo].[tb_pr_dtl]  WITH CHECK ADD  CONSTRAINT [tb_pr_dtl_fk1] FOREIGN KEY([pr_no])
REFERENCES [dbo].[tb_pr] ([pr_no])
GO
ALTER TABLE [dbo].[tb_pr_dtl] CHECK CONSTRAINT [tb_pr_dtl_fk1]
GO
ALTER TABLE [dbo].[tb_pr_dtl]  WITH CHECK ADD  CONSTRAINT [tb_pr_dtl_fk2] FOREIGN KEY([item_code])
REFERENCES [dbo].[tb_item] ([item_code])
GO
ALTER TABLE [dbo].[tb_pr_dtl] CHECK CONSTRAINT [tb_pr_dtl_fk2]
GO
