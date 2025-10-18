using System;
using System.Windows.Forms;
using StudentAssignmentManager.Forms;
using StudentAssignmentManager.Data;

namespace StudentAssignmentManager
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Ensure database is created
            try
            {
                using (var context = new StudentSystemDbContext())
                {
                    context.Database.EnsureCreated();
                }

                // Show login form
                var loginForm = new LoginForm();
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // Login successful, show dashboard
                    Application.Run(new DashboardForm(loginForm.LoggedInStudent));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Database Error:\n{ex.Message}\n\n" +
                    "Please ensure SQL Server is running and the connection string is correct.",
                    "Database Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

            }
        }
    }
}