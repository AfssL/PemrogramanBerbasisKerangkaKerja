CREATE DATABASE StudentDB;
GO

USE StudentDB;
GO

CREATE TABLE Students (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NIM VARCHAR(20) NOT NULL,
    Nama VARCHAR(100) NOT NULL,
    Jurusan VARCHAR(100) NOT NULL,
    Gender VARCHAR(20) NOT NULL,
    Email VARCHAR(100)
);

INSERT INTO Students (NIM, Nama, Jurusan, Gender, Email) VALUES
('5025241001', 'Afsal Murtaza', 'Informatika', 'Laki-laki', 'afsal@gmail.com'),
('5025241002', 'Abrorus Shobah', 'Sistem Informasi', 'Laki-laki', 'abror@gmail.com');