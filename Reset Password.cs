using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ninelivesbooks
{
    public partial class frmResetPassword : Form
    {
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
        public frmResetPassword()
        {
            InitializeComponent();
        }

        private bool ValidarCodigo(string email, string codigo)
        {
            using (MySqlConnection conn = new MySqlConnection(conexao))
            {
                conn.Open();

                string sql = @"SELECT COUNT(*) FROM usuario WHERE user_email = @email AND reset_code = @codigo AND reset_code_expires_at > NOW(); ";


                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@codigo", codigo);

                    int resultado = Convert.ToInt32(cmd.ExecuteScalar());
                    return resultado > 0;
                }
            }
        }
        private string conexao = "datasource=localhost; username=root; password=; database=ninelivebooks";

        private void btnEnter_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();

            lblTextConfirmation.Text = "Sending code...";

            if (string.IsNullOrEmpty(email))
            {
                lblTextConfirmation.Text = "";
                MessageBox.Show("Please enter your email address to continue.", "Attention",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                lblTextConfirmation.Text = "A verification code has been sent to the registered email address.";
                string codigo = GerarCodigo();
                SalvarCodigo(email, codigo);
                EnviarEmail(email, codigo);
            }

            catch (Exception ex)
            {
                if (ex.Message == "EMAIL_NOT_FOUND")
                {
                    lblTextConfirmation.Text =
                    "No account was found with this email address.";
                }
                else
                {
                    lblTextConfirmation.Text =
                    "An error occurred while processing your request. Please try again later.";

                }
            }
        }
        private void EnviarEmail(string emailDestino, string codigo)
        {
            using (MailMessage mail = new MailMessage())
            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                mail.From = new MailAddress("ninelivest3@gmail.com", "Sistema");
                mail.To.Add(emailDestino);
                mail.Subject = "Password Reset";
                mail.Body = $"Your password reset code is:{codigo} " +
                ". This code expires in 10 minute.";

                smtp.Credentials = new NetworkCredential(
                "ninelivest3@gmail.com",
                "pntg rkgm kvza xvtj"
                );
                smtp.EnableSsl = true;

                smtp.Send(mail);
            }
        }
        private string GerarCodigo()
        {
            Random rnd = new Random();
            return rnd.Next(100000, 999999).ToString();
        }
        private void SalvarCodigo(string email, string codigo)
        {
            using (MySqlConnection conn = new MySqlConnection(conexao))
            {
                conn.Open();

                string sql = @"UPDATE usuario SET reset_code = @codigo, reset_code_expires_at = DATE_ADD(NOW(), INTERVAL 10 MINUTE) WHERE user_email = @email AND user_status = 'Ative';";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@codigo", codigo);
                    cmd.Parameters.AddWithValue("@email", email);
                    int rows = cmd.ExecuteNonQuery();


                    if (rows == 0)
                    {
                        throw new Exception("EMAIL_NOT_FOUND");
                    }
                }
            }
        }

        private void btnValidarCodigo_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string codigo = txtCode.Text.Trim();



            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(codigo))
            {

                lblCode.Text = "Please enter both email and verification code.";
                return;

            }

            bool codigoValido = ValidarCodigo(email, codigo);
            {

                if (codigoValido)
                {

                    lblCode.Text = "Verification code validated successfully. Please set your new password.";

                    txtNewPassword.Visible = true;
                    txtConfirmNewPassword.Visible = true;
                    btnRegister.Visible = true;
                    lblPassword.Visible = true;
                    lblConfirm.Visible = true;
                    return;
                }
                else
                {
                    lblCode.Text = "Invalid or expired verification code.";
                }
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string novaSenha = txtNewPassword.Text;
            string confirmarSenha = txtConfirmNewPassword.Text;


            if (string.IsNullOrEmpty(novaSenha) || string.IsNullOrEmpty(confirmarSenha))
            {
                MessageBox.Show(
                "Please enter and confirm your new password.",
                "Attention",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
                );
                return;
            }

            if (novaSenha != confirmarSenha)
            {
                MessageBox.Show(
                "The passwords do not match.",
                "Attention",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
                );
                return;
            }

            string passwordHash = Hashpassword(novaSenha);
            AtualizarSenha(email, passwordHash);

            MessageBox.Show("Your password has been successfully updated.");

            frmLogin form = new frmLogin();
            form.Show();
            this.Close();

        }
        private void AtualizarSenha(string email, string passwordHash)
        {
            using (MySqlConnection conn = new MySqlConnection(conexao))
            {
                conn.Open();

                string sql = @"UPDATE usuario SET user_password_hash = @senha, reset_code = NULL, reset_code_expires_at = NULL WHERE user_email = @email;";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@senha", passwordHash);
                    cmd.Parameters.AddWithValue("@email", email);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows == 0)
                        throw new Exception("User not found.");

                }
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
        
    
   