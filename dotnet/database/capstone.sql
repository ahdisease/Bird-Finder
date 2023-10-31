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
	skill_level varchar(20) NOT NULL DEFAULT 'beginner',	--beginner, intermediate, advanced
	favorite_bird varchar(20),
	most_common_bird varchar(20),
	profile_active bit NOT NULL DEFAULT 0

	CONSTRAINT PK_user PRIMARY KEY (user_id)
);

CREATE TABLE list (
	id int IDENTITY(1,1) NOT NULL UNIQUE,
	user_id int NOT NULL,
	name varchar(50)

	CONSTRAINT PK_list PRIMARY KEY (id)
);

CREATE TABLE bird (
	id int IDENTITY(1,1) NOT NULL UNIQUE,
	list_id int NOT NULL,
	name varchar(50) NOT NULL,
	picture varchar(max),
	zip_code varchar(5)

	CONSTRAINT PK_bird PRIMARY KEY (id)
)

CREATE TABLE bird_sighting (
	id int IDENTITY(1,1) NOT NULL,
	bird_id int NOT NULL,
	date_sighted date NOT NULL DEFAULT GETDATE(),
	males_spotted tinyint,
	females_spotted tinyint,
	feeder_type varchar(20) NOT NULL,
	food_blend varchar(20) NOT NULL,
	notes varchar(1000)

	CONSTRAINT PK_bird_sighting PRIMARY KEY (id)
)

--add foreign key contraints

ALTER TABLE list
	ADD CONSTRAINT FK_list_users FOREIGN KEY (user_id) REFERENCES users (user_id);
ALTER TABLE bird
	ADD CONSTRAINT FK_bird_list FOREIGN KEY (list_id) REFERENCES list (id);
ALTER TABLE bird_sighting
	ADD CONSTRAINT FK_bird_sighting_bird FOREIGN KEY (bird_id) REFERENCES bird (id);




--populate default data
	--users
INSERT INTO users (username, password_hash, salt, user_role) VALUES ('user','Jg45HuwT7PZkfuKTz6IB90CtWY4=','LHxP4Xh7bN0=','user');		--id 1
INSERT INTO users (username, password_hash, salt, user_role) VALUES ('user2','Jg45HuwT7PZkfuKTz6IB90CtWY4=','LHxP4Xh7bN0=','user');		--id 2
INSERT INTO users (username, password_hash, salt, user_role,skill_level) VALUES ('admin','YhyGVQ+Ch69n4JMBncM4lNF/i9s=', 'Ar/aB2thQTI=','admin','pro');	--id 3

	--list
INSERT INTO list ( user_id, name ) VALUES ( 1, 'Home'); --id 1
INSERT INTO list ( user_id, name ) VALUES ( 1, 'Work'); --id 2
INSERT INTO list ( user_id, name ) VALUES ( 2, 'Home'); --id 3
INSERT INTO list ( user_id, name ) VALUES ( 2, 'Vacation'); --id 4
INSERT INTO list ( user_id, name ) VALUES ( 3, 'Detroit'); --id 5

--user profiles
UPDATE users 
	SET favorite_bird = 'Blue Jay', most_common_bird = 'Vulture'
	WHERE users.username = 'user'
UPDATE users 
	SET favorite_bird = 'Crow', most_common_bird = 'Canary'
	WHERE users.username = 'user2'
UPDATE users 
	SET favorite_bird = 'Hummingbird', most_common_bird = 'Pigeon'
	WHERE users.username = 'admin'

	--bird
		--id 1
INSERT INTO bird (list_id, name, picture, zip_code) VALUES ( 1, 'Blue Jay', 'https://macaulaylibrary.org/asset/609892314', '11111'); 
		--id 2
INSERT INTO bird (list_id, name, picture, zip_code) VALUES ( 1, 'Canary', 'https://macaulaylibrary.org/asset/609854044', '11111');
		--id 3
