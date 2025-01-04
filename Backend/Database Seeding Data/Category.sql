-- Insert 1000 Categories
DECLARE @counter INT = 1;
DECLARE @parentCategoryId UNIQUEIDENTIFIER;

WHILE @counter <= 1000
BEGIN
    -- Randomly assign ParentCategoryId for some records
    IF @counter % 5 = 0
        SET @parentCategoryId = (SELECT TOP 1 Id FROM Categories ORDER BY NEWID());
    ELSE
        SET @parentCategoryId = NULL;

    INSERT INTO Categories (Id, Name, Description, ParentCategoryId, Status, CreatedDate, UpdatedDate)
    VALUES (
        NEWID(),
        CONCAT('Category_', @counter),
        CONCAT('Description for Category_', @counter),
        @parentCategoryId,
        1, -- Assume 1 = Active
        GETDATE(),
        GETDATE()
    );

    SET @counter = @counter + 1;
END;
