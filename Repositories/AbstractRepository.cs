using MySql.Data.MySqlClient;
using DotNetEnv;

namespace CaloryCounterWeb.Repositories
{
    abstract public class AbstractRepository
    {
        protected MySqlConnection mySqlConnection;
        public AbstractRepository()
        {
            this.mySqlConnection = new MySqlConnection($"server={Env.GetString("MYSQL_HOST")};database={Env.GetString("MYSQL_DATABASE")};uid={Env.GetString("MYSQL_LOGIN")};pwd={Env.GetString("MYSQL_PASSWORD")};port={Env.GetString("MYSQL_PORT")}");
        }
        
    }
}