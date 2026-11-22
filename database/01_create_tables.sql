-- Talent Acquisition Strategies - SQL Server Schema
-- Database: TalentAcquisitionDB (SQL Server)

CREATE DATABASE TalentAcquisitionDB;
GO

USE TalentAcquisitionDB;
GO

-- 1. Branch Master Table
CREATE TABLE BRANCH (
    Branch_Code VARCHAR(8) PRIMARY KEY,
    Address VARCHAR(100) NOT NULL,
    City VARCHAR(25) NULL,
    State VARCHAR(25) NOT NULL,
    Country VARCHAR(25) NOT NULL,
    Pincode VARCHAR(10) NOT NULL,
    Phone VARCHAR(15) NULL,
    FaxNo VARCHAR(15) NULL,
    Email VARCHAR(50) NULL
);

-- 2. Executive Applications Table
CREATE TABLE EXE_APP (
    App_no VARCHAR(8) PRIMARY KEY,
    App_Date DATE NOT NULL,
    Name VARCHAR(50) NOT NULL,
    Gender CHAR(1) NOT NULL CHECK (Gender IN ('M', 'F')),
    Marital_Status VARCHAR(15) NOT NULL,
    DOB DATE NOT NULL,
    App_Post VARCHAR(30) NOT NULL,
    Qualification VARCHAR(50) NOT NULL,
    Exp VARCHAR(100) NOT NULL,
    Address VARCHAR(100) NOT NULL,
    City VARCHAR(25) NULL,
    State VARCHAR(25) NOT NULL,
    Country VARCHAR(25) NOT NULL,
    Pincode VARCHAR(10) NOT NULL,
    Phone VARCHAR(15) NOT NULL,
    Email VARCHAR(50) NULL
);

-- 3. Executive Confirmation Table
CREATE TABLE EXE_CONFIRM (
    Exe_Code VARCHAR(8) PRIMARY KEY,
    App_no VARCHAR(8) NOT NULL REFERENCES EXE_APP(App_no),
    Desig VARCHAR(30) NOT NULL,
    Conf_Date DATE NOT NULL,
    Basic DECIMAL(10, 2) NOT NULL,
    DOJ DATE NOT NULL,
    Branch_Code VARCHAR(8) NOT NULL REFERENCES BRANCH(Branch_Code),
    BmgrNo VARCHAR(8) NULL,
    Flag CHAR(1) NOT NULL DEFAULT 'Y' CHECK (Flag IN ('Y', 'N')),
    CONSTRAINT CHK_Conf_After_App CHECK (Conf_Date >= DOJ OR Conf_Date IS NOT NULL)
);

-- 4. Executive Monthly Target Table
CREATE TABLE EXE_TARGET (
    Target_Id INT IDENTITY(1,1) PRIMARY KEY,
    Exe_Code VARCHAR(8) NOT NULL REFERENCES EXE_CONFIRM(Exe_Code),
    Target_Cards INT NOT NULL CHECK (Target_Cards >= 0),
    Extra_Cards INT NOT NULL DEFAULT 0 CHECK (Extra_Cards >= 0),
    Target_Month_Yr VARCHAR(15) NOT NULL,
    Noc INT NOT NULL DEFAULT 0 CHECK (Noc >= 0),
    Noc_Pending INT NOT NULL DEFAULT 0 CHECK (Noc_Pending >= 0),
    Bonus_pts INT NOT NULL DEFAULT 0 CHECK (Bonus_pts >= 0)
);

-- 5. Executive Quarterly Performance Table
CREATE TABLE EXE_PERFORMANCE (
    Perf_Id INT IDENTITY(1,1) PRIMARY KEY,
    Exe_Code VARCHAR(8) NOT NULL REFERENCES EXE_CONFIRM(Exe_Code),
    Total_Points INT NOT NULL DEFAULT 0,
    Pchk_date DATE NOT NULL,
    Rank_Code VARCHAR(10) NULL,
    Rating DECIMAL(5, 2) NULL,
    Next_pchkdate DATE NULL
);

-- 6. Debit Card Applications Table
CREATE TABLE DEBITCARD_APP (
    DCApp_no VARCHAR(8) PRIMARY KEY,
    App_Date DATE NOT NULL,
    Name VARCHAR(50) NOT NULL,
    Gender CHAR(1) NOT NULL CHECK (Gender IN ('M', 'F')),
    Marital_Status VARCHAR(15) NOT NULL,
    DOB DATE NOT NULL,
    App_type VARCHAR(25) NOT NULL,
    Qualification VARCHAR(50) NOT NULL,
    Exe_Code VARCHAR(8) NOT NULL REFERENCES EXE_CONFIRM(Exe_Code),
    Nom_name VARCHAR(50) NULL,
    Relation VARCHAR(25) NULL,
    Deposit DECIMAL(12, 2) NOT NULL CHECK (Deposit >= 5000),
    Address VARCHAR(100) NOT NULL,
    City VARCHAR(25) NULL,
    State VARCHAR(25) NOT NULL,
    Country VARCHAR(25) NOT NULL,
    Pincode VARCHAR(10) NOT NULL,
    Phone VARCHAR(15) NULL,
    Email VARCHAR(50) NULL,
    Card_Name VARCHAR(25) NULL
);

