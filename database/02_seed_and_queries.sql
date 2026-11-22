-- Talent Acquisition Strategies - Sanitized Sample Seed Data & Reporting Queries
USE TalentAcquisitionDB;
GO

-- Seed Branch data
INSERT INTO BRANCH (Branch_Code, Address, City, State, Country, Pincode, Phone, FaxNo, Email)
VALUES
    ('BR001', '100 Main Street', 'Sample City', 'Sample State', 'Sample Country', '500001', '0000000001', '0000000002', 'branch1@example.com'),
    ('BR002', '200 Market Avenue', 'Sample City', 'Sample State', 'Sample Country', '500002', '0000000003', '0000000004', 'branch2@example.com');

-- Seed Executive Applications
INSERT INTO EXE_APP (App_no, App_Date, Name, Gender, Marital_Status, DOB, App_Post, Qualification, Exp, Address, City, State, Country, Pincode, Phone, Email)
VALUES
    ('A001', '2010-01-10', 'Sample Applicant One', 'M', 'Single', '1986-03-15', 'Marketing Executive', 'MBA', '2 Yrs', '101 Sample Lane', 'Sample City', 'Sample State', 'Sample Country', '500001', '0000000101', 'applicant1@example.com'),
    ('A002', '2010-01-12', 'Sample Applicant Two', 'F', 'Married', '1984-07-20', 'Marketing Manager', 'MBA', '4 Yrs', '102 Sample Lane', 'Sample City', 'Sample State', 'Sample Country', '500002', '0000000102', 'applicant2@example.com'),
    ('A003', '2010-01-15', 'Sample Applicant Three', 'M', 'Single', '1987-11-05', 'Marketing Executive', 'B.Com', '1 Yr', '103 Sample Lane', 'Sample City', 'Sample State', 'Sample Country', '500001', '0000000103', 'applicant3@example.com');

-- Seed Confirmed Executives
INSERT INTO EXE_CONFIRM (Exe_Code, App_no, Desig, Conf_Date, Basic, DOJ, Branch_Code, BmgrNo, Flag)
VALUES
    ('MM001', 'A002', 'Marketing Manager', '2010-02-01', 22000.00, '2010-01-20', 'BR001', NULL, 'Y'),
    ('ME001', 'A001', 'Marketing Executive', '2010-02-05', 14000.00, '2010-01-25', 'BR002', 'MM001', 'Y');

-- Seed Monthly Targets & Bonus Points
INSERT INTO EXE_TARGET (Exe_Code, Target_Cards, Extra_Cards, Target_Month_Yr, Noc, Noc_Pending, Bonus_pts)
VALUES
    ('ME001', 5, 2, 'FEB 2010', 7, 0, 3),
    ('MM001', 10, 0, 'FEB 2010', 8, 2, 0);

-- Seed Quarterly Performance Records
INSERT INTO EXE_PERFORMANCE (Exe_Code, Total_Points, Pchk_date, Rank_Code, Rating, Next_pchkdate)
VALUES
    ('ME001', 15, '2010-03-26', 'R1', 4.50, '2010-06-25');

-- Seed Debit Card Application, Card & Transaction
INSERT INTO DEBITCARD_APP (DCApp_no, App_Date, Name, Gender, Marital_Status, DOB, App_type, Qualification, Exe_Code, Nom_name, Relation, Deposit, Address, City, State, Country, Pincode, Phone, Email, Card_Name)
VALUES
    ('DC001', '2010-02-10', 'Sample Customer One', 'F', 'Married', '1985-05-10', 'SAVINGS', 'B.Com', 'ME001', 'Sample Nominee One', 'Spouse', 50000.00, '201 Sample Road', 'Sample City', 'Sample State', 'Sample Country', '500001', '0000000201', 'customer1@example.com', 'GOLD');

