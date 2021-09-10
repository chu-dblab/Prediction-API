CREATE TABLE Users(
	Email varchar(MAX) PRIMARY KEY NOT NULL,
	Whois nvarchar(10) NOT NULL,
	schoolLocation nvarchar(10) NOT NULL,
	schoolName nvarchar(50) NOT NULL,
	token varchar(64),
	isPass smallint
);