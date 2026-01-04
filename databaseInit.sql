USE master;

-- cutting existing connections to the database and dropping it if it exists
IF EXISTS ( 
    SELECT name
    FROM sys.databases
    WHERE name = 'LanguageCenterDB'
) BEGIN
    ALTER DATABASE LanguageCenterDB
    SET SINGLE_USER
    WITH ROLLBACK IMMEDIATE;
    DROP DATABASE LanguageCenterDB;
END 

CREATE DATABASE LanguageCenterDB;
GO 
USE LanguageCenterDB;
GO

CREATE TABLE Languages (
    language_code CHAR(2) PRIMARY KEY,
    language_name NVARCHAR (50) NOT NULL,
    is_active BIT DEFAULT 1
);

CREATE TABLE Languages_Levels (
    language_level_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    language_code CHAR(2) NOT NULL,
    level_name NVARCHAR (50) NOT NULL,
    level_order INT,
    description NVARCHAR (MAX),
    is_active BIT DEFAULT 1,
    CONSTRAINT FK_Languages_Levels_Languages FOREIGN KEY (language_code) REFERENCES Languages (language_code) ON DELETE CASCADE
);

CREATE TABLE Users (
    user_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    username NVARCHAR (50) NOT NULL UNIQUE,
    password_hash NVARCHAR (255) NOT NULL,
    email NVARCHAR (100) NOT NULL UNIQUE,
    phone NVARCHAR (20),
    full_name NVARCHAR (100) NOT NULL,
    date_of_birth DATE,
    gender NVARCHAR (10) CHECK (gender IN ('male', 'female', 'other')),
    address NVARCHAR (255),
    role NVARCHAR (20) NOT NULL CHECK (role IN ('admin', 'teacher', 'student')),
    avatar_url NVARCHAR (2048),
    is_active BIT DEFAULT 1,
) 

GO
CREATE TABLE Teacher (
    teacher_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    user_id UNIQUEIDENTIFIER NOT NULL,
    specialization NVARCHAR (100),
    years_of_experience INT,
    qualifications NVARCHAR (255),
    bio NVARCHAR (MAX),
    is_active BIT DEFAULT 1,
    CONSTRAINT FK_Teacher_Users FOREIGN KEY (user_id) REFERENCES Users (user_id),
);

GO
CREATE TABLE Students (
    student_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    user_id UNIQUEIDENTIFIER NOT NULL UNIQUE,
    is_active BIT DEFAULT 1,
    CONSTRAINT FK_Students_Users FOREIGN KEY (user_id) REFERENCES Users (user_id),
);

GO
CREATE TABLE Courses (
    course_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    course_name NVARCHAR (150) NOT NULL,
    language_level_id UNIQUEIDENTIFIER NOT NULL,
    description NVARCHAR (MAX),
    duration_hours INT NOT NULL,
    fee DECIMAL(10, 2) NOT NULL,
    thumbnail_url NVARCHAR (2048),
    course_status NVARCHAR (20) CHECK (course_status IN ('active', 'inactive')) DEFAULT 'active',
    CONSTRAINT FK_Courses_LanguageLevels FOREIGN KEY (language_level_id) REFERENCES Languages_Levels (language_level_id)
);

CREATE TABLE Classes (
    class_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    course_id UNIQUEIDENTIFIER NOT NULL,
    teacher_id UNIQUEIDENTIFIER NOT NULL,
    class_name NVARCHAR (150) NOT NULL,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    max_students INT NOT NULL,
    class_status NVARCHAR (20) CHECK (class_status IN ('scheduled', 'ongoing', 'completed', 'canceled')) DEFAULT 'scheduled',
    CONSTRAINT FK_Classes_Courses FOREIGN KEY (course_id) REFERENCES Courses (course_id) ON DELETE CASCADE,
    CONSTRAINT FK_Classes_Teacher FOREIGN KEY (teacher_id) REFERENCES Teacher (teacher_id)
);

GO
CREATE TABLE Enrollments (
    enrollment_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    student_id UNIQUEIDENTIFIER NOT NULL,
    class_id UNIQUEIDENTIFIER NOT NULL,
    enrollment_date DATE DEFAULT GETDATE (),
    enrollment_status NVARCHAR (20) CHECK (enrollment_status IN ('pending', 'active', 'completed')) DEFAULT 'pending',
    CONSTRAINT FK_Enrollments_Students FOREIGN KEY (student_id) REFERENCES Students (student_id),
    CONSTRAINT FK_Enrollments_Classes FOREIGN KEY (class_id) REFERENCES Classes (class_id)
);


GO
CREATE TABLE Invoices (
    invoice_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    enrollment_id UNIQUEIDENTIFIER NOT NULL,
    amount DECIMAL(10, 2) NOT NULL,
    issue_date DATE DEFAULT GETDATE (),
    due_date DATE NOT NULL,
    payment_date DATE,
    invoice_status NVARCHAR (20) CHECK (invoice_status IN ('unpaid', 'paid', 'overdue')) DEFAULT 'unpaid',
    CONSTRAINT FK_Invoices_Enrollments FOREIGN KEY (enrollment_id) REFERENCES Enrollments (enrollment_id)
)

GO
CREATE TABLE Schedules (
    schedule_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    class_id UNIQUEIDENTIFIER NOT NULL,
    study_date DATE NOT NULL,
    start_time TIME NOT NULL,
    end_time TIME NOT NULL,
    room NVARCHAR (2048),
    CONSTRAINT FK_Schedules_Classes FOREIGN KEY (class_id) REFERENCES Classes (class_id) ON DELETE CASCADE
);

