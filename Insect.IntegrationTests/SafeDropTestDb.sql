﻿
ALTER DATABASE InsectTest
SET SINGLE_USER
WITH ROLLBACK IMMEDIATE;
ALTER DATABASE InsectTest
SET MULTI_USER;


DROP DATABASE InsectTest