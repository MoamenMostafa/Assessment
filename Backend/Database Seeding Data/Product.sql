-- Insert 1000 Products
DECLARE @counter INT = 1;

WHILE @counter <= 1000
BEGIN
    INSERT INTO Products (Id, Name, Description, Price, CategoryId, Status, StockQuantity, ImageUrl, CreatedDate, UpdatedDate)
    VALUES (
        NEWID(),
        CONCAT('Product_', @counter),
        CONCAT('Description for Product_', @counter),
        ROUND(10 + RAND() * 1000, 2), -- Random price between 10 and 1000
        (SELECT TOP 1 Id FROM Categories ORDER BY NEWID()), -- Random CategoryId
        1, -- Assume 1 = Active
        FLOOR(1 + RAND() * 100), -- Random stock quantity between 1 and 100
        CONCAT('https://example.com/images/product_', @counter, '.jpg'), -- Example image URL
        GETDATE(),
        GETDATE()
    );

    SET @counter = @counter + 1;
END;
