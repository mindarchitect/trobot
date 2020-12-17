CREATE TABLE "Robots" (
	"Id"	INTEGER NOT NULL UNIQUE PRIMARY KEY AUTOINCREMENT,
	"FactoryId"	INTEGER NOT NULL,
	"Name"	TEXT NOT NULL,
	"Guid"	TEXT NOT NULL,
	"CreatedDate" TEXT NOT NULL,
	"ModifiedDate" TEXT,
	FOREIGN KEY("FactoryId") REFERENCES "Factories"("Id")
);

CREATE TABLE "Factories" (
	"Id"	INTEGER NOT NULL UNIQUE PRIMARY KEY AUTOINCREMENT,
	"Name"	TEXT NOT NULL,
	"CreatedDate" TEXT NOT NULL,
	"ModifiedDate" TEXT
);

CREATE TABLE "Users" (
	"Id"	INTEGER NOT NULL UNIQUE PRIMARY KEY AUTOINCREMENT,
	"UserName"	TEXT NOT NULL,
	"Password"	TEXT NOT NULL,
	"FirstName"	TEXT NOT NULL,
	"LastName"	TEXT NOT NULL,
	"CreatedDate" TEXT NOT NULL,
	"ModifiedDate" TEXT
);

CREATE TABLE "Roles" (
	"Id"	INTEGER NOT NULL UNIQUE PRIMARY KEY AUTOINCREMENT,
	"Name"	TEXT NOT NULL,
	"CreatedDate" TEXT,
	"ModifiedDate" TEXT
);

CREATE TABLE "UserRoles" (
	"Id"	INTEGER NOT NULL UNIQUE PRIMARY KEY AUTOINCREMENT,
	"UserId"	TEXT NOT NULL,
	"RoleId"	TEXT NOT NULL,
	"CreatedDate" TEXT NOT NULL,
	"ModifiedDate" TEXT,
	FOREIGN KEY("UserId") REFERENCES "Users"("Id"),
	FOREIGN KEY("RoleId") REFERENCES "Roles"("Id")
);

INSERT INTO "Users" ("Id", "UserName", "Password", "FirstName" , "LastName", "CreatedDate") VALUES (1, "User1", "User1", "First name", "Last name", "2020-12-15 12:00:00:000");
INSERT INTO "Users" ("Id", "UserName", "Password", "FirstName" , "LastName", "CreatedDate") VALUES (2, "User2", "User1", "First name", "Last name", "2020-12-15 12:00:00:000");

INSERT INTO "Roles" ("Id", "Name", "CreatedDate") VALUES (1, "Role 1",	"2020-12-15 12:00:00:000");
INSERT INTO "Roles" ("Id", "Name", "CreatedDate") VALUES (2, "Role 2",	"2020-12-15 12:00:00:000");

INSERT INTO "UserRoles" ("Id", "UserId", "RoleId", "CreatedDate") VALUES (1, 1, 1,	"2020-12-15 12:00:00:000");
INSERT INTO "UserRoles" ("Id", "UserId", "RoleId", "CreatedDate") VALUES (2, 1, 2,	"2020-12-15 12:00:00:000");
INSERT INTO "UserRoles" ("Id", "UserId", "RoleId", "CreatedDate") VALUES (3, 2, 1,	"2020-12-15 12:00:00:000");

INSERT INTO "Factories" ("Id", "Name", "CreatedDate") VALUES( 1, "Warehouse factory", "2020-12-15 12:00:00:000");

INSERT INTO "Robots" ("Id", "FactoryId", "Name" , "Guid", "CreatedDate") VALUES (1, 1,	"Warehouse Robot 1", "6e0382ca-e956-4123-a65b-c5124c004fc5", "2020-12-15 12:00:00:000");
INSERT INTO "Robots" ("Id", "FactoryId", "Name" , "Guid", "CreatedDate") VALUES (2, 1,	"Warehouse Robot 2", "44326f33-0b07-46d8-b694-95f0c891c61b", "2020-12-15 12:00:00:000");