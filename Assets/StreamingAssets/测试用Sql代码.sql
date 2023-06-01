CREATE TABLE IF NOT EXISTS Teachers (
    teacher_id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL,
    email TEXT NOT NULL,
    password TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Courses (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL,
    description TEXT NOT NULL,
    teacherId INTEGER NOT NULL,
    FOREIGN KEY (teacherId) REFERENCES Teachers(teacher_id)
);

INSERT INTO Teachers (name, email, password)
VALUES ('张三', 'zhangsan@email.com', 'password123'),
       ('李四', 'lisi@email.com', 'password456');

INSERT INTO Courses (name, description, teacherId)
VALUES ('数学101', '代数入门', 1),
       ('英语101', '文学入门', 2),
       ('科学101', '生物学入门', 1);
