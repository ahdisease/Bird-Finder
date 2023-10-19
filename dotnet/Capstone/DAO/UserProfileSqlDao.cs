using Capstone.Models;
using System.Data.SqlClient;
using System;
using Capstone.Exceptions;


namespace Capstone.DAO
{
    public class UserProfileSqlDao : IUserProfileDao
    {
        private readonly string connectionString;
        private readonly BirdDao birdDao;

        public UserProfileSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
            birdDao = new BirdSqlDao(dbConnectionString);
        }

        public UserProfile GetUserProfileByUsername(string username)
        {
            UserProfile profile = null;

            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Must call a specific profile.");
            }

            string sql = "SELECT location, skill_level, favorite_bird, most_common_bird, profile_active FROM users WHERE username=@username;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = conn;
                    command.CommandText = sql;

                    command.Parameters.AddWithValue("@username", username);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        profile = MapRowToProfile(reader);
                    }
                    //if (profile.ProfileActive)
                    //{
                    return profile;
                    //}

                    //throw new DaoException("Unable to locate profile");
                    // TODO : Since there's currently no way to deactivate a profile on the front end, I've removed that restriction.
                    //        The front end is currently confused by user and profile being the same object in the database, so its logic
                    //        is making this feature use breaking.
                }
            }
            catch (SqlException)
            {
                throw new DaoException("A SQL error occurred.");
            }
        }

        public UserProfile UpdateUserProfile(UserProfile profile, string username)
        {
            if (profile == null || string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Unable to locate profile");
            }

            //if (!profile.ProfileActive)
            //{
            //    throw new DaoException("Unable to locate profile");
            //}

            //build sql command based on given parameters. Most common bird is always calculated for username.
            string sql =    "UPDATE users";
            string sqlSet =     " SET users.most_common_bird = ( SELECT TOP(1) bird_id FROM bird_sighting JOIN users ON users.user_id = bird_sighting.user_id WHERE users.username = @username GROUP BY bird_id ORDER BY count(bird_id) DESC ) ";
            string sqlOutput =  " OUTPUT INSERTED.location, INSERTED.skill_level, INSERTED.favorite_bird, INSERTED.most_common_bird, INSERTED.profile_active";
            string sqlWhere =   " WHERE users.username = @username";

            if (!string.IsNullOrEmpty(profile.ZipCode))
            {
                sqlSet += ", users.location = @location";
            }
            if (!string.IsNullOrEmpty(profile.SkillLevel))
            {
                sqlSet += ", users.skill_level = @skill_level";
            }
            if (profile.FavoriteBird.BirdId > 0)
            {
                sqlSet += ", users.favorite_bird = @favorite_bird"; 
            }

            sql = sql + sqlSet + sqlOutput + sqlWhere;

            //open connection and perform update
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand();
                    
                    command.Connection = conn;
                    command.CommandText = sql;

                    //update parameters as necessary
                    command.Parameters.AddWithValue("@username", username);
                    if (sql.Contains("@location"))
                    {
                        command.Parameters.AddWithValue("@location", profile.ZipCode.ToString());
                    }
                    if (sql.Contains("@skill_level"))
                    {
                        command.Parameters.AddWithValue("@skill_level", profile.SkillLevel.ToString());
                    }
                    if (sql.Contains("@favorite_bird"))
                    {
                        command.Parameters.AddWithValue("@favorite_bird", profile.FavoriteBird.BirdId);

                    }

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        return MapRowToProfile(reader);
                    }

                    throw new DaoException("Unable to locate profile");
                }
            } 
            catch (SqlException)
            {
                throw new DaoException("SQL Exception occurred.");
            }
        }

        public void DeleteUserProfile(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Profile could not be found");
            }

            string sql = "UPDATE users SET profile_active = 0 WHERE users.username = @username;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = conn;
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@username", username);
                   
                    SqlDataReader reader = command.ExecuteReader();
                }
            } catch (SqlException)
            {
                throw new DaoException("SQL Exception occurred.");
            }
        }

        public void ActivateUserProfile(UserProfile profile, string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Unable to activate profile without username");
            }

            string sql = "UPDATE users SET profile_active = 1 WHERE users.username = @username;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = conn;
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@username", username);

                    SqlDataReader reader = command.ExecuteReader();
                }
                profile.ProfileActive = true;
            } catch (SqlException)
            {
                throw new DaoException("SQL Exception occurred.");
            }
        }

        private UserProfile MapRowToProfile(SqlDataReader reader)
        {
            UserProfile profile = new UserProfile();

            profile.ZipCode = GetSafeString(reader, "location");
            profile.SkillLevel = GetSafeString(reader, "skill_level");
            profile.FavoriteBird = GetSafeBird(reader, "favorite_bird");
            profile.MostCommonBird = GetSafeBird(reader, "most_common_bird");
            profile.ProfileActive = Convert.ToBoolean(reader["profile_active"]);

            return profile;
        }

        private Bird GetSafeBird(SqlDataReader reader, string columnName)
        {
            Bird bird = null;
            int birdId = GetSafeInt(reader, columnName);
            if (birdId <= 0)
            {
                bird = new Bird();
                return bird;
            }

            try
            {
                bird = birdDao.getBird(birdId);
            }
            catch
            {
                bird = new Bird();
            }

            return bird;
        }

        private string GetSafeString(SqlDataReader reader, string columnName)
        {
            if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
            {
                return Convert.ToString(reader[columnName]);
            }
            return "";
        }

        private int GetSafeInt(SqlDataReader reader, string columnName)
        {
            if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
            {
                return Convert.ToInt32(reader[columnName]);
            }
            return -1;
        }

       
    }
}