GO
CREATE TABLE Attendance (
    attendance_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    schedule_id UNIQUEIDENTIFIER NOT NULL,
    student_id UNIQUEIDENTIFIER NOT NULL,
    attendance_status NVARCHAR (20) NOT NULL CHECK (attendance_status IN ('present', 'absent', 'late', 'permittedAbsence')),
    CONSTRAINT FK_Attendance_Schedules FOREIGN KEY (schedule_id) REFERENCES Schedules (schedule_id) ON DELETE CASCADE,
    CONSTRAINT FK_Attendance_Students FOREIGN KEY (student_id) REFERENCES Students (student_id) ON DELETE CASCADE
);
GO

CREATE TABLE Exams (
    exam_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    class_id UNIQUEIDENTIFIER NOT NULL,
    exam_date DATE NOT NULL,
    start_time TIME NOT NULL,
    end_time TIME NOT NULL,
    room NVARCHAR (2048),
    max_score FLOAT NOT NULL,
    passing_score FLOAT DEFAULT 0,
    weightage FLOAT CHECK (weightage >= 0 AND weightage <= 1) DEFAULT 0,
    CONSTRAINT FK_Exams_Classes FOREIGN KEY (class_id) REFERENCES Classes (class_id) ON DELETE CASCADE
)

GO
CREATE TABLE ExamParts (
    exam_part_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    exam_id UNIQUEIDENTIFIER NOT NULL,
    part_name NVARCHAR (100) NOT NULL,
    max_score FLOAT NOT NULL,
    passing_score FLOAT DEFAULT 0,
    weightage FLOAT NOT NULL,
    CONSTRAINT FK_ExamParts_Exams FOREIGN KEY (exam_id) REFERENCES Exams (exam_id) ON DELETE CASCADE
); 

GO
CREATE TABLE ExamResults (
    exam_result_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    exam_id UNIQUEIDENTIFIER NOT NULL,
    student_id UNIQUEIDENTIFIER NOT NULL,
    score INT NOT NULL,
    CONSTRAINT FK_ExamResults_Exams FOREIGN KEY (exam_id) REFERENCES Exams (exam_id),
    CONSTRAINT FK_ExamResults_Students FOREIGN KEY (student_id) REFERENCES Students (student_id)
);


GO 
CREATE TABLE ExamPartResults (
    exam_part_result_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    exam_part_id UNIQUEIDENTIFIER NOT NULL,
    exam_result_id UNIQUEIDENTIFIER NOT NULL,
    score DECIMAL(10, 2) NOT NULL,

    CONSTRAINT FK_ExamPartResults_ExamParts FOREIGN KEY (exam_part_id) REFERENCES ExamParts (exam_part_id),
    CONSTRAINT FK_ExamPartResults_ExamResults FOREIGN KEY (exam_result_id) REFERENCES ExamResults (exam_result_id)
);


GO 
CREATE TABLE Certificates (
    certificate_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    student_id UNIQUEIDENTIFIER NOT NULL,
    course_id UNIQUEIDENTIFIER NOT NULL,
    issue_date DATE DEFAULT GETDATE (),
    certificate_url NVARCHAR (2048),
    certificate_status NVARCHAR (20) CHECK (certificate_status IN ('issued', 'revoked')) DEFAULT 'issued',
    CONSTRAINT FK_Certificates_Students FOREIGN KEY (student_id) REFERENCES Students (student_id),
    CONSTRAINT FK_Certificates_Courses FOREIGN KEY (course_id) REFERENCES Courses (course_id)
);

GO
CREATE PROCEDURE sp_CreateExamWithParts
    @exam_id UNIQUEIDENTIFIER,
    @class_id UNIQUEIDENTIFIER,
    @exam_date DATE,
    @start_time VARCHAR(20), -- Hoặc kiểu TIME tùy DB của bạn
    @end_time VARCHAR(20),
    @room NVARCHAR(100),
    @max_score FLOAT,
    @passing_score FLOAT,
    @weightage FLOAT,
    @json_parts NVARCHAR(MAX) -- <--- Đây là chỗ chứa danh sách Parts
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;

        -- 1. Insert Exam (Cha)
        INSERT INTO Exams (
            exam_id, class_id, exam_date, start_time, end_time, 
            room, max_score, passing_score, weightage
        ) VALUES (
            @exam_id, @class_id, @exam_date, @start_time, @end_time, 
            @room, @max_score, @passing_score, @weightage
        );

        -- 2. Insert ExamParts (Con) từ chuỗi JSON
        -- OPENJSON sẽ biến chuỗi JSON thành bảng để Insert
        IF @json_parts IS NOT NULL AND LEN(@json_parts) > 0
        BEGIN
            INSERT INTO ExamParts (
                exam_part_id, exam_id, part_name, max_score, passing_score, weightage
            )
            SELECT 
                part_id, @exam_id, part_name, max_score, passing_score, weightage
            FROM OPENJSON(@json_parts)
            WITH (
                part_id UNIQUEIDENTIFIER '$.exam_part_id',
                part_name NVARCHAR(200) '$.part_name',
                max_score FLOAT '$.max_score',
                passing_score FLOAT '$.passing_score',
                weightage FLOAT '$.weightage'
            );
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        -- Ném lỗi ra để C# biết
        THROW; 
    END CATCH
END
GO

