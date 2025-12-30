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
    max_score INT NOT NULL,
    passing_score INT NOT NULL,
    weightage FLOAT CHECK (weightage >= 0 AND weightage <= 1) DEFAULT 0,
    CONSTRAINT FK_Exams_Classes FOREIGN KEY (class_id) REFERENCES Classes (class_id) ON DELETE CASCADE
)

CREATE TABLE ExamParts (
    exam_part_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    exam_id UNIQUEIDENTIFIER NOT NULL,
    part_name NVARCHAR (100) NOT NULL,
    max_score INT NOT NULL,
    passing_score INT NOT NULL,
    weightage FLOAT NOT NULL,
    CONSTRAINT FK_ExamParts_Exams FOREIGN KEY (exam_id) REFERENCES Exams (exam_id) ON DELETE CASCADE
); 

GO
CREATE TABLE ExamResults (
    exam_result_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    exam_id UNIQUEIDENTIFIER NOT NULL,
    student_id UNIQUEIDENTIFIER NOT NULL,
    score INT NOT NULL,
    CONSTRAINT FK_ExamResults_Exams FOREIGN KEY (exam_id) REFERENCES Exams (exam_id) ON DELETE CASCADE,
    CONSTRAINT FK_ExamResults_Students FOREIGN KEY (student_id) REFERENCES Students (student_id)
);


