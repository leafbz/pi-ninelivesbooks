using ninelivesbooks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Management.Instrumentation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xdd;

namespace ninelivesbooks
{
    public partial class frmPrincipal : Form
    {
        public static frmPrincipal PrincipalInstance;
        public static int parentX, parentY;

        public frmPrincipal()
        {
            InitializeComponent();
            PrincipalInstance = this;
            AbrirForm<Book>();
        }

        public void AbrirForm<MeuForm>() where MeuForm : Form, new()
        {
            Form form = panelForms.Controls.OfType<MeuForm>().FirstOrDefault();

            if (form == null)
            {
                form = new MeuForm();
                form.TopLevel = false;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = DockStyle.Fill;

                panelForms.Controls.Clear();
                panelForms.Controls.Add(form);
                panelForms.Tag = form;

                form.Show();
                form.BringToFront();
            }
            else
            {
                form.BringToFront();
            }
        }
        
        public void AbrirForm(Form form)
        {
            if (form == null)
                return;
        
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
        
            panelForms.Controls.Clear();
            panelForms.Controls.Add(form);
            panelForms.Tag = form;
        
            form.Show();
            form.BringToFront();
        }

        public void OpenBookForm(ClassBook book, BookFormMode mode)
        {
            frmAddBook form = new frmAddBook(book, mode);
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            panelForms.Controls.Clear();
            panelForms.Controls.Add(form);
            panelForms.Tag = form;

            form.Show();
            form.BringToFront();
        }
        
        public void OpenBookForm(ClassBook book, BookFormMode mode, bool returnToArchive = false)
        {
            frmAddBook form = new frmAddBook(book, mode, returnToArchive);
        
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
        
            panelForms.Controls.Clear();
            panelForms.Controls.Add(form);
            panelForms.Tag = form;
        
            form.Show();
            form.BringToFront();
        }

        public void OpenAddBookForm()
        {
            frmAddBook form = new frmAddBook();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            panelForms.Controls.Clear();
            panelForms.Controls.Add(form);
            panelForms.Tag = form;

            form.Show();
            form.BringToFront();
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            foreach (var pnl in tableLayoutPanel1.Controls.OfType<Panel>())
            {
                pnl.BackColor = Color.FromArgb(1, 38, 10);
            }

            Button btn = (Button)sender;

            switch (btn.Name)
            {
                case "btnBooks":
                    AbrirForm<Book>();
                    panelBook.BackColor = Color.FromArgb(166, 78, 27);
                    break;

                case "btnBundles":
                    AbrirForm<frmBundles>();
                    panelBundle.BackColor = Color.FromArgb(166, 78, 27);
                    break;

                case "btnAdd":
                    OpenAddBookForm();
                    panelAdd.BackColor = Color.FromArgb(166, 78, 27);
                    break;

                case "btnArchive":
                    AbrirForm<frmArchive>();
                    panelArchive.BackColor = Color.FromArgb(166, 78, 27);
                    break;

                case "btnUser":
                    AbrirForm<frmLog>();
                    panelUser.BackColor = Color.FromArgb(166, 78, 27);
                    break;
            }
        }
    }
}
