# 🎓 Course Management System

A complete and scalable **Course Management System** built using **ASP.NET Core** and **Entity Framework Core**, designed to facilitate structured online learning with support for **courses, modules, lessons, quizzes, and progress tracking**.

## 📌 Features

### ✅ Authentication & Authorization
- JWT-based authentication
- Role-based access control (Admin, Instructor, Student)
- Secure login and registration
- Email confirmation and password reset functionality

### 🎓 Course Structure
- **Courses** contain multiple modules
- **Modules** contain multiple lessons
- **Lessons** support:
  - 📹 Videos (VideoUrl)
  - 📄 Documents (HTML content)
  - ❓ Quizzes (linked to lesson)

### 📝 Quizzes
- Instructors can create quizzes for any lesson
- Students can attempt each quiz only once
- Each attempt is scored and saved
- Attempt history available for instructors (includes:
  - Student Name
  - Question
  - Student Answer
  - Correct Answer)

### 🧠 Student Progress
- Students can mark lessons as completed
- View completed lessons per module
- Easy tracking of progress through each course

## 🧱 Tech Stack

- **Backend:** ASP.NET Core (.NET 8)
- **ORM:** Entity Framework Core
- **Database:** SQL Server (can be swapped with any EF-supported provider)
- **Authentication:** Identity + JWT
- **Architecture:** Clean separation of concerns using:
  - Repository Pattern
  - DTOs
  - AutoMapper
  - 
## 📂 Project Structure

```
├── Controllers
├── Data
├── Helper
├── Mappings
├── Middlewares
├── Migrations
├── Models
│   ├── Domain
│   └── DTO
├── Repositories
│   ├── Implementation
│   └── Interfaces
├── Videos
├── appsettings.json
├── Program.cs
```

## 🚀 How to Run

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/course-management-system.git
   ```

2. Set up your database connection in `appsettings.json`

3. Run EF Core migrations:
   ```bash
   dotnet ef database update
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

## 🙌 Author

Built with ❤️ by **Hossam**  
