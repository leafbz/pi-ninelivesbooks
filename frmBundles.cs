using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ninelivesbooks
{
    public partial class frmBundles : Form
    {
        private string _selectedBundleId = null;
        private string _selectedBundleStatus = null;

        public frmBundles()
        {
            InitializeComponent();
            
            this.Load -= frmBundles_Load;
            this.Load += frmBundles_Load;
        }

        private void frmBundles_Load(object sender, EventArgs e)
        {
            ConfigureGrid();
            WireEvents();
            ClearPreview();
            LoadBundles();
        }

        private void WireEvents()
        {
            btnAdd.Click -= btnAdd_Click;
            btnAdd.Click += btnAdd_Click;

            btnView.Click -= btnView_Click;
            btnView.Click += btnView_Click;

            btnEdit.Click -= btnEdit_Click;
            btnEdit.Click += btnEdit_Click;

            btnDelete.Click -= btnDelete_Click;
            btnDelete.Click += btnDelete_Click;

            btnMarkSold.Click -= btnMarkSold_Click;
            btnMarkSold.Click += btnMarkSold_Click;

            dgvBundles.SelectionChanged -= dgvBundles_SelectionChanged;
            dgvBundles.SelectionChanged += dgvBundles_SelectionChanged;
        }

        private void ConfigureGrid()
        {
            dgvBundles.AutoGenerateColumns = true;
            dgvBundles.ReadOnly = true;
            dgvBundles.AllowUserToAddRows = false;
            dgvBundles.AllowUserToDeleteRows = false;
            dgvBundles.MultiSelect = false;
            dgvBundles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBundles.RowHeadersVisible = false;
            dgvBundles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvBundles.BackgroundColor = Color.FromArgb(255, 245, 220);
            dgvBundles.BorderStyle = BorderStyle.None;

            dgvBundles.EnableHeadersVisualStyles = false;
            dgvBundles.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 60, 40);
            dgvBundles.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(255, 240, 200);
            dgvBundles.ColumnHeadersDefaultCellStyle.Font = new Font("Georgia", 10F, FontStyle.Bold);

            dgvBundles.DefaultCellStyle.BackColor = Color.FromArgb(255, 245, 220);
            dgvBundles.DefaultCellStyle.ForeColor = Color.Black;
            dgvBundles.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 215, 160);
            dgvBundles.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvBundles.DefaultCellStyle.Font = new Font("Georgia", 10F);
        }

        private void LoadBundles()
        {
            string query = @"
                SELECT
                    b.bundle_id AS ID,
                    b.bundle_name AS `Bundle Name`,
                    b.bundle_theme AS Theme,
                    COUNT(bb.book_id_in_bundle_book) AS `Total Books`,
                    b.bundle_price AS `Total Price`,
                    b.bundle_status AS Status,
                    b.bundle_updated_at AS `Updated At`
                FROM bundle b
                LEFT JOIN bundle_book bb
                    ON bb.bundle_id_in_bundle_book = b.bundle_id
                GROUP BY
                    b.bundle_id,
                    b.bundle_name,
                    b.bundle_theme,
                    b.bundle_price,
                    b.bundle_status,
                    b.bundle_updated_at
                ORDER BY b.bundle_updated_at DESC, b.bundle_id DESC;";

            try
            {
                using (var conn = Db.GetConnection())
                using (var da = new MySqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvBundles.DataSource = dt;
                }

                if (dgvBundles.Columns["ID"] != null)
                    dgvBundles.Columns["ID"].Width = 90;

                if (dgvBundles.Rows.Count > 0)
                {
                    dgvBundles.ClearSelection();
                    dgvBundles.Rows[0].Selected = true;
                    dgvBundles.CurrentCell = dgvBundles.Rows[0].Cells["ID"];
                
                    _selectedBundleId = dgvBundles.Rows[0].Cells["ID"].Value.ToString();
                    LoadPreview(_selectedBundleId);
                }
                else
                {
                    ClearPreview();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading bundles: " + ex.Message);
            }
        }

        private void dgvBundles_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvBundles.CurrentRow == null)
            {
                ClearPreview();
                return;
            }

            object idValue = dgvBundles.CurrentRow.Cells["ID"].Value;

            if (idValue == null || idValue == DBNull.Value)
            {
                ClearPreview();
                return;
            }

            _selectedBundleId = idValue.ToString();
            LoadPreview(_selectedBundleId);
        }

        private void LoadPreview(string bundleId)
        {
            string query = @"
                SELECT
                    b.bundle_id,
                    b.bundle_name,
                    b.bundle_status,
                    b.bundle_theme,
                    b.bundle_price,
                    b.bundle_approx_weight,
                    b.bundle_description,
                    b.bundle_image,
                    COUNT(bb.book_id_in_bundle_book) AS total_books
                FROM bundle b
                LEFT JOIN bundle_book bb
                    ON bb.bundle_id_in_bundle_book = b.bundle_id
                WHERE b.bundle_id = @bundle_id
                GROUP BY
                    b.bundle_id,
                    b.bundle_name,
                    b.bundle_status,
                    b.bundle_theme,
                    b.bundle_price,
                    b.bundle_approx_weight,
                    b.bundle_description,
                    b.bundle_image;";

            try
            {
                using (var conn = Db.GetConnection())
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@bundle_id", bundleId);

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            _selectedBundleStatus = reader["bundle_status"].ToString();

                            lblBundleName.Text = reader["bundle_name"].ToString();
                            lblTheme.Text = reader["bundle_theme"].ToString();
                            lblTotalBooks.Text = reader["total_books"].ToString();
                            lblTotalPrice.Text = Convert.ToDecimal(reader["bundle_price"]).ToString("C");
                            lblStatus.Text = reader["bundle_status"].ToString();
                            txtDescription.Text = reader["bundle_description"] == DBNull.Value ? "" : reader["bundle_description"].ToString();

                            if (reader["bundle_image"] != DBNull.Value)
                            {
                                byte[] imgBytes = (byte[])reader["bundle_image"];

                                using (MemoryStream ms = new MemoryStream(imgBytes))
                                {
                                    picBundle.Image = Image.FromStream(new MemoryStream(ms.ToArray()));
                                }
                            }
                            else
                            {
                                picBundle.Image = null;
                            }

                            btnView.Enabled = true;
                            btnEdit.Enabled = true;
                            btnDelete.Enabled = true;
                            btnMarkSold.Enabled = _selectedBundleStatus != "SOLD";
                        }
                        else
                        {
                            ClearPreview();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading bundle preview: " + ex.Message);
            }
        }

        private void ClearPreview()
        {
            _selectedBundleId = null;
            _selectedBundleStatus = null;

            lblBundleName.Text = "";
            lblTheme.Text = "";
            lblTotalBooks.Text = "";
            lblTotalPrice.Text = "";
            lblStatus.Text = "";
            txtDescription.Text = "";
            picBundle.Image = null;

            btnView.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnMarkSold.Enabled = false;
        }

        private bool HasSelectedBundle()
        {
            if (string.IsNullOrWhiteSpace(_selectedBundleId))
            {
                MessageBox.Show("Select a bundle first.");
                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmPrincipal.PrincipalInstance.AbrirForm<frmAddBundle>();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (!HasSelectedBundle())
                return;
        
            frmPrincipal.PrincipalInstance.AbrirForm(
                new FrmBundleDetails(_selectedBundleId)
            );
        }

        //   LoadBundles();
        
        //    if (!string.IsNullOrWhiteSpace(_selectedBundleId))
       //         LoadPreview(_selectedBundleId);
        

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!HasSelectedBundle())
                return;
        
            frmPrincipal.PrincipalInstance.AbrirForm(
                new frmAddBundle(_selectedBundleId)
            );
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!HasSelectedBundle())
                return;

            if (_selectedBundleStatus == "SOLD")
            {
                MessageBox.Show("Sold bundles should not be deleted.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Delete this bundle?\n\nBooks inside the bundle will become AVAILABLE again.",
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

                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            RestoreBooksFromBundle(conn, transaction, _selectedBundleId);
                            DeleteBundleBooks(conn, transaction, _selectedBundleId);
                            DeleteBundle(conn, transaction, _selectedBundleId);

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                MessageBox.Show("Bundle deleted successfully.");
                LoadBundles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting bundle: " + ex.Message);
            }
        }

        private void btnMarkSold_Click(object sender, EventArgs e)
        {
            if (!HasSelectedBundle())
                return;

            MarkSelectedBundleAsSold();
        }

        private void MarkSelectedBundleAsSold()
        {
            if (_selectedBundleStatus == "SOLD")
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
                using (var conn = Db.GetConnection())
                {
                    conn.Open();

                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            string updateBundleSql = @"
                                UPDATE bundle
                                SET bundle_status = 'SOLD'
                                WHERE bundle_id = @bundle_id;";

                            using (var cmd = new MySqlCommand(updateBundleSql, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@bundle_id", _selectedBundleId);
                                cmd.ExecuteNonQuery();
                            }

                            string updateBooksSql = @"
                                UPDATE book bk
                                INNER JOIN bundle_book bb
                                    ON bb.book_id_in_bundle_book = bk.book_id
                                SET
                                    bk.book_status = 'SOLD',
                                    bk.reason_status = NULL
                                WHERE bb.bundle_id_in_bundle_book = @bundle_id;";

                            using (var cmd = new MySqlCommand(updateBooksSql, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@bundle_id", _selectedBundleId);
                                cmd.ExecuteNonQuery();
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

                MessageBox.Show("Bundle and books marked as SOLD.");
                LoadBundles();
                LoadPreview(_selectedBundleId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error marking bundle as sold: " + ex.Message);
            }
        }

        private void RestoreBooksFromBundle(MySqlConnection conn, MySqlTransaction transaction, string bundleId)
        {
            string sql = @"
                UPDATE book bk
                INNER JOIN bundle_book bb
                    ON bb.book_id_in_bundle_book = bk.book_id
                SET
                    bk.book_status = 'AVAILABLE',
                    bk.reason_status = NULL
                WHERE bb.bundle_id_in_bundle_book = @bundle_id;";

            using (var cmd = new MySqlCommand(sql, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@bundle_id", bundleId);
                cmd.ExecuteNonQuery();
            }
        }

        private void DeleteBundleBooks(MySqlConnection conn, MySqlTransaction transaction, string bundleId)
        {
            string sql = @"
                DELETE FROM bundle_book
                WHERE bundle_id_in_bundle_book = @bundle_id;";

            using (var cmd = new MySqlCommand(sql, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@bundle_id", bundleId);
                cmd.ExecuteNonQuery();
            }
        }

        private void DeleteBundle(MySqlConnection conn, MySqlTransaction transaction, string bundleId)
        {
            string sql = @"
                DELETE FROM bundle
                WHERE bundle_id = @bundle_id;";

            using (var cmd = new MySqlCommand(sql, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@bundle_id", bundleId);
                cmd.ExecuteNonQuery();
            }

            LogHelper.RegistrarLogTrans(
                       conn,
                       transaction,
                       "BUNDLE",
                       _selectedBundleId,
                       "DELETE",
                       $"Bundle '{_selectedBundleId}' deleted"
                   );

        }
    }
}
