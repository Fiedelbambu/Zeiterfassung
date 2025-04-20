# IST-Zeiterfassung

IST-Zeiterfassung ist eine moderne, webbasierte Plattform zur Arbeitszeiterfassung für kleine bis mittlere Unternehmen. Sie ermöglicht die Erfassung, Verwaltung und Auswertung von Arbeitszeiten und integriert Funktionen wie NFC-Terminals, Home Office-Unterstützung und Monatsberichte mit Export.

## Projektziel

Bereitstellung eines flexiblen, sicheren und modularen Systems zur Zeitbuchung. Mitarbeitende erfassen ihre Arbeitszeiten, während Administratoren einen vollständigen Überblick und Exportmöglichkeiten erhalten.

## Technologiestack

- **Frontend:** React oder Vue.js mit Tailwind CSS oder Bootstrap
- **Backend:** ASP.NET Core Web API (C#)
- **Datenbank:** PostgreSQL (alternativ SQLite oder MariaDB)
- **Authentifizierung:** JWT (JSON Web Tokens)
- **Export:** PDF (iText), Excel (EPPlus)
- **CI/CD:** GitHub Actions
- **Dokumentation:** Swagger UI (OpenAPI 3.1 JSON)

## Hauptfunktionen

- Zeitbuchung: Kommen, Gehen, Pause, Dienstgang
- Buchungshistorie und Stundensaldo
- Abwesenheitsmanagement: Urlaub, Krankheit, Home Office
- Monatsberichte und Exporte (PDF, Excel, BMD-kompatibel)
- NFC- und QR-Code-Unterstützung für Terminals
- Benutzerrollen (Mitarbeiter, Admin)
- E-Mail-Benachrichtigungen und Berichtversand
- Mobile Nutzung (responsive Design)

## Benutzerrollen

**Mitarbeiter**
- Eigene Zeiten buchen
- Abwesenheiten beantragen
- Monatsberichte einsehen

**Admin**
- Mitarbeiter verwalten
- Buchungen korrigieren und filtern
- Monatsberichte generieren, exportieren und versenden
- Live-Anwesenheitsübersicht

## Modulübersicht

- **User:** Authentifizierung, Profilverwaltung, Rollen
- **TimeEntry:** Zeitbuchungen mit Buchungstyp
- **Absence:** Urlaubs- und Krankmeldungen inkl. Genehmigungsprozess
- **Report:** Auswertungen, Monatsberichte, PDF-/Excel-Export
- **Admin:** Verwaltung aller Nutzer und Buchungen
- **Terminal:** NFC-/QR-Code-Zeiterfassung mit API-Token

## Datenstruktur (Auszug)

- `users`: Benutzer (Admin, Mitarbeiter)
- `time_entries`: Zeitstempel mit Typ (Kommen, Gehen etc.)
- `absence_requests`: Urlaubs- und Krankmeldungen
- `monthly_reports`: Berichte mit Arbeits-, Über- und Fehlstunden
- `nfc_terminals`: Lokale Terminals mit Token
- `api_tokens`: Zugriffsschlüssel für Terminals

## API-Dokumentation

Die REST-API ist vollständig dokumentiert über Swagger UI und entspricht dem OpenAPI 3.1 Standard (JSON). Die API unterstützt rollenbasierten Zugriff und ist vorbereitet für Postman, Code-Generatoren und ReDoc.

## Teststrategie

- Unit-Tests mit xUnit
- Mocking mit Moq
- InMemory-Datenbank für Integrationstests
- E2E-Tests mit Cypress (geplant)
- Testszenarien pro Modul (Absence, TimeEntry, Report etc.)

## Projektstruktur

- `API`: Webschnittstelle, Auth, Swagger, Middleware
- `Application`: Geschäftslogik, DTOs, Interfaces
- `Domain`: Entitäten, Regeln, Value Objects
- `Infrastructure`: Mail, PDF, Excel, QR-Code, Logging
- `Persistence`: EF Core, DbContext, Migrations
- `Common`: Result-Typ, Hilfsklassen, Zeitprovider
- `Tests`: Modularer Testaufbau (Unit + Integration)

## Lizenz

MIT License

## Beitrag leisten

Pull Requests sind willkommen. Bitte beachte die demnächst verfügbare Datei CONTRIBUTING.md.

## Weiterführende Informationen

- API-Dokumentation: `/swagger`
- GitHub-Projektboard (optional)
- Dokumentation im Ordner `/docs`

################################################################################################################################################


# IST Time Tracking

IST Time Tracking is a modern, web-based time tracking platform for small to mid-sized companies. It enables employees to record their working hours while providing administrators with full control, export options, and hardware integration.

## Project Goal

To provide a flexible, secure, and modular system for managing working hours. The platform includes time entries, absence requests, reports, and optional NFC or QR code authentication.

## Technology Stack

- **Frontend:** React or Vue.js with Tailwind CSS or Bootstrap
- **Backend:** ASP.NET Core Web API (C#)
- **Database:** PostgreSQL (alternatively SQLite or MariaDB)
- **Authentication:** JWT (JSON Web Tokens)
- **Export:** PDF (iText), Excel (EPPlus)
- **CI/CD:** GitHub Actions
- **API Documentation:** Swagger UI (OpenAPI 3.1 JSON)

## Key Features

- Time entries: Check-in, Check-out, Breaks, Business trips
- Time history and working hour balance
- Absence management: Vacation, Sick leave, Home office
- Monthly reports with export options (PDF, Excel, BMD)
- NFC and QR code support for terminals
- Role-based access: Employee and Admin
- Email notifications and report distribution
- Responsive interface for desktop and mobile use

## User Roles

**Employee**
- Record own working times
- Request absences (vacation, sick leave, etc.)
- View monthly reports and time balances

**Administrator**
- Manage users
- Correct or delete entries
- Generate and export reports
- View live presence overview
- Manage NFC terminals

## Module Overview

- **User:** Authentication, profile, and role management
- **TimeEntry:** Time tracking with type (Check-in, Break, etc.)
- **Absence:** Absence requests with approval workflow
- **Report:** Monthly summary, overtime calculation, exports
- **Admin:** Manage employees, bookings, and reports
- **Terminal:** NFC/QR-based booking with API tokens

## Database Overview (excerpt)

- `users`: All system users (employee/admin)
- `time_entries`: Time entries with type and timestamp
- `absence_requests`: Absence periods and approval status
- `monthly_reports`: Reports with working hours, absences
- `nfc_terminals`: Configured terminal devices with API tokens
- `api_tokens`: Secure authentication for terminal requests

## API Documentation

The REST API is fully documented using Swagger UI and follows the OpenAPI 3.1 (JSON) standard. The documentation supports integration with Postman, ReDoc, code generators, and includes security annotations for JWT and roles.

## Test Strategy

- Unit tests using xUnit
- Mocking via Moq
- InMemory database for integration testing
- End-to-end tests with Cypress (planned)
- Test scenarios for all modules (Absence, TimeEntry, Report, etc.)

## Project Structure

- `API`: REST entry points, authentication, Swagger setup
- `Application`: Business logic, DTOs, service interfaces
- `Domain`: Core models, enums, validation rules
- `Infrastructure`: Export services, email, QR code, logging
- `Persistence`: EF Core, DbContext, repositories
- `Common`: Shared helpers, Result<T>, constants
- `Tests`: Unit and integration tests using xUnit

## License

MIT License

## Contributing

Contributions and pull requests are welcome. Please refer to the upcoming CONTRIBUTING.md file.

## Further Information

- Swagger UI: `/swagger`
- GitHub Project Board (optional)
- Additional docs available in the `/docs` folder
