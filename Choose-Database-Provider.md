# Choose a Database Provider

For small or educational projects, several free 
or nearly free databases are available: from local 
solutions (SQLite, PostgreSQL, MySQL) to cloud services 
with free tiers (Azure SQL, AWS RDS, Google Cloud SQL). 
The most convenient option depends on whether you need a 
production deployment in the cloud or just local development.

## Local Databases

### SQLite

Lightweight, file-based database.
Ideal for small projects, demos, and local development.
Does not require a server.

### PostgreSQL

Powerful open-source relational DBMS.
Free, supports complex queries, extensions, JSON.

### MySQL / MariaDB

Popular relational databases.
Free in the open-source version, well-suited for web projects.

## Cloud Databases (free‑tier or conditionally free)

| Provider | Free‑tier conditions | Limitations | When beneficial |
| --- | --- | --- | --- |
| **Azure SQL Database** | **32 GB storage + 100,000 vCore seconds** per month free [itfb.com.ua](https://itfb.com.ua/uk/nova-bezkoshtovna-propozitsiya-bazi-danih-azure-sql-teper-dostupna-dlya-vsih-pidpisok/) | Only one database per subscription, paid after limit | For educational projects, PoC |
| **AWS RDS (MySQL/PostgreSQL/MariaDB)** | Free‑tier: **750 hours/month** instance usage + **20 GB storage** for 12 months [AWS](https://aws.amazon.com/ru/free/database/) | Time and storage limits, paid after one year | For test deployments in AWS |
| **Amazon Aurora DSQL** | Always free basic tier: **1 GB storage + 100,000 processing units** [AWS](https://aws.amazon.com/ru/free/database/) | Very limited capacity | For experiments with Aurora |
| **Google Cloud SQL / Firestore** | Free‑tier: **1 GB storage** + small operation quota | Paid after limit | For integration with GCP |
| **Supabase (Postgres‑as‑a‑Service)** | Free‑tier: **500 MB database + 2 GB files + 50,000 requests** | Limited capacity | For quick start with Postgres |
| **MongoDB Atlas** | Free‑tier: **512 MB cluster** | Limited performance | For NoSQL projects |


## Comparison of Free/Nearly Free Databases

| Database | Type | EF Core Integration | Free‑tier / Conditions | When Beneficial |
| --- | --- | --- | --- | --- |
| **SQLite** | Relational, file-based | Full support | Completely free | Local development, demos, small projects |
| **PostgreSQL** | Relational | Full support | Free (open‑source) | Production, complex queries, extensions |
| **MySQL / MariaDB** | Relational | Full support | Free (open‑source) | Web projects, classic CMS |
| **Supabase (Postgres‑as‑a‑Service)** | Cloud relational | EF Core via Postgres | Free‑tier: 500 MB DB, 2 GB files, 50,000 requests | Quick cloud start, small production projects |
| **MongoDB Atlas** | NoSQL (document) | EF Core not native, requires third‑party providers | Free‑tier: 512 MB cluster | For NoSQL projects, JSON storage |
| **Azure SQL Database** | Relational (SQL Server) | Full support | Free‑tier: 32 GB storage + 100,000 vCore seconds | ASP.NET Core in Azure, educational projects |
| **AWS RDS (Postgres/MySQL/MariaDB)** | Relational | Full support | Free‑tier: 750 hours/month + 20 GB storage (12 months) | Test deployments in AWS |
| **Google Cloud SQL / Firestore** | Relational / NoSQL | EF Core for SQL, Firestore via third‑party providers | Free‑tier: ~1 GB | Integration with GCP |

## In the cloud:

If you want a simple start — Supabase (Postgres with a ready free‑tier).
If you plan deployment in Azure — go with Azure SQL Free‑tier.
If you prefer AWS — RDS Free‑tier (but only for the first year).

## Official Database Websites
- [SQLite](https://www.sqlite.org/)
- [PostgreSQL](https://www.postgresql.org/)
- [MySQL](https://www.mysql.com/)
- [MariaDB](https://mariadb.org/)
- [Supabase](https://supabase.com/)
- [MongoDB](https://www.mongodb.com/)
- [Azure SQL Database](https://azure.microsoft.com/services/sql-database/)
- [AWS RDS](https://aws.amazon.com/rds/)
- [Google Cloud SQL](https://cloud.google.com/sql)

## Summary Table of Databases

| Database | Type | Official Website |
| --- | --- | --- |
| **SQLite** | Lightweight file-based relational DB | [sqlite.org](https://www.sqlite.org) |
| **PostgreSQL** | Powerful open‑source relational DB | [postgresql.org](https://www.postgresql.org) |
| **MySQL (Community Edition)** | Popular relational DB | [mysql.com](https://www.mysql.com) |
| **Supabase (Postgres‑as‑a‑Service)** | Cloud Postgres platform | [supabase.com](https://supabase.com) |
| **MongoDB Atlas** | Cloud NoSQL document DB | [mongodb.com/atlas](https://www.mongodb.com/atlas) |
| **Azure SQL Database (Free Tier)** | Cloud SQL Server by Microsoft | learn.microsoft.com/azure/azure-sql [(learn.microsoft.com in Bing)](https://www.bing.com/search?q="https%3A%2F%2Flearn.microsoft.com%2Fen-us%2Fazure%2Fazure-sql%2Fdatabase%2Ffree-offer-overview") |
| **AWS RDS (Free Tier)** | Cloud relational DB (Postgres/MySQL/MariaDB/SQL Server Express) | [aws.amazon.com/rds/free](https://aws.amazon.com/rds/free) |
| **Google Cloud SQL (Free Trial)** | Cloud relational DB (Postgres/MySQL/SQL Server) | [cloud.google.com/sql](https://cloud.google.com/sql) |


## I think we should choose from three databases:

Supabase, MongoDB Atlas — they have free‑tier plans, but with data volume limits (500 MB – 1 GB).
Azure SQL Database — free with 32 GB + 100,000 vCore seconds per month.
AWS RDS — free for 12 months (750 hours/month + 20 GB).

## Or even from just two databases:

- Supabase (Postgres) — free‑tier: 500 MB DB, 2 GB files, 50,000 requests.
- AWS RDS — free for 12 months (750 hours/month + 20 GB).

## All three can technically support:

### The required entities 
— users.
- authentication
- interests.
- onboarding data.
- pins.
- boards.
- likes.
- comments.
- and future social features.