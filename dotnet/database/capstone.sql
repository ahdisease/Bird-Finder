USE master
GO

--drop database if it exists
IF DB_ID('final_capstone') IS NOT NULL
BEGIN
	ALTER DATABASE final_capstone SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE final_capstone;
END

CREATE DATABASE final_capstone
GO

USE final_capstone
GO

--create tables
CREATE TABLE users (
	user_id int IDENTITY(1,1) NOT NULL,
	username varchar(50) NOT NULL,
	password_hash varchar(200) NOT NULL,
	salt varchar(200) NOT NULL,
	user_role varchar(50) NOT NULL,
	location varchar(50),
	skill_level varchar(20) NOT NULL DEFAULT 'beginner',	--beginner, intermediate, pro
	favorite_bird int,
	most_common_bird int

	CONSTRAINT PK_user PRIMARY KEY (user_id)
);

CREATE TABLE bird (
	id int IDENTITY(1,1) NOT NULL UNIQUE,
	name varchar(50) NOT NULL,
	description varchar(1000),
	picture varchar(max)

	CONSTRAINT PK_bird PRIMARY KEY (id)
)

CREATE TABLE bird_sighting (
	id int IDENTITY(1,1) NOT NULL,
	user_id int NOT NULL,
	bird_id int NOT NULL,
	date_sighted date NOT NULL DEFAULT GETDATE()

	CONSTRAINT PK_bird_sighting PRIMARY KEY (id)
)

--add foreign key contraints
ALTER TABLE users
	ADD CONSTRAINT FK_favorite_bird FOREIGN KEY (favorite_bird) REFERENCES bird (id);

ALTER TABLE users
	ADD CONSTRAINT FK_most_common_bird FOREIGN KEY (most_common_bird) REFERENCES bird (id);

ALTER TABLE bird_sighting
	ADD CONSTRAINT FK_bird_sighting_users FOREIGN KEY (user_id) REFERENCES users (user_id);

ALTER TABLE bird_sighting
	ADD CONSTRAINT FK_bird_sighting_users FOREIGN KEY (bird_id) REFERENCES bird (id);


--populate default data
INSERT INTO users (username, password_hash, salt, user_role) VALUES ('user','Jg45HuwT7PZkfuKTz6IB90CtWY4=','LHxP4Xh7bN0=','user');
INSERT INTO users (username, password_hash, salt, user_role) VALUES ('admin','YhyGVQ+Ch69n4JMBncM4lNF/i9s=', 'Ar/aB2thQTI=','admin');

GO