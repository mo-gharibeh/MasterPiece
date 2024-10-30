CREATE DATABASE MotoStation;



USE MotoStation;


-- جدول المستخدمين
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    UserName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
	Password NVARCHAR(256) NOT NULL,
    PasswordHash varbinary(MAX) NULL,
	FullName NVARCHAR(200),
    ProfileImageURL NVARCHAR(max) NULL,
	CoverImageURL NVARCHAR(max),
    Role NVARCHAR(50) NOT NULL DEFAULT 'User',-- 'User', 'Manager', 'SuperAdmin'
    CreatedDate DATETIME DEFAULT GETDATE(),
    LastLoginDate DATETIME NULL
);

ALTER TABLE Users
ADD PasswordSalt VARBINARY(MAX) NULL;

ALTER TABLE Users
ADD PhoneNumber NVARCHAR(20)NULL ;

ALTER TABLE Users
ADD Location NVARCHAR(50) NULL;

-- جدول المحلات التجارية
CREATE TABLE Stores (
    StoreID INT PRIMARY KEY IDENTITY(1,1),
    StoreName NVARCHAR(100) NOT NULL,
    Address NVARCHAR(255) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    Email NVARCHAR(100) NULL,
    WorkingHours NVARCHAR(50) NULL,
    StoreImageURL NVARCHAR(MAX) NULL,
	Location NVARCHAR(255), -- يمكن أن يكون إحداثيات أو عنوان
    CreatedDate DATETIME DEFAULT GETDATE(),
	ManagerID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID)
);

--   جدول التقييمات 11111   
CREATE TABLE StoreRatings (
    RatingID INT PRIMARY KEY IDENTITY(1,1),
    StoreID INT FOREIGN KEY REFERENCES Stores(StoreID),
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    RatingValue INT NOT NULL CHECK (RatingValue BETWEEN 1 AND 5), -- القيم من 1 إلى 5
    ReviewText NVARCHAR(MAX) NULL, -- تعليق المستخدم
    CreatedDate DATETIME DEFAULT GETDATE()
);


-- جدول الفئات 
CREATE TABLE [MotoStation].[dbo].[Categories] (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    ImageURL NVARCHAR(255),
    IsActive BIT DEFAULT 1,
	);

--جدول المنتجات
CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    CategoryID INT NOT NULL FOREIGN KEY REFERENCES Categories(CategoryID),
    ProductName NVARCHAR(150) NOT NULL,
    Description NVARCHAR(500),
    SellerID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID), -- User selling the product
    ProductType NVARCHAR(10) NOT NULL,  -- "Sale" or "Rent"
    Price DECIMAL(18, 2),
    RentalPrice DECIMAL(18, 2),
    RentalDuration INT, -- Max rental period in days or weeks
    IsCurrentlyRented BIT DEFAULT 0,
    StockQuantity INT NOT NULL,
    ImageURL NVARCHAR(255),
    Brand NVARCHAR(100),
	ProductCondition NVARCHAR(50) NOT NULL,  -- e.g., "New", "Used"
	IsSold BIT DEFAULT 0,
    IsAvailable BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME NULL
);






-- 22222 جدول الدراجات
CREATE TABLE Motorcycles (
    MotorcycleID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
	MotorcycleType NVARCHAR(50) NOT NULL DEFAULT 'Motorcycle',
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    IsForSale BIT NOT NULL,
    IsForRent BIT NOT NULL,
    Location NVARCHAR(255) NOT NULL,
    MotorcycleImageURL NVARCHAR(255) NULL,
    CreatedDate DATETIME DEFAULT GETDATE()
);

-- جدول الفعاليات
--CREATE TABLE Events (
  --  EventID INT PRIMARY KEY IDENTITY(1,1),
 --   UserID INT FOREIGN KEY REFERENCES Users(UserID),
  --  Title NVARCHAR(100) NOT NULL,
   -- Description NVARCHAR(MAX) NOT NULL,
   -- Location NVARCHAR(255) NOT NULL,
   -- EventDate DATETIME NOT NULL,
 --   CreatedDate DATETIME DEFAULT GETDATE()
--);

DROP TABLE Events;

