--
-- PostgreSQL database cluster dump
--

\restrict EPzQeoor9U6bPCIIdpoXwTAG4WPXUhWQ1LPZ9opYIpmAe8HIcGcFv4QdUY7Cm9m

SET default_transaction_read_only = off;

SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;

--
-- Roles
--

CREATE ROLE postgres;
ALTER ROLE postgres WITH SUPERUSER INHERIT CREATEROLE CREATEDB LOGIN REPLICATION BYPASSRLS PASSWORD 'SCRAM-SHA-256$4096:ite3EQksuuW2luKcjViBcg==$LqAOQlMVmQn3lnXY7FSgR+Myz4wwwAssQKDEhoWzVxY=:Qvfc932/TQt3SwtuyUqIGEnpKAWkspC/5ngMiOs7dU4=';

--
-- User Configurations
--








\unrestrict EPzQeoor9U6bPCIIdpoXwTAG4WPXUhWQ1LPZ9opYIpmAe8HIcGcFv4QdUY7Cm9m

--
-- Databases
--

--
-- Database "template1" dump
--

\connect template1

--
-- PostgreSQL database dump
--

\restrict frnz9JVyzqabrWupXxWzCPzEn0R8mvt9h92vfRjDFX9FHCmWM7EwM2tYcSaIhn4

-- Dumped from database version 16.15
-- Dumped by pg_dump version 16.15

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- PostgreSQL database dump complete
--

\unrestrict frnz9JVyzqabrWupXxWzCPzEn0R8mvt9h92vfRjDFX9FHCmWM7EwM2tYcSaIhn4

--
-- Database "AuthDb" dump
--

--
-- PostgreSQL database dump
--

\restrict OdyRecJYTRsVfRHhxSVYI4hamozKiXcPIAJcuVGsxxd7G1OgegKTZvUoohXdEgw

-- Dumped from database version 16.15
-- Dumped by pg_dump version 16.15

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: AuthDb; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE "AuthDb" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'en_US.utf8';


ALTER DATABASE "AuthDb" OWNER TO postgres;

\unrestrict OdyRecJYTRsVfRHhxSVYI4hamozKiXcPIAJcuVGsxxd7G1OgegKTZvUoohXdEgw
\connect "AuthDb"
\restrict OdyRecJYTRsVfRHhxSVYI4hamozKiXcPIAJcuVGsxxd7G1OgegKTZvUoohXdEgw

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: Credentials; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Credentials" (
    "Id" uuid NOT NULL,
    "Username" text NOT NULL,
    "PasswordHash" text NOT NULL,
    "Role" integer NOT NULL,
    "UserId" uuid NOT NULL
);


ALTER TABLE public."Credentials" OWNER TO postgres;

