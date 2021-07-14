CREATE PROCEDURE dbo.[SP_CoursesPaged]
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

	SELECT C.[Id]
	, C.[Title]
	, C.[Description]
	, C.[PublishingDate]
	, C.[ProfilePicture]
	, C.[CreatedAt]
	, C.[UpdatedAt]
	, C.[CreatedBy]
	, C.[UpdatedBy]
	, P.[CurrentPrice]
	, P.[Promotion]
	, dbo.[GetInstructorsIds](C.[Id]) AS [InstructorsIds]
	FROM dbo.[Course] C
	LEFT JOIN dbo.[Price] P ON P.[CourseId] = C.[Id]
	ORDER BY
		CASE WHEN @IsSortDescendent = 1 THEN
			CASE
				WHEN @SortBy = 'title' THEN C.[Title]
				WHEN @SortBy = 'publishingdate' THEN CONVERT(VARCHAR(10), C.[PublishingDate], 112)
				WHEN @SortBy = 'createdat' THEN CONVERT(VARCHAR(20), C.[CreatedAt], 120)
				WHEN @SortBy = 'updatedat' THEN CONVERT(VARCHAR(20), C.[UpdatedAt], 120)
				ELSE CONVERT(VARCHAR(40), C.[Id])
			END
		END DESC,
		CASE WHEN @IsSortDescendent = 0 THEN
			CASE
				WHEN @SortBy = 'title' THEN C.[Title]
				WHEN @SortBy = 'publishingdate' THEN CONVERT(VARCHAR(10), C.[PublishingDate], 112)
				WHEN @SortBy = 'createdat' THEN CONVERT(VARCHAR(20), C.[CreatedAt], 120)
				WHEN @SortBy = 'updatedat' THEN CONVERT(VARCHAR(20), C.[UpdatedAt], 120)
				ELSE CONVERT(VARCHAR(40), C.[Id])
			END
		END ASC

		OFFSET ((@Page - 1) * @PageSize) ROWS
		FETCH NEXT (@PageSize) ROWS ONLY;

	SET NOCOUNT OFF;
END
GO