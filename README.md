# Travel-Platform

## Introduction
![images](https://github.com/ShiRu19/Travel-Platform/blob/main/TravelPlatform/TravelPlatform/wwwroot/images/happygotravel_logo.png?raw=true)

Happy Go Travel 是一款提供給旅行團業務進行行程推廣使用的旅行團資訊平台。

網站入口：```https://localhost:[port]/index.html```

## Development Environment
- Frontend
	HTML5、CSS、JavaScript
- Backend
	.net 7
- Database
	MSSQL

## Deployment
1. Start SQL Server
2. Import database:
	1. Execute travel.create.sql to create tables
	2. Execute travel.insert.sql to insert data
3. Create .env for back-end
4. Modify .env for back-end
5. Clear Browser localStorage if needed. The same address will use the same space to records localStorage key-value pairs and it may conflict with mine. (Optional)