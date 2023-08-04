create table products (
	id int not null identity(1,1),
	name varchar(255) not null,
	price decimal(19,2) not null,
	primary key (id)
)

create table Users (
	username varchar(255) not null,
	password varchar(255) not null,
	primary key (username)
)

insert into Users (username, password) values ('client', 'client')

create table requests (
	id int not null identity(1,1),
	username varchar(255) not null,
	product_id int not null,
	product_name varchar(255) not null,
	product_price decimal(19,2) not null,
	primary key (id),
	foreign key (username) references Users(username),
	foreign key (product_id) references products(id)
)




delete from requests