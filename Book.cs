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

namespace xdd
{
    public partial class Book : Form
    {
        public Book()
        {
            InitializeComponent();
            LoadBooks();
        }

        public void LoadBooks()
        {
            try
            {
                cardContainer.Controls.Clear();
                List<ClassBook> books = GetAllBooks();

                foreach (ClassBook book in books)
                {
                    Card card = new Card();
                    card.SetBook(book);
                    cardContainer.Controls.Add(card);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading books: " + ex.Message);
            }
        }

        private List<ClassBook> GetAllBooks()
        {
            List<ClassBook> books = new List<ClassBook>();

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
            
                WHERE 
                    b.book_status = 'AVAILABLE'
                    OR (
                        b.book_status = 'UNAVAILABLE'
                        AND b.reason_status = 'RemovedFromCuration'
                    )
            
                ORDER BY bt.title ASC, b.book_id ASC;";
    
            using (var conn = Db.GetConnection())
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(new ClassBook
                        {
                            BookId = reader["book_id"].ToString(),
                            TitleId = reader["title_id"].ToString(),
                            Price = Convert.ToDecimal(reader["price"]),
                            Condition = reader["book_condition"].ToString(),
                            Status = reader["book_status"].ToString(),
                            ReasonStatus = reader["reason_status"] == DBNull.Value ? null : reader["reason_status"].ToString(),
                            DefectedNotes = reader["defected_notes"].ToString(),
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
                        });
                    }
                }
            }

            return books;
        }
    }
}