CREATE PROCEDURE sp_UpdateExamWithParts
    @exam_id UNIQUEIDENTIFIER,
    @class_id UNIQUEIDENTIFIER,
    @exam_date DATE,
    @start_time VARCHAR(20),
    @end_time VARCHAR(20),
    @room NVARCHAR(100),
    @max_score FLOAT,
    @passing_score FLOAT,
    @weightage FLOAT,
    @json_parts NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON; -- Vẫn giữ cái này để tối ưu performance
    
    BEGIN TRY
        BEGIN TRANSACTION;

        -- 1. Cập nhật thông tin Exam (Cha)
        UPDATE Exams
        SET 
            class_id = @class_id,
            exam_date = @exam_date,
            start_time = @start_time,
            end_time = @end_time,
            room = @room,
            max_score = @max_score,
            passing_score = @passing_score,
            weightage = @weightage
        WHERE exam_id = @exam_id;

        -- 2. CHECK QUAN TRỌNG: Kiểm tra xem có dòng nào được update không?
        IF @@ROWCOUNT = 0
        BEGIN
            -- Không tìm thấy ID để update -> Rollback và báo thất bại
            ROLLBACK TRANSACTION;
            SELECT 0; -- Trả về 0 (Thất bại/Không tìm thấy)
            RETURN;
        END

        -- 3. Nếu tìm thấy thì mới làm tiếp: Xóa ExamParts cũ
        DELETE FROM ExamParts WHERE exam_id = @exam_id;

        -- 4. Insert ExamParts mới
        IF @json_parts IS NOT NULL AND LEN(@json_parts) > 0
        BEGIN
            INSERT INTO ExamParts (
                exam_part_id, exam_id, part_name, max_score, passing_score, weightage
            )
            SELECT 
                part_id, @exam_id, part_name, max_score, passing_score, weightage
            FROM OPENJSON(@json_parts)
            WITH (
                part_id UNIQUEIDENTIFIER '$.exam_part_id',
                part_name NVARCHAR(200) '$.part_name',
                max_score FLOAT '$.max_score',
                passing_score FLOAT '$.passing_score',
                weightage FLOAT '$.weightage'
            );
        END

        COMMIT TRANSACTION;
        SELECT 1; -- Trả về 1 (Thành công)
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END

GO 
CREATE OR ALTER PROCEDURE sp_DeleteExam
    @exam_id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;

        -- 1. Xóa các ExamParts (Con) trước
        -- Phải xóa con trước thì mới xóa được cha (nếu có khóa ngoại)
        DELETE FROM ExamParts WHERE exam_id = @exam_id;

        -- 2. Xóa Exam (Cha)
        DELETE FROM Exams WHERE exam_id = @exam_id;

        -- 3. Kiểm tra xem có xóa được dòng nào của bảng Exam không
        IF @@ROWCOUNT > 0 
        BEGIN
            COMMIT TRANSACTION;
            SELECT 1; -- Trả về 1 (Thành công)
        END
        ELSE
        BEGIN
            -- Không tìm thấy ID để xóa
            ROLLBACK TRANSACTION;
            SELECT 0; -- Trả về 0 (Thất bại / Không tìm thấy)
        END

    END TRY
    BEGIN CATCH
        -- Nếu lỗi (ví dụ: Đề thi đã có sinh viên làm bài -> dính khóa ngoại bảng StudentScores)
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        -- Ném lỗi ra để C# bắt được (hoặc ông có thể return 0 tùy logic)
        THROW; 
    END CATCH
END
GO


-- =============================================
-- INSERT SAMPLE DATA FOR LANGUAGE LEARNING SYSTEM
-- =============================================

INSERT INTO Languages (language_code, language_name, is_active) VALUES
('EN', N'English', 1),
('FR', N'French', 1),
('DE', N'German', 1),
('JP', N'Japanese', 1),
('KR', N'Korean', 1);

-- 2. INSERT Language Levels
INSERT INTO Languages_Levels (language_code, level_name, level_order, description, is_active) VALUES
-- English Levels
('EN', N'Beginner A1', 1, N'Basic English for absolute beginners', 1),
('EN', N'Elementary A2', 2, N'Elementary level English', 1),
('EN', N'Intermediate B1', 3, N'Intermediate English skills', 1),
('EN', N'Upper-Intermediate B2', 4, N'Advanced intermediate English', 1),
('EN', N'Advanced C1', 5, N'Advanced English proficiency', 1),
-- French Levels
('FR', N'Débutant A1', 1, N'French for beginners', 1),
('FR', N'Élémentaire A2', 2, N'Elementary French', 1),
('FR', N'Intermédiaire B1', 3, N'Intermediate French', 1),
-- German Levels
('DE', N'Anfänger A1', 1, N'German for beginners', 1),
('DE', N'Grundlagen A2', 2, N'Elementary German', 1),
-- Japanese Levels
('JP', N'Beginner N5', 1, N'JLPT N5 Level', 1),
('JP', N'Elementary N4', 2, N'JLPT N4 Level', 1),
-- Korean Levels
('KR', N'Beginner Level 1', 1, N'Basic Korean', 1),
('KR', N'Elementary Level 2', 2, N'Elementary Korean', 1);

-- 3. INSERT Users (Admin, Teachers, Students)
-- Admin
INSERT INTO Users (username, password_hash, email, phone, full_name, date_of_birth, gender, address, role, is_active) VALUES
('admin01', 'hashed_password_admin01', 'admin@languagecenter.com', '0901234567', N'Nguyễn Văn An', '1985-05-15', 'male', N'123 Trần Hưng Đạo, Hà Nội', 'admin', 1);

