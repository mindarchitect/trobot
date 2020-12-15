CREATE TABLE "Robots" (
	"Id"	INTEGER NOT NULL UNIQUE,
	"FactoryId"	INTEGER NOT NULL,
	"Name"	TEXT NOT NULL,
	"Guid"	TEXT NOT NULL,
	"CreatedDate" TEXT NOT NULL,
	"ModifiedDate" TEXT,
	PRIMARY KEY("Id" AUTOINCREMENT)
	FOREIGN KEY("FactoryId") REFERENCES "Factories"("Id")
);

CREATE TABLE "Factories" (
	"Id"	INTEGER NOT NULL UNIQUE,
	"Name"	TEXT NOT NULL,
	"CreatedDate" TEXT NOT NULL,
	"ModifiedDate" TEXT,
	PRIMARY KEY("Id" AUTOINCREMENT)
);

INSERT INTO "Factories" ("Id", "Name", "CreatedDate") VALUES( 1, "Warehouse factory", "2020-12-15 12:00:00:000");

INSERT INTO "Robots" ("Id", "FactoryId", "Name" , "Guid", "CreatedDate") VALUES( 1, 1,	"Warehouse Robot 1", "6e0382ca-e956-4123-a65b-c5124c004fc5", "2020-12-15 12:00:00:000");
INSERT INTO "Robots" ("Id", "FactoryId", "Name" , "Guid", "CreatedDate") VALUES( 2, 1,	"Warehouse Robot 2", "44326f33-0b07-46d8-b694-95f0c891c61b", "2020-12-15 12:00:00:000");