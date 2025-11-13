using MySql.Data.MySqlClient;

namespace CaloryCounterWeb.Repositories
{
    public class ProductRepository : AbstractRepository
    {
        private string TableName = "product_and_recipe_list";

        public List<Dictionary<string, object>> FindAll()
        {
            var results = new List<Dictionary<string, object>>();
            this.mySqlConnection.Open();
            string query = $"SELECT * FROM {this.TableName}";
            var command = new MySqlCommand(query, this.mySqlConnection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var row = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.GetValue(i);
                }
                results.Add(row);
            }

            reader.Close();
            this.mySqlConnection.Close();

            return results;
        }

        public List<Dictionary<string, object>> FindById(int productId)
        {
            var results = new List<Dictionary<string, object>>();
            this.mySqlConnection.Open();
            string query = $"SELECT * FROM {TableName} WHERE product_id = @productId";
            var command = new MySqlCommand(query, this.mySqlConnection);
            command.Parameters.AddWithValue("@productId", productId);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var row = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.GetValue(i);
                }
                results.Add(row);
            }

            reader.Close();
            this.mySqlConnection.Close();

            return results;
        }

        public void Create(string productName, int calory, float protein, float fats, float carbohydrates, string foodType)
        {
            this.mySqlConnection.Open();

            string insertQuery = $@"INSERT INTO {TableName} 
                            (product_name, calory, protein, fats, carbohydrates, food_type) 
                            VALUES (@productName, @calory, @protein, @fats, @carbohydrates, @foodType)";

            var command = new MySqlCommand(insertQuery, this.mySqlConnection);
            command.Parameters.AddWithValue("@productName", productName);
            command.Parameters.AddWithValue("@calory", calory);
            command.Parameters.AddWithValue("@protein", protein);
            command.Parameters.AddWithValue("@fats", fats);
            command.Parameters.AddWithValue("@carbohydrates", carbohydrates);
            command.Parameters.AddWithValue("@foodType", foodType);

            command.ExecuteNonQuery();
            this.mySqlConnection.Close();
        }

        public List<Dictionary<string, object>> Update(int productId, Dictionary<string, object> updateFields)
        {
            var results = new List<Dictionary<string, object>>();
            this.mySqlConnection.Open();

            var setParts = new List<string>();
            var parameters = new Dictionary<string, object>();

            foreach (var field in updateFields)
            {
                if (field.Key != "product_id")
                {
                    setParts.Add($"{field.Key} = @{field.Key}");
                    parameters.Add($"@{field.Key}", field.Value);
                }
            }

            if (setParts.Count > 0)
            {
                string setClause = string.Join(", ", setParts);
                string updateQuery = $"UPDATE {TableName} SET {setClause} WHERE product_id = @productId";

                var command = new MySqlCommand(updateQuery, this.mySqlConnection);
                command.Parameters.AddWithValue("@productId", productId);

                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }

                command.ExecuteNonQuery();
            }

            string selectQuery = $"SELECT * FROM {TableName} WHERE product_id = @productId";
            var selectCommand = new MySqlCommand(selectQuery, this.mySqlConnection);
            selectCommand.Parameters.AddWithValue("@productId", productId);
            var reader = selectCommand.ExecuteReader();

            while (reader.Read())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.GetValue(i);
                }
                results.Add(row);
            }

            reader.Close();
            this.mySqlConnection.Close();

            return results;
        }

        public bool Delete(int productId)
        {
            this.mySqlConnection.Open();

            string deleteQuery = $"DELETE FROM {TableName} WHERE product_id = @productId";
            var deleteCommand = new MySqlCommand(deleteQuery, this.mySqlConnection);
            deleteCommand.Parameters.AddWithValue("@productId", productId);
            int rowsAffected = deleteCommand.ExecuteNonQuery();

            this.mySqlConnection.Close();

            return rowsAffected > 0;
        }

        public List<Dictionary<string, object>> FindProducts()
        {
            return FindByFoodType("product");
        }

        public List<Dictionary<string, object>> FindRecipes()
        {
            return FindByFoodType("recipe");
        }

        private List<Dictionary<string, object>> FindByFoodType(string foodType)
        {
            var results = new List<Dictionary<string, object>>();
            this.mySqlConnection.Open();
            string query = $"SELECT * FROM {TableName} WHERE food_type = @foodType";
            var command = new MySqlCommand(query, this.mySqlConnection);
            command.Parameters.AddWithValue("@foodType", foodType);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var row = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.GetValue(i);
                }
                results.Add(row);
            }

            reader.Close();
            this.mySqlConnection.Close();

            return results;
        }
    }
}