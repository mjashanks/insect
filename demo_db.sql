
ALTER DATABASE InsectDemo
SET SINGLE_USER
WITH ROLLBACK IMMEDIATE;
ALTER DATABASE InsectDemo
SET MULTI_USER;

go
DROP DATABASE InsectDemo
go
create database InsectDemo
go
create table InsectDemo.dbo.[Users]
(Id int identity(1,1) not null,
Username varchar(max),
Salt varchar(max),
MobileFor2Factor varchar(max),
TwoFactorCode varchar(max),
FailedLoginCount int,
PasswordExpiryDate datetime null,
IsAdministrator bit,
IsLocked bit)

create table InsectDemo.dbo.[Sessions]
(
Id int identity(1,1) not null,
SessionId uniqueidentifier,
UserId int,
ExpiryDate datetime null,
Level int
)

create table InsectDemo.dbo.PasswordHashs
(
Id int identity(1,1) not null,
UserId int,
[Hash] varbinary(max)
)

