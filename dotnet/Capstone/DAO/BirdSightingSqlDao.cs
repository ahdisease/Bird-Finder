using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public class BirdSightingSqlDao : IBirdSightingDao

    {
        private readonly string connectionString;

        public BirdSightingSqlDao(string connectionString)
        {
            this.connectionString = connectionString;
        }



        public BirdSighting addSighting(BirdSighting birdSighting, int userId, int birdId, DateTime dateSighted)
        {
            throw new NotImplementedException();
        }

        public void deleteSighting(int id)
        {
            throw new NotImplementedException();
        }

        public void editSighting(BirdSighting sighting, int id)
        {
            throw new NotImplementedException();
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
            birdSighting.dateSighted = Convert.ToDateTime(reader["date_sighted"]);

            return birdSighting;
        }
    }
}
