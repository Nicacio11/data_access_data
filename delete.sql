create or alter procedure spDeleteStudent
(
    @StudentId uniqueidentifier
)
as 
    BEGIN TRANSACTION 
    DELETE FROM [StudentCourse] where [StudentId] = @StudentId
    DELETE FROM [Student] where [Id] = @StudentId
    COMMIT