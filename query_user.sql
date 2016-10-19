use InsectDemo

select

	u.username, 
	u.salt, 
	u.twofactorcode, 
	p.hash as PasswordHash, 
	s.sessionId, 
	s.expirydate as SessionExpiryDate

from Users u 
left join [Sessions] s on s.UserId = u.Id
left join PasswordHashs p on u.Id = p.UserId