-- 7. Debit Card Master Table
CREATE TABLE DEBIT_CARD (
    DCNO VARCHAR(20) PRIMARY KEY,
    DCApp_no VARCHAR(8) NOT NULL REFERENCES DEBITCARD_APP(DCApp_no),
    Issue_date DATE NOT NULL,
    Pin VARCHAR(20) NOT NULL,
    Balance DECIMAL(12, 2) NOT NULL,
    Bal_up_date DATE NOT NULL,
    Maint_charge DECIMAL(8, 2) NOT NULL DEFAULT 0
);

-- 8. Debit Card Transactions Table
CREATE TABLE DC_TRANS (
    DC_Trans_No INT PRIMARY KEY,
    DCNO VARCHAR(20) NOT NULL REFERENCES DEBIT_CARD(DCNO),
    Draw_date DATE NULL,
    Draw_Amt DECIMAL(12, 2) NOT NULL DEFAULT 0,
    Draw_Type INT NULL CHECK (Draw_Type IN (0, 1, 2, 3)),
    Deposit_date DATE NULL,
    Deposit_Amt DECIMAL(12, 2) NOT NULL DEFAULT 0,
    Deposit_Type INT NULL CHECK (Deposit_Type IN (0, 1, 2)),
    Balance_Amt DECIMAL(12, 2) NOT NULL,
    CONSTRAINT CHK_DC_Balance_Vs_Deposit CHECK (Balance_Amt >= Deposit_Amt)
);

-- 9. Credit Card Applications Table
CREATE TABLE CREDITCARD_APP (
    CCApp_no VARCHAR(8) PRIMARY KEY,
    CApp_Date DATE NOT NULL,
    Name VARCHAR(50) NOT NULL,
    Gender CHAR(1) NOT NULL CHECK (Gender IN ('M', 'F')),
    Marital_Status VARCHAR(15) NOT NULL,
    DOB DATE NOT NULL,
    Card_type VARCHAR(25) NOT NULL,
    Qualification VARCHAR(50) NOT NULL,
    Exe_Code VARCHAR(8) NOT NULL REFERENCES EXE_CONFIRM(Exe_Code),
    Nom_name VARCHAR(50) NULL,
    Relation VARCHAR(25) NULL,
    Address VARCHAR(100) NOT NULL,
    City VARCHAR(25) NULL,
    State VARCHAR(25) NOT NULL,
    Country VARCHAR(25) NOT NULL,
    Pincode VARCHAR(10) NOT NULL,
    Phone VARCHAR(15) NULL,
    Email VARCHAR(50) NULL
);

-- 10. Credit Card Enquiry Table
CREATE TABLE CC_ENQUIRY (
    Enquiry_Id INT IDENTITY(1,1) PRIMARY KEY,
    CCApp_no VARCHAR(8) NOT NULL REFERENCES CREDITCARD_APP(CCApp_no),
    Receive_date DATE NOT NULL,
    Enq_date DATE NULL,
    Enq_Status CHAR(1) NOT NULL DEFAULT 'P' CHECK (Enq_Status IN ('F', 'P')),
    Remarks VARCHAR(150) NULL,
    Enq_comp_date DATE NULL
);

-- 11. Credit Card Master Table
CREATE TABLE CREDIT_CARD (
    CCNO VARCHAR(20) PRIMARY KEY,
    CCApp_no VARCHAR(8) NOT NULL REFERENCES CREDITCARD_APP(CCApp_no),
    Issue_date DATE NOT NULL,
    Credit_Limit DECIMAL(12, 2) NOT NULL,
    Credit_Amt DECIMAL(12, 2) NOT NULL DEFAULT 0,
    Card_Type VARCHAR(20) NOT NULL,
    Credit_date DATE NULL,
    Maint_charge DECIMAL(8, 2) NOT NULL DEFAULT 0,
    YearlyRent DECIMAL(8, 2) NOT NULL DEFAULT 0,
    Rent_Pay_Date DATE NULL,
    CONSTRAINT CHK_Credit_Within_Limit CHECK (Credit_Amt <= Credit_Limit)
);

-- 12. Credit Card Transactions Table
CREATE TABLE CC_TRANS (
    CC_Trans_No INT PRIMARY KEY,
    CCNO VARCHAR(20) NOT NULL REFERENCES CREDIT_CARD(CCNO),
    Draw_date DATE NOT NULL,
    Draw_Amt DECIMAL(12, 2) NOT NULL DEFAULT 0,
    Draw_Type INT NOT NULL CHECK (Draw_Type IN (0, 1, 2)),
    Interest_Amount DECIMAL(10, 2) NOT NULL DEFAULT 0
);

-- 13. Credit Card Bills Table
CREATE TABLE BILLS (
    Bill_No VARCHAR(12) PRIMARY KEY,
    CC_Trans_No INT NOT NULL REFERENCES CC_TRANS(CC_Trans_No),
    CCNO VARCHAR(20) NOT NULL REFERENCES CREDIT_CARD(CCNO),
    Bill_date DATE NOT NULL,
    Due_date DATE NOT NULL,
    Bill_Amt DECIMAL(12, 2) NOT NULL,
    Amt_Paid DECIMAL(12, 2) NOT NULL DEFAULT 0,
    Bill_Status VARCHAR(15) NOT NULL DEFAULT 'NOT CLEARED'
);
GO
