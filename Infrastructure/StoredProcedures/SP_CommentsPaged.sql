CREATE PROCEDURE dbo.[SP_CommentsPaged]
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

	SELECT CO.[Id]
	, CO.[CourseId]
	, CO.[StudentName]
	, CO.[Message]
	, CO.[Score]
	, CO.[CreatedAt]
	, CO.[UpdatedAt]
	, CO.[CreatedBy]
	, CO.[UpdatedBy]
	FROM dbo.[Comment] CO
	ORDER BY
		CASE WHEN @IsSortDescendent = 1 THEN
			CASE
				WHEN @SortBy = 'courseid' THEN CONVERT(VARCHAR(40), CO.[CourseId])
				WHEN @SortBy = 'studentname' THEN CO.[StudentName]
				WHEN @SortBy = 'message' THEN CO.[Message]
				WHEN @SortBy = 'score' THEN CO.[Score]
				WHEN @SortBy = 'createdat' THEN CONVERT(VARCHAR(20), CO.[CreatedAt], 120)
				WHEN @SortBy = 'updatedat' THEN CONVERT(VARCHAR(20), CO.[UpdatedAt], 120)
				ELSE CONVERT(VARCHAR(40), CO.[Id])
			END
		END DESC,
		CASE WHEN @IsSortDescendent = 0 THEN
			CASE
				WHEN @SortBy = 'courseid' THEN CONVERT(VARCHAR(40), CO.[CourseId])
				WHEN @SortBy = 'studentname' THEN CO.[StudentName]
				WHEN @SortBy = 'message' THEN CO.[Message]
				WHEN @SortBy = 'score' THEN CO.[Score]
				WHEN @SortBy = 'createdat' THEN CONVERT(VARCHAR(20), CO.[CreatedAt], 120)
				WHEN @SortBy = 'updatedat' THEN CONVERT(VARCHAR(20), CO.[UpdatedAt], 120)
				ELSE CONVERT(VARCHAR(40), CO.[Id])
			END
		END ASC

		OFFSET ((@Page - 1) * @PageSize) ROWS
		FETCH NEXT (@PageSize) ROWS ONLY;

	SET NOCOUNT OFF;
END
GO