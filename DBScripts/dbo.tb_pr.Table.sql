USE [db_logistic]
GO
/****** Object:  Table [dbo].[tb_pr]    Script Date: 19/03/2021 13:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_pr](
	[pr_no] [varchar](16) NOT NULL,
	[pr_date] [date] NOT NULL,
	[req_org] [varchar](16) NOT NULL,
	[create_by] [varchar](16) NOT NULL,
	[create_on] [datetime] NOT NULL,
	[update_by] [varchar](16) NULL,
	[update_on] [datetime] NULL,
 CONSTRAINT [tb_pr_pk] PRIMARY KEY CLUSTERED 
(
	[pr_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[tb_pr] ([pr_no], [pr_date], [req_org], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR001', CAST(N'2016-09-01' AS Date), N'ORG001', N'admin', CAST(N'2016-09-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr] ([pr_no], [pr_date], [req_org], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR002', CAST(N'2016-09-02' AS Date), N'ORG002', N'admin', CAST(N'2016-09-02T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr] ([pr_no], [pr_date], [req_org], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR003', CAST(N'2016-09-06' AS Date), N'ORG001', N'admin', CAST(N'2016-09-06T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr] ([pr_no], [pr_date], [req_org], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR004', CAST(N'2016-09-07' AS Date), N'ORG001', N'admin', CAST(N'2016-09-07T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr] ([pr_no], [pr_date], [req_org], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR005', CAST(N'2016-09-12' AS Date), N'ORG002', N'admin', CAST(N'2016-09-12T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr] ([pr_no], [pr_date], [req_org], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR006', CAST(N'2016-09-15' AS Date), N'ORG001', N'admin', CAST(N'2016-09-15T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr] ([pr_no], [pr_date], [req_org], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR007', CAST(N'2016-09-20' AS Date), N'ORG002', N'admin', CAST(N'2016-09-20T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr] ([pr_no], [pr_date], [req_org], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR008', CAST(N'2016-09-22' AS Date), N'ORG002', N'admin', CAST(N'2016-09-22T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr] ([pr_no], [pr_date], [req_org], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR009', CAST(N'2016-09-26' AS Date), N'ORG002', N'admin', CAST(N'2016-09-26T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr] ([pr_no], [pr_date], [req_org], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR010', CAST(N'2016-09-28' AS Date), N'ORG001', N'admin', CAST(N'2016-09-28T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr] ([pr_no], [pr_date], [req_org], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR011', CAST(N'2016-10-04' AS Date), N'ORG003', N'admin', CAST(N'2016-10-04T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr] ([pr_no], [pr_date], [req_org], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR012', CAST(N'2016-10-05' AS Date), N'ORG003', N'admin', CAST(N'2016-10-05T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr] ([pr_no], [pr_date], [req_org], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR013', CAST(N'2016-10-05' AS Date), N'ORG001', N'admin', CAST(N'2016-10-05T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr] ([pr_no], [pr_date], [req_org], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR014', CAST(N'2016-10-06' AS Date), N'ORG002', N'admin', CAST(N'2016-10-06T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr] ([pr_no], [pr_date], [req_org], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR015', CAST(N'2016-10-07' AS Date), N'ORG002', N'admin', CAST(N'2016-10-07T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr] ([pr_no], [pr_date], [req_org], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR016', CAST(N'2016-10-10' AS Date), N'ORG001', N'admin', CAST(N'2016-10-10T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr] ([pr_no], [pr_date], [req_org], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR017', CAST(N'2016-10-12' AS Date), N'ORG003', N'admin', CAST(N'2016-10-12T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr] ([pr_no], [pr_date], [req_org], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR018', CAST(N'2016-10-13' AS Date), N'ORG001', N'admin', CAST(N'2016-10-13T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr] ([pr_no], [pr_date], [req_org], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR019', CAST(N'2016-10-14' AS Date), N'ORG003', N'admin', CAST(N'2016-10-14T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tb_pr] ([pr_no], [pr_date], [req_org], [create_by], [create_on], [update_by], [update_on]) VALUES (N'PR020', CAST(N'2016-10-17' AS Date), N'ORG002', N'admin', CAST(N'2016-10-17T00:00:00.000' AS DateTime), NULL, NULL)
ALTER TABLE [dbo].[tb_pr]  WITH CHECK ADD  CONSTRAINT [tb_pr_fk1] FOREIGN KEY([req_org])
REFERENCES [dbo].[tb_org] ([org_code])
GO
ALTER TABLE [dbo].[tb_pr] CHECK CONSTRAINT [tb_pr_fk1]
GO
