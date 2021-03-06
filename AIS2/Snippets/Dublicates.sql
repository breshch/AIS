USE AV_New;

DECLARE @carParts table(Id int, Article nvarchar(max), Mark nvarchar(max))

INSERT INTO @carParts SELECT DISTINCT cp1.Id, cp1.Article, cp1.Mark
FROM DirectoryCarParts cp1
JOIN DirectoryCarParts cp2 ON cp1.Article = cp2.Article AND cp1.Mark = cp2.Mark AND cp1.Id <> cp2.Id

DECLARE @uniqueCarParts table(Id int, Article nvarchar(max), Mark nvarchar(max))

INSERT INTO @uniqueCarParts SELECT MIN(Id), Article, Mark
FROM @carParts
GROUP BY Article, Mark

UPDATE CurrentCarParts
SET DirectoryCarPartId = u.Id
FROM @uniqueCarParts u
JOIN @carParts cp ON u.Article = cp.Article AND u.Mark = cp.Mark AND u.Id <> cp.Id
JOIN CurrentCarParts ccp ON cp.Id = ccp.DirectoryCarPartId

UPDATE CurrentContainerCarParts
SET DirectoryCarPartId = u.Id
FROM @uniqueCarParts u
JOIN @carParts cp ON u.Article = cp.Article AND u.Mark = cp.Mark AND u.Id <> cp.Id
JOIN CurrentContainerCarParts ccp ON cp.Id = ccp.DirectoryCarPartId

DELETE FROM DirectoryCarParts
WHERE Id IN
(SELECT DISTINCT cp1.Id
FROM DirectoryCarParts cp1
JOIN DirectoryCarParts cp2 ON cp1.Article = cp2.Article AND cp1.Mark = cp2.Mark AND cp1.Id <> cp2.Id
WHERE cp1.Id NOT IN 
	(SELECT DISTINCT DirectoryCarPartId
	FROM CurrentCarParts))