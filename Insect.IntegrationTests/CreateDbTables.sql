create table InsectTest.dbo.[Users]
(Id int identity(1,1) not null,
Username varchar(max),
Salt varchar(max),
MobileFor2Factor varchar(max),
SecurityAnswer1 varchar(max),
SecurityAnswer2 varchar(max),
SecurityQuestion1 varchar(max),
SecurityQuestion2 varchar(max),
FailedLoginCount int,
PasswordExpiryDate datetime null,
IsAdministrator bit,
IsLocked bit)

create table InsectTest.dbo.[Sessions]
(
Id int identity(1,1) not null,
SessionId uniqueidentifier,
UserId int,
ExpiryDate datetime null,
Level int
)

create table InsectTest.dbo.PasswordHashs
(
Id int identity(1,1) not null,
UserId int,
[Hash] varbinary(max)
)

