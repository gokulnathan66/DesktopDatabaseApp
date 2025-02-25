Overview
This report provides a comprehensive breakdown of the Desktop Database Management System(DDMS) designed as a desktop application, utilizing .NET MAUI for cross-platform support and SQLite for data storage. The application is specifically modeled to manage the suppliers and leaf types in a tea leaf factory in Ooty. The application also supports the generation of reports in PDF format, with dependencies such as SQL and PDF generation being handled by the NuGet Package Manager.

Created By
Gokulnathan B : Dept Computer Science Engineering

Kumaraguru College of Technology , coimbatore.

gokulnathan.22cs@kct.ac.in

Key Functionalities:
Supplier Tracking: Manage supplier information efficiently.
Supplier Management: Add, update, and remove suppliers from the database.
Transaction Logging: Track and log transactions made by each supplier.
Leaf Type Management: Handle different types of tea leaves, including quality and quantity.
Application Overview:
The DDMS is modeled on the operations of a tea leaf factory, focusing on managing leaf table suppliers table in DB. Tracking leaf type and quantity. The system records transactions composed of:

Supplier Name
Date
Leaf Type: Including base rate and total value.
These transaction entries are updated into the Master Database Table, providing a detailed record of all operations.

Reporting Capabilities:
DDMS provides two types of reports:

Generic Report: Displays the transaction details of a specific supplier for a specified financial year.
Monthly Report: Provides an overview of all transactions that occurred within a particular month.
Both reports can be generated as PDF files, ensuring easy sharing and archival.

Application Components:
The app structure is divided into several key components:

Services: Responsible for handling business logic and operations.
Models: Define the structure of data used within the app.
Pages (UI): User interface components that allow interaction with the application.
Database Interactions: Handle all operations related to SQLite, ensuring smooth data storage and retrieval.
Each of these components plays a critical role in ensuring the functionality and performance of the DDMS.

You tube Demo Video:

[https://youtu.be/f319-V47ZrA]


Github Repo: https://github.com/gokulnathan66/DesktopDatabaseApp.git
