using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ninelivesbooks.frmLogin;

namespace ninelivesbooks
{
    public partial class frmRegistration : Form
    {
        bool VisiblePassword = false;
        bool VisibleConfirmPassword = false;

        public static string Hashpassword(string password)
        {
            int iterations = 100_000;
            byte[] salt = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var pbkdf2 = new Rfc2898DeriveBytes(
                 password,
                 salt,
                 iterations,
                 HashAlgorithmName.SHA256
             );

            byte[] hash = pbkdf2.GetBytes(32);

            return $"{iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        MySqlConnection Conn;
        string data_source = "datasource=localhost; username=root; password=; database=ninelivebooks";
        public frmRegistration()
        {
            InitializeComponent();

            if (Sessao.User_Role != "Administrator")

            {
                btnAdd.Visible = false;
            }

            if (Sessao.User_Role == "Administrator" || Sessao.User_Role == "Staff")

            {
                btnQuit.Visible = false;
            }

            if (Sessao.User_Role == "Administrator")
            {
                btnRegister.Visible = false;
            }

            if (Sessao.User_Role == "Administrator")
            {
                cbRole.Items.Add("Administrator");
            }
        }
             private string GenerateNextUserId()
        {
            string query = @"
        SELECT MAX(CAST(SUBSTRING(user_id, 4) AS UNSIGNED))
        FROM usuario
        WHERE user_id LIKE 'NLU%'";

            using (var conn = Db.GetConnection())
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();

                object result = cmd.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                    return "NLU0001";

                int number = Convert.ToInt32(result) + 1;
                return "NLU" + number.ToString("D4");
            }
        }
        private void btnRegister_Click(object sender, EventArgs e)
        {
            DateTime hoje = DateTime.Now;
            try
            {
                if (string.IsNullOrEmpty(txtName.Text.Trim()) || string.IsNullOrEmpty(txtEmail.Text.Trim()) || string.IsNullOrEmpty(cbRole.Text.Trim()) ||
                    string.IsNullOrEmpty(txtPassword.Text.Trim()) || string.IsNullOrEmpty(cbStatus.Text.Trim()) ||
                    string.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
                {
                    MessageBox.Show("Please fill in all required fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtPassword.Text.Trim().Length < 6 || txtPassword.Text.Trim().Length > 10)
                {
                    MessageBox.Show("Password require a minimum number of characters.");
                    return;
                }
                Conn = new MySqlConnection(data_source);

                Conn.Open();

                MySqlCommand cmd = new MySqlCommand { Connection = Conn };
                cmd.Prepare();


                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    MessageBox.Show("Passwords do not match.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string passwordHash = Hashpassword(txtPassword.Text.Trim());


                string status = cbStatus.Text.Trim();

                if (status != "Ative" && status != "Inactive")
                {
                    MessageBox.Show("Invalid status. Use only Active or Inactive.");
                    return;
                }


                string role = cbRole.Text.Trim();

                if (role != "Administrator" && role != "Staff")
                {
                    MessageBox.Show("Invalid status. Use only Administrator or Staff.");
                    return;
                }

                string email = txtEmail.Text.Trim().ToLower();

                if (!email.EndsWith("@gmail.com"))
                {
                    MessageBox.Show("Only Gmail accounts are allowed. Use @gmail.com",
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                MySqlCommand checkCmd = new MySqlCommand("SELECT COUNT(*) FROM usuario", Conn);
                int totalUsers = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (totalUsers == 0)
                {

                    role = "Administrator";
                }
                else
                {
                    role = cbRole.Text.Trim();
                }
                string UserId = GenerateNextUserId();

                cmd.CommandText = "INSERT INTO usuario(user_name, user_role, user_email, user_status, user_password_hash, user_created_at)" +
                    "VALUES(@user_name, @user_role, @user_email, @user_status, @user_password_hash, @user_created_at)";
                cmd.Parameters.AddWithValue("@user_name", txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@user_role", cbRole.Text.Trim());
                cmd.Parameters.AddWithValue("@user_email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@user_status", cbStatus.Text.Trim());
                cmd.Parameters.AddWithValue("user_password_hash", passwordHash);
                cmd.Parameters.AddWithValue("@user_created_at", hoje);


                cmd.ExecuteNonQuery();

                MessageBox.Show("Registration was successful.");

                frmLogin form = new frmLogin();
                form.Show();
                this.Close();
            }

            catch (MySqlException ex)
            {
                MessageBox.Show($"An error has occurred. Please try again. + {ex.Number} : {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Occurred: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                if (Conn != null && Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }

        }
        private void lblShowPassword_Click(object sender, EventArgs e)
        {
            if (VisiblePassword)
            {
                txtPassword.UseSystemPasswordChar = true;
                VisiblePassword = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = false;
                VisiblePassword = true;
            }
        }

        private void lblShowConfirmPassword_Click(object sender, EventArgs e)
        {
            if (VisiblePassword)
            {
                txtConfirmPassword.UseSystemPasswordChar = true;
                VisiblePassword = false;
            }
            else
            {
                txtConfirmPassword.UseSystemPasswordChar = false;
                VisibleConfirmPassword = true;
            }
        }

        private void picPassword_Click(object sender, EventArgs e)
        {
            if (VisiblePassword)
            {
                txtPassword.UseSystemPasswordChar = true;
                picPassword.Image = global::ninelivesbooks.Properties.Resources.naovisivel;
                VisiblePassword = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = false;
                picPassword.Image = global::ninelivesbooks.Properties.Resources.visivel;
                VisiblePassword = true;
            }
        }

        private void picConfirmPassword_Click(object sender, EventArgs e)
        {
            if (VisibleConfirmPassword)
            {
                txtConfirmPassword.UseSystemPasswordChar = true;
                picConfirmPassword.Image = global::ninelivesbooks.Properties.Resources.naovisivel;
                VisibleConfirmPassword = false;
            }
            else
            {
                txtConfirmPassword.UseSystemPasswordChar = false;
                picConfirmPassword.Image = global::ninelivesbooks.Properties.Resources.visivel;
                VisibleConfirmPassword = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DateTime hoje = DateTime.Now;
            try
            {
                if (string.IsNullOrEmpty(txtName.Text.Trim()) || string.IsNullOrEmpty(txtEmail.Text.Trim()) || string.IsNullOrEmpty(cbRole.Text.Trim()) ||
                string.IsNullOrEmpty(txtPassword.Text.Trim()) || string.IsNullOrEmpty(cbStatus.Text.Trim()) ||
                string.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
                {
                    MessageBox.Show("Please fill in all required fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Conn = new MySqlConnection(data_source);

                Conn.Open();

                MySqlCommand cmd = new MySqlCommand { Connection = Conn };
                cmd.Prepare();

                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    MessageBox.Show("Passwords do not match.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string passwordHash = Hashpassword(txtPassword.Text.Trim());

                string status = cbStatus.Text.Trim();

                if (status != "Ative" && status != "Inactive")
                {
                    MessageBox.Show("Invalid status. Use only Active or Inactive.");
                    return;
                }

                string role = cbRole.Text.Trim();

                if (role != "Administrator" && role != "Staff")
                {
                    MessageBox.Show("Invalid status. Use only Administrator or Staff.");
                    return;
                }
                string UserId = GenerateNextUserId();

                cmd.CommandText = "INSERT INTO usuario(user_name, user_role, user_email, user_status, user_password_hash, user_created_at)" +
                "VALUES(@user_name, @user_role, @user_email, @user_status, @user_password_hash, @user_created_at)";
                cmd.Parameters.AddWithValue("@user_name", txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@user_role", cbRole.Text.Trim());
                cmd.Parameters.AddWithValue("@user_email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@user_status", cbStatus.Text.Trim());
                cmd.Parameters.AddWithValue("user_password_hash", passwordHash);
                cmd.Parameters.AddWithValue("@user_created_at", hoje);

                cmd.ExecuteNonQuery();

                LogHelper.RegistrarLog(
                    "USER",
                    UserId,
                    "CREATE_EMPLOYEE",
                    $"Employee '{txtName.Text}' created"
                );

                MessageBox.Show("New Employee added.");

            }

            catch (MySqlException ex)
            {
                MessageBox.Show($"An error has occurred. Please try again. + {ex.Number} : {ex.Message}",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Occurred: " + ex.Message,
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                if (Conn != null && Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }

        private void btnAdd_Paint(object sender, PaintEventArgs e)
        {
            Button btn = (Button)sender;
            int radius = 20; // Ajuste o raio para mudar a curvatura

            using (GraphicsPath path = new GraphicsPath())
            {
                path.StartFigure();
                // Canto superior esquerdo
                path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
                // Canto superior direito
                path.AddArc(new Rectangle(btn.Width - radius, 0, radius, radius), 270, 90);
                // Canto inferior direito
                path.AddArc(new Rectangle(btn.Width - radius, btn.Height - radius, radius, radius), 0, 90);
                // Canto inferior esquerdo
                path.AddArc(new Rectangle(0, btn.Height - radius, radius, radius), 90, 90);
                path.CloseFigure();

                btn.Region = new Region(path);
            }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            frmLogin form = new frmLogin();
            form.Show();
            this.Hide();
        }
    }
}
