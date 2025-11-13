create database if not exists courseproject;

use courseproject;

create table if not exists product_and_recipe_list(
	product_id int not null primary key auto_increment,
    product_name varchar(100) not null,
    calory int not null,
    protein float not null,
    fats float not null,
    carbohydrates float not null,
    food_type varchar(20) not null
);

create table if not exists user_account(
	user_id int not null primary key auto_increment,
    user_name varchar(45) not null,
    user_lastname varchar(45) not null,
    user_login varchar(45) not null,
    user_password varchar (1000) not null,
    user_height int not null,
    user_birthdate date not null,
    user_gender varchar(20) not null
);

create table if not exists user_weight_story(
	user_wieght_story_id int not null primary key auto_increment,
    user_id int not null,
    user_weight float not null,
    weight_date date not null,
	foreign key (user_id) 
    references courseproject.user_account(user_id) 
    on delete cascade
);

create table if not exists eating(
	eating_id int not null primary key auto_increment,
    user_id int not null,
    eating_date date not null,
    product_id int not null,
    product_count int not null,
    eating_type varchar(20),
	foreign key (user_id) 
    references courseproject.user_account(user_id) 
    on delete cascade,
    foreign key (product_id) 
    references courseproject.product_and_recipe_list(product_id) 
    on delete cascade
);
