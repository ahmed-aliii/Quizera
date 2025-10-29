
DELETE FROM Answers;
DBCC CHECKIDENT ('Answers', RESEED, 0);

DELETE FROM Attempts;
DBCC CHECKIDENT ('Attempts', RESEED, 0);

DELETE FROM Options;
DBCC CHECKIDENT ('Options', RESEED, 0);

DELETE FROM Questions;
DBCC CHECKIDENT ('Questions', RESEED, 0);

DELETE FROM Exams;
DBCC CHECKIDENT ('Exams', RESEED, 0);

DELETE FROM Courses;
DBCC CHECKIDENT ('Courses', RESEED, 0);
GO


-- 2. Insert Seed Data
-- ==========================================

-- 🔹 Course by Instructor
INSERT INTO [dbo].[Courses] ([Title], [Description], [ImageUrl], [InstructorId])
VALUES 
('Introduction to Databases', 'Basic concepts of relational databases, SQL, and normalization.', NULL, '5294114f-93f7-46cc-ba79-7cabc4b6269c');
GO

-- 🔹 Exam for the Course
INSERT INTO [dbo].[Exams] ([Title], [Description], [DurationInMinutes], [CourseId])
VALUES 
('Database Fundamentals Exam', 'Covers SQL basics and relational concepts.', 30, 1);
GO

-- 🔹 Questions for the Exam
INSERT INTO [dbo].[Questions] ([Text], [Marks], [ExamId])
VALUES 
('What does SQL stand for?', 1, 1),
('Which of the following is a valid SQL command?', 1, 1);
GO

-- 🔹 Options for Question 1
INSERT INTO [dbo].[Options] ([Text], [IsCorrect], [QuestionId])
VALUES 
('Structured Query Language', 1, 1),
('Simple Query List', 0, 1),
('Strong Question Logic', 0, 1),
('Sequential Query Layer', 0, 1);
GO

-- 🔹 Options for Question 2
INSERT INTO [dbo].[Options] ([Text], [IsCorrect], [QuestionId])
VALUES 
('SELECT * FROM Students;', 1, 2),
('GET ALL Students;', 0, 2),
('SHOW Students;', 0, 2),
('FETCH ALL FROM Students;', 0, 2);
GO

-- 🔹 Student Attempt
INSERT INTO [dbo].[Attempts] ([UserId], [ExamId], [Score])
VALUES 
('2266e396-bd5c-4d6c-be68-b0680a049948', 1, 0);
GO

-- 🔹 Student Answers
INSERT INTO [dbo].[Answers] ([WrittenAnswer], [IsCorrect], [QuestionId], [OptionId], [AttemptId])
VALUES 
(NULL, 1, 1, 1, 1), -- Selected correct option for Q1
(NULL, 1, 2, 5, 1); -- Selected correct option for Q2
GO