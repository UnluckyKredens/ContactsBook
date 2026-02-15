SET NOCOUNT ON;
DECLARE @Table SYSNAME = N'dbo.Contacts';
DECLARE @i INT = 1;

DECLARE @Domains TABLE (DomainId INT IDENTITY(1,1), Domain NVARCHAR(100));
INSERT INTO @Domains (Domain) VALUES
(N'gmail.com'),
(N'outlook.com'),
(N'yahoo.com'),
(N'icloud.com'),
(N'proton.me'),
(N'wp.pl'),
(N'onet.pl'),
(N'interia.pl'),
(N'gazeta.pl'),
(N'example.com');


DECLARE @FirstNames TABLE (Id INT IDENTITY(1,1), Name NVARCHAR(50));
INSERT INTO @FirstNames(Name) VALUES
(N'Jan'),(N'Piotr'),(N'Kamil'),(N'Micha³'),(N'Tomasz'),(N'Pawe³'),(N'Adam'),(N'Mateusz'),(N'Jakub'),(N'Marcin'),
(N'Anna'),(N'Katarzyna'),(N'Magdalena'),(N'Aleksandra'),(N'Zuzanna'),(N'Julia'),(N'Maja'),(N'Natalia'),(N'Patrycja'),(N'Monika');


DECLARE @LastNames TABLE (Id INT IDENTITY(1,1), Name NVARCHAR(80));
INSERT INTO @LastNames(Name) VALUES
(N'Kowalski'),(N'Nowak'),(N'Wiœniewski'),(N'Wójcik'),(N'Kowalczyk'),(N'Kamiñski'),(N'Lewandowski'),(N'Zieliñski'),(N'Szymañski'),(N'WoŸniak'),
(N'D¹browski'),(N'Koz³owski'),(N'Jankowski'),(N'Mazur'),(N'Wojciechowski'),(N'Kwiatkowski'),(N'Krawczyk'),(N'Kaczmarek'),(N'Piotrowski'),(N'Grabowski');

DECLARE @Places TABLE (
  Id INT IDENTITY(1,1),
  City NVARCHAR(80),
  Zip NVARCHAR(10),
  Street NVARCHAR(120)
);

INSERT INTO @Places(City, Zip, Street) VALUES
(N'Warszawa', N'00-001', N'ul. Marsza³kowska'),
(N'Kraków',   N'30-001', N'ul. Karmelicka'),
(N'Wroc³aw',  N'50-001', N'ul. Œwidnicka'),
(N'Poznañ',   N'60-001', N'ul. Pó³wiejska'),
(N'Gdañsk',   N'80-001', N'ul. D³uga'),
(N'Gdynia',   N'81-001', N'ul. Œwiêtojañska'),
(N'Szczecin', N'70-001', N'al. Wyzwolenia'),
(N'£ódŸ',     N'90-001', N'ul. Piotrkowska'),
(N'Katowice', N'40-001', N'ul. 3 Maja'),
(N'Lublin',   N'20-001', N'ul. Krakowskie Przedmieœcie');

WHILE @i <= 600
BEGIN
    DECLARE @fn NVARCHAR(50),
            @ln NVARCHAR(80),
            @domain NVARCHAR(100),
            @city NVARCHAR(80),
            @zip NVARCHAR(10),
            @street NVARCHAR(120),
            @address NVARCHAR(200),
            @email NVARCHAR(450),
            @phone NVARCHAR(50);

    SELECT @fn = Name
    FROM @FirstNames
    WHERE Id = ((@i - 1) % (SELECT COUNT(*) FROM @FirstNames)) + 1;

    SELECT @ln = Name
    FROM @LastNames
    WHERE Id = ((@i - 1) % (SELECT COUNT(*) FROM @LastNames)) + 1;

    SELECT @domain = Domain
    FROM @Domains
    WHERE DomainId = ((@i - 1) % 10) + 1;  -- dok³adnie 10 domen

    SELECT @city = City, @zip = Zip, @street = Street
    FROM @Places
    WHERE Id = ((@i - 1) % (SELECT COUNT(*) FROM @Places)) + 1;

    SET @address = CONCAT(@street, N' ', ((@i * 7) % 180) + 1, N'/', ((@i * 11) % 60) + 1);

    SET @phone = CONCAT(N'+48 5',
                        RIGHT(CONCAT('00', ((@i * 13) % 100)), 2),
                        N' ',
                        RIGHT(CONCAT('00', ((@i * 17) % 1000)), 3),
                        N' ',
                        RIGHT(CONCAT('00', ((@i * 19) % 1000)), 3));


    DECLARE @emailLocal NVARCHAR(200) = LOWER(CONCAT(@fn, N'.', @ln, @i));
    SET @emailLocal = REPLACE(@emailLocal, N'¹', N'a');
    SET @emailLocal = REPLACE(@emailLocal, N'æ', N'c');
    SET @emailLocal = REPLACE(@emailLocal, N'ê', N'e');
    SET @emailLocal = REPLACE(@emailLocal, N'³', N'l');
    SET @emailLocal = REPLACE(@emailLocal, N'ñ', N'n');
    SET @emailLocal = REPLACE(@emailLocal, N'ó', N'o');
    SET @emailLocal = REPLACE(@emailLocal, N'œ', N's');
    SET @emailLocal = REPLACE(@emailLocal, N'¿', N'z');
    SET @emailLocal = REPLACE(@emailLocal, N'Ÿ', N'z');

    SET @email = CONCAT(@emailLocal, N'@', @domain);

    DECLARE @sql NVARCHAR(MAX) = N'
      INSERT INTO ' + @Table + N' (FirstName, LastName, Email, PhoneNumber, Address, City, Zip)
      VALUES (@fn, @ln, @email, @phone, @address, @city, @zip);';

    EXEC sp_executesql
        @sql,
        N'@fn NVARCHAR(MAX), @ln NVARCHAR(MAX), @email NVARCHAR(450), @phone NVARCHAR(MAX), @address NVARCHAR(MAX), @city NVARCHAR(MAX), @zip NVARCHAR(MAX)',
        @fn=@fn, @ln=@ln, @email=@email, @phone=@phone, @address=@address, @city=@city, @zip=@zip;

    SET @i += 1;
END;