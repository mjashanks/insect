create table InsectTest.dbo.[Users]
(Id int identity(1,1) not null,
Username varchar(max),
Salt varchar(max),
MobileFor2Factor varchar(max),
TwoFactorCode varchar(max),
FailedLoginCount int,
PasswordExpiryDate datetime null,
IsAdministrator bit,
EmailVerificationPath varchar(max),
VerificationExpiryDate datetime null,
IsLocked bit,
IsVerified bit)

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

