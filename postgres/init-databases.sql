SELECT 'CREATE DATABASE "UserDb"'
WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'UserDb')\gexec

SELECT 'CREATE DATABASE "DeviceDb"'
WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'DeviceDb')\gexec

SELECT 'CREATE DATABASE "AuthDb"'
WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'AuthDb')\gexec
