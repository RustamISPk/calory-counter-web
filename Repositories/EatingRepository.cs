using MySql.Data.MySqlClient;

namespace CaloryCounterWeb.Repositories
{
    public class EatingRepository : AbstractRepository
    {
        private string TableName = "eating";

        public bool WriteEating(int userId, int productId, int productCount, string eatingType)
        {
            try
            {
                DateTime today = DateTime.Today;
                
                this.mySqlConnection.Open();
                
                string insertQuery = $@"INSERT INTO {TableName} 
                                (user_id, eating_date, product_id, product_count, eating_type) 
                                VALUES (@userId, @eatingDate, @productId, @productCount, @eatingType)";

                var command = new MySqlCommand(insertQuery, this.mySqlConnection);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@eatingDate", today);
                command.Parameters.AddWithValue("@productId", productId);
                command.Parameters.AddWithValue("@productCount", productCount);
                command.Parameters.AddWithValue("@eatingType", eatingType);

                int rowsAffected = command.ExecuteNonQuery();
                this.mySqlConnection.Close();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                this.mySqlConnection.Close();
                throw new Exception($"Error writing eating record: {ex.Message}");
            }
        }

        public List<Dictionary<string, object>> GetAteFoodByType(int userId, string eatingType)
        {
            var results = new List<Dictionary<string, object>>();
            
            try
            {
                DateTime today = DateTime.Today;
                
                this.mySqlConnection.Open();
                string query = $@"SELECT * FROM {TableName} 
                          WHERE user_id = @userId 
                          AND eating_date = @eatingDate 
                          AND eating_type = @eatingType";
                
                var command = new MySqlCommand(query, this.mySqlConnection);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@eatingDate", today);
                command.Parameters.AddWithValue("@eatingType", eatingType);
                
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
            catch (Exception ex)
            {
                this.mySqlConnection.Close();
                throw new Exception($"Error getting ate food by type: {ex.Message}");
            }
        }

        public List<Dictionary<string, object>> GetProductById(int productId, int eatingId)
        {
            var results = new List<Dictionary<string, object>>();
            
            try
            {
                DateTime today = DateTime.Today;
                
                this.mySqlConnection.Open();
                string query = $@"SELECT 
                            p.product_name, 
                            p.calory, 
                            p.protein, 
                            p.fats, 
                            p.carbohydrates,
                            e.eating_date, 
                            e.product_count, 
                            e.eating_type 
                          FROM product_and_recipe_list p 
                          INNER JOIN eating e ON p.product_id = e.product_id 
                          WHERE p.product_id = @productId 
                          AND e.eating_date = @eatingDate 
                          AND e.eating_id = @eatingId";

                var command = new MySqlCommand(query, this.mySqlConnection);
                command.Parameters.AddWithValue("@productId", productId);
                command.Parameters.AddWithValue("@eatingDate", today);
                command.Parameters.AddWithValue("@eatingId", eatingId);
                
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
            catch (Exception ex)
            {
                this.mySqlConnection.Close();
                throw new Exception($"Error getting product by ID: {ex.Message}");
            }
        }

        public bool RemoveEating(int eatingId)
        {
            try
            {
                this.mySqlConnection.Open();

                string deleteQuery = $"DELETE FROM {TableName} WHERE eating_id = @eatingId";
                var command = new MySqlCommand(deleteQuery, this.mySqlConnection);
                command.Parameters.AddWithValue("@eatingId", eatingId);
                
                int rowsAffected = command.ExecuteNonQuery();
                this.mySqlConnection.Close();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                this.mySqlConnection.Close();
                throw new Exception($"Error removing eating record: {ex.Message}");
            }
        }

        public List<Dictionary<string, object>> GetEatingByUserId(int userId)
        {
            var results = new List<Dictionary<string, object>>();
            
            try
            {
                this.mySqlConnection.Open();
                string query = $"SELECT * FROM {TableName} WHERE user_id = @userId ORDER BY eating_date DESC";
                var command = new MySqlCommand(query, this.mySqlConnection);
                command.Parameters.AddWithValue("@userId", userId);
                
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
            catch (Exception ex)
            {
                this.mySqlConnection.Close();
                throw new Exception($"Error getting eating by user ID: {ex.Message}");
            }
        }

        public List<Dictionary<string, object>> GetEatingByDate(int userId, DateTime date)
        {
            var results = new List<Dictionary<string, object>>();
            
            try
            {
                this.mySqlConnection.Open();
                string query = $"SELECT * FROM {TableName} WHERE user_id = @userId AND eating_date = @eatingDate ORDER BY eating_type";
                var command = new MySqlCommand(query, this.mySqlConnection);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@eatingDate", date);
                
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
            catch (Exception ex)
            {
                this.mySqlConnection.Close();
                throw new Exception($"Error getting eating by date: {ex.Message}");
            }
        }

        public List<Dictionary<string, object>> GetEatingById(int eatingId)
        {
            var results = new List<Dictionary<string, object>>();
            
            try
            {
                this.mySqlConnection.Open();
                string query = $"SELECT * FROM {TableName} WHERE eating_id = @eatingId";
                var command = new MySqlCommand(query, this.mySqlConnection);
                command.Parameters.AddWithValue("@eatingId", eatingId);
                
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
            catch (Exception ex)
            {
                this.mySqlConnection.Close();
                throw new Exception($"Error getting eating by ID: {ex.Message}");
            }
        }
    }
}