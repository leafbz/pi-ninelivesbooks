using MySql.Data.MySqlClient;
using ninelivesbooks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ninelivesbooks.frmLogin;
using static xdd.frmLog;

namespace ninelivesbooks
{
    public partial class frmLog : Form
    {
        Color headerBack = Color.FromArgb(0, 60, 40);
        Color headerFore = Color.FromArgb(255, 240, 200);

        Color rowEven = Color.FromArgb(255, 245, 220);
        Color rowOdd = Color.FromArgb(255, 235, 200);

        Color gridColor = Color.FromArgb(255, 215, 160);

        MySqlConnection conn;
        string data_source = "datasource=localhost; username=root; password=; database=ninelivebooks";
        private void lstUser_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (SolidBrush back = new SolidBrush(headerBack))
            using (SolidBrush fore = new SolidBrush(headerFore))
            {
                e.Graphics.FillRectangle(back, e.Bounds);

                TextRenderer.DrawText(
                    e.Graphics,
                    e.Header.Text,
                    new Font("Georgia", 10F, FontStyle.Bold),
                    e.Bounds,
                    headerFore,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );

                e.Graphics.DrawRectangle(new Pen(gridColor), e.Bounds);

            }
        }

        private void lstUser_DrawItem(object sender, DrawListViewItemEventArgs e)
        {

        }