-- Teachers
INSERT INTO Users (username, password_hash, email, phone, full_name, date_of_birth, gender, address, role, avatar_url, is_active) VALUES
('teacher01', 'hashed_password_teacher01', 'john.smith@languagecenter.com', '0912345678', N'John Smith', '1988-03-20', 'male', N'45 Lê Lợi, Hà Nội', 'teacher', 'https://example.com/avatars/john.jpg', 1),
('teacher02', 'hashed_password_teacher02', 'marie.dubois@languagecenter.com', '0923456789', N'Marie Dubois', '1990-07-12', 'female', N'67 Nguyễn Huệ, Hà Nội', 'teacher', 'https://example.com/avatars/marie.jpg', 1),
('teacher03', 'hashed_password_teacher03', 'hans.mueller@languagecenter.com', '0934567890', N'Hans Mueller', '1987-11-25', 'male', N'89 Hai Bà Trưng, Hà Nội', 'teacher', 'https://example.com/avatars/hans.jpg', 1),
('teacher04', 'hashed_password_teacher04', 'yuki.tanaka@languagecenter.com', '0945678901', N'Yuki Tanaka', '1992-04-08', 'female', N'12 Lý Thường Kiệt, Hà Nội', 'teacher', 'https://example.com/avatars/yuki.jpg', 1),
('teacher05', 'hashed_password_teacher05', 'kim.minsoo@languagecenter.com', '0956789012', N'Kim Min-soo', '1989-09-30', 'male', N'34 Láng Hạ, Hà Nội', 'teacher', 'https://example.com/avatars/kim.jpg', 1);

-- Students
INSERT INTO Users (username, password_hash, email, phone, full_name, date_of_birth, gender, address, role, avatar_url, is_active) VALUES
('student01', 'hashed_password_student01', 'nguyen.van.a@gmail.com', '0967890123', N'Nguyễn Văn A', '2000-01-15', 'male', N'56 Giảng Võ, Hà Nội', 'student', 'https://example.com/avatars/student01.jpg', 1),
('student02', 'hashed_password_student02', 'tran.thi.b@gmail.com', '0978901234', N'Trần Thị B', '2001-05-22', 'female', N'78 Đê La Thành, Hà Nội', 'student', 'https://example.com/avatars/student02.jpg', 1),
('student03', 'hashed_password_student03', 'le.van.c@gmail.com', '0989012345', N'Lê Văn C', '1999-08-10', 'male', N'90 Nguyễn Chí Thanh, Hà Nội', 'student', 'https://example.com/avatars/student03.jpg', 1),
('student04', 'hashed_password_student04', 'pham.thi.d@gmail.com', '0990123456', N'Phạm Thị D', '2002-03-18', 'female', N'12 Cầu Giấy, Hà Nội', 'student', 'https://example.com/avatars/student04.jpg', 1),
('student05', 'hashed_password_student05', 'hoang.van.e@gmail.com', '0901234568', N'Hoàng Văn E', '2000-11-05', 'male', N'34 Xã Đàn, Hà Nội', 'student', 'https://example.com/avatars/student05.jpg', 1),
('student06', 'hashed_password_student06', 'vu.thi.f@gmail.com', '0912345679', N'Vũ Thị F', '2001-07-28', 'female', N'56 Thái Hà, Hà Nội', 'student', 'https://example.com/avatars/student06.jpg', 1),
('student07', 'hashed_password_student07', 'do.van.g@gmail.com', '0923456780', N'Đỗ Văn G', '1998-12-14', 'male', N'78 Phố Huế, Hà Nội', 'student', 'https://example.com/avatars/student07.jpg', 1),
('student08', 'hashed_password_student08', 'bui.thi.h@gmail.com', '0934567891', N'Bùi Thị H', '2002-06-20', 'female', N'90 Tây Sơn, Hà Nội', 'student', 'https://example.com/avatars/student08.jpg', 1),
('student09', 'hashed_password_student09', 'dinh.van.i@gmail.com', '0945678902', N'Đinh Văn I', '2000-09-09', 'male', N'12 Chùa Bộc, Hà Nội', 'student', 'https://example.com/avatars/student09.jpg', 1),
('student10', 'hashed_password_student10', 'ngo.thi.k@gmail.com', '0956789013', N'Ngô Thị K', '2001-02-25', 'female', N'34 Khâm Thiên, Hà Nội', 'student', 'https://example.com/avatars/student10.jpg', 1);

-- 4. INSERT Teachers (linked to Users)
INSERT INTO Teacher (user_id, specialization, years_of_experience, qualifications, bio, is_active)
SELECT u.user_id, N'English Language Teaching', 8, N'TESOL, CELTA Certified', N'Experienced English teacher specializing in business English and IELTS preparation', 1
FROM Users u WHERE u.username = 'teacher01'
UNION ALL
SELECT u.user_id, N'French Language Teaching', 6, N'DELF/DALF Examiner, MA in French', N'Native French speaker with expertise in French culture and literature', 1
FROM Users u WHERE u.username = 'teacher02'
UNION ALL
SELECT u.user_id, N'German Language Teaching', 7, N'Goethe-Institut Certified', N'German language expert focusing on business German', 1
FROM Users u WHERE u.username = 'teacher03'
UNION ALL
SELECT u.user_id, N'Japanese Language Teaching', 5, N'JLPT N1, Teaching Certification', N'Japanese native speaker specializing in JLPT preparation', 1
FROM Users u WHERE u.username = 'teacher04'
UNION ALL
SELECT u.user_id, N'Korean Language Teaching', 4, N'TOPIK Level 6, MA in Korean Studies', N'Korean language and culture specialist', 1
FROM Users u WHERE u.username = 'teacher05';

