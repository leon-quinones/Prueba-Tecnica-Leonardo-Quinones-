CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TABLE "GameResults" (
    "Id" uuid NOT NULL,
    "Number" integer,
    "Color" text NOT NULL,
    "Category" integer NOT NULL,
    CONSTRAINT "PK_GameResults" PRIMARY KEY ("Id")
);

CREATE TABLE "Players" (
    "Username" text NOT NULL,
    "Balance" numeric NOT NULL,
    CONSTRAINT "PK_Players" PRIMARY KEY ("Username")
);

CREATE TABLE "Games" (
    "Id" uuid NOT NULL,
    "OutcomeId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    CONSTRAINT "PK_Games" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Games_GameResults_OutcomeId" FOREIGN KEY ("OutcomeId") REFERENCES "GameResults" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Wagers" (
    "Id" uuid NOT NULL,
    "Username" text NOT NULL,
    "GameId" uuid NOT NULL,
    "BetResultId" uuid NOT NULL,
    "Amount" numeric NOT NULL,
    "Winnings" numeric,
    "IncludedInBalance" boolean DEFAULT (False),
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    CONSTRAINT "PK_Wagers" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Wagers_GameResults_BetResultId" FOREIGN KEY ("BetResultId") REFERENCES "GameResults" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Wagers_Games_GameId" FOREIGN KEY ("GameId") REFERENCES "Games" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Wagers_Players_Username" FOREIGN KEY ("Username") REFERENCES "Players" ("Username") ON DELETE CASCADE
);

CREATE INDEX "IX_Games_OutcomeId" ON "Games" ("OutcomeId");

CREATE INDEX "IX_Wagers_BetResultId" ON "Wagers" ("BetResultId");

CREATE INDEX "IX_Wagers_GameId" ON "Wagers" ("GameId");

CREATE INDEX "IX_Wagers_Username" ON "Wagers" ("Username");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250404204235_initial', '9.0.3');

CREATE TABLE "ActiveSessions" (
    "Id" text NOT NULL,
    "Token" text NOT NULL,
    "Username" text NOT NULL,
    CONSTRAINT "PK_ActiveSessions" PRIMARY KEY ("Id")
);

CREATE TABLE "GameResults" (
    "Id" uuid NOT NULL,
    "Number" integer,
    "Color" text NOT NULL,
    "Category" integer NOT NULL,
    CONSTRAINT "PK_GameResults" PRIMARY KEY ("Id")
);

CREATE TABLE "Players" (
    "Username" text NOT NULL,
    "Balance" numeric NOT NULL,
    CONSTRAINT "PK_Players" PRIMARY KEY ("Username")
);

CREATE TABLE "Games" (
    "Id" uuid NOT NULL,
    "OutcomeId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    CONSTRAINT "PK_Games" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Games_GameResults_OutcomeId" FOREIGN KEY ("OutcomeId") REFERENCES "GameResults" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Wagers" (
    "Id" uuid NOT NULL,
    "Username" text NOT NULL,
    "GameId" uuid NOT NULL,
    "BetResultId" uuid NOT NULL,
    "Amount" numeric NOT NULL,
    "Winnings" numeric,
    "IncludedInBalance" boolean DEFAULT (False),
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    CONSTRAINT "PK_Wagers" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Wagers_GameResults_BetResultId" FOREIGN KEY ("BetResultId") REFERENCES "GameResults" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Wagers_Games_GameId" FOREIGN KEY ("GameId") REFERENCES "Games" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Wagers_Players_Username" FOREIGN KEY ("Username") REFERENCES "Players" ("Username") ON DELETE CASCADE
);

CREATE INDEX "IX_Games_OutcomeId" ON "Games" ("OutcomeId");

CREATE INDEX "IX_Wagers_BetResultId" ON "Wagers" ("BetResultId");

CREATE INDEX "IX_Wagers_GameId" ON "Wagers" ("GameId");

CREATE INDEX "IX_Wagers_Username" ON "Wagers" ("Username");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250408152554_Initial', '9.0.3');

COMMIT;

