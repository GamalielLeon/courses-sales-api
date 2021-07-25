CREATE OR ALTER PROCEDURE dbo.[usp_InstructorsPaged]
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

	SELECT I.[Id]
	, I.[FirstName]
	, I.[LastName]
	, I.[Degree]
	, I.[ProfilePicture]
	, I.[CreatedAt]
	, I.[UpdatedAt]
	, I.[CreatedBy]
	, I.[UpdatedBy]
	FROM dbo.[Instructor] I
	ORDER BY
		CASE WHEN @IsSortDescendent = 1 THEN
			CASE
				WHEN @SortBy = 'firstname' THEN I.[FirstName]
				WHEN @SortBy = 'lastname' THEN I.[LastName]
				WHEN @SortBy = 'degree' THEN I.[Degree]
				WHEN @SortBy = 'createdat' THEN CONVERT(VARCHAR(20), I.[CreatedAt], 120)
				WHEN @SortBy = 'updatedat' THEN CONVERT(VARCHAR(20), I.[UpdatedAt], 120)
				ELSE CONVERT(VARCHAR(40), I.[Id])
			END
		END DESC,
		CASE WHEN @IsSortDescendent = 0 THEN
			CASE
				WHEN @SortBy = 'firstname' THEN I.[FirstName]
				WHEN @SortBy = 'lastname' THEN I.[LastName]
				WHEN @SortBy = 'degree' THEN I.[Degree]
				WHEN @SortBy = 'createdat' THEN CONVERT(VARCHAR(20), I.[CreatedAt], 120)
				WHEN @SortBy = 'updatedat' THEN CONVERT(VARCHAR(20), I.[UpdatedAt], 120)
				ELSE CONVERT(VARCHAR(40), I.[Id])
			END
		END ASC

		OFFSET ((@Page - 1) * @PageSize) ROWS
		FETCH NEXT (@PageSize) ROWS ONLY;

	SET NOCOUNT OFF;
END
GO