-- 5. INSERT Students (linked to Users)
INSERT INTO Students (user_id, is_active)
SELECT user_id, 1 FROM Users WHERE role = 'student';

-- 6. INSERT Courses
DECLARE @EnglishA1 UNIQUEIDENTIFIER = (SELECT language_level_id FROM Languages_Levels WHERE language_code = 'EN' AND level_order = 1);
DECLARE @EnglishA2 UNIQUEIDENTIFIER = (SELECT language_level_id FROM Languages_Levels WHERE language_code = 'EN' AND level_order = 2);
DECLARE @EnglishB1 UNIQUEIDENTIFIER = (SELECT language_level_id FROM Languages_Levels WHERE language_code = 'EN' AND level_order = 3);
DECLARE @FrenchA1 UNIQUEIDENTIFIER = (SELECT language_level_id FROM Languages_Levels WHERE language_code = 'FR' AND level_order = 1);
DECLARE @GermanA1 UNIQUEIDENTIFIER = (SELECT language_level_id FROM Languages_Levels WHERE language_code = 'DE' AND level_order = 1);
DECLARE @JapaneseN5 UNIQUEIDENTIFIER = (SELECT language_level_id FROM Languages_Levels WHERE language_code = 'JP' AND level_order = 1);
DECLARE @KoreanL1 UNIQUEIDENTIFIER = (SELECT language_level_id FROM Languages_Levels WHERE language_code = 'KR' AND level_order = 1);

INSERT INTO Courses (course_name, language_level_id, description, duration_hours, fee, thumbnail_url, course_status) VALUES
(N'English for Beginners - A1', @EnglishA1, N'Complete beginner English course covering basics of grammar, vocabulary, and conversation', 60, 3000000, 'https://example.com/courses/english-a1.jpg', 'active'),
(N'Elementary English - A2', @EnglishA2, N'Build on your English foundation with expanded vocabulary and grammar structures', 60, 3200000, 'https://example.com/courses/english-a2.jpg', 'active'),
(N'Intermediate English - B1', @EnglishB1, N'Develop fluency and confidence in everyday English communication', 80, 4000000, 'https://example.com/courses/english-b1.jpg', 'active'),
(N'French Basics - A1', @FrenchA1, N'Introduction to French language and culture for complete beginners', 60, 3500000, 'https://example.com/courses/french-a1.jpg', 'active'),
(N'German for Beginners - A1', @GermanA1, N'Start your journey in learning German language', 60, 3500000, 'https://example.com/courses/german-a1.jpg', 'active'),
(N'Japanese N5 - Beginner', @JapaneseN5, N'JLPT N5 preparation course with Hiragana, Katakana, and basic Kanji', 70, 4000000, 'https://example.com/courses/japanese-n5.jpg', 'active'),
(N'Korean Level 1', @KoreanL1, N'Learn Korean alphabet (Hangul) and basic conversation', 60, 3800000, 'https://example.com/courses/korean-l1.jpg', 'active');

-- 7. INSERT Classes
DECLARE @Teacher1 UNIQUEIDENTIFIER = (SELECT teacher_id FROM Teacher WHERE user_id = (SELECT user_id FROM Users WHERE username = 'teacher01'));
DECLARE @Teacher2 UNIQUEIDENTIFIER = (SELECT teacher_id FROM Teacher WHERE user_id = (SELECT user_id FROM Users WHERE username = 'teacher02'));
DECLARE @Teacher3 UNIQUEIDENTIFIER = (SELECT teacher_id FROM Teacher WHERE user_id = (SELECT user_id FROM Users WHERE username = 'teacher03'));
DECLARE @Teacher4 UNIQUEIDENTIFIER = (SELECT teacher_id FROM Teacher WHERE user_id = (SELECT user_id FROM Users WHERE username = 'teacher04'));
DECLARE @Teacher5 UNIQUEIDENTIFIER = (SELECT teacher_id FROM Teacher WHERE user_id = (SELECT user_id FROM Users WHERE username = 'teacher05'));

DECLARE @CourseEN_A1 UNIQUEIDENTIFIER = (SELECT TOP 1 course_id FROM Courses WHERE course_name LIKE N'%English for Beginners%');
DECLARE @CourseEN_A2 UNIQUEIDENTIFIER = (SELECT TOP 1 course_id FROM Courses WHERE course_name LIKE N'%Elementary English%');
DECLARE @CourseEN_B1 UNIQUEIDENTIFIER = (SELECT TOP 1 course_id FROM Courses WHERE course_name LIKE N'%Intermediate English%');
DECLARE @CourseFR_A1 UNIQUEIDENTIFIER = (SELECT TOP 1 course_id FROM Courses WHERE course_name LIKE N'%French Basics%');
DECLARE @CourseDE_A1 UNIQUEIDENTIFIER = (SELECT TOP 1 course_id FROM Courses WHERE course_name LIKE N'%German for Beginners%');
DECLARE @CourseJP_N5 UNIQUEIDENTIFIER = (SELECT TOP 1 course_id FROM Courses WHERE course_name LIKE N'%Japanese N5%');
DECLARE @CourseKR_L1 UNIQUEIDENTIFIER = (SELECT TOP 1 course_id FROM Courses WHERE course_name LIKE N'%Korean Level 1%');

