CREATE FUNCTION dbo.[GetInstructorsIds](@CourseId UNIQUEIDENTIFIER) RETURNS VARCHAR(MAX)
AS
BEGIN
	DECLARE @InstructorsIds VARCHAR(MAX) = '';
	SELECT @InstructorsIds += CONVERT(VARCHAR(40), I.[Id]) + ','
	FROM dbo.[Instructor] I
	LEFT JOIN dbo.[CourseInstructor] CI ON CI.[InstructorId] = I.[Id]
	WHERE CI.[CourseId] = @CourseId

	IF LEN(@InstructorsIds) > 0
		BEGIN SET @InstructorsIds = LEFT(@InstructorsIds, LEN(@InstructorsIds) - 1) END
	RETURN @InstructorsIds
END