using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace ninelivesbooks
{
    public partial class FrmBundleDetails : Form
    {
        private readonly string bundleId;
        private string bookIdSelecionado = null;

        public FrmBundleDetails(string idBundle)
        {
            InitializeComponent();
            bundleId = idBundle;
        
            this.Load -= FrmBundleDetails_Load_1;
            this.Load += FrmBundleDetails_Load_1;
        }

        private void FrmBundleDetails_Load_1(object sender, EventArgs e)
        {
            ConfigurarGrid();
            CarregarResumoBundle();
            CarregarLivrosDoBundle();
        }

        private void ConfigurarGrid()
        {
            dgvBooksBundle.BackgroundColor = Color.FromArgb(255, 245, 220);
            dgvBooksBundle.BorderStyle = BorderStyle.None;
            dgvBooksBundle.EnableHeadersVisualStyles = false;
            dgvBooksBundle.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 60, 40);
            dgvBooksBundle.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(255, 240, 200);
            dgvBooksBundle.ColumnHeadersDefaultCellStyle.Font = new Font("Georgia", 10F, FontStyle.Bold);
            dgvBooksBundle.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 215, 160);
        
            dgvBooksBundle.AutoGenerateColumns = true;
            dgvBooksBundle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBooksBundle.MultiSelect = false;
            dgvBooksBundle.ReadOnly = true;
            dgvBooksBundle.AllowUserToAddRows = false;
            dgvBooksBundle.AllowUserToDeleteRows = false;
            dgvBooksBundle.RowHeadersVisible = false;
            dgvBooksBundle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        
            dgvBooksBundle.CellClick -= dgvBooksBundle_CellClick;
            dgvBooksBundle.CellClick += dgvBooksBundle_CellClick;
        
            dgvBooksBundle.CellDoubleClick -= dgvBooksBundle_CellDoubleClick;
            dgvBooksBundle.CellDoubleClick += dgvBooksBundle_CellDoubleClick;
        }

        private void CarregarResumoBundle()
        {
            try
            {
                using (MySqlConnection con = Db.GetConnection())
                {
                    string sql = @"
                    SELECT
                        b.bundle_id,
                        b.bundle_name,
                        b.bundle_status,
                        b.bundle_theme,
                        b.bundle_description,
                        b.bundle_created_at,
                        b.bundle_updated_at,
                        b.bundle_image,
                        COUNT(bb.book_id_in_bundle_book) AS total_books,
                        IFNULL(SUM(bk.price), 0) AS total_price
                    FROM bundle b
                    LEFT JOIN bundle_book bb
                        ON bb.bundle_id_in_bundle_book = b.bundle_id
                    LEFT JOIN book bk
                        ON bk.book_id = bb.book_id_in_bundle_book
                    WHERE b.bundle_id = @bundleId
                    GROUP BY
                        b.bundle_id,
                        b.bundle_name,
                        b.bundle_status,
                        b.bundle_theme,
                        b.bundle_created_at,
                        b.bundle_updated_at,
                        b.bundle_image;";

                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@bundleId", bundleId);
                        con.Open();

                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                lblBundleName.Text = dr["bundle_name"] == DBNull.Value ? "" : dr["bundle_name"].ToString();
                                lblTheme.Text = dr["bundle_theme"] == DBNull.Value ? "" : dr["bundle_theme"].ToString();
                                lblTotalBooks.Text = dr["total_books"] == DBNull.Value ? "0" : dr["total_books"].ToString();
                                lblTotalPrice.Text = dr["total_price"] == DBNull.Value ? "0,00" : Convert.ToDecimal(dr["total_price"]).ToString("N2");
                                lblStatus.Text = dr["bundle_status"] == DBNull.Value ? "" : dr["bundle_status"].ToString();
                                txtDescription.Text = dr["bundle_description"] == DBNull.Value ? "" : dr["bundle_description"].ToString();
                                lblCreatedAt.Text = dr["bundle_created_at"] == DBNull.Value ? "" : Convert.ToDateTime(dr["bundle_created_at"]).ToString("dd-MM-yyyy");
                                lblUpdatedAt.Text = dr["bundle_updated_at"] == DBNull.Value ? "" : Convert.ToDateTime(dr["bundle_updated_at"]).ToString("dd-MM-yyyy");
                                btnMarkSold.Enabled = lblStatus.Text != "SOLD";

                                if (dr["bundle_image"] != DBNull.Value)
                                {
                                    byte[] imgBytes = (byte[])dr["bundle_image"];
                                    using (MemoryStream ms = new MemoryStream(imgBytes))
                                    {
                                        picBundle.Image = Image.FromStream(ms);
                                    }
                                }
                                else
                                {
                                    picBundle.Image = null;
                                }
                            }
                            else
                            {
                                MessageBox.Show("No bundle found for the provided ID.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading bundle summary: " + ex.Message);
            }
        }

        private void CarregarLivrosDoBundle()
        {
            try
            {
                using (MySqlConnection con = Db.GetConnection())
                {
                    string sql = @"
                        SELECT
                            bk.book_id AS IdInterno,
                            bk.book_id AS ID,
                            bt.iSBN AS ISBN,
                            bt.title AS Title,
                            bt.author AS Author,
                            bk.book_condition AS `Condition`,
                            bk.price AS Price
                        FROM bundle_book bb
                        INNER JOIN book bk
                            ON bk.book_id = bb.book_id_in_bundle_book
                        INNER JOIN book_titles bt
                            ON bt.title_id = bk.title_id_in_book
                        WHERE bb.bundle_id_in_bundle_book = @bundleId
                        ORDER BY bt.title ASC;";

                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@bundleId", bundleId);

                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        dgvBooksBundle.DataSource = dt;

                        if (dgvBooksBundle.Columns["IdInterno"] != null)
                            dgvBooksBundle.Columns["IdInterno"].Visible = false;
                        
                        if (dgvBooksBundle.Columns["ISBN"] != null)
                            dgvBooksBundle.Columns["ISBN"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading books from bundle: " + ex.Message);
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
                }
            }
        
            return null;
        }

        private void LimparResumoBundle()
        {
            lblBundleName.Text = "";
            lblTheme.Text = "";
            lblTotalBooks.Text = "";
            lblTotalPrice.Text = "";
            lblStatus.Text = "";
            lblCreatedAt.Text = "";
            lblUpdatedAt.Text = "";
            txtDescription.Text = "";
            picBundle.Image = null;
        }

        private void dgvBooksBundle_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvBooksBundle.Rows[e.RowIndex];

                if (row.Cells["IdInterno"].Value != null)
                    bookIdSelecionado = row.Cells["IdInterno"].Value.ToString();
            }
        }
        
        private void btnBack_Click(object sender, EventArgs e)
        {
            frmPrincipal.PrincipalInstance.AbrirForm<frmBundles>();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            frmPrincipal.PrincipalInstance.AbrirForm(
                new frmAddBundle(bundleId)
            );
        }
        
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(bookIdSelecionado))
            {
                MessageBox.Show("Select a book to remove from the bundle.");
                return;
            }
        
            DialogResult result = MessageBox.Show(
                "Do you want to remove this book from the bundle?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
        
            if (result != DialogResult.Yes)
                return;
        
            try
            {
                using (MySqlConnection con = Db.GetConnection())
                {
                    con.Open();
        
                    using (MySqlTransaction transaction = con.BeginTransaction())
                    {
                        try
                        {
                            string deleteSql = @"
                                DELETE FROM bundle_book
                                WHERE bundle_id_in_bundle_book = @bundleId
                                  AND book_id_in_bundle_book = @bookId;";
        
                            using (MySqlCommand deleteCmd = new MySqlCommand(deleteSql, con, transaction))
                            {
                                deleteCmd.Parameters.AddWithValue("@bundleId", bundleId);
                                deleteCmd.Parameters.AddWithValue("@bookId", bookIdSelecionado);
                                deleteCmd.ExecuteNonQuery();
                            }
        
                            string updateBookSql = @"
                                UPDATE book
                                SET
                                    book_status = 'AVAILABLE',
                                    reason_status = NULL
                                WHERE book_id = @bookId
                                  AND book_status <> 'SOLD';";
        
                            using (MySqlCommand updateCmd = new MySqlCommand(updateBookSql, con, transaction))
                            {
                                updateCmd.Parameters.AddWithValue("@bookId", bookIdSelecionado);
                                updateCmd.ExecuteNonQuery();
                            }
        
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
        
                bookIdSelecionado = null;
        
                CarregarLivrosDoBundle();
                CarregarResumoBundle();
        
                MessageBox.Show("Book successfully removed from bundle.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error removing book from bundle: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text == "SOLD")
            {
                MessageBox.Show("Sold bundles should not be deleted.");
                return;
            }
        
            DialogResult result = MessageBox.Show(
                "Do you want to delete this bundle?\n\nBooks inside this bundle will become AVAILABLE again, except books already SOLD.",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );
        
            if (result != DialogResult.Yes)
                return;
        
            try
            {
                using (MySqlConnection con = Db.GetConnection())
                {
                    con.Open();
        
                    using (MySqlTransaction transaction = con.BeginTransaction())
                    {
                        try
                        {
                            string restoreBooksSql = @"
                                UPDATE book bk
                                INNER JOIN bundle_book bb
                                    ON bb.book_id_in_bundle_book = bk.book_id
                                SET
                                    bk.book_status = 'AVAILABLE',
                                    bk.reason_status = NULL
                                WHERE bb.bundle_id_in_bundle_book = @bundleId
                                  AND bk.book_status <> 'SOLD';";
        
                            using (MySqlCommand restoreCmd = new MySqlCommand(restoreBooksSql, con, transaction))
                            {
                                restoreCmd.Parameters.AddWithValue("@bundleId", bundleId);
                                restoreCmd.ExecuteNonQuery();
                            }
        
                            string deleteRelationSql = @"
                                DELETE FROM bundle_book
                                WHERE bundle_id_in_bundle_book = @bundleId;";
        
                            using (MySqlCommand relationCmd = new MySqlCommand(deleteRelationSql, con, transaction))
                            {
                                relationCmd.Parameters.AddWithValue("@bundleId", bundleId);
                                relationCmd.ExecuteNonQuery();
                            }
        
                            string deleteBundleSql = @"
                                DELETE FROM bundle
                                WHERE bundle_id = @bundleId;";
        
                            using (MySqlCommand bundleCmd = new MySqlCommand(deleteBundleSql, con, transaction))
                            {
                                bundleCmd.Parameters.AddWithValue("@bundleId", bundleId);
                                bundleCmd.ExecuteNonQuery();
                            }
        
                            LogHelper.RegistrarLogTrans(
                                con,
                                transaction,
                                "BUNDLE",
                                bundleId,
                                "DELETE",
                                $"Bundle '{bundleId}' deleted"
                            );
        
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
        
                MessageBox.Show("Bundle successfully deleted.");
        
                frmPrincipal.PrincipalInstance.AbrirForm<frmBundles>();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting bundle: " + ex.Message);
            }
        }

        private void RestoreBooksFromBundle(MySqlConnection con, MySqlTransaction transaction)
        {
            string sql = @"
                UPDATE book bk
                INNER JOIN bundle_book bb
                    ON bb.book_id_in_bundle_book = bk.book_id
                SET
                    bk.book_status = 'AVAILABLE',
                    bk.reason_status = NULL
                WHERE bb.bundle_id_in_bundle_book = @bundleId
                  AND bk.book_status <> 'SOLD';";
        
            using (MySqlCommand cmd = new MySqlCommand(sql, con, transaction))
            {
                cmd.Parameters.AddWithValue("@bundleId", bundleId);
                cmd.ExecuteNonQuery();
            }
        }
        
        private void dgvBooksBundle_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
        
            DataGridViewRow row = dgvBooksBundle.Rows[e.RowIndex];
        
            if (row.Cells["IdInterno"].Value == null)
                return;
        
            string selectedBookId = row.Cells["IdInterno"].Value.ToString();
        
            ClassBook book = GetBookById(selectedBookId);
        
            if (book == null)
            {
                MessageBox.Show("Book not found.");
                return;
            }
        
            frmPrincipal.PrincipalInstance.OpenBookForm(book, BookFormMode.Edit);
        }

        private void btnMarkSold_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text == "SOLD")
            {
                MessageBox.Show("This bundle is already sold.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Mark this bundle as SOLD?\n\nAll books inside this bundle will also be marked as SOLD.",
                "Confirm Sale",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (MySqlConnection con = Db.GetConnection())
                {
                    con.Open();

                    using (MySqlTransaction transaction = con.BeginTransaction())
                    {
                        try
                        {
                            string updateBundleSql = @"
                        UPDATE bundle
                        SET bundle_status = 'SOLD'
                        WHERE bundle_id = @bundleId;";

                            using (MySqlCommand cmd = new MySqlCommand(updateBundleSql, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@bundleId", bundleId);
                                cmd.ExecuteNonQuery();
                            }

                            string updateBooksSql = @"
                        UPDATE book bk
                        INNER JOIN bundle_book bb
                            ON bb.book_id_in_bundle_book = bk.book_id
                        SET
                            bk.book_status = 'SOLD',
                            bk.reason_status = NULL
                        WHERE bb.bundle_id_in_bundle_book = @bundleId;";

                            using (MySqlCommand cmd = new MySqlCommand(updateBooksSql, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@bundleId", bundleId);
                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();

                            MessageBox.Show("Bundle and books marked as SOLD.");

                            CarregarResumoBundle();
                            CarregarLivrosDoBundle();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error marking bundle as sold: " + ex.Message);
            }
        }




        private void btnDelete_Paint(object sender, PaintEventArgs e)
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

        private void btnRemove_Paint(object sender, PaintEventArgs e)
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

        private void btnEdit_Paint(object sender, PaintEventArgs e)
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

        private void btnBack_Paint(object sender, PaintEventArgs e)
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

        
    }
}
