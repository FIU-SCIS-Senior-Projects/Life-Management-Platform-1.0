USE [SeniorDB]
GO

INSERT INTO [dbo].[Categories]
           ([Name])
     VALUES
           ('Joy'), ('Passion'), ('Giving Back')
GO
INSERT INTO [dbo].[Activities]
           ([Name]
           ,[CategoryId]
           ,[Img]
           ,[ImgMime])
     VALUES
           ('jogging'
           ,1
           ,CONVERT(IMAGE, CAST(N'TEST TEXT' AS VARBINARY(MAX)))
           ,'') , ('beach'
           ,1
           ,CONVERT(IMAGE, CAST(N'TEST TEXT' AS VARBINARY(MAX)))
           ,''),
  ('skating'
           ,1
           ,CONVERT(IMAGE, CAST(N'TEST TEXT' AS VARBINARY(MAX)))
           ,''),
 ('p1'
           ,2
           ,CONVERT(IMAGE, CAST(N'TEST TEXT' AS VARBINARY(MAX)))
           ,''),
 ('p2'
           ,2
           ,CONVERT(IMAGE, CAST(N'TEST TEXT' AS VARBINARY(MAX)))
           ,''),
 ('p3'
           ,2
           ,CONVERT(IMAGE, CAST(N'TEST TEXT' AS VARBINARY(MAX)))
           ,''),
('gb1'
           ,3
           ,CONVERT(IMAGE, CAST(N'TEST TEXT' AS VARBINARY(MAX)))
           ,''),
 ('gb2'
           ,3
           ,CONVERT(IMAGE, CAST(N'TEST TEXT' AS VARBINARY(MAX)))
           ,''),
 ('gb3'
           ,3
           ,CONVERT(IMAGE, CAST(N'TEST TEXT' AS VARBINARY(MAX)))
           ,'')






GO
USE [SeniorDB]
GO

INSERT INTO [dbo].[Roles]
           ([Name])
     VALUES
           ('Guest'),('User'),('Admin')
