-- Talent Acquisition Strategies - Sample Seed Data & Reporting Queries
USE TalentAcquisitionDB;
GO

-- Seed Branch data (aligned with Branch Details screen example)
INSERT INTO BRANCH (Branch_Code, Address, City, State, Country, Pincode, Phone, FaxNo, Email)
VALUES
    ('BR001', 'Cyber Towers, Madhapur', 'Hyderabad', 'AP', 'India', '500082', '23110003', '23110004', 'branch1@bank.example'),
    ('BR002', 'Banjara Hills Main Road', 'Hyderabad', 'AP', 'India', '500034', '23350011', '23350012', 'branch2@bank.example');

-- Seed Executive Applications (aligned with Application Form screen examples)
INSERT INTO EXE_APP (App_no, App_Date, Name, Gender, Marital_Status, DOB, App_Post, Qualification, Exp, Address, City, State, Country, Pincode, Phone, Email)
VALUES
    ('A001', '2009-12-22', 'Y. Sreedhar Babu', 'M', 'Unmarried', '2003-03-02', 'Marketing Executive', 'M.Sc. Computers', '4 Yrs', '18/323-P Rajendra Nagar, Guntakal', 'Guntakal', 'AP', 'India', '515801', '220175', 'sreemsc@yahoo.com'),
    ('A012', '2009-11-20', 'K. Ramesh', 'M', 'Married', '1986-05-14', 'Marketing Executive', 'MBA', '2 Yrs', 'SR Nagar', 'Hyderabad', 'AP', 'India', '500038', '9849012345', 'ramesh.k@example.com'),
    ('A014', '2010-05-01', 'Himaja', 'F', 'Unmarried', '1997-12-31', 'Marketing Manager', 'PhD', '1 Yr', 'SR Nagar', 'Hyderabad', 'AP', 'India', '500012', '3211233456', 'himaja@yahoo.com');

-- Seed Confirmed Executives (aligned with Employee Confirmation screen example)
INSERT INTO EXE_CONFIRM (Exe_Code, App_no, Desig, Conf_Date, Basic, DOJ, Branch_Code, BmgrNo, Flag)
VALUES
    ('MM001', 'A014', 'Marketing Manager', '2009-04-05', 22000.00, '2009-04-12', 'BR001', NULL, 'Y'),
    ('ME001', 'A012', 'Marketing Executive', '2009-04-12', 14000.00, '2009-11-01', 'BR002', 'MM001', 'Y');

-- Seed Monthly Targets & Bonus Points
INSERT INTO EXE_TARGET (Exe_Code, Target_Cards, Extra_Cards, Target_Month_Yr, Noc, Noc_Pending, Bonus_pts)
VALUES
    ('ME001', 5, 2, 'JAN 2010', 7, 0, 3),
    ('MM001', 10, 0, 'JAN 2010', 8, 2, 0);

-- Seed Quarterly Performance Records
INSERT INTO EXE_PERFORMANCE (Exe_Code, Total_Points, Pchk_date, Rank_Code, Rating, Next_pchkdate)
VALUES
    ('ME001', 15, '2010-03-26', 'R1', 4.50, '2010-06-25');

-- Seed Debit Card Application, Card & Transaction (aligned with Debit Card screens)
INSERT INTO DEBITCARD_APP (DCApp_no, App_Date, Name, Gender, Marital_Status, DOB, App_type, Qualification, Exe_Code, Nom_name, Relation, Deposit, Address, City, State, Country, Pincode, Phone, Email, Card_Name)
VALUES
    ('DC002', '2009-12-30', 'Sree Kala', 'F', 'Married', '1995-12-28', 'SAVINGS', 'B.Com', 'ME001', 'Chandra Kala', 'Daughter', 50000.00, 'Haryana', 'Yanam', 'AP', 'India', '6098787', '87897098', 'sk@yahoo.com', 'GOLD');

INSERT INTO DEBIT_CARD (DCNO, DCApp_no, Issue_date, Pin, Balance, Bal_up_date, Maint_charge)
VALUES
    ('47310002', 'DC002', '2009-12-24', '1234', 50000.00, '2009-12-24', 200.00);

INSERT INTO DC_TRANS (DC_Trans_No, DCNO, Draw_date, Draw_Amt, Draw_Type, Deposit_date, Deposit_Amt, Deposit_Type, Balance_Amt)
VALUES
    (100004, '47310002', '2009-12-26', 6000.00, 1, '2009-12-25', 0.00, 0, 44000.00);

-- Seed Credit Card Application, Enquiry, Card, Transaction & Bill (aligned with Credit Card screens)
INSERT INTO CREDITCARD_APP (CCApp_no, CApp_Date, Name, Gender, Marital_Status, DOB, Card_type, Qualification, Exe_Code, Nom_name, Relation, Address, City, State, Country, Pincode, Phone, Email)
VALUES
    ('CC001', '2009-12-01', 'Vijaykumar', 'F', 'Unmarried', '1990-12-01', 'GOLD', 'SSC', 'MM001', 'Sujai', 'Father', 'KJDKDJ', 'Hyderabad', 'AP', 'India', '827483', '837483748', 'cc@yahoo.com'),
    ('CC002', '2009-12-10', 'Anil Kumar', 'M', 'Married', '1988-07-15', 'CLASSIC', 'B.Tech', 'ME001', 'Sunita', 'Wife', 'Madhapur', 'Hyderabad', 'AP', 'India', '500081', '9848011122', 'anil.k@example.com');

INSERT INTO CC_ENQUIRY (CCApp_no, Receive_date, Enq_date, Enq_Status, Remarks, Enq_comp_date)
VALUES
    ('CC002', '2009-12-12', '2009-12-15', 'F', 'Verified documents and address', '2009-12-20');

INSERT INTO CREDIT_CARD (CCNO, CCApp_no, Issue_date, Credit_Limit, Credit_Amt, Card_Type, Credit_date, Maint_charge, YearlyRent, Rent_Pay_Date)
VALUES
    ('12340002', 'CC002', '2009-12-24', 50000.00, 6000.00, 'CLASSIC', '2009-12-24', 500.00, 100.00, '2010-12-03');

INSERT INTO CC_TRANS (CC_Trans_No, CCNO, Draw_date, Draw_Amt, Draw_Type, Interest_Amount)
VALUES
    (200003, '12340002', '2009-12-30', 5000.00, 1, 125.00);

INSERT INTO BILLS (Bill_No, CC_Trans_No, CCNO, Bill_date, Due_date, Bill_Amt, Amt_Paid, Bill_Status)
VALUES
    ('1002', 200003, '12340002', '2010-01-31', '2010-02-15', 5125.00, 5125.00, 'CLEARED');
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
