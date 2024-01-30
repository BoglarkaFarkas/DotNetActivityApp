CREATE TABLE IF NOT EXISTS "Location" (
    "Id" serial PRIMARY KEY,
    "NameCity" VARCHAR(100),
    "ExactLocation" VARCHAR(255)
);

INSERT INTO "Location" ("NameCity", "ExactLocation")
SELECT 'Home', 'Your home'
WHERE NOT EXISTS (
    SELECT 1 FROM "Location" 
    WHERE "NameCity" = 'Home' AND "ExactLocation" = 'Your home'
);

INSERT INTO "Location" ("NameCity", "ExactLocation")
SELECT 'Dunajská Streda', 'Galantská cesta 5692/20, 929 01 Dunajská Streda'
WHERE NOT EXISTS (
    SELECT 1 FROM "Location" 
    WHERE "NameCity" = 'Dunajská Streda' AND "ExactLocation" = 'Galantská cesta 5692/20, 929 01 Dunajská Streda'
);

INSERT INTO "Location" ("NameCity", "ExactLocation")
SELECT 'Dunajská Streda', 'Korzo Bélu Bartóka 791, 929 01 Dunajská Streda'
WHERE NOT EXISTS (
    SELECT 1 FROM "Location" 
    WHERE "NameCity" = 'Dunajská Streda' AND "ExactLocation" = 'Korzo Bélu Bartóka 791, 929 01 Dunajská Streda'
);

INSERT INTO "Location" ("NameCity", "ExactLocation")
SELECT 'Orechová Potôň', 'Orechová Potôň 810, 930 02 Orechová Potôň'
WHERE NOT EXISTS (
    SELECT 1 FROM "Location" 
    WHERE "NameCity" = 'Orechová Potôň' AND "ExactLocation" = 'Orechová Potôň 810, 930 02 Orechová Potôň'
);

INSERT INTO "Location" ("NameCity", "ExactLocation")
SELECT 'Gabčíkovo', 'Nový rad, 4009, 930 05 Gabčíkovo'
WHERE NOT EXISTS (
    SELECT 1 FROM "Location" 
    WHERE "NameCity" = 'Gabčíkovo' AND "ExactLocation" = 'Nový rad, 4009, 930 05 Gabčíkovo'
);

INSERT INTO "Location" ("NameCity", "ExactLocation")
SELECT 'Dunajská Streda', 'Námestie Ármina Vámberyho 2-13, 929 01 Dunajská Streda'
WHERE NOT EXISTS (
    SELECT 1 FROM "Location" 
    WHERE "NameCity" = 'Dunajská Streda' AND "ExactLocation" = 'Námestie Ármina Vámberyho 2-13, 929 01 Dunajská Streda'
);

INSERT INTO "Location" ("NameCity", "ExactLocation")
SELECT 'Dunajská Streda', 'Segafredo Family Center, Galantská cesta, 929 01 Dunajská Streda'
WHERE NOT EXISTS (
    SELECT 1 FROM "Location" 
    WHERE "NameCity" = 'Dunajská Streda' AND "ExactLocation" = 'Segafredo Family Center, Galantská cesta, 929 01 Dunajská Streda'
);

INSERT INTO "Location" ("NameCity", "ExactLocation")
SELECT 'Dunajská Streda', 'Kiss Burger, Galantská cesta 5542, 929 01 Dunajská Streda'
WHERE NOT EXISTS (
    SELECT 1 FROM "Location" 
    WHERE "NameCity" = 'Dunajská Streda' AND "ExactLocation" = 'Kiss Burger, Galantská cesta 5542, 929 01 Dunajská Streda'
);

INSERT INTO "Location" ("NameCity", "ExactLocation")
SELECT 'Šamorín', 'Francesco PIZZA, Veterná 2283/39, 931 01 Šamorín'
WHERE NOT EXISTS (
    SELECT 1 FROM "Location" 
    WHERE "NameCity" = 'Šamorín' AND "ExactLocation" = 'Francesco PIZZA, Veterná 2283/39, 931 01 Šamorín'
);

INSERT INTO "Location" ("NameCity", "ExactLocation")
SELECT 'Dunajská Streda', 'Wellness Hotel THERMA, Priemyselná 624, 929 01 Dunajská Streda'
WHERE NOT EXISTS (
    SELECT 1 FROM "Location" 
    WHERE "NameCity" = 'Dunajská Streda' AND "ExactLocation" = 'Wellness Hotel THERMA, Priemyselná 624, 929 01 Dunajská Streda'
);

