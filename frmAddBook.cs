using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace ninelivesbooks
{
    public partial class frmAddBook : Form
    {
        private BookFormMode _mode = BookFormMode.Add;
        private ClassBook _selectedBook;
        private bool _loadingForm = false;
        private bool _returnToArchive = false;
        private string _currentBookId;
        private string _currentTitleId;

        public frmAddBook()
        {
            InitializeComponent();
            _mode = BookFormMode.Add;
        }

        public frmAddBook(ClassBook book, BookFormMode mode)
        {
            InitializeComponent();
            _selectedBook = book;
            _mode = mode;
        }

        public frmAddBook(ClassBook book, BookFormMode mode, bool returnToArchive)
        {
            InitializeComponent();

            _selectedBook = book;
            _mode = mode;
            _returnToArchive = returnToArchive;
        }

        private void frmAddBook_Load(object sender, EventArgs e)
        {
            _loadingForm = true;

            SetupNumericControls();
            LoadCombos();
            SetupAutoComplete();
            WireEvents();

            if (_mode == BookFormMode.Add)
            {
                PrepareAddMode();
            }
            else
            {
                FillForm(_selectedBook);
                SetMode(_mode);
            }

            _loadingForm = false;
        }

        private void WireEvents()
        {
            btnSave.Click -= btnSave_Click;
            btnSave.Click += btnSave_Click;

            btnEdit.Click -= btnEdit_Click;
            btnEdit.Click += btnEdit_Click;

            btnCreateCopy.Click -= btnCreateCopy_Click;
            btnCreateCopy.Click += btnCreateCopy_Click;

            btnCancel.Click -= btnCancel_Click;
            btnCancel.Click += btnCancel_Click;

            btnAddImg.Click -= btnAddImg_Click;
            btnAddImg.Click += btnAddImg_Click;

            btnRemoveImg.Click -= btnRemoveImg_Click;
            btnRemoveImg.Click += btnRemoveImg_Click;

            btnDelete.Click -= btnDelete_Click;
            btnDelete.Click += btnDelete_Click;

            cmbBookType.SelectedIndexChanged -= cmbBookType_SelectedIndexChanged;
            cmbBookType.SelectedIndexChanged += cmbBookType_SelectedIndexChanged;

            txtISBN.Leave -= txtISBN_Leave;
            txtISBN.Leave += txtISBN_Leave;

            numPublicationYear.Enter -= NumericUpDown_Enter;
            numPages.Enter -= NumericUpDown_Enter;
            numWeight.Enter -= NumericUpDown_Enter;
            numPrice.Enter -= NumericUpDown_Enter;

            numPublicationYear.Enter += NumericUpDown_Enter;
            numPages.Enter += NumericUpDown_Enter;
            numWeight.Enter += NumericUpDown_Enter;
            numPrice.Enter += NumericUpDown_Enter;
        }

        private void SetupNumericControls()
        {
            int currentYear = DateTime.Now.Year;

            numPublicationYear.Minimum = 1450;
            numPublicationYear.Maximum = currentYear;
            numPublicationYear.Value = currentYear;
            numPublicationYear.Increment = 1;

            numPages.Minimum = 1;
            numPages.Maximum = 5000;
            numPages.Value = 1;
            numPages.Increment = 1;

            numWeight.Minimum = 1;
            numWeight.Maximum = 2000;
            numWeight.DecimalPlaces = 2;
            numWeight.Increment = 10;

            numPrice.Minimum = 0;
            numPrice.Maximum = 9999;
            numPrice.DecimalPlaces = 2;
            numPrice.Increment = 0.50M;
        }

        private void LoadCombos()
        {
            cmbBookType.Items.Clear();
            cmbBookType.Items.AddRange(new string[] { "PB", "TPB", "HB" });

            cmbCondition.Items.Clear();
            cmbCondition.Items.AddRange(new string[] { "NEW", "VERY GOOD", "GOOD", "ACCEPTABLE" });

            cmbStatus.Items.Clear();
            cmbStatus.Items.AddRange(new string[] { "AVAILABLE", "SOLD", "UNAVAILABLE" });

            cmbReasonStatus.Items.Clear();
            cmbReasonStatus.Items.AddRange(new string[]
            {
                "Damaged",
                "ManufacturingDefect",
                "CustomerDamage",
                "RemovedFromCuration",
                "Lost",
                "Other"
            });

            cmbOrigin.Items.Clear();
            cmbOrigin.Items.AddRange(new string[] { "Donation", "Second-hand", "Other" });
        }

        private void PrepareAddMode()
        {
            ClearForm();

            _currentBookId = GenerateNextBookId();
            _currentTitleId = GenerateNextTitleId();

            SetMode(BookFormMode.Add);
        }

        private void SetMode(BookFormMode mode)
        {
            _mode = mode;

            bool isView = mode == BookFormMode.View;
            bool isEdit = mode == BookFormMode.Edit;
            bool isAdd = mode == BookFormMode.Add;
            bool isAddCopy = mode == BookFormMode.AddCopy;

            SetFieldsReadOnly(isView);

            lblBookId.Text = string.IsNullOrWhiteSpace(_currentBookId) ? "" : _currentBookId;
            lblBookId.Visible = isView || isEdit;

            btnSave.Visible = isAdd || isEdit || isAddCopy;
            btnEdit.Visible = isView;
            btnCreateCopy.Visible = isView;
            btnCancel.Visible = true;
            btnDelete.Visible = isView || isEdit;
            btnArchive.Visible = isView || isEdit;

            if (mode == BookFormMode.AddCopy)
            {
                txtISBN.ReadOnly = true;
                txtTitle.ReadOnly = true;
                txtAuthor.ReadOnly = true;
                txtPublisher.ReadOnly = true;
                txtLanguage.ReadOnly = true;
                txtGenre.ReadOnly = true;
                txtDescription.ReadOnly = true;

                numPages.Enabled = false;
                numWeight.Enabled = false;
                numPublicationYear.Enabled = false;

                cmbBookType.Enabled = false;

                numPrice.Enabled = true;
                cmbCondition.Enabled = true;
                cmbStatus.Enabled = true;
                cmbReasonStatus.Enabled = true;
                cmbOrigin.Enabled = true;

                txtDefectedNotes.ReadOnly = false;

                lblBookId.Visible = false;

                btnSave.Text = "Create Copy";
            }

            if (isAdd)
                btnSave.Text = "Add Book";
            else if (isAddCopy)
                btnSave.Text = "Create Copy";
            else
                btnSave.Text = "Save Changes";
        }

        private void SetFieldsReadOnly(bool readOnly)
        {
            txtTitle.ReadOnly = readOnly;
            txtAuthor.ReadOnly = readOnly;
            txtISBN.ReadOnly = readOnly;
            txtPublisher.ReadOnly = readOnly;
            txtLanguage.ReadOnly = readOnly;
            txtGenre.ReadOnly = readOnly;
            txtDescription.ReadOnly = readOnly;
            txtDefectedNotes.ReadOnly = readOnly;

            numPages.Enabled = !readOnly;
            numWeight.Enabled = !readOnly;
            numPublicationYear.Enabled = !readOnly;
            numPrice.Enabled = !readOnly;

            cmbBookType.Enabled = !readOnly;
            cmbCondition.Enabled = !readOnly;
            cmbStatus.Enabled = !readOnly;
            cmbReasonStatus.Enabled = !readOnly;
            cmbOrigin.Enabled = !readOnly;

            btnAddImg.Enabled = !readOnly;
            btnRemoveImg.Enabled = !readOnly;
        }

        private void FillForm(ClassBook book)
        {
            if (book == null) return;

            _currentBookId = string.IsNullOrWhiteSpace(book.BookId) ? GenerateNextBookId() : book.BookId;

            _currentTitleId = book.TitleId;

            lblBookId.Text = _currentBookId;

            txtTitle.Text = book.Title;
            txtAuthor.Text = book.Author;
            txtISBN.Text = book.ISBN;
            txtPublisher.Text = book.Publisher;
            txtLanguage.Text = book.Language;
            txtGenre.Text = book.Genre;
            txtDescription.Text = book.Description;

            SetNumericValue(numPages, book.Pages);
            SetNumericValue(numWeight, book.ApproxWeight);
            SetNumericValue(numPublicationYear, book.PublicationYear);
            SetNumericValue(numPrice, book.Price);

            cmbBookType.SelectedItem = book.BookType;
            cmbCondition.SelectedItem = book.Condition;
            cmbStatus.SelectedItem = book.Status;
            cmbReasonStatus.SelectedItem = book.ReasonStatus;
            cmbOrigin.SelectedItem = book.Origin;

            txtDefectedNotes.Text = book.DefectedNotes;

            image.Image = book.CoverImage;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            ClassBook book = GetBookFromForm();

            using (var conn = Db.GetConnection())
            {
                conn.Open();

                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string existingTitleId = GetExistingTitleIdByISBN(book.ISBN, conn, transaction);

                        if (_mode == BookFormMode.Edit)
                        {
                            SaveBookTitle(book, conn, transaction);
                        }
                        else if (_mode == BookFormMode.AddCopy)
                        {
                            book.TitleId = _currentTitleId;
                        }
                        else if (!string.IsNullOrWhiteSpace(existingTitleId))
                        {
                            book.TitleId = existingTitleId;
                            _currentTitleId = existingTitleId;
                        }
                        else
                        {
                            SaveBookTitle(book, conn, transaction);
                        }

                        SaveInventoryBook(book, conn, transaction);
                        UpdateBundleStatusIfBookWasSold(
                            book.BookId,
                            book.Status,
                            conn,
                            transaction
                        );
                        
                        transaction.Commit();
                        
                        if (_returnToArchive)
                        {
                            MessageBox.Show("Book saved successfully.");
                            frmPrincipal.PrincipalInstance.AbrirForm<frmArchive>();
                            return;
                        }
                        if (_mode == BookFormMode.Add)
                        {
                            PrepareAddMode();
                        }
                        else
                        {
                            _selectedBook = book;
                            SetMode(BookFormMode.View);
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error saving book: " + ex.Message);
                    }
                }
            }
        }

        private ClassBook GetBookFromForm()
        {
            return new ClassBook
            {
                BookId = _currentBookId,
                TitleId = _currentTitleId,

                Title = txtTitle.Text.Trim(),
                Author = txtAuthor.Text.Trim(),
                ISBN = txtISBN.Text.Trim(),
                Publisher = txtPublisher.Text.Trim(),
                Language = txtLanguage.Text.Trim(),
                Genre = txtGenre.Text.Trim(),
                Description = txtDescription.Text.Trim(),

                Pages = (int)numPages.Value,
                BookType = cmbBookType.SelectedItem?.ToString(),
                ApproxWeight = numWeight.Value,
                PublicationYear = (int)numPublicationYear.Value,

                Price = numPrice.Value,
                Condition = cmbCondition.SelectedItem?.ToString(),
                Status = cmbStatus.SelectedItem?.ToString(),
                ReasonStatus = cmbReasonStatus.SelectedItem?.ToString(),
                DefectedNotes = txtDefectedNotes.Text.Trim(),
                Origin = cmbOrigin.SelectedItem?.ToString(),

                ImageBytes = GetImageBytesFromPictureBox()
            };
        }

        private byte[] GetImageBytesFromPictureBox()
        {
            if (image.Image == null)
                return null;

            using (MemoryStream ms = new MemoryStream())
            {
                image.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(_currentBookId))
            {
                MessageBox.Show("Book ID was not generated.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(_currentTitleId))
            {
                MessageBox.Show("Title ID was not generated.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtISBN.Text))
            {
                MessageBox.Show("ISBN is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Title is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAuthor.Text))
            {
                MessageBox.Show("Author is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPublisher.Text))
            {
                MessageBox.Show("Publisher is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLanguage.Text))
            {
                MessageBox.Show("Language is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtGenre.Text))
            {
                MessageBox.Show("Genre is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Description is required.");
                return false;
            }

            if (cmbBookType.SelectedIndex == -1)
            {
                MessageBox.Show("Book type is required.");
                return false;
            }

            if (cmbCondition.SelectedIndex == -1)
            {
                MessageBox.Show("Condition is required.");
                return false;
            }

            if (cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Status is required.");
                return false;
            }

            if (cmbOrigin.SelectedIndex == -1)
            {
                MessageBox.Show("Origin is required.");
                return false;
            }

            if (cmbStatus.SelectedItem.ToString() == "UNAVAILABLE" &&
                cmbReasonStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Reason status is required when book is unavailable.");
                return false;
            }

            return true;
        }

        private void SaveBookTitle(ClassBook book, MySqlConnection conn, MySqlTransaction transaction)
        {
            string checkQuery = "SELECT COUNT(*) FROM book_titles WHERE title_id = @title_id";

            using (var checkCmd = new MySqlCommand(checkQuery, conn, transaction))
            {
                checkCmd.Parameters.AddWithValue("@title_id", book.TitleId);

                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count == 0)
                    InsertBookTitle(book, conn, transaction);
                else
                    UpdateBookTitle(book, conn, transaction);
            }
        }

        private void InsertBookTitle(ClassBook book, MySqlConnection conn, MySqlTransaction transaction)
        {
            string query = @"
                INSERT INTO book_titles
                (
                    title_id, title, author, iSBN, pages, book_type, book_approx_weight,
                    publisher, publication_year, book_language, genre, book_description, book_image
                )
                VALUES
                (
                    @title_id, @title, @author, @isbn, @pages, @book_type, @weight,
                    @publisher, @year, @language, @genre, @description, @image
                )";

            using (var cmd = new MySqlCommand(query, conn, transaction))
            {
                AddBookTitleParameters(cmd, book);
                cmd.ExecuteNonQuery();
            }
        }

        private void UpdateBookTitle(ClassBook book, MySqlConnection conn, MySqlTransaction transaction)
        {
            string query = @"
                UPDATE book_titles
                SET
                    title = @title,
                    author = @author,
                    iSBN = @isbn,
                    pages = @pages,
                    book_type = @book_type,
                    book_approx_weight = @weight,
                    publisher = @publisher,
                    publication_year = @year,
                    book_language = @language,
                    genre = @genre,
                    book_description = @description,
                    book_image = @image
                WHERE title_id = @title_id";

            using (var cmd = new MySqlCommand(query, conn, transaction))
            {
                AddBookTitleParameters(cmd, book);
                cmd.ExecuteNonQuery();
            }
        }

        private void AddBookTitleParameters(MySqlCommand cmd, ClassBook book)
        {
            cmd.Parameters.AddWithValue("@title_id", book.TitleId);
            cmd.Parameters.AddWithValue("@title", book.Title);
            cmd.Parameters.AddWithValue("@author", book.Author);
            cmd.Parameters.AddWithValue("@isbn", book.ISBN);
            cmd.Parameters.AddWithValue("@pages", book.Pages);
            cmd.Parameters.AddWithValue("@book_type", book.BookType);
            cmd.Parameters.AddWithValue("@weight", book.ApproxWeight);
            cmd.Parameters.AddWithValue("@publisher", book.Publisher);
            cmd.Parameters.AddWithValue("@year", book.PublicationYear);
            cmd.Parameters.AddWithValue("@language", book.Language);
            cmd.Parameters.AddWithValue("@genre", book.Genre);
            cmd.Parameters.AddWithValue("@description", book.Description);
            cmd.Parameters.AddWithValue("@image", (object)book.ImageBytes ?? DBNull.Value);
        }

        private void SaveInventoryBook(ClassBook book, MySqlConnection conn, MySqlTransaction transaction)
        {
            string checkQuery = "SELECT COUNT(*) FROM book WHERE book_id = @book_id";

            using (var checkCmd = new MySqlCommand(checkQuery, conn, transaction))
            {
                checkCmd.Parameters.AddWithValue("@book_id", book.BookId);

                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count == 0)
                    InsertInventoryBook(book, conn, transaction);
                else
                    UpdateInventoryBook(book, conn, transaction);
            }
        }

        private void InsertInventoryBook(ClassBook book, MySqlConnection conn, MySqlTransaction transaction)
        {
            string query = @"
                INSERT INTO book
                (
                    book_id, price, book_condition, book_status, reason_status,
                    defected_notes, origin, title_id_in_book
                )
                VALUES
                (
                    @book_id, @price, @condition, @status, @reason_status,
                    @defected_notes, @origin, @title_id
                )";

            using (var cmd = new MySqlCommand(query, conn, transaction))
            {
                AddInventoryBookParameters(cmd, book);
                cmd.ExecuteNonQuery();
            }
        }

        private void UpdateInventoryBook(ClassBook book, MySqlConnection conn, MySqlTransaction transaction)
        {
            string query = @"
                UPDATE book
                SET
                    price = @price,
                    book_condition = @condition,
                    book_status = @status,
                    reason_status = @reason_status,
                    defected_notes = @defected_notes,
                    origin = @origin,
                    title_id_in_book = @title_id
                WHERE book_id = @book_id";

            using (var cmd = new MySqlCommand(query, conn, transaction))
            {
                AddInventoryBookParameters(cmd, book);
                cmd.ExecuteNonQuery();
            }
        }

        private void AddInventoryBookParameters(MySqlCommand cmd, ClassBook book)
        {
            cmd.Parameters.AddWithValue("@book_id", book.BookId);
            cmd.Parameters.AddWithValue("@price", book.Price);
            cmd.Parameters.AddWithValue("@condition", book.Condition);
            cmd.Parameters.AddWithValue("@status", book.Status);
            cmd.Parameters.AddWithValue("@reason_status", (object)book.ReasonStatus ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@defected_notes", book.DefectedNotes);
            cmd.Parameters.AddWithValue("@origin", (object)book.Origin ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@title_id", book.TitleId);
        }

        private string GetExistingTitleIdByISBN(string isbn, MySqlConnection conn, MySqlTransaction transaction)
        {
            string query = "SELECT title_id FROM book_titles WHERE iSBN = @isbn LIMIT 1";

            using (var cmd = new MySqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@isbn", isbn);

                object result = cmd.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                    return null;

                return result.ToString();
            }
        }

        private void LoadExistingTitleByISBN(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                return;

            string query = @"
                SELECT 
                    title_id, title, author, pages, book_type, book_approx_weight,
                    publisher, publication_year, book_language, genre, book_description, book_image
                FROM book_titles
                WHERE iSBN = @isbn
                LIMIT 1";

            using (var conn = Db.GetConnection())
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@isbn", isbn);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        _loadingForm = true;

                        _currentTitleId = reader["title_id"].ToString();

                        txtTitle.Text = reader["title"].ToString();
                        txtAuthor.Text = reader["author"].ToString();
                        txtPublisher.Text = reader["publisher"].ToString();
                        txtLanguage.Text = reader["book_language"].ToString();
                        txtGenre.Text = reader["genre"].ToString();
                        txtDescription.Text = reader["book_description"].ToString();

                        cmbBookType.SelectedItem = reader["book_type"].ToString();

                        SetNumericValue(numPages, Convert.ToDecimal(reader["pages"]));
                        SetNumericValue(numWeight, Convert.ToDecimal(reader["book_approx_weight"]));
                        SetNumericValue(numPublicationYear, Convert.ToDecimal(reader["publication_year"]));

                        if (reader["book_image"] != DBNull.Value)
                        {
                            byte[] imageBytes = (byte[])reader["book_image"];

                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                image.Image = Image.FromStream(new MemoryStream(ms.ToArray()));
                            }
                        }

                        _loadingForm = false;

                        MessageBox.Show("Existing title found. A new physical copy will be created.");
                    }
                }
            }
        }

        private string GenerateNextBookId()
        {
            string query = @"
                SELECT MAX(CAST(SUBSTRING(book_id, 3) AS UNSIGNED))
                FROM book
                WHERE book_id LIKE 'NL%'";

            using (var conn = Db.GetConnection())
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();

                object result = cmd.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                    return "NL0001";

                int number = Convert.ToInt32(result) + 1;
                return "NL" + number.ToString("D4");
            }
        }

        private string GenerateNextTitleId()
        {
            string query = @"
                SELECT MAX(CAST(SUBSTRING(title_id, 4) AS UNSIGNED))
                FROM book_titles
                WHERE title_id LIKE 'NLT%'";

            using (var conn = Db.GetConnection())
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();

                object result = cmd.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                    return "NLT0001";

                int number = Convert.ToInt32(result) + 1;
                return "NLT" + number.ToString("D4");
            }
        }

        private void SetupAutoComplete()
        {
            SetupAutoCompleteForTextBox(txtPublisher, "SELECT DISTINCT publisher FROM book_titles ORDER BY publisher");
            SetupAutoCompleteForTextBox(txtGenre, "SELECT DISTINCT genre FROM book_titles ORDER BY genre");
            SetupAutoCompleteForTextBox(txtLanguage, "SELECT DISTINCT book_language FROM book_titles ORDER BY book_language");
        }
        
        private void SetupAutoCompleteForTextBox(TextBox textBox, string query)
        {
            AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
        
            using (var conn = Db.GetConnection())
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
        
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        collection.Add(reader[0].ToString());
                    }
                }
            }
        
            textBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox.AutoCompleteCustomSource = collection;
        }
        private void ClearForm()
        {
            lblBookId.Text = string.Empty;

            txtTitle.Clear();
            txtAuthor.Clear();
            txtISBN.Clear();
            txtPublisher.Clear();
            txtLanguage.Text = "English";
            txtGenre.Clear();
            txtDescription.Clear();

            SetNumericValue(numPages, 1);
            SetNumericValue(numWeight, 1);
            SetNumericValue(numPublicationYear, DateTime.Now.Year);
            SetNumericValue(numPrice, 0);

            cmbBookType.SelectedIndex = -1;
            cmbCondition.SelectedIndex = -1;
            cmbStatus.SelectedIndex = 0;
            cmbReasonStatus.SelectedIndex = -1;
            cmbOrigin.SelectedIndex = 0;

            txtDefectedNotes.Clear();

            image.Image = null;
        }

        private void SetNumericValue(NumericUpDown control, decimal value)
        {
            if (value < control.Minimum)
                value = control.Minimum;

            if (value > control.Maximum)
                value = control.Maximum;

            control.Value = value;
        }

        private void SetWeightByBookType()
        {
            if (_loadingForm)
                return;

            if (cmbBookType.SelectedItem == null)
                return;

            switch (cmbBookType.SelectedItem.ToString())
            {
                case "PB":
                    numWeight.Minimum = 180;
                    numWeight.Maximum = 250;
                    SetNumericValue(numWeight, 220);
                    break;

                case "TPB":
                    numWeight.Minimum = 350;
                    numWeight.Maximum = 550;
                    SetNumericValue(numWeight, 450);
                    break;

                case "HB":
                    numWeight.Minimum = 600;
                    numWeight.Maximum = 1200;
                    SetNumericValue(numWeight, 750);
                    break;
            }

            numWeight.Enabled = _mode != BookFormMode.View;
        }

        private void cmbBookType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetWeightByBookType();
        }

        private void NumericUpDown_Enter(object sender, EventArgs e)
        {
            if (sender is NumericUpDown numeric)
            {
                numeric.Select(0, numeric.Text.Length);
            }
        }

        private void txtISBN_Leave(object sender, EventArgs e)
        {
            if (_mode == BookFormMode.Add)
            {
                LoadExistingTitleByISBN(txtISBN.Text.Trim());
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            SetMode(BookFormMode.Edit);
        }

        private void btnCreateCopy_Click(object sender, EventArgs e)
        {
            if (_selectedBook == null)
                return;

            _currentBookId = GenerateNextBookId();
            _currentTitleId = _selectedBook.TitleId;
            lblBookId.Text = _currentBookId;

            cmbCondition.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;
            cmbReasonStatus.SelectedIndex = -1;
            cmbOrigin.SelectedIndex = -1;

            txtDefectedNotes.Clear();
            SetNumericValue(numPrice, 0);

            SetMode(BookFormMode.AddCopy);

            txtISBN.ReadOnly = true;
            txtTitle.ReadOnly = true;
            txtAuthor.ReadOnly = true;
            txtPublisher.ReadOnly = true;
            txtLanguage.ReadOnly = true;
            txtGenre.ReadOnly = true;
            txtDescription.ReadOnly = true;

            numPages.Enabled = false;
            numWeight.Enabled = false;
            numPublicationYear.Enabled = false;
            cmbBookType.Enabled = false;

            lblBookId.Visible = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_returnToArchive)
            {
                frmPrincipal.PrincipalInstance.AbrirForm<frmArchive>();
                return;
            }
            if (_mode == BookFormMode.Add)
            {
                frmPrincipal.PrincipalInstance.AbrirForm<Book>();
                return;
            }

            if (_mode == BookFormMode.AddCopy)
            {
                FillForm(_selectedBook);
                SetMode(BookFormMode.View);
                return;
            }

            if (_mode == BookFormMode.Edit)
            {
                FillForm(_selectedBook);
                SetMode(BookFormMode.View);
                return;
            }

            if (_mode == BookFormMode.View)
            {
                frmPrincipal.PrincipalInstance.AbrirForm<Book>();
                return;
            }
        }

        private void btnAddImg_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    image.Image = new Bitmap(dlg.FileName);
                }
            }
        }

        private void btnRemoveImg_Click(object sender, EventArgs e)
        {
            image.Image = null;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_currentBookId))
            {
                MessageBox.Show("Invalid book ID.");
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Delete book {_currentBookId}?\n\n" +
                "This will remove only this physical copy.\n" +
                "The book title information will remain.",
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
                        cmd.Parameters.AddWithValue("@book_id", _currentBookId);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Book deleted successfully.");

                            // Return to books screen
                            frmPrincipal.PrincipalInstance.AbrirForm<Book>();
                        }
                        else
                        {
                            MessageBox.Show("Book not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting book: " + ex.Message);
            }
        }
        
        private void UpdateBundleStatusIfBookWasSold(
            string bookId,
            string bookStatus,
            MySqlConnection conn,
            MySqlTransaction transaction)
        {
            if (bookStatus != "SOLD")
                return;
        
            string query = @"
                UPDATE bundle b
                INNER JOIN bundle_book bb
                    ON bb.bundle_id_in_bundle_book = b.bundle_id
                SET b.bundle_status = 'UNAVAILABLE'
                WHERE bb.book_id_in_bundle_book = @book_id
                  AND b.bundle_status <> 'SOLD';";
        
            using (var cmd = new MySqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@book_id", bookId);
                cmd.ExecuteNonQuery();
            }
        }

        #region Round Buttons
        private void btnArchive_Paint(object sender, PaintEventArgs e) { RoundButton(sender); }
        private void btnDelete_Paint(object sender, PaintEventArgs e) { RoundButton(sender); }
        private void btnAddImg_Paint(object sender, PaintEventArgs e) { RoundButton(sender); }
        private void btnRemove_Paint(object sender, PaintEventArgs e) { RoundButton(sender); }
        private void btnCancel_Paint(object sender, PaintEventArgs e) { RoundButton(sender); }
        private void btnSave_Paint(object sender, PaintEventArgs e) { RoundButton(sender); }
        private void btnCreateCopy_Paint(object sender, PaintEventArgs e) { RoundButton(sender); }
        private void btnEdit_Paint(object sender, PaintEventArgs e) { RoundButton(sender); }

        private void RoundButton(object sender)
        {
            Button btn = (Button)sender;
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

        private void btnArchive_Click(object sender, EventArgs e)
        {

        }
    }
}
