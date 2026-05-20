using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ninelivesbooks.frmLogin;

namespace ninelivesbooks
{
    internal static class LogHelper
    {

        public static string GenerateNextLogId()
        {
            string query = @"
                SELECT MAX(CAST(SUBSTRING(inventory_log_id, 4) AS UNSIGNED))
                FROM inventory
                WHERE inventory_log_id LIKE 'NLL%'";

            using (MySqlConnection conn = Db.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);

                object result = cmd.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                    return "NLL0001";

                int number = Convert.ToInt32(result) + 1;
                return "NLL" + number.ToString("D4");
            }
        }


        public static void RegistrarLog(
            string entityType,
            string entityId,
            string action,
            string description)
        {
            using (MySqlConnection conn = Db.GetConnection())
            {
                conn.Open();

                string sql = @"
            INSERT INTO inventory
            (inventory_log_id, entity_type, entity_id, inventory_action,
             new_value, user_id_in_inventory)
            VALUES
            (@log_id, @entity_type, @entity_id, @action,
             @new_value, @user_id)";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@log_id", GenerateNextLogId());
                cmd.Parameters.AddWithValue("@entity_type", entityType);
                cmd.Parameters.AddWithValue("@entity_id", entityId);
                cmd.Parameters.AddWithValue("@action", action);
                cmd.Parameters.AddWithValue("@new_value", description);
                cmd.Parameters.AddWithValue("@user_id", Sessao.User_Id);

                cmd.ExecuteNonQuery();
            }
        }



        public static void RegistrarLogTrans(
            MySqlConnection conn,
            MySqlTransaction transaction,
            string entityType,
            string entityId,
            string action,
            string description)
        {
            string sql = @"
        INSERT INTO inventory
        (inventory_log_id, entity_type, entity_id, inventory_action,
         new_value, user_id_in_inventory)
        VALUES
        (@log_id, @entity_type, @entity_id, @action,
         @new_value, @user_id)";

            using (MySqlCommand cmd = new MySqlCommand(sql, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@log_id", GenerateNextLogId());
                cmd.Parameters.AddWithValue("@entity_type", entityType);
                cmd.Parameters.AddWithValue("@entity_id", entityId);
                cmd.Parameters.AddWithValue("@action", action);
                cmd.Parameters.AddWithValue("@new_value", description);
                cmd.Parameters.AddWithValue("@user_id", Sessao.User_Id);

                cmd.ExecuteNonQuery();
            }
        }

    }
}