GO 
CREATE TABLE ExamPartResults (
    exam_part_result_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    exam_part_id UNIQUEIDENTIFIER NOT NULL,
    exam_result_id UNIQUEIDENTIFIER NOT NULL,
    score DECIMAL(10, 2) NOT NULL,

    CONSTRAINT FK_ExamPartResults_ExamParts FOREIGN KEY (exam_part_id) REFERENCES ExamParts (exam_part_id) ON DELETE CASCADE,
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


-- =============================================
-- INSERT SAMPLE DATA FOR LANGUAGE LEARNING SYSTEM
-- =============================================

-- 1. Languages
INSERT INTO Languages (language_code, language_name, is_active) VALUES
('EN', N'English', 1),
('VI', N'Vietnamese', 1),
('JP', N'Japanese', 1),
('KR', N'Korean', 1),
('CN', N'Chinese', 1);

-- 2. Languages_Levels
INSERT INTO Languages_Levels (language_code, level_name, level_order, description, is_active) VALUES
('EN', N'Beginner A1', 1, N'Basic English for beginners', 1),
('EN', N'Elementary A2', 2, N'Elementary level English', 1),
('EN', N'Intermediate B1', 3, N'Intermediate English', 1),
('EN', N'Upper Intermediate B2', 4, N'Upper intermediate English', 1),
('EN', N'Advanced C1', 5, N'Advanced English proficiency', 1),
('JP', N'N5 - Beginner', 1, N'JLPT N5 Level', 1),
('JP', N'N4 - Elementary', 2, N'JLPT N4 Level', 1),
('JP', N'N3 - Intermediate', 3, N'JLPT N3 Level', 1),
('KR', N'TOPIK I Level 1', 1, N'Korean beginner level 1', 1),
('KR', N'TOPIK I Level 2', 2, N'Korean beginner level 2', 1);

-- 3. Users (Admin, Teachers, Students)
INSERT INTO Users (username, password_hash, email, phone, full_name, date_of_birth, gender, address, role, is_active) VALUES
-- Admin
('admin01', 'hash_admin_password', 'admin@languagecenter.com', '0901234567', N'Nguyễn Văn Admin', '1985-05-15', 'male', N'123 Lê Lợi, Q1, TP.HCM', 'admin', 1),

-- Teachers
('teacher01', 'hash_teacher1_pass', 'john.smith@languagecenter.com', '0912345678', N'John Smith', '1988-03-20', 'male', N'456 Nguyễn Huệ, Q1, TP.HCM', 'teacher', 1),
('teacher02', 'hash_teacher2_pass', 'sarah.nguyen@languagecenter.com', '0923456789', N'Sarah Nguyễn', '1990-07-12', 'female', N'789 Trần Hưng Đạo, Q5, TP.HCM', 'teacher', 1),
('teacher03', 'hash_teacher3_pass', 'tanaka.yuki@languagecenter.com', '0934567890', N'Tanaka Yuki', '1987-11-08', 'female', N'321 Võ Văn Tần, Q3, TP.HCM', 'teacher', 1),
('teacher04', 'hash_teacher4_pass', 'kim.minji@languagecenter.com', '0945678901', N'Kim Min-ji', '1992-02-25', 'female', N'654 Pasteur, Q1, TP.HCM', 'teacher', 1),

-- Students
('student01', 'hash_student1_pass', 'nguyenvana@email.com', '0956789012', N'Nguyễn Văn A', '2000-01-15', 'male', N'12 Lý Thường Kiệt, Q10, TP.HCM', 'student', 1),
('student02', 'hash_student2_pass', 'tranthib@email.com', '0967890123', N'Trần Thị B', '2001-05-20', 'female', N'34 Hai Bà Trưng, Q3, TP.HCM', 'student', 1),
('student03', 'hash_student3_pass', 'lequangc@email.com', '0978901234', N'Lê Quang C', '1999-08-10', 'male', N'56 Nguyễn Thị Minh Khai, Q1, TP.HCM', 'student', 1),
('student04', 'hash_student4_pass', 'phamthid@email.com', '0989012345', N'Phạm Thị D', '2002-03-25', 'female', N'78 Điện Biên Phủ, Bình Thạnh, TP.HCM', 'student', 1),
('student05', 'hash_student5_pass', 'hoangvane@email.com', '0990123456', N'Hoàng Văn E', '2000-12-05', 'male', N'90 Cách Mạng Tháng 8, Q3, TP.HCM', 'student', 1),
('student06', 'hash_student6_pass', 'vuthif@email.com', '0901234568', N'Vũ Thị F', '2001-09-18', 'female', N'111 Lê Văn Sỹ, Q3, TP.HCM', 'student', 1),
('student07', 'hash_student7_pass', 'doanvang@email.com', '0912345679', N'Đoàn Văn G', '1998-06-30', 'male', N'222 Hoàng Văn Thụ, Tân Bình, TP.HCM', 'student', 1),
('student08', 'hash_student8_pass', 'buithih@email.com', '0923456780', N'Bùi Thị H', '2003-04-12', 'female', N'333 Phạm Văn Đồng, Thủ Đức, TP.HCM', 'student', 1);

-- 4. Teacher
DECLARE @teacher1_user_id UNIQUEIDENTIFIER = (SELECT user_id FROM Users WHERE username = 'teacher01');
DECLARE @teacher2_user_id UNIQUEIDENTIFIER = (SELECT user_id FROM Users WHERE username = 'teacher02');
DECLARE @teacher3_user_id UNIQUEIDENTIFIER = (SELECT user_id FROM Users WHERE username = 'teacher03');
DECLARE @teacher4_user_id UNIQUEIDENTIFIER = (SELECT user_id FROM Users WHERE username = 'teacher04');

INSERT INTO Teacher (user_id, specialization, years_of_experience, qualifications, bio, is_active) VALUES
(@teacher1_user_id, N'English Communication', 8, N'TESOL Certificate, MA in Applied Linguistics', N'Experienced English teacher with focus on business communication', 1),
(@teacher2_user_id, N'English Grammar & Writing', 6, N'CELTA, BA in English Literature', N'Passionate about teaching English grammar and academic writing', 1),
(@teacher3_user_id, N'Japanese Language & Culture', 10, N'JLPT N1, MA in Japanese Studies', N'Native Japanese speaker with extensive teaching experience', 1),
(@teacher4_user_id, N'Korean Language', 5, N'TOPIK Level 6, BA in Korean Language Education', N'Korean language specialist focusing on conversational skills', 1);

-- 5. Students
DECLARE @student1_user_id UNIQUEIDENTIFIER = (SELECT user_id FROM Users WHERE username = 'student01');
DECLARE @student2_user_id UNIQUEIDENTIFIER = (SELECT user_id FROM Users WHERE username = 'student02');
DECLARE @student3_user_id UNIQUEIDENTIFIER = (SELECT user_id FROM Users WHERE username = 'student03');
DECLARE @student4_user_id UNIQUEIDENTIFIER = (SELECT user_id FROM Users WHERE username = 'student04');
DECLARE @student5_user_id UNIQUEIDENTIFIER = (SELECT user_id FROM Users WHERE username = 'student05');
DECLARE @student6_user_id UNIQUEIDENTIFIER = (SELECT user_id FROM Users WHERE username = 'student06');
DECLARE @student7_user_id UNIQUEIDENTIFIER = (SELECT user_id FROM Users WHERE username = 'student07');
DECLARE @student8_user_id UNIQUEIDENTIFIER = (SELECT user_id FROM Users WHERE username = 'student08');

INSERT INTO Students (user_id, is_active) VALUES
(@student1_user_id, 1),
(@student2_user_id, 1),
(@student3_user_id, 1),
(@student4_user_id, 1),
(@student5_user_id, 1),
(@student6_user_id, 1),
(@student7_user_id, 1),
(@student8_user_id, 1);

-- 6. Courses
DECLARE @en_a1_level UNIQUEIDENTIFIER = (SELECT language_level_id FROM Languages_Levels WHERE language_code = 'EN' AND level_order = 1);
DECLARE @en_a2_level UNIQUEIDENTIFIER = (SELECT language_level_id FROM Languages_Levels WHERE language_code = 'EN' AND level_order = 2);
DECLARE @en_b1_level UNIQUEIDENTIFIER = (SELECT language_level_id FROM Languages_Levels WHERE language_code = 'EN' AND level_order = 3);
DECLARE @jp_n5_level UNIQUEIDENTIFIER = (SELECT language_level_id FROM Languages_Levels WHERE language_code = 'JP' AND level_order = 1);
DECLARE @kr_t1_level UNIQUEIDENTIFIER = (SELECT language_level_id FROM Languages_Levels WHERE language_code = 'KR' AND level_order = 1);

INSERT INTO Courses (course_name, language_level_id, description, duration_hours, fee, course_status) VALUES
(N'English for Beginners A1', @en_a1_level, N'Basic English course for absolute beginners', 60, 3000000, 'active'),
(N'English Elementary A2', @en_a2_level, N'Elementary English with focus on daily communication', 80, 3500000, 'active'),
(N'English Intermediate B1', @en_b1_level, N'Intermediate English for work and study', 100, 4500000, 'active'),
(N'Japanese N5 Foundation', @jp_n5_level, N'JLPT N5 preparation course', 90, 5000000, 'active'),
(N'Korean TOPIK I Level 1', @kr_t1_level, N'Korean language basics for beginners', 70, 4000000, 'active');

-- 7. Classes
DECLARE @course1_id UNIQUEIDENTIFIER = (SELECT TOP 1 course_id FROM Courses WHERE course_name LIKE N'%Beginners A1%');
DECLARE @course2_id UNIQUEIDENTIFIER = (SELECT TOP 1 course_id FROM Courses WHERE course_name LIKE N'%Elementary A2%');
DECLARE @course3_id UNIQUEIDENTIFIER = (SELECT TOP 1 course_id FROM Courses WHERE course_name LIKE N'%Intermediate B1%');
DECLARE @course4_id UNIQUEIDENTIFIER = (SELECT TOP 1 course_id FROM Courses WHERE course_name LIKE N'%Japanese N5%');
DECLARE @course5_id UNIQUEIDENTIFIER = (SELECT TOP 1 course_id FROM Courses WHERE course_name LIKE N'%Korean TOPIK%');

DECLARE @teacher1_id UNIQUEIDENTIFIER = (SELECT teacher_id FROM Teacher WHERE user_id = @teacher1_user_id);
DECLARE @teacher2_id UNIQUEIDENTIFIER = (SELECT teacher_id FROM Teacher WHERE user_id = @teacher2_user_id);
DECLARE @teacher3_id UNIQUEIDENTIFIER = (SELECT teacher_id FROM Teacher WHERE user_id = @teacher3_user_id);
DECLARE @teacher4_id UNIQUEIDENTIFIER = (SELECT teacher_id FROM Teacher WHERE user_id = @teacher4_user_id);

INSERT INTO Classes (course_id, teacher_id, class_name, start_date, end_date, max_students, class_status) VALUES
(@course1_id, @teacher1_id, N'ENG-A1-2024-01', '2024-01-15', '2024-04-15', 20, 'completed'),
(@course1_id, @teacher1_id, N'ENG-A1-2024-02', '2024-09-01', '2024-12-01', 20, 'ongoing'),
(@course2_id, @teacher2_id, N'ENG-A2-2024-01', '2024-02-01', '2024-06-01', 18, 'completed'),
(@course3_id, @teacher2_id, N'ENG-B1-2024-01', '2024-10-01', '2025-02-28', 15, 'ongoing'),
(@course4_id, @teacher3_id, N'JPN-N5-2024-01', '2024-03-01', '2024-07-31', 15, 'completed'),
(@course5_id, @teacher4_id, N'KOR-T1-2024-01', '2024-11-01', '2025-02-15', 12, 'ongoing');

-- 8. Enrollments
DECLARE @class1_id UNIQUEIDENTIFIER = (SELECT TOP 1 class_id FROM Classes WHERE class_name = N'ENG-A1-2024-01');
DECLARE @class2_id UNIQUEIDENTIFIER = (SELECT TOP 1 class_id FROM Classes WHERE class_name = N'ENG-A1-2024-02');
DECLARE @class3_id UNIQUEIDENTIFIER = (SELECT TOP 1 class_id FROM Classes WHERE class_name = N'ENG-A2-2024-01');
DECLARE @class4_id UNIQUEIDENTIFIER = (SELECT TOP 1 class_id FROM Classes WHERE class_name = N'ENG-B1-2024-01');
DECLARE @class5_id UNIQUEIDENTIFIER = (SELECT TOP 1 class_id FROM Classes WHERE class_name = N'JPN-N5-2024-01');
DECLARE @class6_id UNIQUEIDENTIFIER = (SELECT TOP 1 class_id FROM Classes WHERE class_name = N'KOR-T1-2024-01');

DECLARE @student1_id UNIQUEIDENTIFIER = (SELECT student_id FROM Students WHERE user_id = @student1_user_id);
DECLARE @student2_id UNIQUEIDENTIFIER = (SELECT student_id FROM Students WHERE user_id = @student2_user_id);
DECLARE @student3_id UNIQUEIDENTIFIER = (SELECT student_id FROM Students WHERE user_id = @student3_user_id);
DECLARE @student4_id UNIQUEIDENTIFIER = (SELECT student_id FROM Students WHERE user_id = @student4_user_id);
DECLARE @student5_id UNIQUEIDENTIFIER = (SELECT student_id FROM Students WHERE user_id = @student5_user_id);
DECLARE @student6_id UNIQUEIDENTIFIER = (SELECT student_id FROM Students WHERE user_id = @student6_user_id);
DECLARE @student7_id UNIQUEIDENTIFIER = (SELECT student_id FROM Students WHERE user_id = @student7_user_id);
DECLARE @student8_id UNIQUEIDENTIFIER = (SELECT student_id FROM Students WHERE user_id = @student8_user_id);

INSERT INTO Enrollments (student_id, class_id, enrollment_date, enrollment_status) VALUES
-- Class 1: ENG-A1-2024-01 (completed)
(@student1_id, @class1_id, '2024-01-10', 'completed'),
(@student2_id, @class1_id, '2024-01-10', 'completed'),
(@student3_id, @class1_id, '2024-01-12', 'completed'),

-- Class 2: ENG-A1-2024-02 (ongoing)
(@student4_id, @class2_id, '2024-08-25', 'active'),
(@student5_id, @class2_id, '2024-08-26', 'active'),
(@student6_id, @class2_id, '2024-08-28', 'active'),

-- Class 3: ENG-A2-2024-01 (completed)
(@student1_id, @class3_id, '2024-01-28', 'completed'),
(@student7_id, @class3_id, '2024-01-29', 'completed'),

-- Class 4: ENG-B1-2024-01 (ongoing)
(@student2_id, @class4_id, '2024-09-25', 'active'),
(@student8_id, @class4_id, '2024-09-26', 'active'),

-- Class 5: JPN-N5-2024-01 (completed)
(@student3_id, @class5_id, '2024-02-25', 'completed'),
(@student4_id, @class5_id, '2024-02-26', 'completed'),

-- Class 6: KOR-T1-2024-01 (ongoing)
(@student5_id, @class6_id, '2024-10-28', 'active'),
(@student6_id, @class6_id, '2024-10-29', 'active');

-- 9. Invoices
DECLARE @enroll1_id UNIQUEIDENTIFIER = (SELECT TOP 1 enrollment_id FROM Enrollments WHERE student_id = @student1_id AND class_id = @class1_id);
DECLARE @enroll2_id UNIQUEIDENTIFIER = (SELECT TOP 1 enrollment_id FROM Enrollments WHERE student_id = @student2_id AND class_id = @class1_id);
DECLARE @enroll3_id UNIQUEIDENTIFIER = (SELECT TOP 1 enrollment_id FROM Enrollments WHERE student_id = @student3_id AND class_id = @class1_id);
DECLARE @enroll4_id UNIQUEIDENTIFIER = (SELECT TOP 1 enrollment_id FROM Enrollments WHERE student_id = @student4_id AND class_id = @class2_id);
DECLARE @enroll5_id UNIQUEIDENTIFIER = (SELECT TOP 1 enrollment_id FROM Enrollments WHERE student_id = @student5_id AND class_id = @class2_id);

INSERT INTO Invoices (enrollment_id, amount, issue_date, due_date, payment_date, invoice_status) VALUES
(@enroll1_id, 3000000, '2024-01-10', '2024-01-20', '2024-01-15', 'paid'),
(@enroll2_id, 3000000, '2024-01-10', '2024-01-20', '2024-01-18', 'paid'),
(@enroll3_id, 3000000, '2024-01-12', '2024-01-22', '2024-01-20', 'paid'),
(@enroll4_id, 3000000, '2024-08-25', '2024-09-05', '2024-08-30', 'paid'),
(@enroll5_id, 3000000, '2024-08-26', '2024-09-06', NULL, 'unpaid');

-- 10. Schedules
INSERT INTO Schedules (class_id, study_date, start_time, end_time, room) VALUES
-- Class 1: ENG-A1-2024-01 (completed - sample dates)
(@class1_id, '2024-01-15', '18:00', '20:00', N'Room 101'),
(@class1_id, '2024-01-17', '18:00', '20:00', N'Room 101'),
(@class1_id, '2024-01-22', '18:00', '20:00', N'Room 101'),

-- Class 2: ENG-A1-2024-02 (ongoing)
(@class2_id, '2024-09-02', '18:00', '20:00', N'Room 102'),
(@class2_id, '2024-09-04', '18:00', '20:00', N'Room 102'),
(@class2_id, '2024-09-09', '18:00', '20:00', N'Room 102'),
(@class2_id, '2024-12-23', '18:00', '20:00', N'Room 102'),
(@class2_id, '2024-12-25', '18:00', '20:00', N'Room 102'),

-- Class 4: ENG-B1-2024-01 (ongoing)
(@class4_id, '2024-10-02', '19:00', '21:00', N'Room 201'),
(@class4_id, '2024-10-07', '19:00', '21:00', N'Room 201');

-- 11. Attendance
DECLARE @schedule1_id UNIQUEIDENTIFIER = (SELECT TOP 1 schedule_id FROM Schedules WHERE class_id = @class1_id AND study_date = '2024-01-15');
DECLARE @schedule2_id UNIQUEIDENTIFIER = (SELECT TOP 1 schedule_id FROM Schedules WHERE class_id = @class1_id AND study_date = '2024-01-17');
DECLARE @schedule3_id UNIQUEIDENTIFIER = (SELECT TOP 1 schedule_id FROM Schedules WHERE class_id = @class2_id AND study_date = '2024-09-02');

INSERT INTO Attendance (schedule_id, student_id, attendance_status) VALUES
-- Schedule 1
(@schedule1_id, @student1_id, 'present'),
(@schedule1_id, @student2_id, 'present'),
(@schedule1_id, @student3_id, 'late'),

-- Schedule 2
(@schedule2_id, @student1_id, 'present'),
(@schedule2_id, @student2_id, 'absent'),
(@schedule2_id, @student3_id, 'present'),

-- Schedule 3
(@schedule3_id, @student4_id, 'present'),
(@schedule3_id, @student5_id, 'present'),
(@schedule3_id, @student6_id, 'permittedAbsence');

-- 12. Exams
INSERT INTO Exams (class_id, exam_date, start_time, end_time, room, max_score, passing_score) VALUES
-- Midterm exam for completed class
(@class1_id, '2024-02-20', '09:00', '11:00', N'Exam Room A', 100, 50),
-- Final exam for completed class
(@class1_id, '2024-04-10', '09:00', '11:30', N'Exam Room A', 100, 50),
-- Midterm for ongoing class
(@class2_id, '2024-10-15', '09:00', '11:00', N'Exam Room B', 100, 50);

-- 13. ExamParts
DECLARE @exam1_id UNIQUEIDENTIFIER = (SELECT TOP 1 exam_id FROM Exams WHERE class_id = @class1_id AND exam_date = '2024-02-20');
DECLARE @exam2_id UNIQUEIDENTIFIER = (SELECT TOP 1 exam_id FROM Exams WHERE class_id = @class1_id AND exam_date = '2024-04-10');
DECLARE @exam3_id UNIQUEIDENTIFIER = (SELECT TOP 1 exam_id FROM Exams WHERE class_id = @class2_id AND exam_date = '2024-10-15');

INSERT INTO ExamParts (exam_id, part_name, max_score, passing_score, weightage) VALUES
-- Exam 1: Midterm
(@exam1_id, N'Listening', 25, 12, 0.25),
(@exam1_id, N'Reading', 25, 12, 0.25),
(@exam1_id, N'Writing', 25, 12, 0.25),
(@exam1_id, N'Speaking', 25, 12, 0.25),

-- Exam 2: Final
(@exam2_id, N'Listening', 30, 15, 0.30),
(@exam2_id, N'Reading', 30, 15, 0.30),
(@exam2_id, N'Writing', 20, 10, 0.20),
(@exam2_id, N'Speaking', 20, 10, 0.20),

-- Exam 3: Midterm ongoing
(@exam3_id, N'Listening', 25, 12, 0.25),
(@exam3_id, N'Reading', 25, 12, 0.25),
(@exam3_id, N'Writing', 25, 12, 0.25),
(@exam3_id, N'Speaking', 25, 12, 0.25);

-- 14. ExamResults
INSERT INTO ExamResults (exam_id, student_id, score) VALUES
-- Exam 1 results
(@exam1_id, @student1_id, 75),
(@exam1_id, @student2_id, 82),
(@exam1_id, @student3_id, 68),

-- Exam 2 results
(@exam2_id, @student1_id, 85),
(@exam2_id, @student2_id, 78),
(@exam2_id, @student3_id, 72);

-- 15. ExamPartResults
DECLARE @examres1_id UNIQUEIDENTIFIER = (SELECT TOP 1 exam_result_id FROM ExamResults WHERE exam_id = @exam1_id AND student_id = @student1_id);
DECLARE @examres2_id UNIQUEIDENTIFIER = (SELECT TOP 1 exam_result_id FROM ExamResults WHERE exam_id = @exam1_id AND student_id = @student2_id);
DECLARE @examres3_id UNIQUEIDENTIFIER = (SELECT TOP 1 exam_result_id FROM ExamResults WHERE exam_id = @exam1_id AND student_id = @student3_id);

DECLARE @exampart1_id UNIQUEIDENTIFIER = (SELECT TOP 1 exam_part_id FROM ExamParts WHERE exam_id = @exam1_id AND part_name = N'Listening');
DECLARE @exampart2_id UNIQUEIDENTIFIER = (SELECT TOP 1 exam_part_id FROM ExamParts WHERE exam_id = @exam1_id AND part_name = N'Reading');
DECLARE @exampart3_id UNIQUEIDENTIFIER = (SELECT TOP 1 exam_part_id FROM ExamParts WHERE exam_id = @exam1_id AND part_name = N'Writing');
DECLARE @exampart4_id UNIQUEIDENTIFIER = (SELECT TOP 1 exam_part_id FROM ExamParts WHERE exam_id = @exam1_id AND part_name = N'Speaking');

INSERT INTO ExamPartResults (exam_part_id, exam_result_id, score) VALUES
-- Student 1 (Total: 75)
(@exampart1_id, @examres1_id, 20),
(@exampart2_id, @examres1_id, 18),
(@exampart3_id, @examres1_id, 19),
(@exampart4_id, @examres1_id, 18),

-- Student 2 (Total: 82)
(@exampart1_id, @examres2_id, 22),
(@exampart2_id, @examres2_id, 20),
(@exampart3_id, @examres2_id, 21),
(@exampart4_id, @examres2_id, 19),

-- Student 3 (Total: 68)
(@exampart1_id, @examres3_id, 17),
(@exampart2_id, @examres3_id, 16),
(@exampart3_id, @examres3_id, 18),
(@exampart4_id, @examres3_id, 17);

-- 16. Certificates
INSERT INTO Certificates (student_id, course_id, issue_date, certificate_url, certificate_status) VALUES
(@student1_id, @course1_id, '2024-04-20', N'https://certificates.languagecenter.com/cert_001.pdf', 'issued'),
(@student2_id, @course1_id, '2024-04-20', N'https://certificates.languagecenter.com/cert_002.pdf', 'issued'),
(@student3_id, @course1_id, '2024-04-20', N'https://certificates.languagecenter.com/cert_003.pdf', 'issued'),
(@student1_id, @course2_id, '2024-06-10', N'https://certificates.languagecenter.com/cert_004.pdf', 'issued'),
(@student7_id, @course2_id, '2024-06-10', N'https://certificates.languagecenter.com/cert_005.pdf', 'issued');

PRINT '✅ Sample data inserted successfully!';
PRINT '📊 Summary:';
PRINT '   - 5 Languages';
PRINT '   - 10 Language Levels';
PRINT '   - 13 Users (1 Admin, 4 Teachers, 8 Students)';
PRINT '   - 5 Courses';
PRINT '   - 6 Classes';
PRINT '   - 14 Enrollments';
PRINT '   - 5 Invoices';
PRINT '   - 10 Schedules';
PRINT '   - 9 Attendance Records';
PRINT '   - 3 Exams';
PRINT '   - 12 Exam Parts';
PRINT '   - 6 Exam Results';
PRINT '   - 18 Exam Part Results';
PRINT '   - 5 Certificates';