INSERT INTO Classes (course_id, teacher_id, class_name, start_date, end_date, max_students, class_status) VALUES
(@CourseEN_A1, @Teacher1, N'English A1 - Morning Class', '2024-01-15', '2024-04-15', 20, 'completed'),
(@CourseEN_A1, @Teacher1, N'English A1 - Evening Class', '2024-09-01', '2024-12-01', 20, 'ongoing'),
(@CourseEN_A2, @Teacher1, N'English A2 - Intensive', '2024-10-01', '2025-01-15', 18, 'ongoing'),
(@CourseEN_B1, @Teacher1, N'English B1 - Weekend', '2025-01-10', '2025-05-10', 15, 'scheduled'),
(@CourseFR_A1, @Teacher2, N'French A1 - Beginners', '2024-09-15', '2024-12-15', 15, 'ongoing'),
(@CourseDE_A1, @Teacher3, N'German A1 - Morning', '2024-10-01', '2025-01-10', 12, 'ongoing'),
(@CourseJP_N5, @Teacher4, N'Japanese N5 - Evening', '2024-08-15', '2024-11-30', 15, 'completed'),
(@CourseJP_N5, @Teacher4, N'Japanese N5 - Winter', '2025-01-05', '2025-04-20', 15, 'scheduled'),
(@CourseKR_L1, @Teacher5, N'Korean L1 - Afternoon', '2024-09-20', '2024-12-20', 18, 'ongoing');

-- 8. INSERT Enrollments
DECLARE @Student1 UNIQUEIDENTIFIER = (SELECT student_id FROM Students WHERE user_id = (SELECT user_id FROM Users WHERE username = 'student01'));
DECLARE @Student2 UNIQUEIDENTIFIER = (SELECT student_id FROM Students WHERE user_id = (SELECT user_id FROM Users WHERE username = 'student02'));
DECLARE @Student3 UNIQUEIDENTIFIER = (SELECT student_id FROM Students WHERE user_id = (SELECT user_id FROM Users WHERE username = 'student03'));
DECLARE @Student4 UNIQUEIDENTIFIER = (SELECT student_id FROM Students WHERE user_id = (SELECT user_id FROM Users WHERE username = 'student04'));
DECLARE @Student5 UNIQUEIDENTIFIER = (SELECT student_id FROM Students WHERE user_id = (SELECT user_id FROM Users WHERE username = 'student05'));
DECLARE @Student6 UNIQUEIDENTIFIER = (SELECT student_id FROM Students WHERE user_id = (SELECT user_id FROM Users WHERE username = 'student06'));
DECLARE @Student7 UNIQUEIDENTIFIER = (SELECT student_id FROM Students WHERE user_id = (SELECT user_id FROM Users WHERE username = 'student07'));
DECLARE @Student8 UNIQUEIDENTIFIER = (SELECT student_id FROM Students WHERE user_id = (SELECT user_id FROM Users WHERE username = 'student08'));
DECLARE @Student9 UNIQUEIDENTIFIER = (SELECT student_id FROM Students WHERE user_id = (SELECT user_id FROM Users WHERE username = 'student09'));
DECLARE @Student10 UNIQUEIDENTIFIER = (SELECT student_id FROM Students WHERE user_id = (SELECT user_id FROM Users WHERE username = 'student10'));

DECLARE @Class1 UNIQUEIDENTIFIER = (SELECT TOP 1 class_id FROM Classes WHERE class_name LIKE N'%English A1 - Morning%');
DECLARE @Class2 UNIQUEIDENTIFIER = (SELECT TOP 1 class_id FROM Classes WHERE class_name LIKE N'%English A1 - Evening%');
DECLARE @Class3 UNIQUEIDENTIFIER = (SELECT TOP 1 class_id FROM Classes WHERE class_name LIKE N'%English A2 - Intensive%');
DECLARE @Class5 UNIQUEIDENTIFIER = (SELECT TOP 1 class_id FROM Classes WHERE class_name LIKE N'%French A1%');
DECLARE @Class7 UNIQUEIDENTIFIER = (SELECT TOP 1 class_id FROM Classes WHERE class_name LIKE N'%Japanese N5 - Evening%');
DECLARE @Class9 UNIQUEIDENTIFIER = (SELECT TOP 1 class_id FROM Classes WHERE class_name LIKE N'%Korean L1%');

INSERT INTO Enrollments (student_id, class_id, enrollment_date, enrollment_status) VALUES
-- Class 1 - Completed
(@Student1, @Class1, '2024-01-05', 'completed'),
(@Student2, @Class1, '2024-01-06', 'completed'),
(@Student3, @Class1, '2024-01-07', 'completed'),
-- Class 2 - Ongoing
(@Student4, @Class2, '2024-08-25', 'active'),
(@Student5, @Class2, '2024-08-26', 'active'),
(@Student6, @Class2, '2024-08-27', 'active'),
(@Student7, @Class2, '2024-08-28', 'active'),
-- Class 3 - Ongoing
(@Student8, @Class3, '2024-09-20', 'active'),
(@Student9, @Class3, '2024-09-21', 'active'),
-- Class 5 - French
(@Student10, @Class5, '2024-09-10', 'active'),
(@Student1, @Class5, '2024-09-11', 'active'),
-- Class 7 - Japanese Completed
(@Student2, @Class7, '2024-08-10', 'completed'),
(@Student3, @Class7, '2024-08-11', 'completed'),
(@Student4, @Class7, '2024-08-12', 'completed'),
-- Class 9 - Korean
(@Student5, @Class9, '2024-09-15', 'active'),
(@Student6, @Class9, '2024-09-16', 'active');