--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- Data for Name: Credentials; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Credentials" ("Id", "Username", "PasswordHash", "Role", "UserId") FROM stdin;
26706674-b3df-453a-805c-02528cd29931	admin4	$2a$11$sOlQDzdWgebGcPgIiIdkhesQ2O9MxEb4f.XX2QJVv7JacbF0juQWu	1	4b485382-33e2-4bf0-987a-a7a53733ad67
f9b6db9e-e8ee-4ad1-a3c7-4d077a180799	john_auth	$2a$11$.gnInM4YrDnb57duz1/0nuIJ1gQarXgujbXeyOjSf32wZl/6bqCQu	2	346a5812-db8a-47b8-a804-ece1befd2b39
9ebc80a3-4aaa-427a-8348-c506c07912a0	john	$2a$11$.gnInM4YrDnb57duz1/0nuIJ1gQarXgujbXeyOjSf32wZl/6bqCQu	2	c207c192-608c-489f-8c6f-0a6be6adbc9d
89f37244-665d-4c86-8b11-c3379cb0d2ff	temp_mihai	$2a$11$1Gxnuie0NEWKXknj1lbwGeArLEtoZIxrzB5DHpGqmjokNrJamAQVW	2	31a76fd5-7503-424d-bda0-d664bf072d2e
18eee86e-7ec6-4124-978d-534fa50e7258	Mihai@gmail.com	$2a$11$1Gxnuie0NEWKXknj1lbwGeArLEtoZIxrzB5DHpGqmjokNrJamAQVW	2	135d6e52-b17f-4aac-885d-fb5a43f49a36
7396c0d4-0cb7-4231-90a7-0432d205843b	email@email.com	$2a$11$ZFEv5eTQyADGy.hdMpIiCun2DPWBrWtqV4tozd..cUs6NTNvjemna	2	e9684642-9fc7-4022-a6d0-ed8510c97361
8e2ab04f-e5dd-45ab-a2b9-f1e6544b5a59	GeneralAdmin	$2a$11$OF3LkKaegnH32aFcs5TXb.gQlnNM.uMBRGYIciJ6QxHx9lkcPawAW	2	2af67dd4-1b33-4871-98d5-d8ce10bc9041
81b19609-a1c9-44ff-bd9b-b44a5fcae05f	admin5	$2a$11$R8q64cDs3K9rDMKfenXFq.rNjbynHIKQGKWkHzEuWFOsu3t8VQRgm	1	7b9c12ce-7451-420a-869d-d9715a7eabbd
05a1a49b-6614-480a-8942-2581870a0ce5	tokenTest	$2a$11$OEftSTuO.Pap4uBkAfslRuAGYMEQjVGIjX3N6nK/h1oDx.3QciaH2	2	116723ac-c64a-41f6-911b-cacefd0d89a6
25d26f6c-fff2-43df-be0c-c168eb96c305	aaa	$2a$11$Hkwj61xLiKdkRsVF1FFi5ekrZsA.99MEZMM5i9QBEFShVPUYaj.Zi	2	6ef389fc-edbe-442e-b52a-c505b51064e5
05bd94d5-af3a-4e01-8cd6-aa62bdb16566	email@aaa.com	$2a$11$F6sqSd5RTckE0h9eTjsFKep5yz2HW6xioCtcK9TbCvsAaXXXdYU7i	2	9174062e-9b24-448c-bd50-ca4462ac916d
1b4359c6-47aa-465a-9f69-2ff33b46ac78	NEWClient	$2a$11$Jxct8vU0jyrNBi3o.ry6sO5HStJdbAvl3k0XSCqz7G7yEq6opqmTK	2	cca517ff-3bce-4af4-a981-a901f1efe5c5
f0a08842-f35b-499b-bd41-68afacca3342	test1	$2a$11$leeMxTz/Jjw7tYNIeDeNT.SbpUiR1MtzrYD4TYa1nKhe/pj7vYJKS	2	b950b172-3e5b-4b21-a18b-aae0b71ce45e
\.


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20261029190351_InitialCreate	10.0.12
\.


--
-- Name: Credentials PK_Credentials; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Credentials"
    ADD CONSTRAINT "PK_Credentials" PRIMARY KEY ("Id");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: IX_Credentials_Username; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Credentials_Username" ON public."Credentials" USING btree ("Username");


--
-- PostgreSQL database dump complete
--

\unrestrict OdyRecJYTRsVfRHhxSVYI4hamozKiXcPIAJcuVGsxxd7G1OgegKTZvUoohXdEgw

--
-- Database "DeviceDb" dump
--

--
-- PostgreSQL database dump
--

\restrict 5hRVB6grejUDb1s3w991yNlk9xA9cayDFjxEDgcFzehlnVSCYpbG3pPJZxgc34U

-- Dumped from database version 16.15
-- Dumped by pg_dump version 16.15

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: DeviceDb; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE "DeviceDb" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'en_US.utf8';


ALTER DATABASE "DeviceDb" OWNER TO postgres;

\unrestrict 5hRVB6grejUDb1s3w991yNlk9xA9cayDFjxEDgcFzehlnVSCYpbG3pPJZxgc34U
\connect "DeviceDb"
\restrict 5hRVB6grejUDb1s3w991yNlk9xA9cayDFjxEDgcFzehlnVSCYpbG3pPJZxgc34U

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: Devices; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Devices" (
    "Id" uuid NOT NULL,
    "DeviceName" text NOT NULL,
    "ConsumptionValue" integer NOT NULL,
    "UserId" uuid
);


