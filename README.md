# Talent Acquisition Strategies

A Windows desktop application design and relational database schema created to automate banking marketing team management, target tracking, performance evaluation, and debit/credit card operations

## Academic & Project Metadata
- **Project Title:** Talent Acquisition Strategies
- **Degree:** Master of Business Administration (M.B.A.)
- **Institution:** Osmania University, Approved by AICTE
- **Academic Year:**
- **Project Guidance / Organization:**
- **Development Methodology:** Rapid Application Development (RAD) model with emphasis on reliability, input validation, error handling, and maintainability

## Problem Statement & Solution Overview
### Problems in the Manual / Existing System
- Relying on manual registers, disconnected records, and manual calculations for transactions, balances, targets, and performance rankings.
- Slower retrieval of employee and customer records and delayed report generation for management decision-making.

### Proposed Automated Solution
- Centralizes records across branches and teams into a shared database environment.
- Automates target-versus-achievement tracking, bonus point calculation, quarterly evaluations, card enrollments, and transaction/billing records.
- Provides structured, menu-driven forms (`MASTER`, `TRANSACTIONS`, `REPORTS`) with input validation and error messages to improve accuracy and speed.

## System Architecture & Technology Stack
- **Application Type:** Windows GUI Desktop Application (Windows Forms / MDI interface).
- **Frontend & Data Access:** VB.NET (Visual Studio 2008 / .NET Framework) with ADO.NET (`SqlConnection`, `SqlCommand`, `SqlDataReader`, `SqlDataAdapter`, `DataSet`)
- **Database:** Microsoft SQL Server (relational tables, primary/foreign keys, referential integrity, and normalization up to Third Normal Form / 3NF).
- **Target Hardware & OS Baseline (from Report):** Windows NT 4 / 2000 / XP, SQL Server 2005 / Visual Studio .NET 2008, Pentium III (500 MHz+) or above, 128 MB RAM+, 100 MB+ free disk space, and network connectivity for branch access.

## Core Modules & Business Rules

### 1. Employee Recruitment & Performance Evaluation
- **Hierarchy:** Marketing teams are structured around **Business Managers** and **Business Executives**.
- **Recruitment & Confirmation (`EXE_APP`, `EXE_CONFIRM`):** Captures applicant details (application number, date, personal details, applied post, qualification, experience, address, contact info) and records confirmation status, designation, executive code, confirmation date, joining date, basic salary, branch code, reporting manager code, and confirmation flag
- **Monthly Targets & Bonus Points (`EXE_TARGET`):** Tracks monthly card targets (`Target_Cards`), actual cards sold (`Noc`), extra cards (`Extra_Cards`), pending cards (`Noc_Pending`), and bonus points (`Bonus_pts`).
  - **Business Executives:** Bonus points are awarded based on monthly target achievement and extra cards sold (e.g., 1 point for achieving target, 2 points per 2 extra cards, and 0 points if cards are pending).
  - **Business Managers:** Evaluated around team target achievement across subordinates under their manager code plus their own card output, with 0 points if targets/cards remain pending.
- **Quarterly Performance (`EXE_PERFORMANCE`):** Evaluates performance on a quarterly (3-month) cycle—aligned to the last Friday of the third month—recording total points, performance check date, team rank, individual rating, and next check date (avoiding Sundays or national holidays).

### 2. Branch Management (`BRANCH`)
- Maintains branch records (`Branch_Code`, address, city, state, country, pincode, phone, fax, email) and supports branch-level tracking of employees, transactions, and card enrollments.

### 3. Debit Card Module (`DEBITCARD_APP`, `DEBIT_CARD`, `DC_TRANS`)
- **Application & Issuance:** Captures customer application details (account type such as Savings or Current, executive code, nominee details, deposit amount with a minimum threshold of 5,000, card type, and address), then records issued card number, issue date, PIN, balance, balance update date, and maintenance charges.
- **Transactions:** Tracks transaction number, card number, withdrawal date/amount/type (`0`–`3` covering enquiries, cash, PIN change, mini statement), deposit date/amount/type (`0-Cash`, `1-DD`, `2-Cheque`), and updated balance.

