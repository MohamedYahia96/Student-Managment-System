# Student Assignment Manager — Project Lifecycle & Sequence

This document summarizes the application lifecycle, runtime sequences, and developer guidance for the Student Assignment Manager (.NET 8 Windows Forms, EF Core - Code First).

## Quick overview
- Type: Windows Forms desktop app (MDI) targeting .NET 8
- Purpose: Let students manage courses and assignments (CRUD), reminders, export data
- Layers: UI Forms -> Repositories -> EF Core DbContext -> Database

## Startup sequence
1. Program.Main
   - Sets UI rendering (EnableVisualStyles/SetCompatibleTextRenderingDefault)
   - Ensures database exists: StudentSystemDbContext.Database.EnsureCreated()
   - Shows LoginForm as a modal dialog
   - If LoginForm returns DialogResult.OK, obtains LoggedInStudent and Application.Run(new DashboardForm(student))

## Login flow
- LoginForm validates credentials via StudentRepository (uses DbContext)
- On success: LoginForm.LoggedInStudent is set and DialogResult = OK -> form closes
- Program.Main receives OK and opens DashboardForm with the authenticated Student

## Dashboard lifecycle
- DashboardForm constructor:
  - Stores Student
  - Creates StudentSystemDbContext and repositories (CourseRepository, AssignmentRepository)
  - Calls InitializeComponent() to build UI programmatically
  - Calls LoadCourses() and LoadAllAssignments() to populate UI
  - Calls StartReminderTimer() to begin periodic checks for upcoming assignments
- Dashboard acts as MDI container: opens child forms (CourseForm, AddEditCourseForm, AddEditAssignmentForm)
- On changes (add/edit/delete), child forms usually set DialogResult.OK; Dashboard.RefreshAll() reloads lists

## Child forms lifecycle (Add/Edit Course or Assignment, CourseForm)
- Each form typically:
  - Creates its own DbContext and repository instances in constructor
  - Builds UI in InitializeComponent()
  - On Save/Delete: uses repository (Add/Update/Delete) + Save()
  - On close: disposes DbContext in OnFormClosing or Dispose
- Typical pattern: new AddEditCourseForm(student) or new AddEditCourseForm(student, existingCourse)

## Repositories & DbContext
- StudentSystemDbContext: EF Core context with DbSet<Student>, DbSet<Course>, DbSet<Assignment>, etc.
- Repositories (Repository base, CourseRepository, AssignmentRepository, StudentRepository):
  - Encapsulate common data operations (GetById, GetByStudent, GetByCourse, Add, Update, Delete, Save)
  - Accept DbContext in constructor and call Save() -> context.SaveChanges()

## Reminder & Timer
- Dashboard uses a System.Windows.Forms.Timer with hourly interval
- On Tick or immediate startup, it queries AssignmentRepository.GetUpcomingAssignments(...) and shows MessageBox reminders

## Common user sequence examples
A. Add Course
  - Dashboard -> Add Course -> AddEditCourseForm -> Save -> repository.Add + Save -> DialogResult.OK -> Dashboard.RefreshAll()
B. Edit Course
  - Select course -> Open AddEditCourseForm(course) -> Modify -> Update + Save -> DialogResult.OK -> Dashboard.RefreshAll()
C. Add Assignment
  - CourseForm or Dashboard -> Add Assignment -> AddEditAssignmentForm -> Save -> repository.Add + Save -> parent.Refresh()
D. Delete
  - UI confirms -> repository.Delete(id) + Save -> parent.Refresh()

## Resource management & best practices
- Forms create and dispose their own DbContext instances. This is simple and acceptable for desktop UI apps.
- Ensure OnFormClosing always disposes resources (_context?.Dispose(), timers stopped/disposed)
- Consider centralizing DbContext lifetime (Unit of Work) if you need transactions across multiple forms or long-lived contexts
- Log exceptions around repository.Save() to gather DB errors

## Developer notes
- UI controls are constructed programmatically in InitializeComponent methods in each form (designer-generated code is not used for these forms)
- To add a new feature: implement model -> update DbContext -> create repository method -> add forms/menus -> wire up events -> test Save/Delete flows

## Build & run
- Requires .NET 8 SDK
- Open solution in Visual Studio or run: `dotnet build` and `dotnet run` from the project folder

## Suggested diagrams (text)
Sequence for "Add Course":
Program -> LoginForm -> (Login) -> Program -> DashboardForm -> User clicks Add -> AddEditCourseForm -> User Save -> CourseRepository.Add -> DbContext.SaveChanges -> AddEditCourseForm closes -> Dashboard.RefreshAll


## Team Roles:

1) Mohamed Yahia

- Setup and link DataBase
- Dashboard Form , Search & Filter
  
=====================================

2) Hossam Abdelhamid

- Login Form 
- Register Form
  
=====================================

2) Anas Eid

- AddEditCourse Form (CRUD)
- AddEditAssignment Form (CRUD)

======================================
