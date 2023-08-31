USE [Travel]
GO
INSERT [dbo].[Chat] ([id], [room_id], [sender], [message], [send_time]) VALUES (1, 1, 1, N'Hi', CAST(N'2023-08-29T14:44:01.000' AS DateTime))
GO
INSERT [dbo].[Chat] ([id], [room_id], [sender], [message], [send_time]) VALUES (2, 1, 0, N'您好', CAST(N'2023-08-29T14:45:03.000' AS DateTime))
GO
INSERT [dbo].[Chat] ([id], [room_id], [sender], [message], [send_time]) VALUES (3, 1, 1, N'請問有北海道的行程嗎？', CAST(N'2023-08-29T14:48:51.000' AS DateTime))
GO
INSERT [dbo].[Chat] ([id], [room_id], [sender], [message], [send_time]) VALUES (4, 2, 1, N'安安', CAST(N'2023-08-29T14:44:01.000' AS DateTime))
GO
INSERT [dbo].[Chat] ([id], [room_id], [sender], [message], [send_time]) VALUES (5, 2, 0, N'您好!!', CAST(N'2023-08-29T14:45:03.000' AS DateTime))
GO
INSERT [dbo].[Chat] ([id], [room_id], [sender], [message], [send_time]) VALUES (6, 1, 0, N'有喔', CAST(N'2023-08-29T16:23:24.150' AS DateTime))
GO
INSERT [dbo].[Chat] ([id], [room_id], [sender], [message], [send_time]) VALUES (7, 1, 1, N'好的', CAST(N'2023-08-29T16:24:38.300' AS DateTime))
GO
INSERT [dbo].[Chat] ([id], [room_id], [sender], [message], [send_time]) VALUES (9, 6, 0, N'123', CAST(N'2023-08-29T21:07:02.833' AS DateTime))
GO
INSERT [dbo].[Chat] ([id], [room_id], [sender], [message], [send_time]) VALUES (10, 6, 1, N'456', CAST(N'2023-08-29T21:07:06.443' AS DateTime))
GO
INSERT [dbo].[Chat] ([id], [room_id], [sender], [message], [send_time]) VALUES (12, 6, 1, N'789', CAST(N'2023-08-29T21:27:16.150' AS DateTime))
GO
INSERT [dbo].[Chat] ([id], [room_id], [sender], [message], [send_time]) VALUES (13, 6, 0, N'123456', CAST(N'2023-08-29T23:58:06.933' AS DateTime))
GO
INSERT [dbo].[Chat] ([id], [room_id], [sender], [message], [send_time]) VALUES (14, 6, 1, N'111', CAST(N'2023-08-29T23:58:35.353' AS DateTime))
GO
INSERT [dbo].[Chat] ([id], [room_id], [sender], [message], [send_time]) VALUES (15, 5, 1, N'Hi', CAST(N'2023-08-31T10:46:27.860' AS DateTime))
GO
INSERT [dbo].[Chat] ([id], [room_id], [sender], [message], [send_time]) VALUES (16, 5, 0, N'HiHi', CAST(N'2023-08-31T10:46:48.773' AS DateTime))
GO
INSERT [dbo].[Follow] ([id], [travel_id], [user_id]) VALUES (1, 1, 1)
GO
INSERT [dbo].[Follow] ([id], [travel_id], [user_id]) VALUES (2, 2, 1)
GO
INSERT [dbo].[Follow] ([id], [travel_id], [user_id]) VALUES (3, 3, 1)
GO
INSERT [dbo].[Follow] ([id], [travel_id], [user_id]) VALUES (4, 1, 2)
GO
INSERT [dbo].[Follow] ([id], [travel_id], [user_id]) VALUES (5, 2, 2)
GO
INSERT [dbo].[Follow] ([id], [travel_id], [user_id]) VALUES (6, 1, 3)
GO
INSERT [dbo].[Follow] ([id], [travel_id], [user_id]) VALUES (7, 3, 3)
GO
INSERT [dbo].[Follow] ([id], [travel_id], [user_id]) VALUES (8, 3, 4)
GO
INSERT [dbo].[Follow] ([id], [travel_id], [user_id]) VALUES (9, 2, 4)
GO
INSERT [dbo].[Follow] ([id], [travel_id], [user_id]) VALUES (10, 4, 1)
GO
INSERT [dbo].[Follow] ([id], [travel_id], [user_id]) VALUES (11, 4, 2)
GO
INSERT [dbo].[Follow] ([id], [travel_id], [user_id]) VALUES (12, 4, 3)
GO
INSERT [dbo].[Follow] ([id], [travel_id], [user_id]) VALUES (13, 4, 4)
GO
INSERT [dbo].[Follow] ([id], [travel_id], [user_id]) VALUES (14, 4, 1)
GO
INSERT [dbo].[Follow] ([id], [travel_id], [user_id]) VALUES (15, 4, 2)
GO
INSERT [dbo].[Follow] ([id], [travel_id], [user_id]) VALUES (16, 1, 3)
GO
INSERT [dbo].[Follow] ([id], [travel_id], [user_id]) VALUES (17, 1, 3)
GO
INSERT [dbo].[Follow] ([id], [travel_id], [user_id]) VALUES (18, 5, 3)
GO
INSERT [dbo].[Order] ([id], [order_date], [nation], [travel_session_id], [user_id], [total], [check], [check_date]) VALUES (1, CAST(N'2023-01-11T21:25:12.000' AS DateTime), N'台灣', 1, 1, 3598, 1, CAST(N'2023-08-31T10:48:07.587' AS DateTime))
GO
INSERT [dbo].[Order] ([id], [order_date], [nation], [travel_session_id], [user_id], [total], [check], [check_date]) VALUES (2, CAST(N'2023-01-20T11:15:55.000' AS DateTime), N'台灣', 12, 3, 2399, 2, CAST(N'2023-08-31T10:48:11.463' AS DateTime))
GO
INSERT [dbo].[Order] ([id], [order_date], [nation], [travel_session_id], [user_id], [total], [check], [check_date]) VALUES (3, CAST(N'2023-02-02T20:33:01.000' AS DateTime), N'台灣', 5, 2, 5397, 0, NULL)
GO
INSERT [dbo].[OrderList] ([id], [order_id], [price], [name], [sex], [birthday], [phone_number], [last_name], [first_name], [identity_code], [passport_number], [special_need]) VALUES (1, 1, 1799, N'王曉明', N'man', CAST(N'2000-01-01' AS Date), N'0912345678', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[OrderList] ([id], [order_id], [price], [name], [sex], [birthday], [phone_number], [last_name], [first_name], [identity_code], [passport_number], [special_need]) VALUES (2, 1, 1799, N'王大陸', N'man', CAST(N'2003-05-19' AS Date), N'0987654321', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[OrderList] ([id], [order_id], [price], [name], [sex], [birthday], [phone_number], [last_name], [first_name], [identity_code], [passport_number], [special_need]) VALUES (3, 2, 2399, N'張小玉', N'woman', CAST(N'2002-04-22' AS Date), N'0944455566', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[OrderList] ([id], [order_id], [price], [name], [sex], [birthday], [phone_number], [last_name], [first_name], [identity_code], [passport_number], [special_need]) VALUES (4, 3, 1799, N'張小玉', N'woman', CAST(N'2002-04-22' AS Date), N'0944455566', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[OrderList] ([id], [order_id], [price], [name], [sex], [birthday], [phone_number], [last_name], [first_name], [identity_code], [passport_number], [special_need]) VALUES (5, 3, 1799, N'張小曼', N'woman', CAST(N'2003-11-12' AS Date), N'0914725836', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[OrderList] ([id], [order_id], [price], [name], [sex], [birthday], [phone_number], [last_name], [first_name], [identity_code], [passport_number], [special_need]) VALUES (6, 3, 1799, N'張曼玉', N'woman', CAST(N'2005-02-15' AS Date), N'0996385274', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Travel] ([id], [title], [date_range_start], [date_range_end], [days], [nation], [departure_location], [pdf_url], [main_image_url]) VALUES (1, N'古坑摘柳丁 & 落羽松秘境二日遊', CAST(N'2023-10-29' AS Date), CAST(N'2023-12-22' AS Date), 2, N'台灣', N'台北', N'uploads/pdf_20230828144558.pdf', N'uploads/images_202308111413.jpg')
GO
INSERT [dbo].[Travel] ([id], [title], [date_range_start], [date_range_end], [days], [nation], [departure_location], [pdf_url], [main_image_url]) VALUES (2, N'趣和逸二日遊', CAST(N'2023-09-17' AS Date), CAST(N'2023-10-30' AS Date), 2, N'台灣', N'台中', N'uploads/pdf_202308141010.pdf', N'uploads/images_202308141011.jpg')
GO
INSERT [dbo].[Travel] ([id], [title], [date_range_start], [date_range_end], [days], [nation], [departure_location], [pdf_url], [main_image_url]) VALUES (3, N'宜蘭一日遊蘇澳冷泉', CAST(N'2023-07-01' AS Date), CAST(N'2023-08-22' AS Date), 1, N'台灣', N'高雄', N'uploads/pdf_202308141020.pdf', N'uploads/images_202308141019.jpg')
GO
INSERT [dbo].[Travel] ([id], [title], [date_range_start], [date_range_end], [days], [nation], [departure_location], [pdf_url], [main_image_url]) VALUES (4, N'嘉義夢幻遊 X 觀止大飯店二日遊', CAST(N'2023-07-01' AS Date), CAST(N'2023-09-15' AS Date), 2, N'台灣', N'台北', N'uploads/pdf_20230823152057.pdf', N'uploads/mainImage_20230823152057.jpg')
GO
INSERT [dbo].[Travel] ([id], [title], [date_range_start], [date_range_end], [days], [nation], [departure_location], [pdf_url], [main_image_url]) VALUES (5, N'花蓮天使的花毯~ 金針花海三日遊', CAST(N'2023-08-15' AS Date), CAST(N'2023-10-05' AS Date), 3, N'台灣', N'台中', N'uploads/pdf_20230824104645.pdf', N'uploads/mainImage_20230824104645.jpg')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (12, 2, N'新屋綠色走廊')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (13, 2, N'永安海螺文化體驗園區')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (14, 2, N'永安漁港')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (15, 2, N'楊梅雅聞魅力博覽館')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (16, 2, N'大溪老街')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (17, 2, N'大溪老茶廠')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (18, 2, N'和逸飯店')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (19, 2, N'濟生 Beauty 觀光工廠')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (20, 2, N'華泰名品城')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (21, 2, N'九湯町拉麵博物館')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (22, 3, N'南方澳')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (23, 3, N'阿里史冷泉')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (24, 3, N'南天宮金媽祖商圈')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (25, 3, N'雙魚生技觀光工廠')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (26, 4, N'夜燈水舞秀')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (27, 4, N'故宮南院')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (28, 4, N'紫薇花園')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (29, 4, N'康倪時代美學莊園')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (30, 5, N'杏輝健康園區')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (31, 5, N'金三角商圈午餐')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (32, 5, N'北回歸線標誌公園')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (33, 5, N'台東大學圖書館')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (34, 5, N'知本飯店')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (35, 5, N'花蓮六十石山')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (36, 5, N'金針花海')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (37, 5, N'阿美麻糬文化館')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (38, 5, N'玉里火車站美食')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (39, 5, N'瑞穗牧場')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (40, 5, N'東大門夜市')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (41, 5, N'四八高地戰備坑道')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (42, 5, N'善盈生技健康食品展售中心')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (43, 5, N'南方澳南天宮金媽祖商圈')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (44, 5, N'台灣國術文化館')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (45, 5, N'九湯町拉麵博物館')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (296, 1, N'鯉魚潭水庫')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (297, 1, N'古坑柳橙觀光果園')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (298, 1, N'青埔落羽松秘境')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (299, 1, N'古坑綠色隧道公園')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (300, 1, N'新瓦屋客家文化保存區')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (301, 1, N'三義詩舒曼蠶絲文化園區')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (302, 1, N'勝昌製藥觀光工廠')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (303, 1, N'雲林')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (304, 1, N'台中')
GO
INSERT [dbo].[TravelAttraction] ([id], [travel_id], [attraction]) VALUES (305, 1, N'嘉義')
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (1, 1, N'TXB011029', CAST(N'2023-10-29' AS Date), 1799, 1, 32, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (2, 1, N'TXB011105', CAST(N'2023-11-05' AS Date), 1799, 4, 32, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (3, 1, N'TXB011112', CAST(N'2023-11-12' AS Date), 1799, 0, 32, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (4, 1, N'TXB011119', CAST(N'2023-11-19' AS Date), 1799, 5, 32, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (5, 1, N'TXB011126', CAST(N'2023-11-26' AS Date), 1799, 5, 32, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (6, 1, N'TXB011203', CAST(N'2023-12-03' AS Date), 1799, 30, 32, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (7, 1, N'TXB011210', CAST(N'2023-12-10' AS Date), 1799, 30, 32, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (8, 1, N'TXB011213', CAST(N'2023-12-13' AS Date), 1799, 30, 32, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (9, 1, N'TXB011215', CAST(N'2023-12-15' AS Date), 1799, 30, 32, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (10, 1, N'TXB011217', CAST(N'2023-12-17' AS Date), 1799, 30, 32, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (11, 1, N'TXB011220', CAST(N'2023-12-20' AS Date), 1799, 30, 32, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (12, 2, N'TXB010917', CAST(N'2023-09-17' AS Date), 2399, 24, 35, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (13, 2, N'TXB010924', CAST(N'2023-09-24' AS Date), 2399, 35, 35, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (14, 2, N'TXB010925', CAST(N'2023-09-25' AS Date), 2399, 35, 35, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (15, 2, N'TXB010928', CAST(N'2023-09-28' AS Date), 2399, 33, 35, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (16, 2, N'TXB011001', CAST(N'2023-10-01' AS Date), 2399, 30, 35, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (17, 2, N'TXB011002', CAST(N'2023-10-02' AS Date), 2399, 25, 35, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (18, 2, N'TXB011006', CAST(N'2023-10-06' AS Date), 2399, 35, 35, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (19, 2, N'TXB011022', CAST(N'2023-10-22' AS Date), 2399, 35, 35, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (20, 2, N'TXB011023', CAST(N'2023-10-23' AS Date), 2399, 35, 35, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (21, 2, N'TXB011027', CAST(N'2023-10-27' AS Date), 2399, 35, 35, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (22, 2, N'TXB011029', CAST(N'2023-10-29' AS Date), 2399, 35, 35, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (23, 2, N'TXB011030', CAST(N'2023-10-30' AS Date), 2399, 35, 35, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (24, 3, N'TXB010701', CAST(N'2023-07-01' AS Date), 299, 1, 32, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (25, 3, N'TXB010705', CAST(N'2023-07-05' AS Date), 199, 2, 32, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (26, 3, N'TXB010708', CAST(N'2023-07-08' AS Date), 299, 0, 32, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (27, 3, N'TXB010712', CAST(N'2023-07-12' AS Date), 199, 6, 32, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (28, 3, N'TXB010715', CAST(N'2023-07-15' AS Date), 299, 0, 32, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (29, 3, N'TXB010719', CAST(N'2023-07-19' AS Date), 199, 11, 32, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (30, 3, N'TXB010722', CAST(N'2023-07-22' AS Date), 299, 0, 32, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (31, 3, N'TXB010726', CAST(N'2023-07-26' AS Date), 199, 2, 32, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (32, 3, N'TXB010729', CAST(N'2023-07-29' AS Date), 299, 0, 32, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (33, 3, N'TXB010802', CAST(N'2023-08-02' AS Date), 199, 7, 32, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (34, 3, N'TXB010805', CAST(N'2023-08-05' AS Date), 299, 0, 32, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (35, 3, N'TXB010809', CAST(N'2023-08-09' AS Date), 199, 9, 32, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (36, 3, N'TXB010812', CAST(N'2023-08-12' AS Date), 299, 1, 32, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (37, 3, N'TXB010816', CAST(N'2023-08-16' AS Date), 199, 5, 32, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (38, 3, N'TXB010819', CAST(N'2023-08-19' AS Date), 299, 0, 32, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (39, 3, N'TXB010823', CAST(N'2023-08-23' AS Date), 199, 10, 32, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (42, 4, N'TPE3GG0820A2', CAST(N'2023-08-20' AS Date), 1799, 0, 42, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (43, 4, N'TPE3GG0903A2', CAST(N'2023-09-03' AS Date), 1799, 0, 42, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (44, 4, N'TPE3GG0903A2', CAST(N'2023-09-03' AS Date), 1799, 40, 42, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (45, 4, N'TPE3GG0908A2', CAST(N'2023-09-08' AS Date), 1799, 37, 42, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (46, 4, N'TPE3GG0910A2', CAST(N'2023-09-10' AS Date), 1799, 0, 42, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (47, 4, N'TPE3GG0915A2', CAST(N'2023-09-15' AS Date), 1799, 13, 42, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (48, 4, N'TPE3GG0915B2', CAST(N'2023-09-15' AS Date), 1799, 0, 42, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (49, 5, N'TPE3BB0817A3', CAST(N'2023-08-17' AS Date), 2599, 38, 42, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (50, 5, N'TPE3BB0820A3', CAST(N'2023-08-20' AS Date), 2599, 36, 42, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (51, 5, N'TPE3BB0821A3', CAST(N'2023-08-21' AS Date), 2599, 28, 42, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (52, 5, N'TPE3BB0822A3', CAST(N'2023-08-22' AS Date), 2599, 42, 42, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (53, 5, N'TPE3BB0823A3', CAST(N'2023-08-23' AS Date), 2599, 42, 42, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (54, 5, N'TPE3BB0824A3', CAST(N'2023-08-24' AS Date), 2599, 0, 42, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (55, 5, N'TPE3BB0907A3', CAST(N'2023-09-07' AS Date), 2599, 23, 42, 1)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (56, 5, N'TPE3BB0914A3', CAST(N'2023-09-14' AS Date), 2599, 30, 42, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (57, 5, N'TPE3BB0921A3', CAST(N'2023-09-21' AS Date), 2599, 30, 42, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (58, 5, N'TPE3BB0926A3', CAST(N'2023-09-26' AS Date), 2599, 40, 42, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (59, 5, N'TPE3BB1001A3', CAST(N'2023-10-01' AS Date), 2599, 40, 42, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (60, 5, N'TPE3BB1002A3', CAST(N'2023-10-02' AS Date), 2599, 36, 42, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (61, 5, N'TPE3BB1003A3', CAST(N'2023-10-03' AS Date), 2599, 42, 42, 0)
GO
INSERT [dbo].[TravelSession] ([id], [travel_id], [product_number], [departure_date], [price], [remaining_seats], [seats], [group_status]) VALUES (62, 1, N'testNum', CAST(N'2023-09-01' AS Date), 1199, 32, 42, 0)
GO
INSERT [dbo].[User] ([id], [role], [provider], [name], [email], [password], [access_token]) VALUES (1, N'admin', N'native', N'王曉明', N'xiaoming@gmail.com', N'$2a$11$z7/xnp9KtdOnbfm6cvpGo.S0Wz1V2mf1RHE7ysrCEPTi13gflcA72', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoi546L5puJ5piOIiwicHJvdmlkZXIiOiJuYXRpdmUiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJhZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InhpYW9taW5nQGdtYWlsLmNvbSIsImV4cCI6MTY5MzMyOTM2NCwiaXNzIjoiTXlBcHAiLCJhdWQiOiJNeUNsaWVudCJ9.M1KYpWwTOJRhH1rzzilTVMMcGyK2MucQ0ClXO7gzIPA')
GO
INSERT [dbo].[User] ([id], [role], [provider], [name], [email], [password], [access_token]) VALUES (2, N'user', N'native', N'陳暖暖', N'nuan@gmail.com', N'0911122233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoi6Zmz5pqW5pqWIiwiZW1haWwiOiJudWFuQGdtYWlsLmNvbSIsIlJvbGUiOiJ1c2VyIiwiZXhwIjoxNjkzMDQ3ODMyLCJpc3MiOiJNeUFwcCIsImF1ZCI6Ik15Q2xpZW50In0.7gXvCPwe4uioFKrEa-bnlCzfnI-6bcJKRmxX7Q4cXgo')
GO
INSERT [dbo].[User] ([id], [role], [provider], [name], [email], [password], [access_token]) VALUES (3, N'user', N'native', N'張小玉', N'yuyu@gmail.com', N'0944455566', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoi5by15bCP546JIiwiZW1haWwiOiJ5dXl1QGdtYWlsLmNvbSIsIlJvbGUiOiJ1c2VyIiwiZXhwIjoxNjkzMDQ3ODU2LCJpc3MiOiJNeUFwcCIsImF1ZCI6Ik15Q2xpZW50In0.wzLGUOH6B05xcZ2omj6ebnnk1sX0SoOff4PtQnMnvUY')
GO
INSERT [dbo].[User] ([id], [role], [provider], [name], [email], [password], [access_token]) VALUES (4, N'user', N'native', N'高雄一', N'syong@gmail.com', N'0977788899', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoi6auY6ZuE5LiAIiwiZW1haWwiOiJzeW9uZ0BnbWFpbC5jb20iLCJSb2xlIjoidXNlciIsImV4cCI6MTY5MzA0Nzg3NSwiaXNzIjoiTXlBcHAiLCJhdWQiOiJNeUNsaWVudCJ9.iyiuPXjlqXAukEv-3btEeqQIF3mhXXd9KAsyRQPcBVE')
GO
INSERT [dbo].[User] ([id], [role], [provider], [name], [email], [password], [access_token]) VALUES (5, N'user', N'facebook', N'黃詩洳', N'afa982000@yahoo.com.tw', NULL, N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjUiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoi6buD6Kmp5rSzIiwicHJvdmlkZXIiOiJmYWNlYm9vayIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6InVzZXIiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhZmE5ODIwMDBAeWFob28uY29tLnR3IiwiZXhwIjoxNjkzNDc2MjI2LCJpc3MiOiJNeUFwcCIsImF1ZCI6Ik15Q2xpZW50In0.JcAGDdPc0tGqOYvbw2z3O9yucrjOtzN0SwdmrPUT-Dw')
GO
INSERT [dbo].[User] ([id], [role], [provider], [name], [email], [password], [access_token]) VALUES (6, N'user', N'native', N'陳小如', N'test@test.com', N'$2a$11$olB0gMgZNUQsOVqs1sf7B.FsHKErMsmgZq2ArQTNDXzT3Gt798Cv6', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoi6Zmz5bCP5aaCIiwicHJvdmlkZXIiOiJuYXRpdmUiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJ1c2VyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjoidGVzdEB0ZXN0LmNvbSIsImV4cCI6MTY5MzMzMzcxNSwiaXNzIjoiTXlBcHAiLCJhdWQiOiJNeUNsaWVudCJ9.Z5QJGPqYFhoy9dJdSBV5LSQ_sfvIttGZHw2_g39wvTg')
GO
INSERT [dbo].[User] ([id], [role], [provider], [name], [email], [password], [access_token]) VALUES (7, N'user', N'native', N'曾欣欣', N'xinxin@gmail.com', N'$2a$11$tQH1nXuiXfsyEZ.v8tGJMe/HR4/P.VovbniYdatiRXIVaPWxYe5pW', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjciLCJwcm92aWRlciI6Im5hdGl2ZSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6InVzZXIiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ4aW54aW5AZ21haWwuY29tIiwiZXhwIjoxNjkzMTk0NjIzLCJpc3MiOiJNeUFwcCIsImF1ZCI6Ik15Q2xpZW50In0.CJC0Dd2x3zcOfmKvITfAeslxrk9qk0vIS0rkSgjT5p8')
GO
INSERT [dbo].[User] ([id], [role], [provider], [name], [email], [password], [access_token]) VALUES (8, N'user', N'native', N'王大大', N'wang@gmail.com', N'$2a$11$z7/xnp9KtdOnbfm6cvpGo.S0Wz1V2mf1RHE7ysrCEPTi13gflcA72', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjgiLCJwcm92aWRlciI6Im5hdGl2ZSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6InVzZXIiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ3YW5nQGdtYWlsLmNvbSIsImV4cCI6MTY5MzMwMzQ1NCwiaXNzIjoiTXlBcHAiLCJhdWQiOiJNeUNsaWVudCJ9.kQIPFjhZsfWGkuInazE17-kHFvDWivVR1VHf8iPAICc')
GO
INSERT [dbo].[User] ([id], [role], [provider], [name], [email], [password], [access_token]) VALUES (9, N'Admin', N'native', N'曾三郎', N'tseng@gmail.com', N'$2a$11$k51JtcAhrh77DDPmNRQcnOLd3jcyHK5653WwdtGSF4QF9cB.ZYLmu', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjkiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoi5pu-5LiJ6YOOIiwicHJvdmlkZXIiOiJuYXRpdmUiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InRzZW5nQGdtYWlsLmNvbSIsImV4cCI6MTY5MzQ3NjIzNywiaXNzIjoiTXlBcHAiLCJhdWQiOiJNeUNsaWVudCJ9.4f1GXQf8YawwdLs7QuTViNzZdFUPsR7y0-QC6g1LCO8')
GO
INSERT [dbo].[Watch] ([id], [date], [travel_id], [user_id], [stay_time]) VALUES (1, CAST(N'2023-01-17T20:48:09.000' AS DateTime), 1, 1, CAST(N'00:25:24' AS Time))
GO
INSERT [dbo].[Watch] ([id], [date], [travel_id], [user_id], [stay_time]) VALUES (2, CAST(N'2023-02-18T19:48:09.000' AS DateTime), 2, 2, CAST(N'00:35:13' AS Time))
GO
INSERT [dbo].[Watch] ([id], [date], [travel_id], [user_id], [stay_time]) VALUES (3, CAST(N'2023-03-19T08:18:09.000' AS DateTime), 3, 4, CAST(N'00:15:54' AS Time))
GO
INSERT [dbo].[Watch] ([id], [date], [travel_id], [user_id], [stay_time]) VALUES (4, CAST(N'2023-03-20T20:48:09.000' AS DateTime), 2, 3, CAST(N'00:18:05' AS Time))
GO
INSERT [dbo].[Watch] ([id], [date], [travel_id], [user_id], [stay_time]) VALUES (5, CAST(N'2023-04-17T10:28:09.000' AS DateTime), 3, 1, CAST(N'00:35:54' AS Time))
GO
INSERT [dbo].[Watch] ([id], [date], [travel_id], [user_id], [stay_time]) VALUES (6, CAST(N'2023-07-20T20:05:09.000' AS DateTime), 3, 4, CAST(N'00:29:15' AS Time))
GO
INSERT [dbo].[Watch] ([id], [date], [travel_id], [user_id], [stay_time]) VALUES (7, CAST(N'2023-07-21T22:41:09.000' AS DateTime), 1, 3, CAST(N'00:10:29' AS Time))
GO
INSERT [dbo].[Watch] ([id], [date], [travel_id], [user_id], [stay_time]) VALUES (8, CAST(N'2023-08-23T20:48:09.000' AS DateTime), 1, 1, CAST(N'00:22:48' AS Time))
GO
INSERT [dbo].[Watch] ([id], [date], [travel_id], [user_id], [stay_time]) VALUES (9, CAST(N'2023-08-30T09:14:09.000' AS DateTime), 2, 2, CAST(N'00:03:29' AS Time))
GO
INSERT [dbo].[Watch] ([id], [date], [travel_id], [user_id], [stay_time]) VALUES (10, CAST(N'2023-09-07T20:22:09.000' AS DateTime), 3, 4, CAST(N'00:07:31' AS Time))
GO
INSERT [dbo].[Watch] ([id], [date], [travel_id], [user_id], [stay_time]) VALUES (11, CAST(N'2023-09-17T23:59:09.000' AS DateTime), 3, 3, CAST(N'00:12:21' AS Time))
GO
INSERT [dbo].[Watch] ([id], [date], [travel_id], [user_id], [stay_time]) VALUES (12, CAST(N'2023-10-21T10:45:09.000' AS DateTime), 1, 1, CAST(N'00:15:57' AS Time))
GO
INSERT [dbo].[Watch] ([id], [date], [travel_id], [user_id], [stay_time]) VALUES (13, CAST(N'2023-10-22T08:48:09.000' AS DateTime), 1, 4, CAST(N'00:04:28' AS Time))
GO
INSERT [dbo].[Watch] ([id], [date], [travel_id], [user_id], [stay_time]) VALUES (14, CAST(N'2023-10-30T09:40:09.000' AS DateTime), 2, 4, CAST(N'00:17:23' AS Time))
GO
INSERT [dbo].[Watch] ([id], [date], [travel_id], [user_id], [stay_time]) VALUES (15, CAST(N'2023-11-19T17:36:09.000' AS DateTime), 2, 2, CAST(N'00:33:57' AS Time))
GO
INSERT [dbo].[Watch] ([id], [date], [travel_id], [user_id], [stay_time]) VALUES (16, CAST(N'2023-11-22T17:20:09.000' AS DateTime), 3, 2, CAST(N'00:11:38' AS Time))
GO
INSERT [dbo].[Watch] ([id], [date], [travel_id], [user_id], [stay_time]) VALUES (17, CAST(N'2023-11-30T10:41:09.000' AS DateTime), 3, 1, CAST(N'00:09:25' AS Time))
GO
INSERT [dbo].[Watch] ([id], [date], [travel_id], [user_id], [stay_time]) VALUES (18, CAST(N'2023-12-01T23:52:09.000' AS DateTime), 3, 3, CAST(N'00:18:14' AS Time))
GO
INSERT [dbo].[Watch] ([id], [date], [travel_id], [user_id], [stay_time]) VALUES (19, CAST(N'2023-12-15T18:39:09.000' AS DateTime), 1, 4, CAST(N'00:10:35' AS Time))
GO
INSERT [dbo].[Watch] ([id], [date], [travel_id], [user_id], [stay_time]) VALUES (20, CAST(N'2023-12-31T20:48:09.000' AS DateTime), 1, 3, CAST(N'00:20:55' AS Time))
GO