ALTER TABLE public."Devices" OWNER TO postgres;

--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- Data for Name: Devices; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Devices" ("Id", "DeviceName", "ConsumptionValue", "UserId") FROM stdin;
d4002a13-3c90-4372-9e11-58f451b4820c	Smart Meter Kitchen	240	\N
e7f91d6c-3b76-4e75-a992-ce6dbf8c7a03	device2	100	e9684642-9fc7-4022-a6d0-ed8510c97361
0c3914c5-0843-4673-a051-fdb283fb10f4	Televizor	390	\N
45a4f844-0f7b-4008-aed5-7b0322f0d617	Ps5	300	\N
5e71d188-1e90-4f39-9c61-80ac5536cd84	Masina Spalat	1000	\N
\.


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20261007073552_InitialCreate	10.0.12
20261014065513_AddUserIdToDevice	10.0.12
\.


--
-- Name: Devices PK_Devices; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Devices"
    ADD CONSTRAINT "PK_Devices" PRIMARY KEY ("Id");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- PostgreSQL database dump complete
--

\unrestrict 5hRVB6grejUDb1s3w991yNlk9xA9cayDFjxEDgcFzehlnVSCYpbG3pPJZxgc34U

--
-- Database "UserDb" dump
--

--
-- PostgreSQL database dump
--

\restrict vc99ob7PjfZ09L81Bz7FBb50sJ9hOY43nphB8RLAvgOe5BqP6xMKGc0ciYPeEHe

-- Dumped from database version 16.15
-- Dumped by pg_dump version 16.15

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: UserDb; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE "UserDb" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'en_US.utf8';


ALTER DATABASE "UserDb" OWNER TO postgres;

\unrestrict vc99ob7PjfZ09L81Bz7FBb50sJ9hOY43nphB8RLAvgOe5BqP6xMKGc0ciYPeEHe
\connect "UserDb"
\restrict vc99ob7PjfZ09L81Bz7FBb50sJ9hOY43nphB8RLAvgOe5BqP6xMKGc0ciYPeEHe

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: Users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Users" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Email" text NOT NULL,
    "Password" text NOT NULL,
    "Type" integer NOT NULL
);


ALTER TABLE public."Users" OWNER TO postgres;

--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- Data for Name: Users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Users" ("Id", "Name", "Email", "Password", "Type") FROM stdin;
e9684642-9fc7-4022-a6d0-ed8510c97361	tester	email@email.com	a1234	2
2af67dd4-1b33-4871-98d5-d8ce10bc9041	GeneralAdmin	GeneralAdmin	admin	2
7b9c12ce-7451-420a-869d-d9715a7eabbd	admin5	admin5	admin	1
116723ac-c64a-41f6-911b-cacefd0d89a6	tokenTest	tokenTest	test	2
6ef389fc-edbe-442e-b52a-c505b51064e5	aaa	aaa	aaa	2
9174062e-9b24-448c-bd50-ca4462ac916d	mihai	email@aaa.com	1234	2
b950b172-3e5b-4b21-a18b-aae0b71ce45e	test1	test	test	2
\.


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20261007073732_InitialCreate	10.0.12
\.


--
-- Name: Users PK_Users; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "PK_Users" PRIMARY KEY ("Id");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- PostgreSQL database dump complete
--

\unrestrict vc99ob7PjfZ09L81Bz7FBb50sJ9hOY43nphB8RLAvgOe5BqP6xMKGc0ciYPeEHe

--
-- Database "postgres" dump
--

\connect postgres

--
-- PostgreSQL database dump
--

\restrict fHak5FLKBdhY5gxbPZXf9W5coybOgeqc85EcUVcFayCFby3oOggEonzwEifpjdt

-- Dumped from database version 16.15
-- Dumped by pg_dump version 16.15

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- PostgreSQL database dump complete
--

\unrestrict fHak5FLKBdhY5gxbPZXf9W5coybOgeqc85EcUVcFayCFby3oOggEonzwEifpjdt

--
-- PostgreSQL database cluster dump complete
--

