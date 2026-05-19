using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ninelivesbooks
{
    public partial class Card : UserControl
    {
        private string text = "Label";
        private Image bookImage;

        public ClassBook BookData { get; private set; }

        public string customText
        {
            get { return text; }
            set
            {
                text = value;
                if (label1 != null)
                    label1.Text = value;
            }
        }

        public Image bookImg
        {
            get { return bookImage; }
            set
            {
                bookImage = value;
                if (pictureBox1 != null)
                {
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox1.Image = value;
                }
            }
        }

        public event EventHandler CardClicked;

        public Card()
        {
            InitializeComponent();
            RegisterClickEvents(this);
        }

        public void SetBook(ClassBook book)
        {
            BookData = book;

            Detail(
                $"{book.Title}\n" +
                $"{book.Author}\n" +
                $"ID: {book.BookId}\n" +
                $"Condition: {book.Condition}\n" +
                $"Status: {book.Status}\n" +
                $"Origin: {book.Origin}\n" +
                $"Price: ${book.Price:F2}"
            );

            bookImg = book.CoverImage;
        }

        public void Detail(string text)
        {
            customText = text;
        }

        private void RegisterClickEvents(Control parent)
        {
            parent.Click += Card_Click;

            foreach (Control control in parent.Controls)
            {
                control.Click += Card_Click;
                if (control.HasChildren)
                    RegisterClickEvents(control);
            }
        }

        private void Card_Click(object sender, EventArgs e)
        {
            CardClicked?.Invoke(this, e);

            if (BookData != null && frmPrincipal.PrincipalInstance != null)
            {
                frmPrincipal.PrincipalInstance.OpenBookForm(BookData, BookFormMode.View);
            }
        }

        private void Card_MouseHover(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(244, 244, 244);
        }

        private void Card_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
        }

        private void Card_Paint(object sender, PaintEventArgs e)
        {
            label1.Text = customText;
        }
    }
}
