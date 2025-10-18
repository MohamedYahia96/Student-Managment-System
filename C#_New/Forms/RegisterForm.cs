using System;
using System.Windows.Forms;
using System.Drawing;
using StudentAssignmentManager.Data;
using StudentAssignmentManager.Repositories;
using StudentAssignmentManager.Models;

namespace StudentAssignmentManager.Forms
{
    public partial class RegisterForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;
        private TextBox txtFullName;
        private TextBox txtEmail;
        private Button btnRegister;
        private Button btnCancel;

        private StudentSystemDbContext _context;
        private StudentRepository _studentRepo;

        public RegisterForm()
        {
            InitializeComponent();
            _context = new StudentSystemDbContext();
            _studentRepo = new StudentRepository(_context);
        }

        private void InitializeComponent()
        {
            this.Text = "Register New Student";
            this.Size = new Size(400, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            int y = 20;

            AddLabelAndTextBox("Username:", ref txtUsername, ref y);
            AddLabelAndTextBox("Password:", ref txtPassword, ref y, true);
            AddLabelAndTextBox("Confirm Password:", ref txtConfirmPassword, ref y, true);
            AddLabelAndTextBox("Full Name:", ref txtFullName, ref y);
            AddLabelAndTextBox("Email:", ref txtEmail, ref y);

            btnRegister = new Button
            {
                Text = "Register",
                Location = new Point(135, y + 20),
                Size = new Size(90, 30)
            };
            btnRegister.Click += BtnRegister_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(250, y + 20),
                Size = new Size(90, 30)
            };
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.Add(btnRegister);
            this.Controls.Add(btnCancel);

            StyleButton(btnRegister, "save");
            StyleButton(btnCancel, "cancel");
        }

        private void AddLabelAndTextBox(string labelText, ref TextBox textBox, ref int yPos, bool isPassword = false)
        {
            var label = new Label
            {
                Text = labelText,
                Location = new Point(30, yPos),
                Size = new Size(120, 20)
            };

            textBox = new TextBox
            {
                Location = new Point(160, yPos),
                Size = new Size(200, 20)
            };

            if (isPassword)
                textBox.PasswordChar = '*';

            this.Controls.Add(label);
            this.Controls.Add(textBox);

            yPos += 40;
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Username and password are required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var existingStudent = _studentRepo.GetByUsername(txtUsername.Text);
            if (existingStudent != null)
            {
                MessageBox.Show("Username already exists.", "Registration Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var newStudent = new Student
            {
                Username = txtUsername.Text,
                PasswordHash = txtPassword.Text,
                FullName = txtFullName.Text,
                Email = txtEmail.Text,
                CreatedDate = DateTime.Now
            };

            _studentRepo.Add(newStudent);
            _studentRepo.Save();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // Utility method to style buttons consistently across the project
        private void StyleButton(Button button, string type = "default")
        {
            button.FlatStyle = FlatStyle.Flat;
            button.Font = new Font("Segoe UI", 9.5f);
            button.Size = new Size(110, 35);
            button.Cursor = Cursors.Hand;
            button.Margin = new Padding(5, 0, 0, 0);
            switch (type)
            {
                case "delete":
                    button.BackColor = Color.FromArgb(220, 53, 69);
                    button.ForeColor = Color.White;
                    button.FlatAppearance.BorderColor = Color.FromArgb(200, 33, 49);
                    button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(200, 33, 49);
                    button.MouseLeave += (s, e) => button.BackColor = Color.FromArgb(220, 53, 69);
                    break;
                case "save":
                    button.BackColor = Color.FromArgb(0, 122, 204);
                    button.ForeColor = Color.White;
                    button.FlatAppearance.BorderColor = Color.FromArgb(0, 102, 184);
                    button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(0, 102, 184);
                    button.MouseLeave += (s, e) => button.BackColor = Color.FromArgb(0, 122, 204);
                    break;
                case "cancel":
                    button.BackColor = Color.White;
                    button.ForeColor = Color.Black;
                    button.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
                    button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(240, 240, 240);
                    button.MouseLeave += (s, e) => button.BackColor = Color.White;
                    break;
                default:
                    button.BackColor = Color.FromArgb(240, 240, 240);
                    button.ForeColor = Color.Black;
                    button.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
                    button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(230, 230, 230);
                    button.MouseLeave += (s, e) => button.BackColor = Color.FromArgb(240, 240, 240);
                    break;
            }
        }
    }
}