-- 9. INSERT Invoices
INSERT INTO Invoices (enrollment_id, amount, issue_date, due_date, payment_date, invoice_status)
SELECT 
    e.enrollment_id,
    c.fee,
    e.enrollment_date,
    DATEADD(DAY, 7, e.enrollment_date) AS due_date,
    CASE 
        WHEN e.enrollment_status = 'completed' THEN DATEADD(DAY, 2, e.enrollment_date)
        WHEN e.enrollment_status = 'active' AND DATEDIFF(DAY, e.enrollment_date, GETDATE()) > 3 THEN DATEADD(DAY, 3, e.enrollment_date)
        ELSE NULL
    END AS payment_date,
    CASE 
        WHEN e.enrollment_status = 'completed' THEN 'paid'
        WHEN e.enrollment_status = 'active' AND DATEDIFF(DAY, e.enrollment_date, GETDATE()) > 3 THEN 'paid'
        WHEN DATEADD(DAY, 7, e.enrollment_date) < GETDATE() THEN 'overdue'
        ELSE 'unpaid'
    END AS invoice_status
FROM Enrollments e
JOIN Classes cl ON e.class_id = cl.class_id
JOIN Courses c ON cl.course_id = c.course_id;

-- 10. INSERT Schedules (for ongoing and completed classes)
INSERT INTO Schedules (class_id, study_date, start_time, end_time, room) VALUES
-- Class 2 - English A1 Evening (ongoing: Sept 1 - Dec 1)
(@Class2, '2024-09-02', '18:00:00', '20:00:00', N'Room 101'),
(@Class2, '2024-09-04', '18:00:00', '20:00:00', N'Room 101'),
(@Class2, '2024-09-09', '18:00:00', '20:00:00', N'Room 101'),
(@Class2, '2024-09-11', '18:00:00', '20:00:00', N'Room 101'),
(@Class2, '2024-09-16', '18:00:00', '20:00:00', N'Room 101'),
-- Class 3 - English A2 Intensive (ongoing: Oct 1 - Jan 15)
(@Class3, '2024-10-02', '09:00:00', '12:00:00', N'Room 202'),
(@Class3, '2024-10-04', '09:00:00', '12:00:00', N'Room 202'),
(@Class3, '2024-10-07', '09:00:00', '12:00:00', N'Room 202'),
(@Class3, '2024-10-09', '09:00:00', '12:00:00', N'Room 202'),
-- Class 5 - French A1 (ongoing: Sept 15 - Dec 15)
(@Class5, '2024-09-17', '14:00:00', '16:00:00', N'Room 301'),
(@Class5, '2024-09-19', '14:00:00', '16:00:00', N'Room 301'),
(@Class5, '2024-09-24', '14:00:00', '16:00:00', N'Room 301'),
-- Class 7 - Japanese N5 Evening (completed: Aug 15 - Nov 30)
(@Class7, '2024-08-16', '18:30:00', '20:30:00', N'Room 401'),
(@Class7, '2024-08-19', '18:30:00', '20:30:00', N'Room 401'),
(@Class7, '2024-08-21', '18:30:00', '20:30:00', N'Room 401'),
(@Class7, '2024-08-26', '18:30:00', '20:30:00', N'Room 401');

-- 11. INSERT Attendance
INSERT INTO Attendance (schedule_id, student_id, attendance_status)
SELECT 
    s.schedule_id,
    e.student_id,
    CASE 
        WHEN ABS(CHECKSUM(NEWID())) % 10 < 8 THEN 'present'
        WHEN ABS(CHECKSUM(NEWID())) % 10 = 8 THEN 'late'
        ELSE 'absent'
    END AS attendance_status
FROM Schedules s
JOIN Classes cl ON s.class_id = cl.class_id
JOIN Enrollments e ON cl.class_id = e.class_id
WHERE cl.class_status IN ('ongoing', 'completed');

-- 12. INSERT Exams
INSERT INTO Exams (class_id, exam_date, start_time, end_time, room, max_score, passing_score, weightage) VALUES
-- Midterm Exam for Class 2
(@Class2, '2024-10-15', '18:00:00', '20:00:00', N'Room 101', 100, 50, 0.3),
-- Final Exam for Class 7 (completed)
(@Class7, '2024-11-28', '18:30:00', '21:00:00', N'Room 401', 100, 60, 0.7),
-- Midterm for Class 3
(@Class3, '2024-11-10', '09:00:00', '11:00:00', N'Room 202', 100, 50, 0.3);

-- 13. INSERT Exam Parts
DECLARE @Exam1 UNIQUEIDENTIFIER = (SELECT TOP 1 exam_id FROM Exams WHERE class_id = @Class2);
DECLARE @Exam2 UNIQUEIDENTIFIER = (SELECT TOP 1 exam_id FROM Exams WHERE class_id = @Class7);
DECLARE @Exam3 UNIQUEIDENTIFIER = (SELECT TOP 1 exam_id FROM Exams WHERE class_id = @Class3);

