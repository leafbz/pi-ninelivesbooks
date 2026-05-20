using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ninelivesbooks
{
    public partial class frmAddBundle : Form
    {
        private readonly Color headerBack = Color.FromArgb(0, 60, 40);
        private readonly Color headerFore = Color.FromArgb(255, 240, 200);
        private readonly Color rowEven = Color.FromArgb(255, 245, 220);
        private readonly Color rowOdd = Color.FromArgb(255, 235, 200);
        private readonly Color gridColor = Color.FromArgb(255, 215, 160);

        private readonly List<BundleBookItem> availableBooks = new List<BundleBookItem>();
        private readonly List<BundleBookItem> bundleBooks = new List<BundleBookItem>();
        
        private bool _isEditMode = false;
        private string _currentBundleId = null;


        public frmAddBundle()
        {
            InitializeComponent();

            SetupListViews();
            WireEvents();
        }

        public frmAddBundle(string bundleId)
        {
            InitializeComponent();
        
            _isEditMode = true;
            _currentBundleId = bundleId;
        
            SetupListViews();
            WireEvents();
        }

        private void WireEvents()
        {
            this.Load -= frmAddBundle_Load;
            this.Load += frmAddBundle_Load;

            btnAdd.Click -= btnAdd_Click;
            btnAdd.Click += btnAdd_Click;

            btnRemove.Click -= btnRemove_Click;
            btnRemove.Click += btnRemove_Click;

            btnSearch.Click -= btnSearch_Click;
            btnSearch.Click += btnSearch_Click;

            txtSearch.TextChanged -= txtSearch_TextChanged;
            txtSearch.TextChanged += txtSearch_TextChanged;

            btnClear.Click -= btnClear_Click;
            btnClear.Click += btnClear_Click;

            btnAddImg.Click -= btnAddImg_Click;
            btnAddImg.Click += btnAddImg_Click;

            btnClearImg.Click -= btnClearImg_Click;
            btnClearImg.Click += btnClearImg_Click;

            BtnSaveBundle.Click -= BtnSaveBundle_Click;
            BtnSaveBundle.Click += BtnSaveBundle_Click;

            btnCancel.Click -= btnCancel_Click;
            btnCancel.Click += btnCancel_Click;
        }

        private void SetupListViews()
        {
            SetupListViewBase(lstBook);
            SetupListViewBase(lstBookBundle);

            lstBook.Columns.Clear();
            lstBook.Columns.Add("ID", 126, HorizontalAlignment.Left);
            lstBook.Columns.Add("Title", 250, HorizontalAlignment.Left);
            lstBook.Columns.Add("Author", 170, HorizontalAlignment.Left);
            lstBook.Columns.Add("Condition", 135, HorizontalAlignment.Left);
            lstBook.Columns.Add("Price", 115, HorizontalAlignment.Left);

            lstBookBundle.Columns.Clear();
            lstBookBundle.Columns.Add("ID", 126, HorizontalAlignment.Left);
            lstBookBundle.Columns.Add("Title", 220, HorizontalAlignment.Left);
            lstBookBundle.Columns.Add("Author", 160, HorizontalAlignment.Left);
            lstBookBundle.Columns.Add("Price", 115, HorizontalAlignment.Left);

            lstBook.DrawColumnHeader -= ListView_DrawColumnHeader;
            lstBook.DrawItem -= ListView_DrawItem;
            lstBook.DrawSubItem -= ListView_DrawSubItem;

            lstBook.DrawColumnHeader += ListView_DrawColumnHeader;
            lstBook.DrawItem += ListView_DrawItem;
            lstBook.DrawSubItem += ListView_DrawSubItem;

            lstBookBundle.DrawColumnHeader -= ListView_DrawColumnHeader;
            lstBookBundle.DrawItem -= ListView_DrawItem;
            lstBookBundle.DrawSubItem -= ListView_DrawSubItem;

            lstBookBundle.DrawColumnHeader += ListView_DrawColumnHeader;
            lstBookBundle.DrawItem += ListView_DrawItem;
            lstBookBundle.DrawSubItem += ListView_DrawSubItem;
        }

        private void SetupListViewBase(ListView listView)
        {
            listView.BackColor = Color.FromArgb(255, 245, 220);
            listView.BorderStyle = BorderStyle.FixedSingle;
            listView.View = View.Details;
            listView.MultiSelect = true;
            listView.AllowColumnReorder = false;
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.HideSelection = false;
            listView.OwnerDraw = true;
            listView.HotTracking = false;
            listView.HoverSelection = false;
            listView.Activation = ItemActivation.Standard;

            listView.ColumnWidthChanging += (s, e) =>
            {
                e.Cancel = true;
                e.NewWidth = listView.Columns[e.ColumnIndex].Width;
            };
        }
        
        private void frmAddBundle_Load(object sender, EventArgs e)
        {
            LoadAvailableBooks();
        
            if (_isEditMode)
            {
                LoadBundleForEdit(_currentBundleId);
                BtnSaveBundle.Text = "Save Changes";
            }
        
            LoadAvailableBooksIntoListView();
            LoadBundleBooksIntoListView();
        
            UpdateBundleWeight();
            UpdateBundlePrice();
        }

        private void LoadAvailableBooks()
        {
            availableBooks.Clear();

            string sql = @"
                SELECT
                    b.book_id,
                    bt.title_id,
                    b.price,
                    b.book_condition,
                    bt.title,
                    bt.author,
                    bt.book_approx_weight
                FROM book b
                INNER JOIN book_titles bt
                    ON b.title_id_in_book = bt.title_id
                WHERE b.book_status = 'AVAILABLE'
                AND NOT EXISTS (
                    SELECT 1
                    FROM bundle_book bb
                    WHERE bb.book_id_in_bundle_book = b.book_id
                )
                ORDER BY bt.title ASC, b.book_id ASC;";
                
            try
            {
                using (var conn = Db.GetConnection())
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            availableBooks.Add(MapBundleBookItem(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading available books:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void SearchAvailableBooks()
        {
            availableBooks.Clear();

            string sql = @"
                SELECT
                    b.book_id,
                    bt.title_id,
                    b.price,
                    b.book_condition,
                    bt.title,
                    bt.author,
                    bt.book_approx_weight
                FROM book b
                INNER JOIN book_titles bt
                    ON b.title_id_in_book = bt.title_id
                WHERE b.book_status = 'AVAILABLE'
                AND (
                    bt.title LIKE @q
                    OR bt.author LIKE @q
                    OR b.book_id LIKE @q
                    OR bt.iSBN LIKE @q
                    OR bt.genre LIKE @q
                )
                ORDER BY bt.title ASC, b.book_id ASC;";

            try
            {
                using (var conn = Db.GetConnection())
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@q", "%" + txtSearch.Text.Trim() + "%");

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            availableBooks.Add(MapBundleBookItem(reader));
                        }
                    }
                }

                LoadAvailableBooksIntoListView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error searching books:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private BundleBookItem MapBundleBookItem(MySqlDataReader reader)
        {
            return new BundleBookItem
            {
                BookId = reader["book_id"].ToString(),
                TitleId = reader["title_id"].ToString(),
                Title = reader["title"].ToString(),
                Author = reader["author"].ToString(),
                Condition = reader["book_condition"].ToString(),
                Price = Convert.ToDecimal(reader["price"]),
                ApproxWeight = Convert.ToDecimal(reader["book_approx_weight"])
            };
        }

        private void LoadAvailableBooksIntoListView()
        {
            lstBook.Items.Clear();

            foreach (BundleBookItem book in availableBooks)
            {
                ListViewItem item = new ListViewItem(book.BookId);
                item.SubItems.Add(book.Title);
                item.SubItems.Add(book.Author);
                item.SubItems.Add(book.Condition);
                item.SubItems.Add(book.Price.ToString("C", CultureInfo.CurrentCulture));
                item.Tag = book;

                lstBook.Items.Add(item);
            }
        }

        private void LoadBundleBooksIntoListView()
        {
            lstBookBundle.Items.Clear();

            foreach (BundleBookItem book in bundleBooks)
            {
                ListViewItem item = new ListViewItem(book.BookId);
                item.SubItems.Add(book.Title);
                item.SubItems.Add(book.Author);
                item.SubItems.Add(book.Price.ToString("C", CultureInfo.CurrentCulture));
                item.Tag = book;

                lstBookBundle.Items.Add(item);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (lstBook.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select at least one book.");
                return;
            }

            foreach (ListViewItem item in lstBook.SelectedItems)
            {
                BundleBookItem book = (BundleBookItem)item.Tag;

                if (bundleBooks.Any(b => b.BookId == book.BookId))
                    continue;

                bundleBooks.Add(book);
            }

            UpdateBundlePrice();
            UpdateBundleWeight();
            LoadBundleBooksIntoListView();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lstBookBundle.SelectedItems.Count == 0)
                return;

            foreach (ListViewItem item in lstBookBundle.SelectedItems)
            {
                BundleBookItem book = (BundleBookItem)item.Tag;
                bundleBooks.RemoveAll(b => b.BookId == book.BookId);
            }

            UpdateBundlePrice();
            UpdateBundleWeight();
            LoadBundleBooksIntoListView();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                LoadAvailableBooks();
                LoadAvailableBooksIntoListView();
            }
            else
            {
                SearchAvailableBooks();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string search = txtSearch.Text.Trim();

            if (search.Length == 0 || search.Length >= 2)
                btnSearch.PerformClick();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtBundleName.Text = string.Empty;
            txtTheme.Text = string.Empty;
            cbStatus.SelectedIndex = -1;
            txtDescription.Text = string.Empty;
            picImage.Image = null;

            SetPriceText(0);
            txtWeight.Text = "0.0 g";

            bundleBooks.Clear();
            LoadBundleBooksIntoListView();
            UpdateBundlePrice();
            UpdateBundleWeight();

            txtBundleName.Focus();
        }

        private void btnAddImg_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    picImage.Image = new Bitmap(dlg.FileName);
                }
            }
        }

        private void btnClearImg_Click(object sender, EventArgs e)
        {
            picImage.Image = null;
        }

        private byte[] GetImageBytesFromPictureBox()
        {
            if (picImage.Image == null)
                return null;

            using (MemoryStream ms = new MemoryStream())
            {
                picImage.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        private void BtnSaveBundle_Click(object sender, EventArgs e)
        {
            if (!ValidateBundle())
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
                            if (_isEditMode)
                            {
                                List<string> oldBookIds = GetCurrentBundleBookIds(_currentBundleId, conn, transaction);
                            
                                UpdateBundle(_currentBundleId, conn, transaction);
                                ReplaceBundleBooks(_currentBundleId, conn, transaction);
                            
                                List<string> newBookIds = bundleBooks.Select(b => b.BookId).ToList();
                            
                                List<string> removedBookIds = oldBookIds
                                    .Except(newBookIds)
                                    .ToList();
                            
                                RestoreRemovedBooksToAvailable(removedBookIds, conn, transaction);
                            
                                MarkBundledBooksAsUnavailable(conn, transaction);
                            }
                            else
                            {
                                string bundleId = GenerateNextBundleId(conn, transaction);
        
                                InsertBundle(bundleId, conn, transaction);
                                InsertBundleBooks(bundleId, conn, transaction);
                                MarkBundledBooksAsUnavailable(conn, transaction);
                            }
        
                            transaction.Commit();
        
                            MessageBox.Show(
                                _isEditMode
                                    ? "Bundle updated successfully."
                                    : "Bundle saved successfully."
                            );
        
                            if (_isEditMode)
                            {
                                frmPrincipal.PrincipalInstance.AbrirForm(
                                    new FrmBundleDetails(_currentBundleId)
                                );
                            
                                return;
                            }
                            else
                            {
                                frmPrincipal.PrincipalInstance.AbrirForm<frmBundles>();
                            }
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(
                    $"Database error {ex.Number}:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error saving bundle:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private bool ValidateBundle()
        {
            if (string.IsNullOrWhiteSpace(txtBundleName.Text))
            {
                MessageBox.Show("Bundle name is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTheme.Text))
            {
                MessageBox.Show("Bundle theme is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(cbStatus.Text))
            {
                MessageBox.Show("Bundle status is required.");
                return false;
            }

            if (bundleBooks.Count == 0)
            {
                MessageBox.Show("Add at least one book to the bundle.");
                return false;
            }

            if (!TryGetBundlePrice(out decimal price) || price < 0)
            {
                MessageBox.Show("Invalid bundle price.");
                return false;
            }

            return true;
        }

        private string GenerateNextBundleId(MySqlConnection conn, MySqlTransaction transaction)
        {
            string query = @"
                SELECT MAX(CAST(SUBSTRING(bundle_id, 4) AS UNSIGNED))
                FROM bundle
                WHERE bundle_id LIKE 'NLB%'";

            using (var cmd = new MySqlCommand(query, conn, transaction))
            {
                object result = cmd.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                    return "NLB0001";

                int number = Convert.ToInt32(result) + 1;
                return "NLB" + number.ToString("D4");
            }
        }

        private void InsertBundle(string bundleId, MySqlConnection conn, MySqlTransaction transaction)
        {
            string sql = @"
                INSERT INTO bundle
                (
                    bundle_id,
                    bundle_name,
                    bundle_status,
                    bundle_theme,
                    bundle_price,
                    bundle_approx_weight,
                    bundle_description,
                    bundle_image
                )
                VALUES
                (
                    @bundle_id,
                    @bundle_name,
                    @bundle_status,
                    @bundle_theme,
                    @bundle_price,
                    @bundle_approx_weight,
                    @bundle_description,
                    @bundle_image
                );";

            using (var cmd = new MySqlCommand(sql, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@bundle_id", bundleId);
                cmd.Parameters.AddWithValue("@bundle_name", txtBundleName.Text.Trim());
                cmd.Parameters.AddWithValue("@bundle_status", cbStatus.Text.Trim());
                cmd.Parameters.AddWithValue("@bundle_theme", txtTheme.Text.Trim());
                cmd.Parameters.AddWithValue("@bundle_price", GetBundlePrice());
                cmd.Parameters.AddWithValue("@bundle_approx_weight", GetBundleTotalWeight());
                cmd.Parameters.AddWithValue("@bundle_description", txtDescription.Text.Trim());
                cmd.Parameters.AddWithValue("@bundle_image", (object)GetImageBytesFromPictureBox() ?? DBNull.Value);

                cmd.ExecuteNonQuery();
            }
        }

        private void InsertBundleBooks(string bundleId, MySqlConnection conn, MySqlTransaction transaction)
        {
            string sql = @"
                INSERT INTO bundle_book
                (
                    bundle_id_in_bundle_book,
                    book_id_in_bundle_book
                )
                VALUES
                (
                    @bundle_id,
                    @book_id
                );";

            foreach (BundleBookItem book in bundleBooks)
            {
                using (var cmd = new MySqlCommand(sql, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@bundle_id", bundleId);
                    cmd.Parameters.AddWithValue("@book_id", book.BookId);
                    cmd.ExecuteNonQuery();
                }

            }
            LogHelper.RegistrarLogTrans(
       conn,
       transaction,
       "BUNDLE",
       bundleId,
       "CREATE",
       $"Bundle '{txtBundleName.Text}' created"
   );
        }

        private void MarkBundledBooksAsUnavailable(MySqlConnection conn, MySqlTransaction transaction)
        {
            string sql = @"
                UPDATE book
                SET
                    book_status = 'UNAVAILABLE',
                    reason_status = 'RemovedFromCuration'
                WHERE book_id = @book_id;";

            foreach (BundleBookItem book in bundleBooks)
            {
                using (var cmd = new MySqlCommand(sql, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@book_id", book.BookId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void UpdateBundleWeight()
        {
            txtWeight.Text = GetBundleTotalWeight().ToString("0.0", CultureInfo.InvariantCulture) + " g";
        }

        private decimal GetBundleTotalWeight()
        {
            decimal totalWeight = 0;

            foreach (BundleBookItem book in bundleBooks)
            {
                totalWeight += book.ApproxWeight;
            }

            return totalWeight;
        }

        private void UpdateBundlePrice()
        {
            decimal totalPrice = 0;

            foreach (BundleBookItem book in bundleBooks)
            {
                totalPrice += book.Price;
            }

            SetPriceText(totalPrice);
        }

        private bool TryGetBundlePrice(out decimal price)
        {
            string text = numPrice.Text
                .Replace("R$", "")
                .Replace("$", "")
                .Trim()
                .Replace(",", ".");

            return decimal.TryParse(
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out price
            );
        }

        private decimal GetBundlePrice()
        {
            TryGetBundlePrice(out decimal price);
            return price;
        }

        private void SetPriceText(decimal value)
        {
            numPrice.Text = value.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_isEditMode)
            {
                frmPrincipal.PrincipalInstance.AbrirForm(
                    new FrmBundleDetails(_currentBundleId)
                );
        
                return;
            }
        
            frmPrincipal.PrincipalInstance.AbrirForm<frmBundles>();
        }

        private void ListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (SolidBrush back = new SolidBrush(headerBack))
            {
                e.Graphics.FillRectangle(back, e.Bounds);
            }

            TextRenderer.DrawText(
                e.Graphics,
                e.Header.Text,
                new Font("Georgia", 10F, FontStyle.Bold),
                e.Bounds,
                headerFore,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
            );

            using (Pen pen = new Pen(gridColor))
            {
                e.Graphics.DrawRectangle(pen, e.Bounds);
            }
        }

        private void ListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            Color back = (e.ItemIndex % 2 == 0) ? rowEven : rowOdd;

            if (e.Item.Selected)
                back = Color.FromArgb(255, 215, 160);

            using (SolidBrush brush = new SolidBrush(back))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }
        }

        private void ListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            Color backColor;
            Color textColor;

            if (e.Item.Selected)
            {
                textColor = Color.Black;
                backColor = Color.FromArgb(255, 215, 160);
            }
            else if ((e.ItemState & ListViewItemStates.Hot) != 0)
            {
                backColor = Color.FromArgb(255, 230, 190);
                textColor = e.ColumnIndex == 0
                    ? Color.FromArgb(0, 80, 60)
                    : Color.FromArgb(20, 60, 40);
            }

            using (SolidBrush back = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(back, e.Bounds);
            }

            using (Font font = new Font("Georgia", 10F, FontStyle.Bold))
            {
                TextRenderer.DrawText(
                    e.Graphics,
                    e.SubItem.Text,
                    font,
                    e.Bounds,
                    textColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter
                );
            }

            using (Pen pen = new Pen(gridColor))
            {
                e.Graphics.DrawRectangle(pen, e.Bounds);
            }
        }

        private void LoadBundleForEdit(string bundleId)
        {
            LoadBundleInfo(bundleId);
            LoadBundleBooksForEdit(bundleId);
        }
        
        private void LoadBundleInfo(string bundleId)
        {
            string query = @"
                SELECT
                    bundle_name,
                    bundle_status,
                    bundle_theme,
                    bundle_price,
                    bundle_description,
                    bundle_image
                FROM bundle
                WHERE bundle_id = @bundle_id
                LIMIT 1;";
        
            using (var conn = Db.GetConnection())
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@bundle_id", bundleId);
        
                conn.Open();
        
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtBundleName.Text = reader["bundle_name"].ToString();
                        cbStatus.Text = reader["bundle_status"].ToString();
                        txtTheme.Text = reader["bundle_theme"].ToString();
                        numPrice.Text = Convert.ToDecimal(reader["bundle_price"]).ToString("0.00");
                        txtDescription.Text = reader["bundle_description"] == DBNull.Value
                            ? ""
                            : reader["bundle_description"].ToString();
        
                        if (reader["bundle_image"] != DBNull.Value)
                        {
                            byte[] imgBytes = (byte[])reader["bundle_image"];
        
                            using (MemoryStream ms = new MemoryStream(imgBytes))
                            {
                                picImage.Image = Image.FromStream(new MemoryStream(ms.ToArray()));
                            }
                        }
                    }
                }
            }
        }
        
        private void LoadBundleBooksForEdit(string bundleId)
        {
            bundleBooks.Clear();
        
            string query = @"
                SELECT
                    b.book_id,
                    bt.title_id,
                    b.price,
                    b.book_condition,
                    bt.title,
                    bt.author,
                    bt.book_approx_weight
                FROM bundle_book bb
                INNER JOIN book b
                    ON b.book_id = bb.book_id_in_bundle_book
                INNER JOIN book_titles bt
                    ON bt.title_id = b.title_id_in_book
                WHERE bb.bundle_id_in_bundle_book = @bundle_id
                ORDER BY bt.title ASC;";
        
            using (var conn = Db.GetConnection())
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@bundle_id", bundleId);
        
                conn.Open();
        
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bundleBooks.Add(MapBundleBookItem(reader));
                    }
                }
            }
        }

        private void UpdateBundle(string bundleId, MySqlConnection conn, MySqlTransaction transaction)
        {
            string sql = @"
                UPDATE bundle
                SET
                    bundle_name = @bundle_name,
                    bundle_status = @bundle_status,
                    bundle_theme = @bundle_theme,
                    bundle_price = @bundle_price,
                    bundle_approx_weight = @bundle_approx_weight,
                    bundle_description = @bundle_description,
                    bundle_image = @bundle_image
                WHERE bundle_id = @bundle_id;";
        
            using (var cmd = new MySqlCommand(sql, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@bundle_id", bundleId);
                cmd.Parameters.AddWithValue("@bundle_name", txtBundleName.Text.Trim());
                cmd.Parameters.AddWithValue("@bundle_status", cbStatus.Text.Trim());
                cmd.Parameters.AddWithValue("@bundle_theme", txtTheme.Text.Trim());
                cmd.Parameters.AddWithValue("@bundle_price", GetBundlePrice());
                cmd.Parameters.AddWithValue("@bundle_approx_weight", GetBundleTotalWeight());
                cmd.Parameters.AddWithValue("@bundle_description", txtDescription.Text.Trim());
                cmd.Parameters.AddWithValue("@bundle_image", (object)GetImageBytesFromPictureBox() ?? DBNull.Value);
        
                cmd.ExecuteNonQuery();
            }

           

        }

        private void ReplaceBundleBooks(string bundleId, MySqlConnection conn, MySqlTransaction transaction)
        {
            string deleteSql = @"
                DELETE FROM bundle_book
                WHERE bundle_id_in_bundle_book = @bundle_id;";
        
            using (var deleteCmd = new MySqlCommand(deleteSql, conn, transaction))
            {
                deleteCmd.Parameters.AddWithValue("@bundle_id", bundleId);
                deleteCmd.ExecuteNonQuery();
            }

           
            LogHelper.RegistrarLogTrans(
                   conn,
                   transaction,
                   "BUNDLE",
                   bundleId,
                   "UPDATE",
                   $"Bundle '{txtBundleName.Text}' updated"
               );
            InsertBundleBooks(bundleId, conn, transaction);
        }


        #region Round Buttons
        private void RoundButton(object sender)
        {
            if (!(sender is Button btn))
                return;

            int radius = 20;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.StartFigure();
                path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
                path.AddArc(new Rectangle(btn.Width - radius, 0, radius, radius), 270, 90);
                path.AddArc(new Rectangle(btn.Width - radius, btn.Height - radius, radius, radius), 0, 90);
                path.AddArc(new Rectangle(0, btn.Height - radius, radius, radius), 90, 90);
                path.CloseFigure();

                btn.Region = new Region(path);
            }
        }
        #endregion
    }
}

