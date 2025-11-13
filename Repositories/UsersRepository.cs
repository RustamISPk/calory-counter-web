using MySql.Data.MySqlClient;

namespace CaloryCounterWeb.Repositories
{
    public class UserAccountRepository : AbstractRepository
    {
        private string TableName = "user_account";

        public bool SaveUserAccount(string name, string surname, string login, string password, int height, DateTime birthDate, string gender)
        {
            try
            {
                this.mySqlConnection.Open();

                string insertQuery = $@"INSERT INTO {TableName} 
                                (user_name, user_lastname, user_login, user_password, user_height, user_birthdate, user_gender) 
                                VALUES (@name, @surname, @login, @password, @height, @birthDate, @gender)";

                var command = new MySqlCommand(insertQuery, this.mySqlConnection);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@surname", surname);
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@height", height);
                command.Parameters.AddWithValue("@birthDate", birthDate);
                command.Parameters.AddWithValue("@gender", gender);

                int rowsAffected = command.ExecuteNonQuery();
                this.mySqlConnection.Close();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                this.mySqlConnection.Close();
                throw new Exception($"Error saving user account: {ex.Message}");
            }
        }

        public (bool success, int? userId) FindUserInDatabase(string login, string password)
        {
            var results = new List<Dictionary<string, object>>();

            try
            {
                this.mySqlConnection.Open();
                string query = $"SELECT * FROM {TableName} WHERE user_login = @login AND user_password = @password";
                var command = new MySqlCommand(query, this.mySqlConnection);
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@password", password);

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

                if (results.Count > 0)
                {
                    int userId = Convert.ToInt32(results[0]["user_id"]);
                    return (true, userId);
                }
                else
                {
                    return (false, null);
                }
            }
            catch (Exception ex)
            {
                this.mySqlConnection.Close();
                throw new Exception($"Error finding user: {ex.Message}");
            }
        }

        public DateTime GetUserBirthDate(int userId)
        {
            var userData = GetUserById(userId);
            if (userData.Count > 0)
            {
                return (DateTime)userData[0]["user_birthdate"];
            }
            throw new Exception($"User with ID {userId} not found");
        }

        public bool ChangeUserParams(int userId, int? userHeight, DateTime? userAge, string userGender)
        {
            try
            {
                this.mySqlConnection.Open();

                if (userHeight.HasValue && userAge.HasValue && !string.IsNullOrEmpty(userGender))
                {
                    string updateQuery = $@"UPDATE {TableName} 
                                    SET user_height = @userHeight, 
                                        user_birthdate = @userAge, 
                                        user_gender = @userGender 
                                    WHERE user_id = @userId";

                    var command = new MySqlCommand(updateQuery, this.mySqlConnection);
                    command.Parameters.AddWithValue("@userHeight", userHeight.Value);
                    command.Parameters.AddWithValue("@userAge", userAge.Value);
                    command.Parameters.AddWithValue("@userGender", userGender);
                    command.Parameters.AddWithValue("@userId", userId);

                    int rowsAffected = command.ExecuteNonQuery();
                    this.mySqlConnection.Close();

                    return rowsAffected > 0;
                }
                else if (userHeight.HasValue && !userAge.HasValue && string.IsNullOrEmpty(userGender))
                {
                    string updateQuery = $@"UPDATE {TableName} 
                                    SET user_height = @userHeight 
                                    WHERE user_id = @userId";

                    var command = new MySqlCommand(updateQuery, this.mySqlConnection);
                    command.Parameters.AddWithValue("@userHeight", userHeight.Value);
                    command.Parameters.AddWithValue("@userId", userId);

                    int rowsAffected = command.ExecuteNonQuery();
                    this.mySqlConnection.Close();

                    return rowsAffected > 0;
                }
                else
                {
                    this.mySqlConnection.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.mySqlConnection.Close();
                throw new Exception($"Error changing user params: {ex.Message}");
            }
        }

        public List<Dictionary<string, object>> GetUserById(int userId)
        {
            var results = new List<Dictionary<string, object>>();

            try
            {
                this.mySqlConnection.Open();
                string query = $"SELECT * FROM {TableName} WHERE user_id = @userId";
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
                throw new Exception($"Error getting user by ID: {ex.Message}");
            }
        }

        public (bool exists, int? userId) CheckUserInDb(string login)
        {
            var results = new List<Dictionary<string, object>>();

            try
            {
                this.mySqlConnection.Open();
                string query = $"SELECT * FROM {TableName} WHERE user_login = @login";
                var command = new MySqlCommand(query, this.mySqlConnection);
                command.Parameters.AddWithValue("@login", login);

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

                if (results.Count > 0)
                {
                    int userId = Convert.ToInt32(results[0]["user_id"]);
                    return (true, userId);
                }
                else
                {
                    return (false, null);
                }
            }
            catch (Exception ex)
            {
                this.mySqlConnection.Close();
                throw new Exception($"Error checking user in DB: {ex.Message}");
            }
        }
    }
}