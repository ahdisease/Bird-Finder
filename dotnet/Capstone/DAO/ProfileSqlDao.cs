using Capstone.Models;
using System.Data.SqlClient;
using System;
using Capstone.Exceptions;

namespace Capstone.DAO
{
    public class ProfileSqlDao : IUserProfileDAO
    {
        private readonly string connectionString;

        public ProfileSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public UserProfile UpdateUserProfile(UserProfile profile, string username)
        {
            string sql = "UPDATE users SET users.most_common_bird = ( SELECT TOP(1) bird_id FROM bird_sighting JOIN users ON users.user_id = bird_sighting.user_id WHERE users.username = @username GROUP BY bird_id ORDER BY count(bird_id) DESC ), users.location = @location, users.favorite_bird = @favorite_bird WHERE users.username = @username";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@location", profile.Location.ToString());
                    cmd.Parameters.AddWithValue("@skill_level", profile.SkillLevel.ToString());
                    cmd.Parameters.AddWithValue("@favorite_bird", profile.FavoriteBird.ToString());
                    SqlDataReader reader = cmd.ExecuteReader();

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

            return profile;
            
        }

        private UserProfile MapRowToProfile(SqlDataReader reader)
        {
            UserProfile profile = new UserProfile();

            profile.SkillLevel = GetSafeString(reader, "location");
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
