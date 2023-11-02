# Task Calendar Project Backend

This is the backend of the task calendar project, a challenge for a job selection process. It uses ASP.NET Core + Entity Framework Core and SQL Server for the database. It provides a RESTful API to interact with the frontend and perform CRUD operations on daily tasks.

## Features

The backend implements the following features, according to the desired seniority level:

- Junior: Registration, editing, and removal of daily tasks
- Full: Task search by title or tags
- Senior: Consultation of national holidays, login, and dashboard

## Requirements

- .NET 6
- SQL Server 2022
- Visual Studio 2022 or VS Code

## Installation

Clone this repository using git clone [https://github.com/TaskLy.git](https://github.com/TaskLy.git)

Open the backend.sln solution in Visual Studio or VS Code

Change the database connection string in the appsettings.json file

Run the command `dotnet ef database update` to create the database and tables

Run the project using `dotnet run` or by pressing F5

## Usage

The backend exposes the following endpoints:

- GET /api/tasks - Returns all tasks of the day
- GET /api/tasks/{id} - Returns a specific task by id
- POST /api/tasks/new - Creates a new task
- PUT /api/tasks/{id} - Updates an existing task by id
- DELETE /api/tasks/{id} - Deletes an existing task by id
- GET /api/tasks/search?title={title}&tags={tags} - Searches tasks by title or tags
- GET /api/holidays - Returns the national holidays of the current year
- POST /api/login - Authenticates a user and returns a JWT token
- GET /api/dashboard - Returns some statistics about the user's tasks