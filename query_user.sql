select

	u.username, 
	u.salt, 
	u.twofactorcode, 
	p.hash as PasswordHash, 
	s.sessionId, 
	s.expirydate as SessionExpiryDate

from InsectDemo.dbo.Users u 
left join InsectDemo.dbo.[Sessions] s on s.UserId = u.Id
left join InsectDemo.dbo.PasswordHashs p on u.Id = p.UserId