CREATE TABLE Events (
    EventID INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    Location NVARCHAR(200),
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    OrganizerID INT,
    EventType NVARCHAR(50),
    Capacity INT,
    RegistrationFee DECIMAL(10, 2) DEFAULT 0,
    Status NVARCHAR(50) DEFAULT 'Upcoming',
    CreatedDate DATETIME DEFAULT GETDATE(),
    LastUpdated DATETIME DEFAULT GETDATE(),
    CoverImageURL NVARCHAR(255),
    Tags NVARCHAR(255),
    FOREIGN KEY (OrganizerID) REFERENCES Users(UserID) -- Assuming an Organizers table
);


-- جدول المسارات
CREATE TABLE Routes (
    RouteID INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    StartLocation NVARCHAR(255),
    EndLocation NVARCHAR(255),
    RestStops NVARCHAR(MAX), -- يمكن استخدام JSON أو تنسيق آخر
    CreatedDate DATETIME DEFAULT GETDATE()
);

-- جدول المنشورات في المجتمع
CREATE TABLE Posts (
    PostID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    Content NVARCHAR(MAX) NOT NULL,
    ImageURL NVARCHAR(255) NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
    LikesCount INT DEFAULT 0,
    CommentsCount INT DEFAULT 0
);

-- جدول التعليقات في المجتمع
CREATE TABLE Comments (
    CommentID INT PRIMARY KEY IDENTITY(1,1),
    PostID INT FOREIGN KEY REFERENCES Posts(PostID),
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    CommentText NVARCHAR(MAX) NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE()
);

-- جدول الإعجابات في المجتمع
CREATE TABLE Likes (
    LikeID INT PRIMARY KEY IDENTITY(1,1),
    PostID INT FOREIGN KEY REFERENCES Posts(PostID),
    UserID INT FOREIGN KEY REFERENCES Users(UserID)
);

-- 3333 جدول المتابعين في المجتمع
CREATE TABLE Followers (
    FollowerID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    FollowedUserID INT FOREIGN KEY REFERENCES Users(UserID)
);

-- 4444 جدول المفضلات
CREATE TABLE Favorites (
    FavoriteID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    ProductID INT NULL FOREIGN KEY REFERENCES Products(ProductID),
    StoreID INT NULL FOREIGN KEY REFERENCES Stores(StoreID),
    EventID INT NULL FOREIGN KEY REFERENCES Events(EventID)
);


-- 555 جدول التنبيهات
CREATE TABLE Notifications (
    NotificationID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    NotificationText NVARCHAR(MAX) NOT NULL,
    IsRead BIT DEFAULT 0,
    CreatedDate DATETIME DEFAULT GETDATE()
);

-- جدول المعاملات
CREATE TABLE Transactions (
    TransactionID INT PRIMARY KEY IDENTITY(1,1),
    SellerID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID), -- البائع
    BuyerID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID), -- المشتري
    ProductID INT NULL FOREIGN KEY REFERENCES Products(ProductID),
    TransactionType NVARCHAR(50) NOT NULL, -- Sale or Rent
    Amount DECIMAL(18, 2) NOT NULL,
    TransactionDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE Payments (--Store Or Event
    PaymentID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    Amount DECIMAL(18, 2) NOT NULL, -- المبلغ المدفوع
    PaymentDate DATETIME DEFAULT GETDATE(), -- تاريخ الدفع
    PaymentMethod NVARCHAR(50) NOT NULL, -- طريقة الدفع (بطاقة، تحويل بنكي، إلخ)
    PaymentStatus NVARCHAR(50) NOT NULL DEFAULT 'Completed' -- حالة الدفع (مكتمل، ملغي، إلخ)
);


CREATE TABLE Subscriptions (
    SubscriptionID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    SubscriptionType NVARCHAR(50) NOT NULL, -- نوع الاشتراك (شهري، سنوي، إلخ)
    Price DECIMAL(18, 2) NOT NULL, -- قيمة الاشتراك
    StartDate DATETIME NOT NULL, -- تاريخ بدء الاشتراك
    EndDate DATETIME NOT NULL, -- تاريخ انتهاء الاشتراك
    IsActive BIT NOT NULL DEFAULT 1 -- حالة الاشتراك (نشط أو غير نشط)
);