INSERT INTO ExamParts (exam_id, part_name, max_score, passing_score, weightage) VALUES
-- Exam 1 Parts (Class 2 Midterm)
(@Exam1, N'Listening', 25, 12.5, 0.25),
(@Exam1, N'Reading', 25, 12.5, 0.25),
(@Exam1, N'Writing', 25, 12.5, 0.25),
(@Exam1, N'Speaking', 25, 12.5, 0.25),
-- Exam 2 Parts (Class 7 Final - Japanese)
(@Exam2, N'Vocabulary', 20, 12, 0.2),
(@Exam2, N'Grammar', 30, 18, 0.3),
(@Exam2, N'Reading', 30, 18, 0.3),
(@Exam2, N'Listening', 20, 12, 0.2),
-- Exam 3 Parts (Class 3 Midterm)
(@Exam3, N'Listening', 30, 15, 0.3),
(@Exam3, N'Reading & Use of English', 40, 20, 0.4),
(@Exam3, N'Writing', 30, 15, 0.3);

-- 14. INSERT Exam Results (for completed exams)
INSERT INTO ExamResults (exam_id, student_id, score)
SELECT @Exam2, e.student_id, 
    CASE 
        WHEN e.student_id = @Student2 THEN 78
        WHEN e.student_id = @Student3 THEN 85
        WHEN e.student_id = @Student4 THEN 72
        ELSE 75
    END AS score
FROM Enrollments e
WHERE e.class_id = @Class7;

-- 15. INSERT Exam Part Results
DECLARE @ExamResult1 UNIQUEIDENTIFIER = (SELECT TOP 1 exam_result_id FROM ExamResults WHERE student_id = @Student2 AND exam_id = @Exam2);
DECLARE @ExamResult2 UNIQUEIDENTIFIER = (SELECT TOP 1 exam_result_id FROM ExamResults WHERE student_id = @Student3 AND exam_id = @Exam2);
DECLARE @ExamResult3 UNIQUEIDENTIFIER = (SELECT TOP 1 exam_result_id FROM ExamResults WHERE student_id = @Student4 AND exam_id = @Exam2);

DECLARE @Part1 UNIQUEIDENTIFIER = (SELECT TOP 1 exam_part_id FROM ExamParts WHERE exam_id = @Exam2 AND part_name = N'Vocabulary');
DECLARE @Part2 UNIQUEIDENTIFIER = (SELECT TOP 1 exam_part_id FROM ExamParts WHERE exam_id = @Exam2 AND part_name = N'Grammar');
DECLARE @Part3 UNIQUEIDENTIFIER = (SELECT TOP 1 exam_part_id FROM ExamParts WHERE exam_id = @Exam2 AND part_name = N'Reading');
DECLARE @Part4 UNIQUEIDENTIFIER = (SELECT TOP 1 exam_part_id FROM ExamParts WHERE exam_id = @Exam2 AND part_name = N'Listening');

INSERT INTO ExamPartResults (exam_part_id, exam_result_id, score) VALUES
-- Student 2 scores (Total: 78)
(@Part1, @ExamResult1, 16.0),
(@Part2, @ExamResult1, 24.0),
(@Part3, @ExamResult1, 23.0),
(@Part4, @ExamResult1, 15.0),
-- Student 3 scores (Total: 85)
(@Part2, @ExamResult2, 18.0),
(@Part2, @ExamResult2, 27.0),
(@Part3, @ExamResult2, 25.0),
(@Part4, @ExamResult2, 15.0),
-- Student 4 scores (Total: 72)
(@Part1, @ExamResult3, 14.0),
(@Part2, @ExamResult3, 22.0),
(@Part3, @ExamResult3, 21.0),
(@Part4, @ExamResult3, 15.0);

-- 16. INSERT Certificates (for completed courses with good results)
INSERT INTO Certificates (student_id, course_id, issue_date, certificate_url, certificate_status)
SELECT 
    e.student_id,
    cl.course_id,
    DATEADD(DAY, 7, cl.end_date) AS issue_date,
    CONCAT('https://example.com/certificates/', LOWER(REPLACE(CAST(e.student_id AS NVARCHAR(36)), '-', '')), '.pdf') AS certificate_url,
    'issued' AS certificate_status
FROM Enrollments e
JOIN Classes cl ON e.class_id = cl.class_id
WHERE e.enrollment_status = 'completed'
AND cl.class_status = 'completed';

-- =============================================
-- VERIFICATION QUERIES (Optional - comment out if not needed)
-- =============================================

-- Verify data insertion
SELECT 'Languages' AS TableName, COUNT(*) AS RecordCount FROM Languages
UNION ALL
SELECT 'Languages_Levels', COUNT(*) FROM Languages_Levels
UNION ALL
SELECT 'Users', COUNT(*) FROM Users
UNION ALL
SELECT 'Teacher', COUNT(*) FROM Teacher
UNION ALL
SELECT 'Students', COUNT(*) FROM Students
UNION ALL
SELECT 'Courses', COUNT(*) FROM Courses
UNION ALL
SELECT 'Classes', COUNT(*) FROM Classes
UNION ALL
SELECT 'Enrollments', COUNT(*) FROM Enrollments
UNION ALL
SELECT 'Invoices', COUNT(*) FROM Invoices
UNION ALL
SELECT 'Schedules', COUNT(*) FROM Schedules
UNION ALL
SELECT 'Attendance', COUNT(*) FROM Attendance
UNION ALL
SELECT 'Exams', COUNT(*) FROM Exams
UNION ALL
SELECT 'ExamParts', COUNT(*) FROM ExamParts
UNION ALL
SELECT 'ExamResults', COUNT(*) FROM ExamResults
UNION ALL
SELECT 'ExamPartResults', COUNT(*) FROM ExamPartResults
UNION ALL
SELECT 'Certificates', COUNT(*) FROM Certificates;