INSERT INTO DEBIT_CARD (DCNO, DCApp_no, Issue_date, Pin, Balance, Bal_up_date, Maint_charge)
VALUES
    ('4000000000000001', 'DC001', '2010-02-12', '0000', 50000.00, '2010-02-12', 200.00);

INSERT INTO DC_TRANS (DC_Trans_No, DCNO, Draw_date, Draw_Amt, Draw_Type, Deposit_date, Deposit_Amt, Deposit_Type, Balance_Amt)
VALUES
    (100001, '4000000000000001', '2010-02-15', 6000.00, 1, '2010-02-14', 0.00, 0, 44000.00);

-- Seed Credit Card Application, Enquiry, Card, Transaction & Bill
INSERT INTO CREDITCARD_APP (CCApp_no, CApp_Date, Name, Gender, Marital_Status, DOB, Card_type, Qualification, Exe_Code, Nom_name, Relation, Address, City, State, Country, Pincode, Phone, Email)
VALUES
    ('CC001', '2010-02-11', 'Sample Customer Two', 'M', 'Single', '1988-09-18', 'CLASSIC', 'B.Tech', 'ME001', 'Sample Nominee Two', 'Parent', '301 Sample Street', 'Sample City', 'Sample State', 'Sample Country', '500002', '0000000301', 'customer2@example.com');

INSERT INTO CC_ENQUIRY (CCApp_no, Receive_date, Enq_date, Enq_Status, Remarks, Enq_comp_date)
VALUES
    ('CC001', '2010-02-12', '2010-02-14', 'F', 'Sample verification completed', '2010-02-16');

INSERT INTO CREDIT_CARD (CCNO, CCApp_no, Issue_date, Credit_Limit, Credit_Amt, Card_Type, Credit_date, Maint_charge, YearlyRent, Rent_Pay_Date)
VALUES
    ('5000000000000001', 'CC001', '2010-02-18', 50000.00, 6000.00, 'CLASSIC', '2010-02-18', 500.00, 100.00, '2011-02-18');

INSERT INTO CC_TRANS (CC_Trans_No, CCNO, Draw_date, Draw_Amt, Draw_Type, Interest_Amount)
VALUES
    (200001, '5000000000000001', '2010-02-20', 5000.00, 1, 125.00);

INSERT INTO BILLS (Bill_No, CC_Trans_No, CCNO, Bill_date, Due_date, Bill_Amt, Amt_Paid, Bill_Status)
VALUES
    ('B1001', 200001, '5000000000000001', '2010-03-01', '2010-03-15', 5125.00, 5125.00, 'CLEARED');
GO

-- Reporting Queries for Management & Decision Support
-- 1. Monthly Executive Target & Bonus Summary
SELECT
    c.Exe_Code,
    a.Name AS Executive_Name,
    c.Desig,
    c.Branch_Code,
    t.Target_Month_Yr,
    t.Target_Cards,
    t.Noc AS Cards_Sold,
    t.Extra_Cards,
    t.Noc_Pending,
    t.Bonus_pts
FROM EXE_TARGET t
INNER JOIN EXE_CONFIRM c ON t.Exe_Code = c.Exe_Code
INNER JOIN EXE_APP a ON c.App_no = a.App_no
ORDER BY t.Target_Month_Yr, t.Bonus_pts DESC;

-- 2. Branch-wise Debit & Credit Card Enrollment Count
SELECT
    b.Branch_Code,
    b.City,
    COUNT(DISTINCT dc.DCApp_no) AS Debit_Card_Apps,
    COUNT(DISTINCT cc.CCApp_no) AS Credit_Card_Apps
FROM BRANCH b
LEFT JOIN EXE_CONFIRM ec ON b.Branch_Code = ec.Branch_Code
LEFT JOIN DEBITCARD_APP dc ON ec.Exe_Code = dc.Exe_Code
LEFT JOIN CREDITCARD_APP cc ON ec.Exe_Code = cc.Exe_Code
GROUP BY b.Branch_Code, b.City;
GO
