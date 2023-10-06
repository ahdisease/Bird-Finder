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
	favorite_bird int,
	most_common_bird int,
	profile_active bit NOT NULL DEFAULT 0

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
	ADD CONSTRAINT FK_bird_sighting_bird FOREIGN KEY (bird_id) REFERENCES bird (id);


--populate default data
INSERT INTO users (username, password_hash, salt, user_role) VALUES ('user','Jg45HuwT7PZkfuKTz6IB90CtWY4=','LHxP4Xh7bN0=','user');		--id 1
INSERT INTO users (username, password_hash, salt, user_role) VALUES ('user2','Jg45HuwT7PZkfuKTz6IB90CtWY4=','LHxP4Xh7bN0=','user');		--id 2

INSERT INTO users (username, password_hash, salt, user_role,skill_level) VALUES ('admin','YhyGVQ+Ch69n4JMBncM4lNF/i9s=', 'Ar/aB2thQTI=','admin','pro');	--id 3

INSERT INTO bird (name,description,picture) VALUES ('Blue Jay','blue and white feathers','https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Ftse3.mm.bing.net%2Fth%3Fid%3DOIP.ynprG77zY_wgz2HStlLb2wHaIL%26pid%3DApi&f=1&ipt=ead3dc40e057ff5f8617f3463abc469cbcd3924a6e01a0207d19df052a07fe69&ipo=images');
INSERT INTO bird (name,description,picture) VALUES ('Canary','Canary is a small yellow bird belonging to the finch family. This bird is common in the western and central regions of Southern Africa. These birds have different species, and they are known for their singing ability. Canaries are often considered as good pets.','https://images.unsplash.com/photo-1586788454110-2d7a4382bd01?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1974&q=80');
INSERT INTO bird (name,description,picture) VALUES ('Crow','Crow is a black medium-sized bird belonging to the Corvidae family. Ravens and rooks belong to this family. Crows are a common bird in India and can be identified by their ‘cawing’ sound.','http://pixnio.com/free-images/fauna-animals/birds/ravens-and-crows-pictures/hawaiian-crow-black-bird.jpg');
INSERT INTO bird (name,description,picture) VALUES ('Hummingbird','Hummingbird is the smallest bird, measuring 7.5 – 13 cm. There are 361 species of hummingbirds around the world. They are brightly coloured birds with long and narrow beaks to drink nectar from flowers. These birds can also fly backwards.','https://images.unsplash.com/photo-1662477551619-ed427ed3d021?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2070&q=80');
INSERT INTO bird (name,description,picture) VALUES ('Goose','Geese are the most common species belonging to the Anatidae family. Geese are somewhere between ducks and swans. They live in ponds, lakes or rivers. Like ducks, geese lay eggs on land.','https://images.unsplash.com/photo-1608453508076-f127b370d51b?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1932&q=80');

UPDATE users 
	SET favorite_bird = (SELECT bird.id FROM bird WHERE bird.name = 'Blue Jay')
	WHERE users.username = 'user'
UPDATE users 
	SET favorite_bird = (SELECT bird.id FROM bird WHERE bird.name = 'Crow')
	WHERE users.username = 'user2'
UPDATE users 
	SET favorite_bird = (SELECT bird.id FROM bird WHERE bird.name = 'Hummingbird')
	WHERE users.username = 'admin'

INSERT INTO bird_sighting (user_id, bird_id) VALUES (1,3);
INSERT INTO bird_sighting (user_id, bird_id) VALUES (1,4);
INSERT INTO bird_sighting (user_id, bird_id) VALUES (1,3);
INSERT INTO bird_sighting (user_id, bird_id) VALUES (1,5);

INSERT INTO bird_sighting (user_id, bird_id) VALUES (2,1);
INSERT INTO bird_sighting (user_id, bird_id) VALUES (2,1);
INSERT INTO bird_sighting (user_id, bird_id) VALUES (2,4);
INSERT INTO bird_sighting (user_id, bird_id) VALUES (2,1);

UPDATE users
	SET most_common_bird = ( 
		SELECT TOP(1) bird_id as count FROM bird_sighting 
			JOIN users ON users.user_id = bird_sighting.user_id 
			WHERE users.user_id = 1
			GROUP BY bird_id
			ORDER BY count(bird_id) DESC
		)
	WHERE users.user_id = 1	

UPDATE users
	SET most_common_bird = ( 
		SELECT TOP(1) bird_id as count FROM bird_sighting 
			JOIN users ON users.user_id = bird_sighting.user_id 
			WHERE users.user_id = 2
			GROUP BY bird_id
			ORDER BY count(bird_id) DESC
		)
	WHERE users.user_id = 2
GO