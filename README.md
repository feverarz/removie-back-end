## 🎬 Removie Back-End

**Removie** is a secure and scalable .NET Web API that powers a movie-based application with user authentication, token refresh logic, and clean RESTful endpoints. Built with performance and maintainability in mind, this backend serves as the foundation for a fullstack movie experience.

---

### 🚀 Technologies Used

- **.NET 8 Web API**
- **Entity Framework Core**
- **PostgreSQL**
- **JWT Authentication**
- **In-Memory & Response Caching**
- **Cookie-based Refresh Token Flow**
- **Railway Deployment**

---

### 📦 Features

- 🔐 **Secure Authentication**  
  Login, register, and logout endpoints with hashed passwords and JWT access tokens.

- 🍪 **Refresh Token via HttpOnly Cookie**  
  Refresh logic using secure cookies with `SameSite=Strict`, `Secure`, and `HttpOnly` flags.

- 🎥 **Movie Management**  
  CRUD operations for movies, genres, and user favorites.

- 🧠 **Role-Based Access Control**  
  Admin-only endpoints for managing content.

- ⚡ **Performance-Oriented**  
  Includes caching strategies and async operations for optimized response times.

- 📄 **Clean Architecture**  
  Separation of concerns with DTOs, services, repositories, and middleware.

---

### 📁 Project Structure

```
Removie.Backend/
├── Controllers/
├── Services/
├── Models/
├── DTOs/
├── Middleware/
├── Persistence/
├── Program.cs
└── README.md
```

---

### 🧪 How to Run Locally

1. **Clone the repository**

```bash
git clone https://github.com/feverarz/removie-back-end.git
cd removie-back-end
```

2. **Set up PostgreSQL connection string**

Update `appsettings.json` or use environment variables:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=removie;Username=youruser;Password=yourpassword"
}
```

3. **Run migrations**

```bash
dotnet ef database update
```

4. **Start the API**

```bash
dotnet run
```

---

### 🔄 Refresh Token Flow

- On login/register:  
  `SetRefreshCookie(token)` stores the refresh token securely.

- On logout:  
  Cookie is deleted via `Response.Cookies.Delete("refreshToken")`.

- On token expiration:  
  `RefreshService` validates the cookie and issues a new access token.

---

### 📌 Endpoints Overview

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/login` | Authenticates user and sets refresh cookie |
| `POST` | `/register` | Creates new user |
| `POST` | `/refresh` | Issues new access token |
| `POST` | `/logout` | Deletes refresh cookie |
| `GET` | `/movies` | Lists all movies |
| `POST` | `/movies` | Adds a new movie (admin only) |

---

### 🌐 Deployment

⚠️ This project is currently in active development. Core features are functional, but the codebase, documentation, and database tooling are still evolving. Contributions, feedback, and testing are welcome as the platform continues to grow.


---

### 🙋‍♂️ Author

**Federico Vera**  
Backend Developer | .NET Specialist | Fullstack Integrator  
GitHub: [@feverarz](https://github.com/feverarz)

---
