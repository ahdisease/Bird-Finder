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


        public BirdSighting addSighting(BirdSighting birdSighting, int birdId)
        {
            BirdSighting newSighting = null;

            String sql = "INSERT INTO bird_sighting(bird_id, date_sighted, males_spotted, females_spotted, feeder_type, food_blend, notes) VALUES( @bird_id, @date_sighted, @males_spotted, @females_spotted, @feeder_type, @food_blend, @notes)";

            int newSightingId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@bird_id", birdId);
                    cmd.Parameters.AddWithValue("@date_sighted", birdSighting.DateSighted);
                    cmd.Parameters.AddWithValue("@males_spotted", birdSighting.MalesSpotted);
                    cmd.Parameters.AddWithValue("@females_spotted", birdSighting.FemalesSpotted);
                    cmd.Parameters.AddWithValue("@feeder_type", birdSighting.FeederType);
                    cmd.Parameters.AddWithValue("@food_blend", birdSighting.FoodBlend);
                    cmd.Parameters.AddWithValue("@notes", birdSighting.Notes);

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
            string sql = "DELETE FROM bird_sighting WHERE id = @id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
        }

        public void editSighting(BirdSighting birdSighting, int id)
        {
            string sql = "UPDATE bird_sighting SET date_sighted = @date_sighted, males_spotted = @males_spotted, females_spotted = @females_spotted, feeder_type = @feeder_type, food_blend = @food_blend, notes = @notes WHERE id = @id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    //cmd.Parameters.AddWithValue("@bird_id", birdSighting.BirdId);
                    cmd.Parameters.AddWithValue("@date_sighted", birdSighting.DateSighted);
                    cmd.Parameters.AddWithValue("@males_spotted", birdSighting.MalesSpotted);
                    cmd.Parameters.AddWithValue("@females_spotted", birdSighting.FemalesSpotted);
                    cmd.Parameters.AddWithValue("@feeder_type", birdSighting.FeederType);
                    cmd.Parameters.AddWithValue("@food_blend", birdSighting.FoodBlend);
                    cmd.Parameters.AddWithValue("@notes", birdSighting.Notes);

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
            string sql = "SELECT id, bird_id, date_sighted, males_spotted, females_spotted, feeder_type, food_blend, notes FROM bird_sighting WHERE id = @id";

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

            string sql = "SELECT id, bird_id, date_sighted, males_spotted, females_spotted, feeder_type, food_blend, notes FROM bird_sighting WHERE bird_id = @bird_id";

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
            birdSighting.Id = Convert.ToInt32(reader["id"]);
            birdSighting.BirdId = Convert.ToInt32(reader["bird_id"]);
            birdSighting.DateSighted = Convert.ToDateTime(reader[("date_sighted")]);
            birdSighting.MalesSpotted = Convert.ToInt32(reader["males_spotted"]);
            birdSighting.FemalesSpotted = Convert.ToInt32(reader["females_spotted"]);
            birdSighting.FeederType = Convert.ToString(reader["feeder_type"]);
            birdSighting.FoodBlend = Convert.ToString(reader["food_blend"]);
            birdSighting.Notes= Convert.ToString(reader["notes"]);

            return birdSighting;
        }
    }
 
}