-- جدول الرسائل في صفحة "تواصل معنا"
CREATE TABLE ContactMessages (
    MessageID INT PRIMARY KEY IDENTITY(1,1),
	Name NVARCHAR(100) NOT NULL,
	Email NVARCHAR(150) NOT NULL,
	Subject NVARCHAR(100) NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    IsApproved BIT DEFAULT 0, -- يجب الموافقة عليها من قبل المدير
    CreatedDate DATETIME DEFAULT GETDATE()
);



-- ادخال البيانات 
INSERT INTO [MotoStation].[dbo].[Categories] (CategoryName, Description, ImageURL, IsActive)
VALUES 
('Motorcycle', 'All types of motorcycles', 'https://www.honda2wheelersindia.com/assets/images/products/Motorcycle/CB200__dev23.jpg', 1),
('Helmet', 'Helmets for rider safety', 'https://m.media-amazon.com/images/I/61xv8k5p1gL._AC_UF1000,1000_QL80_.jpg', 1),
('Protection', 'Protective gear and armor', 'https://m.media-amazon.com/images/I/61G0KfBVEoL._AC_SX679_.jpg', 1),
('Accessories', 'Motorcycle accessories and parts', 'https://images-cdn.ubuy.co.in/635a5543abac6f2be33f9b67-kithelp-bike-accessories-set-for-adult.jpg', 1);

-- Inserting 4 products for the 'Motorcycle' category (CategoryID = 1)
INSERT INTO Products (CategoryID, ProductName, Description, SellerID, ProductType, Price, RentalPrice, RentalDuration, StockQuantity, ImageURL, Brand, ProductCondition)
VALUES
(1, 'Honda CB200X', 'Honda CB200X sports motorcycle', 6, 'Sale', 1500.00, NULL, NULL, 5, 'https://www.honda2wheelersindia.com/assets/images/products/Motorcycle/CB200__dev23.jpg', 'Honda', 'New'),
(1, 'Yamaha R3', 'Yamaha R3 sports motorcycle', 6, 'Rent', NULL, 100.00, 7, 2, 'https://example.com/yamaha-r3.jpg', 'Yamaha', 'Used'),
(1, 'Suzuki Gixxer', 'Suzuki Gixxer standard motorcycle', 6, 'Sale', 1200.00, NULL, NULL, 3, 'https://example.com/suzuki-gixxer.jpg', 'Suzuki', 'New'),
(1, 'Kawasaki Ninja', 'Kawasaki Ninja 400 sports motorcycle', 6, 'Rent', NULL, 150.00, 10, 4, 'https://example.com/kawasaki-ninja.jpg', 'Kawasaki', 'Used');

-- Inserting 4 products for the 'Helmet' category (CategoryID = 2)
INSERT INTO Products (CategoryID, ProductName, Description, SellerID, ProductType, Price, RentalPrice, RentalDuration, StockQuantity, ImageURL, Brand, ProductCondition)
VALUES
(2, 'HJC CL-17 Full Face', 'HJC full face helmet', 6, 'Sale', 100.00, NULL, NULL, 10, 'https://m.media-amazon.com/images/I/61xv8k5p1gL._AC_UF1000,1000_QL80_.jpg', 'HJC', 'New'),
(2, 'Shoei RF-1200', 'Shoei premium helmet', 6, 'Sale', 300.00, NULL, NULL, 5, 'https://example.com/shoei-rf1200.jpg', 'Shoei', 'New'),
(2, 'Bell Qualifier', 'Bell budget full face helmet', 6, 'Sale', 150.00, NULL, NULL, 8, 'https://example.com/bell-qualifier.jpg', 'Bell', 'New'),
(2, 'Arai Corsair X', 'Arai high-end helmet', 6, 'Sale', 400.00, NULL, NULL, 3, 'https://example.com/arai-corsair.jpg', 'Arai', 'New');

