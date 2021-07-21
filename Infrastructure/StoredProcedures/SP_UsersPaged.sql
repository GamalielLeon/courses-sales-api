CREATE PROCEDURE dbo.[SP_UsersPaged]
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

	SELECT U.[Id]
	, U.[FirstName]
	, U.[LastName]
	, U.[UserName]
	, U.[Email]
	, U.[PhoneNumber]
	, U.[CreatedAt]
	, U.[UpdatedAt]
	, U.[CreatedBy]
	, U.[UpdatedBy]
	FROM dbo.[User] U
	ORDER BY
		CASE WHEN @IsSortDescendent = 1 THEN
			CASE
				WHEN @SortBy = 'firstname' THEN U.[FirsName]
				WHEN @SortBy = 'lastname' THEN U.[LastName]
				WHEN @SortBy = 'username' THEN U.[UserName]
				WHEN @SortBy = 'email' THEN U.[Email]
				WHEN @SortBy = 'phonenumber' THEN U.[PhoneNumber]
				WHEN @SortBy = 'createdat' THEN CONVERT(VARCHAR(20), U.[CreatedAt], 120)
				WHEN @SortBy = 'updatedat' THEN CONVERT(VARCHAR(20), U.[UpdatedAt], 120)
				ELSE CONVERT(VARCHAR(40), U.[Id])
			END
		END DESC,
		CASE WHEN @IsSortDescendent = 0 THEN
			CASE
				WHEN @SortBy = 'firstname' THEN U.[FirsName]
				WHEN @SortBy = 'lastname' THEN U.[LastName]
				WHEN @SortBy = 'username' THEN U.[UserName]
				WHEN @SortBy = 'email' THEN U.[Email]
				WHEN @SortBy = 'phonenumber' THEN U.[PhoneNumber]
				WHEN @SortBy = 'createdat' THEN CONVERT(VARCHAR(20), U.[CreatedAt], 120)
				WHEN @SortBy = 'updatedat' THEN CONVERT(VARCHAR(20), U.[UpdatedAt], 120)
				ELSE CONVERT(VARCHAR(40), U.[Id])
			END
		END ASC

		OFFSET ((@Page - 1) * @PageSize) ROWS
		FETCH NEXT (@PageSize) ROWS ONLY;

	SET NOCOUNT OFF;
END
GO