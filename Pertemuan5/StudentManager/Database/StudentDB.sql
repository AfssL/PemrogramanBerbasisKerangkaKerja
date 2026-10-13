CREATE DATABASE StudentDB;
GO

USE StudentDB;
GO

CREATE TABLE Students
(
    Id       INT IDENTITY(1,1) PRIMARY KEY,
    NIM      VARCHAR(20)  NOT NULL,
    Nama     VARCHAR(100) NOT NULL,
    Jurusan  VARCHAR(100) NOT NULL,
    Gender   VARCHAR(20)  NOT NULL,
    Email    VARCHAR(100)
);
GO

INSERT INTO Students (NIM, Nama, Jurusan, Gender, Email)
VALUES
('01001', 'Afsal Murtaza', 'Teknik Informatika',  'Laki-laki', 'afsal@gmail.com'),
('02001', 'M. Ridho Utomo',  'Sistem Informasi',  'Laki-laki', 'mario@gmail.com');
GO

SELECT * FROM Students;
GO