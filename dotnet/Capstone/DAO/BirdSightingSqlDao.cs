using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace Capstone.DAO
{
    public class BirdSightingSqlDao : IBirdSightingDao

    {
        private readonly string connectionString;

        private readonly IUserDao userDao;

        public BirdSightingSqlDao(string connectionString)
        {
            this.connectionString = connectionString;
            this.userDao = new UserSqlDao(connectionString);
        }


        public BirdSighting addSighting(BirdSighting birdSighting, string username)
        {
            BirdSighting newSighting = null;

            //int userId = userDao.GetUserIdByUsername(username);

            String sql = "INSERT INTO bird_sighting(user_id, bird_id, date_sighted) VALUES((SELECT user_id FROM users WHERE username = @username), @bird_id, @date_sighted)";

            //String sql = "INSERT INTO bird_sighting(user_id, bird_id, date_sighted) VALUES( user_id, @bird_id, @date_sighted)";

            int newSightingId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    //cmd.Parameters.AddWithValue("@user_id", userId);
                    cmd.Parameters.AddWithValue("@bird_id", birdSighting.birdId);
                    cmd.Parameters.AddWithValue("@date_sighted", birdSighting.dateSighted);

                    newSightingId = Convert.ToInt32(cmd.ExecuteScalar());

                }
                newSighting = getBirdSighting(newSightingId);

            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return newSighting;
        }

        public void deleteSighting(int id)
        {
            throw new NotImplementedException();
        }

        public void editSighting(BirdSighting sighting, int id)
        {
            string sql = "UPDATE bird_sighting SET bird_id = @bird_id, date_sighted = @date_sighted WHERE id = @id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@bird_id", sighting.birdId);
                    cmd.Parameters.AddWithValue("@date_sighted", sighting.dateSighted);

                    SqlDataReader reader = cmd.ExecuteReader();
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
        }

        public BirdSighting getBirdSighting(int id)
        {
            BirdSighting birdSighting = null;
            string sql = "SELECT id, user_id, bird_id, date_sighted FROM bird_sighting WHERE id = @id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        birdSighting = MapRowToBirdSighting(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return birdSighting;
        }

        public List<BirdSighting> getSightings(int birdId)
        {
            List<BirdSighting> sightingList = new List<BirdSighting>();

            string sql = "SELECT id, user_id, bird_id, date_sighted FROM bird_sighting WHERE bird_id = @bird_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@bird_id", birdId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        BirdSighting birdSighting = MapRowToBirdSighting(reader);
                        sightingList.Add(birdSighting);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return sightingList;
        }

        private BirdSighting MapRowToBirdSighting(SqlDataReader reader)
        {
            BirdSighting birdSighting = new BirdSighting();
            birdSighting.id = Convert.ToInt32(reader["id"]);
            birdSighting.userId = Convert.ToInt32(reader["user_id"]);
            birdSighting.birdId = Convert.ToInt32(reader["bird_id"]);
            birdSighting.dateSighted = Convert.ToDateTime(reader[("date_sighted")]);

            return birdSighting;
        }
    }
 
}