-- Inserting 4 products for the 'Protection' category (CategoryID = 3)
INSERT INTO Products (CategoryID, ProductName, Description, SellerID, ProductType, Price, RentalPrice, RentalDuration, StockQuantity, ImageURL, Brand, ProductCondition)
VALUES
(3, 'Alpinestars Jacket', 'Alpinestars motorcycle jacket with armor', 6, 'Sale', 200.00, NULL, NULL, 7, 'https://m.media-amazon.com/images/I/61G0KfBVEoL._AC_SX679_.jpg', 'Alpinestars', 'New'),
(3, 'Dainese Gloves', 'Dainese protective gloves', 6, 'Sale', 50.00, NULL, NULL, 15, 'https://example.com/dainese-gloves.jpg', 'Dainese', 'New'),
(3, 'Fox Racing Boots', 'Fox Racing motorcycle boots', 6, 'Sale', 180.00, NULL, NULL, 6, 'https://example.com/fox-racing-boots.jpg', 'Fox Racing', 'New'),
(3, 'Klim Pants', 'Klim protective pants', 6, 'Sale', 120.00, NULL, NULL, 4, 'https://example.com/klim-pants.jpg', 'Klim', 'New');

-- Inserting 4 products for the 'Accessories' category (CategoryID = 4)
INSERT INTO Products (CategoryID, ProductName, Description, SellerID, ProductType, Price, RentalPrice, RentalDuration, StockQuantity, ImageURL, Brand, ProductCondition)
VALUES
(4, 'Motorcycle Cover', 'Protective cover for motorcycles', 6, 'Sale', 30.00, NULL, NULL, 20, 'https://images-cdn.ubuy.co.in/635a5543abac6f2be33f9b67-kithelp-bike-accessories-set-for-adult.jpg', 'Generic', 'New'),
(4, 'Phone Mount', 'Universal phone mount for motorcycles', 6, 'Sale', 15.00, NULL, NULL, 25, 'https://example.com/phone-mount.jpg', 'Generic', 'New'),
(4, 'Saddlebags', 'Waterproof saddlebags for motorcycles', 6, 'Sale', 80.00, NULL, NULL, 10, 'https://example.com/saddlebags.jpg', 'Generic', 'New'),
(4, 'Motorcycle Stand', 'Adjustable motorcycle stand', 6, 'Sale', 60.00, NULL, NULL, 12, 'https://example.com/motorcycle-stand.jpg', 'Generic', 'New');


INSERT INTO Events (Title, Description, Location, StartDate, EndDate, EventType, Capacity, RegistrationFee, Status, CoverImageURL, Tags)
VALUES 
('Off-Road Adventure', 
 'Join us for an exhilarating off-road adventure through the rugged terrains of Irbid, perfect for motorcycle enthusiasts!', 
 'Irbid', 
 '2024-11-10 09:00:00', 
 '2024-11-10 17:00:00', 
 'Adventure', 
 100, 
 50.00, 
 'Upcoming', 
 'https://example.com/images/off-road-adventure.jpg', 
 'Adventure,Off-road,Motorcycle'),

('Irbid Bike Rally', 
 'Experience the thrill of the annual Irbid Bike Rally with live music, food trucks, and group rides through Irbid’s best roads.', 
 'Irbid', 
 '2024-12-05 12:00:00', 
 '2024-12-05 20:00:00', 
 'Rally', 
 300, 
 20.00, 
 'Upcoming', 
 'https://example.com/images/irbid-bike-rally.jpg', 
 'Rally,Motorcycle,Community'),

('Vintage Motorcycle Show', 
 'A showcase of vintage motorcycles from the 70s, 80s, and 90s, with awards for the best-preserved classics.', 
 'Irbid', 
 '2025-01-15 10:00:00', 
 '2025-01-15 18:00:00', 
 'Show', 
 150, 
 15.00, 
 'Upcoming', 
 'https://example.com/images/vintage-motorcycle-show.jpg', 
 'Vintage,Motorcycle,Show'),

('Night Ride Challenge', 
 'A night-time challenge across Irbid’s scenic routes for experienced riders looking for an unforgettable ride.', 
 'Irbid', 
 '2024-11-25 20:00:00', 
 '2024-11-26 02:00:00', 
 'Challenge', 
 50, 
 30.00, 
 'Upcoming', 
 'https://example.com/images/night-ride-challenge.jpg', 
 'Night,Challenge,Motorcycle'),

('Motorcycle Safety Workshop', 
 'A free workshop focused on motorcycle safety practices, ideal for new and experienced riders alike.', 
 'Irbid', 
 '2024-11-18 10:00:00', 
 '2024-11-18 14:00:00', 
 'Workshop', 
 200, 
 0.00, 
 'Upcoming', 
 'https://example.com/images/safety-workshop.jpg', 
 'Safety,Workshop,Motorcycle');