INSERT INTO "Location" ("NameCity", "ExactLocation")
SELECT 'Kyselica', 'Danube riverside, 930 30, Kyselica'
WHERE NOT EXISTS (
    SELECT 1 FROM "Location" 
    WHERE "NameCity" = 'Kyselica' AND "ExactLocation" = 'Danube riverside, 930 30, Kyselica'
);

CREATE TABLE IF NOT EXISTS "Activities" (
    "Id" serial PRIMARY KEY,
    "Name" VARCHAR(100),
    "Price" DOUBLE PRECISION,
    "Time" VARCHAR(50),
    "LocationId" INT,
    FOREIGN KEY ("LocationId") REFERENCES "Location"("Id")
);

INSERT INTO "Activities" ("Name", "Price", "Time", "LocationId")
SELECT 'Deep cleaning', 0.0, '1-5 hours', (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Home' AND "ExactLocation" = 'Your home')
WHERE NOT EXISTS (
    SELECT 1 FROM "Activities" 
    WHERE "Name" = 'Deep cleaning' AND "Price" = 0.0 AND "Time" = '1-5 hours' AND "LocationId" = (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Home' AND "ExactLocation" = 'Your home')
);

INSERT INTO "Activities" ("Name", "Price", "Time", "LocationId")
SELECT 'Relax at home', 0.0, '1-2 hours', (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Home' AND "ExactLocation" = 'Your home')
WHERE NOT EXISTS (
    SELECT 1 FROM "Activities" 
    WHERE "Name" = 'Relax at home' AND "Price" = 0.0 AND "Time" = '1-2 hours' AND "LocationId" = (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Home' AND "ExactLocation" = 'Your home')
);

INSERT INTO "Activities" ("Name", "Price", "Time", "LocationId")
SELECT 'Netflix and Chill', 7.99, '1-2 hours', (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Home' AND "ExactLocation" = 'Your home')
WHERE NOT EXISTS (
    SELECT 1 FROM "Activities" 
    WHERE "Name" = 'Netflix and Chill' AND "Price" = 7.99 AND "Time" = '1-2 hours' AND "LocationId" = (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Home' AND "ExactLocation" = 'Your home')
);

INSERT INTO "Activities" ("Name", "Price", "Time", "LocationId")
SELECT 'Cinema', 6.5, '1-2 hours', (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Dunajská Streda' AND "ExactLocation" = 'Galantská cesta 5692/20, 929 01 Dunajská Streda')
WHERE NOT EXISTS (
    SELECT 1 FROM "Activities" 
    WHERE "Name" = 'Cinema' AND "Price" = 6.5 AND "Time" = '1-2 hours' AND "LocationId" = (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Dunajská Streda' AND "ExactLocation" = 'Galantská cesta 5692/20, 929 01 Dunajská Streda')
);

INSERT INTO "Activities" ("Name", "Price", "Time", "LocationId")
SELECT 'Theatre', 15.0, '2-3 hours', (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Dunajská Streda' AND "ExactLocation" = 'Korzo Bélu Bartóka 791, 929 01 Dunajská Streda')
WHERE NOT EXISTS (
    SELECT 1 FROM "Activities" 
    WHERE "Name" = 'Theatre' AND "Price" = 15.0 AND "Time" = '2-3 hours' AND "LocationId" = (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Dunajská Streda' AND "ExactLocation" = 'Korzo Bélu Bartóka 791, 929 01 Dunajská Streda')
);

INSERT INTO "Activities" ("Name", "Price", "Time", "LocationId")
SELECT 'ZOO - Malkia Park', 10.0, '1-3 hours', (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Orechová Potôň' AND "ExactLocation" = 'Orechová Potôň 810, 930 02 Orechová Potôň')
WHERE NOT EXISTS (
    SELECT 1 FROM "Activities" 
    WHERE "Name" = 'ZOO - Malkia Park' AND "Price" = 10.0 AND "Time" = '1-3 hours' AND "LocationId" = (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Orechová Potôň' AND "ExactLocation" = 'Orechová Potôň 810, 930 02 Orechová Potôň')
);

INSERT INTO "Activities" ("Name", "Price", "Time", "LocationId")
SELECT 'A walk outdoors', 0.0, '1-3 hours', (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Gabčíkovo' AND "ExactLocation" = 'Nový rad, 4009, 930 05 Gabčíkovo')
WHERE NOT EXISTS (
    SELECT 1 FROM "Activities" 
    WHERE "Name" = 'A walk outdoors' AND "Price" = 0.0 AND "Time" = '1-3 hours' AND "LocationId" = (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Gabčíkovo' AND "ExactLocation" = 'Nový rad, 4009, 930 05 Gabčíkovo')
);

INSERT INTO "Activities" ("Name", "Price", "Time", "LocationId")
SELECT 'Desserts and ice creams (Balkan Ice Cream Tiffany)', 3.0, '1-2 hours', (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Dunajská Streda' AND "ExactLocation" = 'Námestie Ármina Vámberyho 2-13, 929 01 Dunajská Streda')
WHERE NOT EXISTS (
    SELECT 1 FROM "Activities" 
    WHERE "Name" = 'Desserts and ice creams (Balkan Ice Cream Tiffany)' AND "Price" = 3.0 AND "Time" = '1-2 hours' AND "LocationId" = (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Dunajská Streda' AND "ExactLocation" = 'Námestie Ármina Vámberyho 2-13, 929 01 Dunajská Streda')
);

INSERT INTO "Activities" ("Name", "Price", "Time", "LocationId")
SELECT 'Coffee with a friend', 5.0, '1-2 hours', (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Dunajská Streda' AND "ExactLocation" = 'Segafredo Family Center, Galantská cesta, 929 01 Dunajská Streda')
WHERE NOT EXISTS (
    SELECT 1 FROM "Activities" 
    WHERE "Name" = 'Coffee with a friend' AND "Price" = 5.0 AND "Time" = '1-2 hours' AND "LocationId" = (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Dunajská Streda' AND "ExactLocation" = 'Segafredo Family Center, Galantská cesta, 929 01 Dunajská Streda')
);

INSERT INTO "Activities" ("Name", "Price", "Time", "LocationId")
SELECT 'Grabbing a quick bite at a fast food restaurant', 5.99, '1-2 hours', (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Dunajská Streda' AND "ExactLocation" = 'Kiss Burger, Galantská cesta 5542, 929 01 Dunajská Streda')
WHERE NOT EXISTS (
    SELECT 1 FROM "Activities" 
    WHERE "Name" = 'Grabbing a quick bite at a fast food restaurant' AND "Price" = 5.99 AND "Time" = '1-2 hours' AND "LocationId" = (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Dunajská Streda' AND "ExactLocation" = 'Kiss Burger, Galantská cesta 5542, 929 01 Dunajská Streda')
);

INSERT INTO "Activities" ("Name", "Price", "Time", "LocationId")
SELECT 'Having pizza with friends', 10.0, '1-2 hours', (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Šamorín' AND "ExactLocation" = 'Francesco PIZZA, Veterná 2283/39, 931 01 Šamorín')
WHERE NOT EXISTS (
    SELECT 1 FROM "Activities" 
    WHERE "Name" = 'Having pizza with friends' AND "Price" = 10.0 AND "Time" = '1-2 hours' AND "LocationId" = (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Šamorín' AND "ExactLocation" = 'Francesco PIZZA, Veterná 2283/39, 931 01 Šamorín')
);

INSERT INTO "Activities" ("Name", "Price", "Time", "LocationId")
SELECT 'Wellness', 14.9, '4-5 hours', (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Dunajská Streda' AND "ExactLocation" = 'Wellness Hotel THERMA, Priemyselná 624, 929 01 Dunajská Streda')
WHERE NOT EXISTS (
    SELECT 1 FROM "Activities" 
    WHERE "Name" = 'Wellness' AND "Price" = 14.9 AND "Time" = '4-5 hours' AND "LocationId" = (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Dunajská Streda' AND "ExactLocation" = 'Wellness Hotel THERMA, Priemyselná 624, 929 01 Dunajská Streda')
);

INSERT INTO "Activities" ("Name", "Price", "Time", "LocationId")
SELECT 'Picnic', 5.0, '1-3 hours', (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Kyselica' AND "ExactLocation" = 'Danube riverside, 930 30, Kyselica')
WHERE NOT EXISTS (
    SELECT 1 FROM "Activities" 
    WHERE "Name" = 'Picnic' AND "Price" = 5.0 AND "Time" = '1-3 hours' AND "LocationId" = (SELECT "Id" FROM "Location" WHERE "NameCity" = 'Kyselica' AND "ExactLocation" = 'Danube riverside, 930 30, Kyselica')
);