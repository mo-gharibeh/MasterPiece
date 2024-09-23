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
CREATE TABLE Events (
    EventID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Location NVARCHAR(255) NOT NULL,
    EventDate DATETIME NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE()
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