INSERT INTO bird (list_id, name, picture, zip_code) VALUES ( 2, 'Crow', 'https://macaulaylibrary.org/asset/609258235', '22222');
		--id 4
INSERT INTO bird (list_id, name, picture, zip_code) VALUES ( 2, 'Goose', 'https://macaulaylibrary.org/asset/609836519', '22222');
		--id 5
INSERT INTO bird (list_id, name, picture, zip_code) VALUES ( 3, 'Hummingbird', 'https://macaulaylibrary.org/asset/609681565', '33333');
		--id 6
INSERT INTO bird (list_id, name, picture, zip_code) VALUES ( 3, 'Crow', 'https://macaulaylibrary.org/asset/609258235', '33333');
		--id 7
INSERT INTO bird (list_id, name, picture, zip_code) VALUES ( 4, 'Crow', 'https://macaulaylibrary.org/asset/609258235', '44444');
		--id 8
INSERT INTO bird (list_id, name, picture, zip_code) VALUES ( 5, 'Blue Jay', 'https://macaulaylibrary.org/asset/609892314', '55555'); 



INSERT INTO bird_sighting (bird_id, males_spotted, females_spotted, feeder_type, food_blend) VALUES (1,0,1,'Cylinder','Live Mealworms');
INSERT INTO bird_sighting (bird_id, males_spotted, females_spotted, feeder_type, food_blend) VALUES (1,0,1,'Cylinder','Suet Cake');
INSERT INTO bird_sighting (bird_id, males_spotted, females_spotted, feeder_type, food_blend) VALUES (2,1,1,'Seed-Tube','Sunflower-blend');


--UPDATE users
--	SET most_common_bird = ( 
--		SELECT TOP(1) bird_id as count FROM bird_sighting 
--			JOIN users ON users.user_id = bird_sighting.user_id 
--		WHERE users.user_id = 1
--		GROUP BY bird_id
--		ORDER BY count(bird_id) DESC
--		)
--	WHERE users.user_id = 1	



--create stored proceedures
GO
CREATE PROCEDURE Delete_List
	-- Add the parameters for the stored procedure here
	@Id int,
	@Username varchar(50)
AS
BEGIN TRANSACTION

DELETE bird_sighting
	FROM bird_sighting
	JOIN bird on bird_sighting.bird_id = bird.id
	JOIN list on bird.list_id = list.id
WHERE list_id = @Id AND user_id = (SELECT user_id FROM users WHERE username = @Username);

DELETE bird
	FROM bird
	JOIN list on bird.list_id = list.id
WHERE list_id = @Id AND user_id = (SELECT user_id FROM users WHERE username = @Username);

DELETE list
WHERE id = @Id AND user_id = (SELECT user_id FROM users WHERE username = @Username);

COMMIT
GO
CREATE PROCEDURE Delete_Bird
	-- Add the parameters for the stored procedure here
	@Id int,
	@Username varchar(50)
AS
BEGIN TRANSACTION

DELETE bird_sighting
	FROM bird_sighting
	JOIN bird on bird_sighting.bird_id = bird.id
	JOIN list on bird.list_id = list.id
WHERE bird.id = @Id AND user_id = (SELECT user_id FROM users WHERE username = @Username);

DELETE bird
	FROM bird
	JOIN list on bird.list_id = list.id
WHERE bird.id = @Id AND user_id = (SELECT user_id FROM users WHERE username = @Username);

COMMIT
GO
CREATE PROCEDURE Delete_Bird_Sighting
	-- Add the parameters for the stored procedure here
	@Id int,
	@Username varchar(50)
AS
BEGIN TRANSACTION

DELETE bird_sighting
	FROM bird_sighting
	JOIN bird on bird_sighting.bird_id = bird.id
	JOIN list on bird.list_id = list.id
WHERE bird_sighting.id = @Id AND user_id = (SELECT user_id FROM users WHERE username = @Username);

COMMIT
GO