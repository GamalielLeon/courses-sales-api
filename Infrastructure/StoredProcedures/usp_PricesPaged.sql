CREATE OR ALTER PROCEDURE dbo.[usp_PricesPaged]
	@Page INT = 1
	, @PageSize INT = 10
	, @SortBy VARCHAR(100) = 'id'
	, @IsSortDescendent BIT = 0
AS
BEGIN
	--Avoids to return the number of transactions performed.
	SET NOCOUNT ON
	--Allows to read queries than have not been committed (that are not confirmed or rejected).
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	SELECT P.[Id]
	, P.[CourseId]
	, P.[CurrentPrice]
	, P.[Promotion]
	, P.[CreatedAt]
	, P.[UpdatedAt]
	, P.[CreatedBy]
	, P.[UpdatedBy]
	FROM dbo.[Price] P
	ORDER BY
		CASE WHEN @IsSortDescendent = 1 THEN
			CASE
				WHEN @SortBy = 'courseid' THEN CONVERT(VARCHAR(40), P.[CourseId])
				WHEN @SortBy = 'currentprice' THEN CONVERT(VARCHAR(12), P.[CurrentPrice])
				WHEN @SortBy = 'promotion' THEN CONVERT(VARCHAR(12), P.[Promotion])
				WHEN @SortBy = 'createdat' THEN CONVERT(VARCHAR(20), P.[CreatedAt], 120)
				WHEN @SortBy = 'updatedat' THEN CONVERT(VARCHAR(20), P.[UpdatedAt], 120)
				ELSE CONVERT(VARCHAR(40), P.[Id])
			END
		END DESC,
		CASE WHEN @IsSortDescendent = 0 THEN
			CASE
				WHEN @SortBy = 'courseid' THEN CONVERT(VARCHAR(40), P.[CourseId])
				WHEN @SortBy = 'currentprice' THEN CONVERT(VARCHAR(12), P.[CurrentPrice])
				WHEN @SortBy = 'promotion' THEN CONVERT(VARCHAR(12), P.[Promotion])
				WHEN @SortBy = 'createdat' THEN CONVERT(VARCHAR(20), P.[CreatedAt], 120)
				WHEN @SortBy = 'updatedat' THEN CONVERT(VARCHAR(20), P.[UpdatedAt], 120)
				ELSE CONVERT(VARCHAR(40), P.[Id])
			END
		END ASC

		OFFSET ((@Page - 1) * @PageSize) ROWS
		FETCH NEXT (@PageSize) ROWS ONLY;

	SET NOCOUNT OFF;
END
GO