-- Pack variance
SELECT p.ProductId,
       p.Title,
	   p.SetSize AS DisplayedSetSize,   
	   pc.ActualSetSize,
	   (p.SetSize - pc.ActualSetSize) AS MissingSetSize,
	   p.LaunchDate	   
FROM Product p
INNER JOIN (
  SELECT ProductId, COUNT(*) AS ActualSetSize FROM ProductCard 
  GROUP BY ProductId
) pc ON pc.ProductId = p.ProductId
WHERE MissingSetSize > 0
ORDER BY p.LaunchDate ASC;