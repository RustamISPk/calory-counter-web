using MySql.Data.MySqlClient;

namespace CaloryCounterWeb.Repositories
{
    public class UserWeightStoryRepository : AbstractRepository
    {
        private string TableName = "user_weight_story";

        public bool SaveUserWeight(int userId, float weight, DateTime weightDate)
        {
            try
            {
                this.mySqlConnection.Open();
                
                string insertQuery = $@"INSERT INTO {TableName} 
                                (user_id, user_weight, weight_date) 
                                VALUES (@userId, @weight, @weightDate)";

                var command = new MySqlCommand(insertQuery, this.mySqlConnection);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@weight", weight);
                command.Parameters.AddWithValue("@weightDate", weightDate);

                int rowsAffected = command.ExecuteNonQuery();
                this.mySqlConnection.Close();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                this.mySqlConnection.Close();
                throw new Exception($"Error saving user weight: {ex.Message}");
            }
        }

        public bool WriteUserNewWeight(int userId, float weight)
        {
            try
            {
                DateTime today = DateTime.Today;
                var existingWeight = GetWeightByDate(userId, today);

                this.mySqlConnection.Open();

                if (existingWeight.Count == 0)
                {
                    string insertQuery = $@"INSERT INTO {TableName} 
                                    (user_id, user_weight, weight_date) 
                                    VALUES (@userId, @weight, @weightDate)";

                    var command = new MySqlCommand(insertQuery, this.mySqlConnection);
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@weight", weight);
                    command.Parameters.AddWithValue("@weightDate", today);

                    int rowsAffected = command.ExecuteNonQuery();
                    this.mySqlConnection.Close();

                    return rowsAffected > 0;
                }
                else
                {
                    string updateQuery = $@"UPDATE {TableName} 
                                    SET user_weight = @weight 
                                    WHERE user_id = @userId AND weight_date = @weightDate";

                    var command = new MySqlCommand(updateQuery, this.mySqlConnection);
                    command.Parameters.AddWithValue("@weight", weight);
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@weightDate", today);

                    int rowsAffected = command.ExecuteNonQuery();
                    this.mySqlConnection.Close();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                this.mySqlConnection.Close();
                throw new Exception($"Error writing user new weight: {ex.Message}");
            }
        }

        public List<Dictionary<string, object>> LoadGraphicData(int userId)
        {
            var results = new List<Dictionary<string, object>>();
            
            try
            {
                this.mySqlConnection.Open();
                string query = $"SELECT * FROM {TableName} WHERE user_id = @userId ORDER BY weight_date";
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
                throw new Exception($"Error loading graphic data: {ex.Message}");
            }
        }

        public float GetUserWeight(int userId)
        {
            var weightData = GetUserWeightHistory(userId);
            
            if (weightData.Count > 0)
            {
                // Return the latest weight (last record in the list)
                var latestWeight = weightData[weightData.Count - 1];
                return Convert.ToSingle(latestWeight["user_weight"]);
            }
            
            throw new Exception($"No weight data found for user ID {userId}");
        }

        public List<Dictionary<string, object>> GetUserWeightHistory(int userId)
        {
            var results = new List<Dictionary<string, object>>();
            
            try
            {
                this.mySqlConnection.Open();
                string query = $"SELECT * FROM {TableName} WHERE user_id = @userId ORDER BY weight_date";
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
                throw new Exception($"Error getting user weight history: {ex.Message}");
            }
        }

        private List<Dictionary<string, object>> GetWeightByDate(int userId, DateTime date)
        {
            var results = new List<Dictionary<string, object>>();
            
            try
            {
                this.mySqlConnection.Open();
                string query = $"SELECT * FROM {TableName} WHERE user_id = @userId AND weight_date = @weightDate";
                var command = new MySqlCommand(query, this.mySqlConnection);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@weightDate", date);
                
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
                throw new Exception($"Error getting weight by date: {ex.Message}");
            }
        }

        public bool ChangeUserWeight(int userId, float userWeight)
        {
            return WriteUserNewWeight(userId, userWeight);
        }

        public List<Dictionary<string, object>> GetWeightByUserId(int userId)
        {
            return GetUserWeightHistory(userId);
        }
    }
}