        private void lstUser_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {

            Color backColor;

            Color textColor;

            if (e.Item.Selected)
            {
                textColor = Color.Black;
            }
            else if (e.ColumnIndex == 0)
            {
                textColor = Color.FromArgb(0, 80, 60);
            }
            else
            {
                textColor = Color.FromArgb(20, 60, 40);
            }

            if (e.Item.Selected)
            {
                backColor = Color.FromArgb(255, 215, 160);
            }
            else if ((e.ItemState & ListViewItemStates.Hot) != 0)
            {
                backColor = Color.FromArgb(255, 230, 190);
            }
            else
            {
                backColor = (e.ItemIndex % 2 == 0) ? rowEven : rowOdd;
            }

            using (SolidBrush back = new SolidBrush(backColor))
                e.Graphics.FillRectangle(back, e.Bounds);

            Font font = new Font("Georgia", 10F, FontStyle.Bold);

            TextRenderer.DrawText(
                e.Graphics,
                e.SubItem.Text,
                font,
                e.Bounds,
                textColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter
            );

            e.Graphics.DrawRectangle(new Pen(gridColor), e.Bounds);
        }
        private void lstUserLog_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (SolidBrush back = new SolidBrush(headerBack))
            using (SolidBrush fore = new SolidBrush(headerFore))
            {
                e.Graphics.FillRectangle(back, e.Bounds);

                TextRenderer.DrawText(
                    e.Graphics,
                    e.Header.Text,
                    new Font("Georgia", 10F, FontStyle.Bold),
                    e.Bounds,
                    headerFore,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );

                e.Graphics.DrawRectangle(new Pen(gridColor), e.Bounds);

            }
        }

        private void lstUserLog_DrawItem(object sender, DrawListViewItemEventArgs e)
        {

        }

        private void lstUserLog_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {

            Color backColor;

            Color textColor;

            if (e.Item.Selected)
            {
                textColor = Color.Black;
            }
            else if (e.ColumnIndex == 0)
            {
                textColor = Color.FromArgb(0, 80, 60);
            }
            else
            {
                textColor = Color.FromArgb(20, 60, 40);
            }

            if (e.Item.Selected)
            {
                backColor = Color.FromArgb(255, 215, 160);
            }
            else if ((e.ItemState & ListViewItemStates.Hot) != 0)
            {
                backColor = Color.FromArgb(255, 230, 190);
            }
            else
            {
                backColor = (e.ItemIndex % 2 == 0) ? rowEven : rowOdd;
            }

            using (SolidBrush back = new SolidBrush(backColor))
                e.Graphics.FillRectangle(back, e.Bounds);

            Font font = new Font("Georgia", 10F, FontStyle.Bold);

            TextRenderer.DrawText(
                e.Graphics,
                e.SubItem.Text,
                font,
                e.Bounds,
                textColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter
            );

            e.Graphics.DrawRectangle(new Pen(gridColor), e.Bounds);
        }
        public frmLog()
        {
            InitializeComponent();


            lstUser.BackColor = Color.FromArgb(255, 245, 220);
            lstUser.BorderStyle = BorderStyle.FixedSingle;
            lstUser.View = View.Details;


            lstUser.MultiSelect = true;
            lstUser.AllowColumnReorder = true;
            lstUser.FullRowSelect = true;
            lstUser.GridLines = true;
            lstUser.HideSelection = false;
            lstUser.OwnerDraw = true;
            lstUser.HoverSelection = false;
            lstUser.UseCompatibleStateImageBehavior = false;

            lstUser.Activation = ItemActivation.Standard;

            lstUser.Columns.Add("ID", 150, HorizontalAlignment.Left);
            lstUser.Columns.Add("Name", 302, HorizontalAlignment.Left);
            lstUser.Columns.Add("Email", 320, HorizontalAlignment.Left);
            lstUser.Columns.Add("Role", 225, HorizontalAlignment.Left);
            lstUser.Columns.Add("Status", 184, HorizontalAlignment.Left);
            lstUser.Columns.Add("Created At", 240, HorizontalAlignment.Left);


            lstUser.DrawColumnHeader += lstUser_DrawColumnHeader;
            lstUser.DrawItem += lstUser_DrawItem;
            lstUser.DrawSubItem += lstUser_DrawSubItem;

            lstUserLog.BackColor = Color.FromArgb(255, 245, 220);
            lstUserLog.BorderStyle = BorderStyle.FixedSingle;
            lstUserLog.View = View.Details;



            lstUserLog.AllowColumnReorder = true;
            lstUserLog.FullRowSelect = true;
            lstUserLog.GridLines = true;
            lstUserLog.HideSelection = false;
            lstUserLog.OwnerDraw = true;
            lstUserLog.HoverSelection = false;
            lstUserLog.UseCompatibleStateImageBehavior = false;

            lstUserLog.Activation = ItemActivation.Standard;

            lstUserLog.Columns.Add("Log ID", 173, HorizontalAlignment.Left);
            lstUserLog.Columns.Add("User ID", 179, HorizontalAlignment.Left);
            lstUserLog.Columns.Add("Entity ID", 179, HorizontalAlignment.Left);
            lstUserLog.Columns.Add("Entity", 180, HorizontalAlignment.Left);
            lstUserLog.Columns.Add("Action", 320, HorizontalAlignment.Left);

            lstUserLog.Columns.Add("New Value", 170, HorizontalAlignment.Left);
            lstUserLog.Columns.Add("Updated At", 220, HorizontalAlignment.Left);



            lstUserLog.DrawColumnHeader += lstUserLog_DrawColumnHeader;
            lstUserLog.DrawItem += lstUserLog_DrawItem;
            lstUserLog.DrawSubItem += lstUserLog_DrawSubItem;

            lstUser.SelectedIndexChanged += lstUsers_SelectedIndexChanged;
            LoadUsers();

            if(Sessao.User_Role != "Administrator")

            {
                btnAdd.Visible = false;
            }
            if (Sessao.User_Role != "Administrator")

            {
                btnEdit.Visible = false;
            }

        }


        private void LoadUsers()
        {
            lstUser.Items.Clear();

            string sql = "SELECT user_id, user_name, user_email, user_role, user_status, user_created_at FROM usuario";

            using (MySqlConnection conn = new MySqlConnection(data_source))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ListViewItem item = new ListViewItem(reader["user_id"].ToString());
                    item.SubItems.Add(reader["user_name"].ToString());
                    item.SubItems.Add(reader["user_email"].ToString());
                    item.SubItems.Add(reader["user_role"].ToString());
                    item.SubItems.Add(reader["user_status"].ToString());
                    item.SubItems.Add(reader["user_created_at"].ToString());

                    lstUser.Items.Add(item);

                }
            }
        }

        private void lstUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstUser.SelectedItems.Count == 0)
                return;

            string userId = lstUser.SelectedItems[0].Text;

            LoadLogs(userId);
        }


        void LoadLogs(string userId)
        {
            lstUserLog.Items.Clear();

            string sql = @"
        SELECT 
            inventory_log_id,
            user_id_in_inventory,
            entity_id,
            entity_type,
            inventory_action,
           
            new_value,
            inventory_log_created_at
        FROM inventory
        WHERE user_id_in_inventory = @user_id
        ORDER BY inventory_log_created_at DESC";

            using (MySqlConnection conn = new MySqlConnection(data_source))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@user_id", userId);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ListViewItem item = new ListViewItem(reader["inventory_log_id"].ToString());

                    item.SubItems.Add(reader["user_id_in_inventory"].ToString());
                    item.SubItems.Add(reader["entity_id"].ToString());
                    item.SubItems.Add(reader["entity_type"].ToString());
                    item.SubItems.Add(reader["inventory_action"].ToString());
                    item.SubItems.Add(reader["new_value"].ToString());
                    item.SubItems.Add(reader["inventory_log_created_at"].ToString());

                    lstUserLog.Items.Add(item);
                }
            }
        }


        private void btnLog_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show(
                    "Do you really want to log out?",
                    "Confirm Logout",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

            if (result != DialogResult.Yes)
                return;

            Sessao.Logout();

        
            foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
            {
                if (form.Name != "frmLogin")
                    form.Hide();
            }

            frmLogin login = new frmLogin();
            login.Show();

        }

        private void frmLog_Load(object sender, EventArgs e)
        {
            CarregarLogsNaListView(lstUserLog);
        }




        private void CarregarLogsNaListView(ListView lista, string filtroUserId = null)
        {
            lista.Items.Clear();

            using (MySqlConnection conn = new MySqlConnection(data_source))
            {
                conn.Open();

                string sql = @"
            SELECT 
                inventory_log_id,
                user_id_in_inventory,
                entity_type,
                entity_id,
                inventory_action,
               
                new_value,
                inventory_log_created_at
            FROM inventory";

                if (!string.IsNullOrEmpty(filtroUserId))
                {
                    sql += " WHERE user_id_in_inventory = @user_id";
                }

                sql += " ORDER BY inventory_log_created_at DESC";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    if (!string.IsNullOrEmpty(filtroUserId))
                    {
                        cmd.Parameters.AddWithValue("@user_id", filtroUserId);
                    }

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ListViewItem item = new ListViewItem(reader["inventory_log_id"].ToString());

                            item.SubItems.Add(reader["user_id_in_inventory"].ToString());
                            item.SubItems.Add(reader["entity_type"].ToString());
                            item.SubItems.Add(reader["entity_id"].ToString());
                            item.SubItems.Add(reader["inventory_action"].ToString());

                            item.SubItems.Add(reader["new_value"].ToString());
                            item.SubItems.Add(
                                Convert.ToDateTime(reader["inventory_log_created_at"])
                                .ToString("dd/MM/yyyy HH:mm")
                            );


                            string action = reader["inventory_action"].ToString();

                            if (action == "CREATE")
                                item.ForeColor = Color.Green;
                            else if (action == "UPDATE")
                                item.ForeColor = Color.Blue;
                            else if (action == "DELETE")
                                item.ForeColor = Color.Red;

                            lista.Items.Add(item);
                        }
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmPrincipal.PrincipalInstance.AbrirForm<frmRegistration>();
        }
    }
}