### 4. Credit Card Module (`CREDITCARD_APP`, `CC_ENQUIRY`, `CREDIT_CARD`, `CC_TRANS`, `BILLS`)
- **Application & Verification Enquiry:** Records credit card applications (applicant info, card type, executive code, nominee, address/contact details) and tracks verification enquiries (receive date, enquiry date, completion date, remarks, and `F` finished / `P` pending status).
- **Card Issuance & Transactions:** Stores issued credit card number, issue date, credit limit, current credit amount (not exceeding the credit limit), card scheme type (such as Classic or Gold), maintenance charge, yearly rent, and rent payment date, alongside transaction records (transaction date, amount, operation type `0-Enquiry`, `1-Cash`, `2-Misc`, and interest amount).
- **Monthly Billing (`BILLS`):** Tracks monthly bill number, linked transaction/card number, bill date, due date, bill amount, amount paid, and clearance status (`CLEARED` / `NOT CLEARED`), accounting for interest after the due date.

## Intended Purpose & Scope

**Talent Acquisition Strategies** is an M.B.A. project system designed around banking marketing team operations—covering Business Managers and Business Executives, monthly card targets, bonus point rules, quarterly performance evaluations, branch records, and debit/credit card workflows—built with **VB.NET (Windows Forms, ADO.NET)** and **Microsoft SQL Server**.

> **Note:** The full project report PDF is not included in this repository. Sample records in `database/02_seed_and_queries.sql` use sanitized, generic placeholder names, `@example.com` emails, and synthetic IDs/numbers intended for safe local testing and demonstration.

## Prerequisites

* **Database:** Microsoft SQL Server (with SQL Server Management Studio or `sqlcmd`)
* **Application Layer:** Visual Studio with .NET / Windows Forms support (for compiling and running the VB.NET forms and modules)

## Setup & Execution Steps

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/mohammadsajiduddinwaheduddin/talent-acquisition-strategies.git](https://github.com/mohammadsajiduddinwaheduddin/talent-acquisition-strategies.git)
   cd talent-acquisition-strategies

2. **Create the database and tables:**
   Run database/01_create_tables.sql in SQL Server Management Studio or via sqlcmd (update <SERVER_NAME> and authentication flags to match your local SQL Server instance):

   sqlcmd -S <SERVER_NAME> -E -i database/01_create_tables.sql

4. **Load sanitized sample data and test reporting queries:**
Execute database/02_seed_and_queries.sql after the schema script completes:

   sqlcmd -S <SERVER_NAME> -E -i database/02_seed_and_queries.sql

6. **Configure and run the VB.NET project:**
Open the VB.NET source files in Visual Studio, update the SQL Server connection string placeholder to point to your local database instance, build the solution, and run the Windows Forms application.
 
### Accuracy Check Before You Commit
* Confirm that the database name (`TalentAcquisitionDB`), script file paths (`database/01_create_tables.sql`, `database/02_seed_and_queries.sql`), and VB.NET class/file names match what is actually committed in your repository.
* Replace `<SERVER_NAME>` in any command examples with clear guidance for the user's own local SQL Server instance, and double-check that no real emails, phone numbers, card numbers, or connection secrets appear anywhere in your code or docs.

## Database Schema & ER Diagrams

The relational model spans *13 tables* across Executive/Branch management, Debit Card operations, and Credit Card lifecycle/billing workflows, aligned with database/01_create_tables.sql and sanitized sample data in database/02_seed_and_queries.sql:

* *Full Schema (assets/er-full-schema.png):* BRANCH, EXE_APP (using Name), EXE_CONFIRM (including BmgrNo manager reference and BRANCH_CODE), EXE_TARGET, EXE_PERFORMANCE, DEBITCARD_APP, DEBIT_CARD, DC_TRANS, CREDITCARD_APP, CC_ENQUIRY, CREDIT_CARD, CC_TRANS, and BILLS.
* *Credit Card Sub-Schema (assets/er-credit-card.png):* Focuses on CREDITCARD_APP, CC_ENQUIRY, CREDIT_CARD, and CC_TRANS.

![Full 13-Table ER Diagram](assets/er-full-schema.pdf)
![Credit Card Sub-Schema ER Diagram](assets/er-credit-card.pdf)

## Repository Structure
```text
talent-acquisition-strategies/
├── README.md
├── .gitignore
├── docs/
│   └── TAS.pdf
├── database/
│   └── 01_create_tables.sql
└── src/
    └── TalentAcquisitionStrategies/
        ├── BonusCalculator.vb
        ├── DatabaseHelper.vb
        └── MainMenuForm.vb
