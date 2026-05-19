using MySql.Data.MySqlClient;
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
    public partial class frmArchive : Form
    {
        public frmArchive()
        {
            InitializeComponent();
        }
        private void frmArchive_Load(object sender, EventArgs e)
        {
            LoadArchive();
            ConfigureGrid();
            WireEvents();
            UpdateActionButtons();
        }

        private void WireEvents()
        {
            btnRefresh.Click -= btnRefresh_Click;
            btnRefresh.Click += btnRefresh_Click;

            btnEdit.Click -= btnEdit_Click;
            btnEdit.Click += btnEdit_Click;

            btnDelete.Click -= btnDelete_Click;
            btnDelete.Click += btnDelete_Click;

            btnCopy.Click -= btnCopy_Click;
            btnCopy.Click += btnCopy_Click;

            dgvArchive.SelectionChanged -= dgvArchive_SelectionChanged;
            dgvArchive.SelectionChanged += dgvArchive_SelectionChanged;
        }

        private void LoadArchive()
        {
            string query = @"
                SELECT
                    b.book_id AS EntityId,
                    bt.title AS Title,
                    bt.author AS Author,
                    bt.iSBN AS ISBN,
                    b.book_status AS Status,
                    b.book_condition AS BookCondition,
                    b.price AS Price,
                    'Book Copy' AS ArchiveType
                FROM book b
                INNER JOIN book_titles bt
                    ON b.title_id_in_book = bt.title_id
                WHERE b.book_status IN ('UNAVAILABLE', 'SOLD')

                UNION ALL

                SELECT
                    bt.title_id AS EntityId,
                    bt.title AS Title,
                    bt.author AS Author,
                    bt.iSBN AS ISBN,
                    'NO COPIES' AS Status,
                    NULL AS BookCondition,
                    NULL AS Price,
                    'Book Title Without Copies' AS ArchiveType
                FROM book_titles bt
                LEFT JOIN book b
                    ON bt.title_id = b.title_id_in_book
                WHERE b.book_id IS NULL

                ORDER BY ArchiveType, Title;";

            try
            {
                using (var conn = Db.GetConnection())
                using (var da = new MySqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvArchive.DataSource = dt;
                }

                UpdateActionButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading archive:\n" + ex.Message,
                    "Archive Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ConfigureGrid()
        {
            dgvArchive.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvArchive.ReadOnly = true;
            dgvArchive.AllowUserToAddRows = false;
            dgvArchive.AllowUserToDeleteRows = false;
            dgvArchive.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvArchive.MultiSelect = false;
            dgvArchive.RowHeadersVisible = false;

            dgvArchive.BackgroundColor = Color.White;
            dgvArchive.BorderStyle = BorderStyle.None;

            dgvArchive.DefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 45, 48);
            dgvArchive.DefaultCellStyle.SelectionForeColor = Color.White;

            dgvArchive.ColumnHeadersDefaultCellStyle.Font = new Font("Georgia", 12, FontStyle.Bold);

            dgvArchive.DefaultCellStyle.Font = new Font("Georgia", 10);

            dgvArchive.EnableHeadersVisualStyles = false;
            dgvArchive.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(46, 70, 50);
            dgvArchive.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(214, 167, 86);
        }

        private string GetSelectedArchiveType()
        {
            if (dgvArchive.CurrentRow == null)
                return null;

            return dgvArchive.CurrentRow.Cells["ArchiveType"].Value?.ToString();
        }

        private string GetSelectedEntityId()
        {
            if (dgvArchive.CurrentRow == null)
                return null;

            return dgvArchive.CurrentRow.Cells["EntityId"].Value?.ToString();
        }

        private void UpdateActionButtons()
        {
            bool hasSelection = dgvArchive.CurrentRow != null;

            btnEdit.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
            btnCopy.Enabled = hasSelection;
        }

        private void dgvArchive_SelectionChanged(object sender, EventArgs e)
        {
            UpdateActionButtons();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadArchive();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string archiveType = GetSelectedArchiveType();
            string entityId = GetSelectedEntityId();

            if (string.IsNullOrWhiteSpace(archiveType) || string.IsNullOrWhiteSpace(entityId))
            {
                MessageBox.Show("Select an archive item first.");
                return;
            }

            if (archiveType == "Book Copy")
            {
                ClassBook book = GetBookById(entityId);
            
                if (book != null)
                    frmPrincipal.PrincipalInstance.OpenBookForm(book, BookFormMode.Edit, true);
            
                return;
            }
            
            if (archiveType == "Book Title Without Copies")
            {
                ClassBook titleOnlyBook = GetBookTitleById(entityId);
            
                if (titleOnlyBook != null)
                    frmPrincipal.PrincipalInstance.OpenBookForm(titleOnlyBook, BookFormMode.EditTitle, true);
            
                return;
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            string archiveType = GetSelectedArchiveType();
            string entityId = GetSelectedEntityId();

            if (string.IsNullOrWhiteSpace(archiveType) || string.IsNullOrWhiteSpace(entityId))
            {
                MessageBox.Show("Select an archive item first.");
                return;
            }

            ClassBook source = null;

            if (archiveType == "Book Copy")
                source = GetBookById(entityId);
            else if (archiveType == "Book Title Without Copies")
                source = GetBookTitleById(entityId);

            if (source != null)
                frmPrincipal.PrincipalInstance.OpenBookForm(source, BookFormMode.AddCopy, true);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string archiveType = GetSelectedArchiveType();
            string entityId = GetSelectedEntityId();

            if (string.IsNullOrWhiteSpace(archiveType) || string.IsNullOrWhiteSpace(entityId))
            {
                MessageBox.Show("Select an archive item first.");
                return;
            }

            if (archiveType == "Book Copy")
            {
                DeleteBook(entityId);
                return;
            }

            if (archiveType == "Book Title Without Copies")
            {
                DeleteBookTitle(entityId);
                return;
            }
        }

        private ClassBook GetBookById(string bookId)
        {
            string query = @"
                SELECT
                    b.book_id,
                    b.price,
                    b.book_condition,
                    b.book_status,
                    b.reason_status,
                    b.defected_notes,
                    b.origin,
                    b.title_id_in_book,

                    bt.title_id,
                    bt.title,
                    bt.author,
                    bt.iSBN,
                    bt.pages,
                    bt.book_type,
                    bt.book_approx_weight,
                    bt.publisher,
                    bt.publication_year,
                    bt.book_language,
                    bt.genre,
                    bt.book_description,
                    bt.book_image
                FROM book b
                INNER JOIN book_titles bt
                    ON b.title_id_in_book = bt.title_id
                WHERE b.book_id = @book_id
                LIMIT 1;";

            using (var conn = Db.GetConnection())
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@book_id", bookId);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return MapBook(reader);
                }
            }

            return null;
        }

        private ClassBook GetBookTitleById(string titleId)
        {
            string query = @"
                SELECT
                    title_id,
                    title,
                    author,
                    iSBN,
                    pages,
                    book_type,
                    book_approx_weight,
                    publisher,
                    publication_year,
                    book_language,
                    genre,
                    book_description,
                    book_image
                FROM book_titles
                WHERE title_id = @title_id
                LIMIT 1;";

            using (var conn = Db.GetConnection())
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@title_id", titleId);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ClassBook
                        {
                            BookId = null,
                            TitleId = reader["title_id"].ToString(),
                            Title = reader["title"].ToString(),
                            Author = reader["author"].ToString(),
                            ISBN = reader["iSBN"].ToString(),
                            Pages = Convert.ToInt32(reader["pages"]),
                            BookType = reader["book_type"].ToString(),
                            ApproxWeight = Convert.ToDecimal(reader["book_approx_weight"]),
                            Publisher = reader["publisher"].ToString(),
                            PublicationYear = Convert.ToInt32(reader["publication_year"]),
                            Language = reader["book_language"].ToString(),
                            Genre = reader["genre"].ToString(),
                            Description = reader["book_description"].ToString(),
                            ImageBytes = reader["book_image"] == DBNull.Value ? null : (byte[])reader["book_image"]
                        };
                    }
                }
            }

            return null;
        }

        private ClassBook MapBook(MySqlDataReader reader)
        {
            return new ClassBook
            {
                BookId = reader["book_id"].ToString(),
                TitleId = reader["title_id"].ToString(),
                Price = Convert.ToDecimal(reader["price"]),
                Condition = reader["book_condition"].ToString(),
                Status = reader["book_status"].ToString(),
                ReasonStatus = reader["reason_status"] == DBNull.Value ? null : reader["reason_status"].ToString(),
                DefectedNotes = reader["defected_notes"] == DBNull.Value ? null : reader["defected_notes"].ToString(),
                Origin = reader["origin"] == DBNull.Value ? null : reader["origin"].ToString(),

                Title = reader["title"].ToString(),
                Author = reader["author"].ToString(),
                ISBN = reader["iSBN"].ToString(),
                Pages = Convert.ToInt32(reader["pages"]),
                BookType = reader["book_type"].ToString(),
                ApproxWeight = Convert.ToDecimal(reader["book_approx_weight"]),
                Publisher = reader["publisher"].ToString(),
                PublicationYear = Convert.ToInt32(reader["publication_year"]),
                Language = reader["book_language"].ToString(),
                Genre = reader["genre"].ToString(),
                Description = reader["book_description"].ToString(),
                ImageBytes = reader["book_image"] == DBNull.Value ? null : (byte[])reader["book_image"]
            };
        }

        private void DeleteBook(string bookId)
        {
            DialogResult result = MessageBox.Show(
                $"Delete book {bookId}?\n\nThis deletes only the physical copy, not the book title.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (var conn = Db.GetConnection())
                {
                    conn.Open();

                    string query = "DELETE FROM book WHERE book_id = @book_id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@book_id", bookId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Book deleted successfully.");
                LoadArchive();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting book:\n" + ex.Message);
            }
        }

        private void DeleteBookTitle(string titleId)
        {
            try
            {
                using (var conn = Db.GetConnection())
                {
                    conn.Open();

                    string checkQuery = @"
                        SELECT COUNT(*)
                        FROM book
                        WHERE title_id_in_book = @title_id";

                    using (var checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@title_id", titleId);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Cannot delete title because books still exist.");
                            return;
                        }
                    }

                    DialogResult result = MessageBox.Show(
                        "Delete this book title permanently?",
                        "Confirm Delete",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (result != DialogResult.Yes)
                        return;

                    string deleteQuery = @"
                        DELETE FROM book_titles
                        WHERE title_id = @title_id";

                    using (var deleteCmd = new MySqlCommand(deleteQuery, conn))
                    {
                        deleteCmd.Parameters.AddWithValue("@title_id", titleId);
                        deleteCmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Book title deleted successfully.");
                LoadArchive();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting title:\n" + ex.Message);
            }
        }
    }
}
