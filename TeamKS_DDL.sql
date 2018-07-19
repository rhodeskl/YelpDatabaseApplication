CREATE TABLE Business(
business_id VARCHAR(40),
name VARCHAR(100),
review_rating FLOAT,
address VARCHAR(150),
numcheckins INTEGER,
state_name CHAR(2),
city CHAR(50),
postal_code VARCHAR(5),
is_open BOOLEAN,
latitude FLOAT,
longitude FLOAT,
stars INTEGER,
review_count INTEGER,
Primary Key (business_id)
);

CREATE TABLE Review(
review_id VARCHAR(40),
business_id VARCHAR(40) REFERENCES Business(business_id),
user_id VARCHAR(40) REFERENCES UserTable(user_id),
stars INTEGER,
date_review DATE,
text VARCHAR(5000),
useful BOOLEAN,
funny BOOLEAN,
cool BOOLEAN,
PRIMARY KEY (review_id)
);

CREATE TABLE UserTable(
user_id VARCHAR(40),
name VARCHAR(50),
avg_stars FLOAT,
fans INTEGER,
review_count INTEGER,
yelping_since DATE,
useful INTEGER,
funny INTEGER,
cool INTEGER,
PRIMARY KEY (user_id)
);

CREATE TABLE Checkin(
day_of_week VARCHAR(9),
business_id VARCHAR(40),
night INTEGER,
evening INTEGER,
afternoon INTEGER,
morning INTEGER,
PRIMARY KEY (business_id,day_of_week),
FOREIGN KEY (business_id) REFERENCES Business(business_id)
);

CREATE TABLE Hours(
day_of_week VARCHAR(9),
business_id VARCHAR(40),
start_hour TIME,
end_hour TIME,
PRIMARY KEY (business_id,day_of_week),
FOREIGN KEY (business_id) REFERENCES Business(business_id)
);

CREATE TABLE Category(
category VARCHAR(50),
business_id VARCHAR(40),
PRIMARY KEY (business_id,category),
FOREIGN KEY (business_id) REFERENCES Business(business_id)
);

CREATE TABLE hasFriends(
user_id VARCHAR(40),
friend_id VARCHAR(40),
PRIMARY KEY (user_id,friend_id),
FOREIGN KEY (user_id) REFERENCES UserTable(user_id),
FOREIGN KEY (friend_id) REFERENCES UserTable(user_id),
CONSTRAINT CHK_not_self CHECK (user_id <> friend_id)
);



