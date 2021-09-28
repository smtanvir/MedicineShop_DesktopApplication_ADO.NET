USE master
GO

IF EXISTS (SELECT NAME FROM SYS.sysdatabases WHERE NAME='MS_Medicine_Corner')
DROP DATABASE MS_Medicine_Corner 
GO

CREATE DATABASE MS_Medicine_Corner
GO

USE MS_Medicine_Corner
GO

CREATE TABLE tbl_gender
(
	genderID INT PRIMARY KEY IDENTITY (1,1),
	gender NVARCHAR(6) NOT NULL
)
GO

--CREATE TABLE tbl_department
--(
--	departmentID INT PRIMARY KEY IDENTITY,
--	department NVARCHAR(20) NOT NULL
--)
--GO

CREATE TABLE tbl_designation
(
	designationID INT PRIMARY KEY IDENTITY (101,1),
	designation NVARCHAR(30) NOT NULL,
	department NVARCHAR(30) NOT NULL
)
GO 

CREATE TABLE tbl_employees
(
	employeeID INT PRIMARY KEY IDENTITY (11001,1),
	firstName NVARCHAR(30) NOT NULL,
	lastName NVARCHAR (30) NOT NULL,
	dateOfBirth DATE NOT NULL,
	genderID INT REFERENCES tbl_gender (genderID) NOT NULL,
	nationalID CHAR(13) UNIQUE NOT NULL,
	email NVARCHAR(50) NOT NULL,
	contactNo CHAR (11) UNIQUE NOT NULL,
	streetAddress NVARCHAR(70) NOT NULL,
	postalCode CHAR(4) NOT NULL,
	city NVARCHAR(30) NOT NULL,
	designationID INT REFERENCES tbl_designation(designationID) NOT NULL,
	empImage IMAGE
)
GO

CREATE TABLE tbl_customers
(
	customerID INT PRIMARY KEY IDENTITY (1,1),
	firstName NVARCHAR(30) NOT NULL,
	lastName NVARCHAR(30),
	genderID INT REFERENCES tbl_gender (genderID) NOT NULL,
	contactNo CHAR (11) UNIQUE NOT NULL,
	email NVARCHAR(50) NOT NULL,
	streetAddress NVARCHAR(70) NOT NULL,
	postalCode CHAR(4) NOT NULL,
	city NVARCHAR(30) NOT NULL,
)
GO 

CREATE TABLE tbl_genericGroup
(
	genericGroupCode INT PRIMARY KEY IDENTITY(501,1),
	genericGroup NVARCHAR(40) NOT NULL
)
GO

CREATE TABLE tbl_drugShelf
(
	shelfID INT PRIMARY KEY IDENTITY(1,1),
	shelfNumber CHAR (5) UNIQUE NOT NULL CHECK (shelfNumber LIKE '[A-Z][-][0-9][0-9][0-9]'),
)
GO

CREATE TABLE tbl_suppliers
(
	suppliersID INT PRIMARY KEY IDENTITY (5001,1),
	companyName NVARCHAR (50) NOT NULL,
	officeLocation NVARCHAR(30),
	contactPersonName NVARCHAR(30) NOT NULL,
	contactNo NVARCHAR(20) NOT NULL,
	email NVARCHAR(50) NOT NULL,
	streetAddress NVARCHAR(70),
	postalCode NVARCHAR(4) NOT NULL
)
GO



CREATE TABLE tbl_Medicine
(
	medicineID INT PRIMARY KEY IDENTITY (1001,1),
	medicineName NVARCHAR(30) NOT NULL,
	genericGroupID INT NOT NULL REFERENCES tbl_genericGroup(genericGroupCode),
	supplierCompanyID INT NOT NULL REFERENCES tbl_suppliers(suppliersID),
	packSize INT NOT NULL,
	sizeUnit NVARCHAR(20) NOT NULL,
	shelfID INT NOT NULL REFERENCES tbl_drugShelf(shelfID),
	unitPrice MONEY NOT NULL CHECK (unitPrice>0),
	discountRate FLOAT NOT NULL,
	productImg Image
)
GO

CREATE TABLE tbl_stock
(
	stockID	INT PRIMARY KEY IDENTITY(50001,1),
	medicineID INT REFERENCES tbl_Medicine(medicineID) UNIQUE,
	quantity INT CHECK(quantity>0),
	quantityUnit NVARCHAR(20)
)
GO

CREATE TABLE tbl_sales
(
	salesID INT IDENTITY(100001,1) PRIMARY KEY,
	cusotmersID INT REFERENCES tbl_customers(customerID),
	medicineID INT REFERENCES tbl_Medicine(medicineID),
	employeeID INT REFERENCES tbl_employees(employeeID),
	salesDate DATETIME DEFAULT GETDATE(),
	quantity INT NOT NULL CHECK(quantity>0),
	price MONEY,
	discountRate FLOAT,
	amount AS quantity*price,
	discount AS price * discountRate*quantity,
	netPrice AS (price*quantity)-(price * discountRate*quantity)
	)
GO

CREATE TABLE tbl_user
(
	userID INT PRIMARY KEY IDENTITY(1,1),
	firstName NVARCHAR(30) NOT NULL,
	lastName NVARCHAR(30) NULL,
	contactNo NVARCHAR(15) NOT NULL,
	email NVARCHAR(50) NOT NULL,
	userName NVARCHAR(20) NOT NULL,
	userPassword CHAR(10) NOT NULL
)
GO


CREATE PROC sp_InsertMedicineWithStock
							@medicineName NVARCHAR(30),
							@genericGroupID INT,
							@supplierCompanyID INT,
							@packSize INT,
							@sizeUnit NVARCHAR(20),
							@shelfID INT,
							@unitPrice MONEY,
							@discountRate FLOAT,
							@image Image,
							@initialStock INT,
							@quantityUnit NVARCHAR(20)
							

AS
BEGIN
	INSERT INTO tbl_Medicine VALUES
		(@medicineName,@genericGroupID,@supplierCompanyID,@packSize,@sizeUnit,@shelfID,@unitPrice,@discountRate,@image)
	DECLARE @ID INT;
	SET @ID=@@IDENTITY
	INSERT INTO tbl_stock VALUES(@@IDENTITY,@initialStock,@quantityUnit)
END
GO

CREATE PROC sp_AddMedicineToSales
							@customerID INT,
							@medicineID INT,
							@quantity INT,
							@employeeID INT

AS
BEGIN
	DECLARE @price MONEY;
	DECLARE @discountRate FLOAT;
	SELECT @price=unitPrice FROM tbl_Medicine WHERE medicineID=@medicineID
	SELECT @discountRate=discountRate FROM tbl_Medicine WHERE medicineID=@medicineID
	INSERT INTO tbl_sales(cusotmersID,medicineID,quantity,employeeID,price,discountRate) VALUES
			(@customerID,@medicineID,@quantity,@employeeID,@price,@discountRate)
END
GO

CREATE TRIGGER tr_AddMedicineToSales
    ON tbl_sales
    AFTER INSERT
	AS
    BEGIN
		DECLARE @quantity INT;
		DECLARE @medicineID INT;
		SELECT @medicineID=medicineID FROM inserted
		SELECT @quantity=quantity FROM inserted
		UPDATE tbl_stock
		SET quantity=quantity-@quantity
		WHERE medicineID=@medicineID
	 END
GO