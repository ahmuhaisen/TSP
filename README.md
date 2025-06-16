# The Societies Portal (TSP) 🎓

A comprehensive university societies management system that empowers student communities through modern technology. Built with Angular and .NET, TSP streamlines society operations, event management, and member engagement with AI-powered insights.

## 🌟 Live Demo

**🔗 [Visit The Societies Portal](https://the-societies-portal.web.app)**

## 📋 Table of Contents

- [Features](#-features)
- [Technology Stack](#-technology-stack)
- [Architecture](#-architecture)
- [Getting Started](#-getting-started)
- [Project Structure](#-project-structure)
- [User Roles](#-user-roles)
- [API Documentation](#-api-documentation)
- [Contributing](#-contributing)
- [License](#-license)

## ✨ Features

### 🎯 Core Features
- **Multi-Role Authentication**: JWT-based authentication with role-based access control
- **Society Management**: Create, manage, and customize university societies
- **Event Management**: Comprehensive event planning, scheduling, and tracking
- **Membership System**: Streamlined membership requests and approval workflow
- **Real-time Notifications**: Live updates using SignalR
- **Resource Allocation**: Efficient management and allocation of university resources

### 🤖 AI-Powered Features
- **Feedback Analysis**: AI-powered sentiment analysis of event feedback
- **Smart Summaries**: Automatic generation of event feedback summaries
- **Topic Extraction**: Intelligent identification of recurring themes in feedback

### 📊 Analytics & Reporting
- **Dashboard Analytics**: Comprehensive insights and statistics
- **PDF Reports**: Generate detailed reports for events and societies
- **Chart Visualizations**: Interactive charts using Chart.js
- **Performance Metrics**: Track society and event performance

### 🔧 Advanced Features
- **QR Code Generation**: Easy event check-ins and information sharing
- **Email Notifications**: Automated email system for important updates
- **File Management**: Secure file upload and management system
- **Search & Filtering**: Advanced search capabilities across the platform
- **Mobile Responsive**: Fully responsive design for all devices

## 🛠 Technology Stack

### Frontend
- **Framework**: Angular 19
- **UI Library**: Ng-Zorro (Ant Design for Angular)
- **Styling**: Tailwind CSS
- **Charts**: Chart.js with ng2-charts
- **Icons**: Ant Design Icons
- **Authentication**: @auth0/angular-jwt
- **PDF Generation**: jsPDF with jsPDF-AutoTable
- **QR Codes**: angularx-qrcode

### Backend
- **Framework**: .NET 9
- **Database**: SQL Server with Entity Framework Core 9
- **Authentication**: ASP.NET Identity with JWT
- **Real-time**: SignalR
- **Background Jobs**: Quartz.NET
- **Validation**: FluentValidation
- **Documentation**: Swagger/OpenAPI

### AI & Machine Learning
- **AI Integration**: Microsoft.Extensions.AI with Ollama
- **Local LLM**: Phi3 model for feedback analysis
- **Sentiment Analysis**: Custom AI-powered sentiment classification

### DevOps & Deployment
- **Frontend Hosting**: Firebase Hosting
- **Backend**: Can be deployed to Azure, AWS, or any cloud provider
- **Database**: SQL Server (Azure SQL, AWS RDS, or on-premises)
- **CI/CD**: GitHub Actions ready

## 🏗 Architecture

TSP follows **Clean Architecture** principles with clear separation of concerns:

```
├── TSP.Domain/          # Core business entities and rules
├── TPS.Application/     # Use cases and business logic
├── TPS.Infrastructure/  # External concerns (DB, Email, AI)
├── TSP.WebAPI/         # API controllers and configuration
└── TSP.App/            # Angular frontend application
```

### Key Patterns
- **CQRS**: Command Query Responsibility Segregation with MediatR
- **Repository Pattern**: Data access abstraction
- **Domain Events**: Decoupled event handling
- **Background Processing**: Asynchronous job processing
- **Clean Architecture**: Dependency inversion and separation of concerns

## 🚀 Getting Started

### Prerequisites
- **.NET 9 SDK**
- **Node.js 18+**
- **SQL Server** (LocalDB for development)
- **Ollama** (for AI features)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-username/TSP.git
   cd TSP
   ```

2. **Backend Setup**
   ```bash
   # Navigate to API project
   cd TSP.WebAPI
   
   # Restore packages
   dotnet restore
   
   # Update database
   dotnet ef database update
   
   # Run the API
   dotnet run
   ```

3. **Frontend Setup**
   ```bash
   # Navigate to Angular app
   cd TSP.App
   
   # Install dependencies
   npm install
   
   # Start development server
   ng serve
   ```

4. **AI Setup (Optional)**
   ```bash
   # Install Ollama
   curl -fsSL https://ollama.ai/install.sh | sh
   
   # Pull the model
   ollama pull phi3
   ```

### Configuration

Update `appsettings.json` in `TSP.WebAPI`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your SQL Server connection string"
  },
  "Jwt": {
    "Key": "Your-Secret-Key",
    "Issuer": "TSP",
    "Audience": "TSP-Users"
  },
  "Email": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "Username": "your-email@gmail.com",
    "Password": "your-app-password"
  },
  "Ollama": {
    "DefaultModel": "phi3",
    "InnerClientUri": "http://localhost:11434"
  }
}
```

## 📁 Project Structure

### Backend Structure
```
TSP.WebAPI/
├── Controllers/          # API controllers
│   ├── AdminArea/       # Admin-specific endpoints
│   ├── StudentArea/     # Student-specific endpoints
│   └── SuperAdminArea/  # Super admin endpoints
├── Validation/          # Request validation
└── DependencyInjection.cs

TPS.Application/
├── Areas/               # Feature-based organization
│   ├── Authentication/ # Auth services
│   ├── Feedback/       # AI feedback analysis
│   └── Shared/         # Shared services
└── SignalR/            # Real-time hubs

TPS.Infrastructure/
├── Data/               # Entity Framework setup
├── Emailing/           # Email services
├── AiClient/           # AI integration
└── BackgroundJobs/     # Quartz jobs

TSP.Domain/
├── Entities/           # Core business entities
├── Enums/             # Domain enumerations
└── Events/            # Domain events
```

### Frontend Structure
```
TSP.App/src/app/
├── areas/                    # Feature modules
│   ├── authentication/      # Login/Register
│   ├── student-area/        # Student dashboard
│   ├── system-admin-area/   # Admin panel
│   ├── super-admin/         # Super admin controls
│   ├── public/             # Landing page
│   └── public-forms/       # Public forms
├── components/             # Shared components
├── common/                # Utilities and services
└── config/               # App configuration
```

## 👥 User Roles

### 🎓 Student
- Join societies and manage memberships
- Register for events and provide feedback
- View personal dashboard and activity history
- Access society-specific resources

### 👨‍🏫 Faculty Member (Society Advisor)
- Oversee assigned societies
- Approve events and membership requests
- Access society analytics and reports
- Manage society resources

### 🛡 System Admin
- Manage all societies and events
- User management and role assignment
- System-wide analytics and reporting
- Resource allocation oversight

### 👑 Super Admin
- Complete system administration
- Manage system configurations
- Access all administrative functions
- System maintenance and monitoring

## 📚 API Documentation

The API is fully documented with Swagger/OpenAPI. After running the backend:

- **Swagger UI**: `https://localhost:7000/swagger`
- **API Base URL**: `https://localhost:7000/api`

### Key Endpoints

```
Authentication
POST /api/authentication/login
POST /api/authentication/register

Societies
GET /api/societies
POST /api/societies
PUT /api/societies/{id}

Events
GET /api/events
POST /api/events
GET /api/events/{id}/feedback

Notifications
GET /api/notifications
WebSocket: /api/hubs/notifications
```

## 🔧 Development

### Adding New Features

1. **Domain First**: Add entities to `TSP.Domain/Entities`
2. **Application Layer**: Create commands/queries in `TPS.Application/Areas`
3. **Infrastructure**: Implement data access in `TPS.Infrastructure`
4. **API**: Add controllers in `TSP.WebAPI/Controllers`
5. **Frontend**: Create components in `TSP.App/src/app/areas`

### Database Migrations

```bash
# Add migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update
```

### Testing

```bash
# Backend tests
dotnet test

# Frontend tests
ng test
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License.

---

**Built with ❤️ for university communities**

For support or questions, please open an issue or contact the development team.
