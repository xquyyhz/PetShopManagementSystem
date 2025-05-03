-- Kiểm tra nếu cơ sở dữ liệu chưa tồn tại thì tạo mới
IF DB_ID('PetShop') IS NULL
BEGIN
    CREATE DATABASE PetShop;
END;

-- Sử dụng cơ sở dữ liệu PetShop
USE PetShop;

-- Tạo bảng User và thêm ràng buộc khóa duy nhất cho trường 'name'
CREATE TABLE dbo.tbUser (
    id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    name VARCHAR(50) NOT NULL,
    address VARCHAR(50) NOT NULL,
    phone VARCHAR(50) NOT NULL,
    role VARCHAR(50) NOT NULL,
    dob DATE NOT NULL,
    password VARCHAR(50),
    CONSTRAINT UQ_tbUser_name UNIQUE (name) -- Khóa duy nhất cho trường 'name'
);

INSERT INTO [dbo].[tbUser] ([name], [address],[phone],[role],[dob],[password]) 
VALUES 
('john', '123 King Street, District 1', '0901123456', 'Administrator', '1985-01-10', 'admin'),
('emily', '45 Queen Avenue, District 2', '0912234567', 'Cashier', '1990-06-15', 'cashier'),
('jessica', '101 Maple Road, District 4', '0934456789', 'Administrator', '1988-11-30', 'admin'),
('daniel', '222 Oak Street, District 5', '0945567890', 'Cashier', '1994-09-05', 'cashier'),
('sarah', '333 Pine Avenue, District 6', '0956678901', 'Employee', '1995-12-12', 'employee'),
('chris', '444 Cedar Blvd, District 7', '0967789012', 'Administrator', '1987-08-08', 'admin'),
('ashley', '555 Birch Lane, District 8', '0978890123', 'Cashier', '1993-04-22', 'cashier'),
('matt', '666 Spruce Court, District 9', '0989901234', 'Employee', '1991-05-18', 'employee'),
('olivia', '777 Walnut Street, District 10', '0991012345', 'Cashier', '1996-10-27', 'cashier'),
('adam', '888 Willow Way, District 11', '0911123456', 'Administrator', '1993-07-09', 'admin');
GO

-- Tạo bảng Product
CREATE TABLE dbo.tbProduct (
    pcode INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    pname VARCHAR(50) NOT NULL,
    ptype VARCHAR(50) NOT NULL,
    pcategory VARCHAR(50) NOT NULL,
    pqty INT NOT NULL,
    pprice DECIMAL(18, 2) NOT NULL
);

INSERT INTO [dbo].[tbProduct] ([pname], [ptype], [pcategory], [pqty], [pprice])
VALUES
('Golden Retriever', 'Large Breed', 'Dog', 50, 1200.00),
('Persian Cat', 'Long Hair', 'Cat', 30, 850.00),
('Parrot', 'Tropical', 'Bird', 40, 300.00),
('Betta Fish', 'Freshwater', 'Fish', 20, 25.00),
('Dog Food Premium', 'Canine', 'Food', 50, 45.00),
('Siamese Cat', 'Short Hair', 'Cat', 30, 600.00),
('Macaw', 'Exotic', 'Bird', 40, 1500.00),
('Goldfish', 'Common', 'Fish', 20, 10.00),
('Cat Food Deluxe', 'Feline', 'Food', 50, 40.00),
('Labrador Retriever', 'Medium Breed', 'Dog', 50, 1000.00);
GO

-- Tạo bảng Customer
CREATE TABLE dbo.tbCustomer (
    id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    name VARCHAR(30) NULL,
    address VARCHAR(100) NULL,
    phone VARCHAR(20) NULL
);

INSERT INTO [dbo].[tbCustomer] ([name], [address], [phone])
VALUES
('Liam Carter', '12 Sunset Blvd, District 1', '0901987654'),
('Ava Collins', '89 River Road, District 2', '0912876543'),
('Noah Bennett', '34 Garden Street, District 3', '0923765432'),
('Isabella Foster', '56 Ocean Avenue, District 4', '0934654321'),
('Ethan Hayes', '78 Valley Lane, District 5', '0945543210'),
('Mia Simmons', '90 Forest Drive, District 6', '0956432109'),
('Lucas Perry', '102 Mountain Road, District 7', '0967321098'),
('Harper Brooks', '11 Highland Ave, District 8', '0978210987'),
('James Reed', '67 Maple Street, District 9', '0989109876'),
('Chloe Jenkins', '24 Willow Way, District 10', '0991098765');
GO

-- Tạo bảng Cash
CREATE TABLE dbo.tbCash (
    cashid INT IDENTITY(1,1) PRIMARY KEY NOT NULL ,
    transno VARCHAR(15) NULL,
    pcode INT NOT NULL,
    pname VARCHAR(50) NULL,
    qty INT NULL,
    price DECIMAL(18, 2) NOT NULL,
    total DECIMAL(18, 2) NULL,
    cid INT NULL,
    cashier VARCHAR(50) NOT NULL,
    CONSTRAINT FK_tbCash_tbProduct FOREIGN KEY (pcode) REFERENCES dbo.tbProduct(pcode),
    CONSTRAINT FK_tbCash_tbCustomer FOREIGN KEY (cid) REFERENCES dbo.tbCustomer(id),
    CONSTRAINT FK_tbCash_tbUser FOREIGN KEY (cashier) REFERENCES dbo.tbUser(name)
);


-- Tạo bảng Invoice và thêm ràng buộc khóa duy nhất cho trường 'transno'
CREATE TABLE dbo.tbInvoice (
    invoiceid INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    transno VARCHAR(15) NOT NULL,
    customerid INT NULL,
    date DATETIME NULL,
    total DECIMAL(18, 2) NOT NULL,
    cashiername VARCHAR(50) NOT NULL,
    CONSTRAINT UQ_tbInvoice_transno UNIQUE (transno),  -- Khóa duy nhất cho trường 'transno'
    CONSTRAINT FK_tbInvoice_tbCustomer FOREIGN KEY (customerid) REFERENCES dbo.tbCustomer(id),
    CONSTRAINT FK_tbInvoice_tbUser FOREIGN KEY (cashiername) REFERENCES dbo.tbUser(name)
);

-- Tạo bảng InvoiceDetails
CREATE TABLE dbo.tbInvoiceDetails (
    id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    transno VARCHAR(15) NOT NULL,
    pcode INT NOT NULL,
    pname VARCHAR(50) NOT NULL,
    price DECIMAL(18, 2) NOT NULL,
    qty INT NOT NULL,
    total DECIMAL(18, 2) NOT NULL,
    CONSTRAINT FK_tbInvoiceDetails_tbInvoice FOREIGN KEY (transno) REFERENCES dbo.tbInvoice(transno),
    CONSTRAINT FK_tbInvoiceDetails_tbProduct FOREIGN KEY (pcode) REFERENCES dbo.tbProduct(pcode)
);


-- Tạo Trigger để tính lại giá trị Total trong bảng tbCash
CREATE TRIGGER trg_UpdateTotal
ON [dbo].[tbCash]
FOR UPDATE
AS
BEGIN
    SET NOCOUNT ON;
   
    -- Cập nhật lại giá trị Total mỗi khi có sự thay đổi về qty hoặc price
    UPDATE tbCash
    SET total = inserted.qty * inserted.price
    FROM tbCash
    INNER JOIN inserted ON tbCash.cashid = inserted.cashid
END;
