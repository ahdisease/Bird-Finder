using Capstone.Models;
using System.Data.SqlClient;
using System;
using Capstone.Exceptions;

namespace Capstone.DAO
{
    public class UserProfileSqlDao : IUserProfileDao
    {
        private readonly string connectionString;

        public UserProfileSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public UserProfile GetUserProfileByUsername(string username)
        {
            UserProfile profile = null;

            if (string.IsNullOrEmpty(username))
            {
                throw new DaoException("Must call a specific profile.");
            }

            string sql = "SELECT location, skill_level, favorite_bird, most_common_bird FROM users WHERE username=@username;";

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

                    return profile;
                }
            }
            catch (SqlException)
            {
                throw new DaoException("A SQL error occurred.");
            }
        }

        public void UpdateUserProfile(UserProfile profile, string username)
        {
            if (profile == null || username == null || username.Trim() == "")
            {
                throw new ArgumentException();
            }

            //build sql command based on given parameters. Most common bird is always calculated for username.
            string sql = "UPDATE users SET users.most_common_bird = ( SELECT TOP(1) bird_id FROM bird_sighting JOIN users ON users.user_id = bird_sighting.user_id WHERE users.username = @username GROUP BY bird_id ORDER BY count(bird_id) DESC ) ";
            
            if (profile.Location != null)
            {
                sql += ", users.location = @location";
            }
            if (profile.SkillLevel != null || profile.SkillLevel == "")
            {
                sql += ", users.skill_level = @skill_level";
            }
            else
            {
                sql += ", users.skill_level = skill_level";
            }
            if (profile.FavoriteBird > 0)
            {
                sql += ", users.favorite_bird = @favorite_bird"; 
            }

            sql += " WHERE users.username = @username";

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
                        command.Parameters.AddWithValue("@location", profile.Location.ToString());
                    }
                    if (sql.Contains("@skill_level"))
                    {
                        command.Parameters.AddWithValue("@skill_level", profile.SkillLevel.ToString());
                    }
                    if (sql.Contains("@favorite_bird"))
                    {
                        command.Parameters.AddWithValue("@favorite_bird", profile.FavoriteBird.ToString());

                    }

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        profile = MapRowToProfile(reader);
                    }
                }
            } 
            catch (SqlException)
            {
                throw new DaoException("SQL Exception occurred.");
            }
        }


        private UserProfile MapRowToProfile(SqlDataReader reader)
        {
            UserProfile profile = new UserProfile();

            profile.Location = GetSafeString(reader, "location");
            profile.SkillLevel = GetSafeString(reader,"skill_level");
            profile.FavoriteBird = GetSafeInt(reader, "favorite_bird");
            profile.MostCommonBird = GetSafeInt(reader, "most_common_bird");

            return profile;
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
