
# 📅 Booking API with Payment Integration

![.NET Core](https://img.shields.io/badge/.NET%20Core-9.0-blue)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core-blueviolet)
![JWT](https://img.shields.io/badge/Auth-JWT-green)
![Payments](https://img.shields.io/badge/Payments-PIX%20%26%20PayPal-yellow)

This project is a robust Booking API designed to manage service appointments (e.g., barbershops, clinics) with secure payment integration (PIX and PayPal). It provides full user authentication, role management, booking creation, payment processing, and administrative controls.

---

## ✨ Features

- 🔒 **Authentication & Authorization**
  - User registration and login with JWT (Access + Refresh Tokens)
  - Role management: Admin, Seller (Service Owner), Buyer (Customer)
- 📆 **Booking Management**
  - Create, edit, view, and cancel appointments
  - Service owners can approve, reschedule, or cancel bookings
- 💳 **Payment Integration (Fake)**
  - PIX: QR Code generation
  - PayPal: Payment link generation
- 🛠️ **Admin System**
  - User management and transaction monitoring
- 🛡️ **Security**
  - Protected routes with token validation middleware
  - Access Token renewal without re-login

---

## 🛠️ Technologies Used

- **.NET Core (C#)** — Backend Framework
- **Entity Framework Core** — Database ORM
- **JWT (JSON Web Tokens)** — Authentication
- **PIX and PayPal APIs (Fakes)** — Payment processing


