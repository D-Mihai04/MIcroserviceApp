# Energy Management System - Distributed Systems (Assignment 1)

**Technical University of Cluj-Napoca (UTCN)**  
**Course**: Distributed Systems  
**Assignment**: Assignment 1 – Request-Reply Communication Paradigm  

---

## 1. Project Overview

A microservices-based Energy Management System deployed with Docker-based containerization. The system enables authenticated users to manage and monitor smart energy metering devices through a role-based web interface, reverse proxy, and API gateway.

### Core Architecture
- **API Gateway & Reverse Proxy**: Traefik v3.1 routing requests, handling CORS, and validating tokens via `ForwardAuth`.
- **Authorization Service (`DS.AuthService`)**: Issues JWT tokens, manages BCrypt password hashing, and validates tokens for gateway requests.
- **User Management Service (`DS.UserService`)**: Handles CRUD operations on user accounts and cascades device unassignment.
- **Device Management Service (`DS.DeviceService`)**: Handles CRUD operations on metering devices and manages 1:N device-to-user associations.
- **Frontend Web Application**: React + Vite SPA served via Nginx with role-based access control (Admin & Client views).
- **Database**: PostgreSQL 16 Alpine container hosting three isolated databases (`AuthDb`, `UserDb`, `DeviceDb`).

---

## 2. Quick Start & Execution

### Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (running with Linux containers)

### Running the System
From the project root directory, run:
```bash
docker compose up -d --build
```

### Accessing the Application
- **Frontend Application**: [http://localhost/](http://localhost/)
- **Traefik Gateway Dashboard**: [http://localhost:8080/](http://localhost:8080/)
- **PostgreSQL Database**: `localhost:5434` (User: `postgres`, Password: `0000`)

---

## 3. Default Demo Accounts

| Role | Username / Email | Password | Access & Capabilities |
| :--- | :--- | :--- | :--- |
| **Administrator** | `admin` | `AdminPassword123!` | Full CRUD on Users & Devices, Assign Devices |
| **Client** | `john@example.com` | `Password123!` | View only assigned metering devices |
| **Client** | `Mihai` | `mihai` | View only assigned metering devices |

*You can also create new Client or Admin accounts at any time directly through the **Register** tab in the UI.*

---

## 4. Microservices & API Reference

All requests pass through the Traefik Gateway on port `80` with the `/api` prefix.

### Authentication Service (`/api/auth`)
| Method | Endpoint | Description | Auth Required |
| :--- | :--- | :--- | :---: |
| `POST` | `/api/auth/register` | Register new user (Admin/Client) and issue JWT | No |
| `POST` | `/api/auth/login` | Authenticate with username & password, return JWT | No |
| `GET` | `/api/auth/validate` | Internal endpoint for gateway ForwardAuth middleware | Bearer Token |
| `PATCH` | `/api/auth/user/{id}` | Update username or role in credentials table | Internal |
| `DELETE` | `/api/auth/user/{id}` | Delete user credentials | Internal |

### User Service (`/api/users`)
| Method | Endpoint | Description | Auth Required |
| :--- | :--- | :--- | :---: |
| `GET` | `/api/users` | List all registered users | Bearer Token |
| `GET` | `/api/users/{id}` | Get user by unique ID | Bearer Token |
| `POST` | `/api/users` | Create user profile (syncs to AuthService) | Bearer Token |
| `PATCH` | `/api/users/{id}` | Update user name/role (syncs to AuthService) | Bearer Token |
| `DELETE` | `/api/users/{id}` | Delete user (unassigns devices & deletes credentials) | Bearer Token |

### Device Service (`/api/devices`)
| Method | Endpoint | Description | Auth Required |
| :--- | :--- | :--- | :---: |
| `GET` | `/api/devices` | List all smart metering devices | Bearer Token |
| `GET` | `/api/devices/{id}` | Get device by ID | Bearer Token |
| `GET` | `/api/devices/user/{userId}`| Get all devices assigned to a specific user | Bearer Token |
| `POST` | `/api/devices` | Create new device (Name, max consumption) | Bearer Token |
| `PATCH` | `/api/devices/{id}` | Update device details | Bearer Token |
| `PATCH` | `/api/devices/{id}/assign/{userId}` | Assign device to a user (validates user existence) | Bearer Token |
| `PATCH` | `/api/devices/{id}/unassign` | Unassign device from user | Bearer Token |
| `DELETE` | `/api/devices/{id}` | Delete a metering device | Bearer Token |

---

## 5. Deliverables
- **UML Deployment Diagram**: [deployment_diagram.md](file:///c:/Users/Matei/Desktop/AN4/Sem1/DS/DS_Application/deployment_diagram.md)

