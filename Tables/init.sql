CREATE TABLE IF NOT EXISTS "Location" (
    "Id" serial PRIMARY KEY,
    "NameCity" VARCHAR(100),
    "ExactLocation" VARCHAR(255)
);

CREATE TABLE IF NOT EXISTS "Activities" (
    "Id" serial PRIMARY KEY,
    "Name" VARCHAR(100),
    "Price" DOUBLE PRECISION,
    "Time" VARCHAR(50),
    "LocationId" INT,
    FOREIGN KEY ("LocationId") REFERENCES "Location"("Id")
);
CREATE TABLE IF NOT EXISTS "MyUser" (
    "Id" serial PRIMARY KEY,
    "Surname" VARCHAR(100),
    "First_name" VARCHAR(100),
    "Password" VARCHAR(100),
    "Email" VARCHAR(255) UNIQUE
);

CREATE TABLE IF NOT EXISTS "MyUser_Activities" (
    "Id" serial PRIMARY KEY,
    "UserId" INT,
    "ActivityId" INT,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("UserId") REFERENCES "MyUser"("Id"),
    FOREIGN KEY ("ActivityId") REFERENCES "Activities"("Id")
);
