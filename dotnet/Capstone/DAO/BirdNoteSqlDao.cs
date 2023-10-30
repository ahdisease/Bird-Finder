using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;


namespace Capstone.DAO
{
    public class BirdNoteSqlDao : IBirdNoteDao

    {
        private readonly string connectionString;

        private readonly IUserDao userDao;

        public BirdNoteSqlDao(string connectionString)
        {
            this.connectionString = connectionString;
            this.userDao = new UserSqlDao(connectionString);
        }


        public BirdNote addSighting(BirdNote birdSighting, int birdId)
        {
            BirdNote newSighting = null;

            String sql = "INSERT INTO bird_sighting(bird_id, date_sighted, males_spotted, females_spotted, feeder_type, food_blend, notes) VALUES( @bird_id, @date_sighted, @males_spotted, @females_spotted, @feeder_type, @food_blend, @notes)";

            int newSightingId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@bird_id", birdId);
                    cmd.Parameters.AddWithValue("@date_sighted", birdSighting.DateSpotted);
                    cmd.Parameters.AddWithValue("@males_spotted", birdSighting.NumMales);
                    cmd.Parameters.AddWithValue("@females_spotted", birdSighting.NumFemales);
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

        public void deleteSighting(int id, string username)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("Delete_Bird_Sighting", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Username", username);

                    SqlDataReader reader = cmd.ExecuteReader();
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
        }

        public void editSighting(BirdNote birdSighting, int id, string username)
        {
            string sqlUpdate =  "UPDATE bird_sighting";
            string sqlSet =     " SET date_sighted = @date_sighted, males_spotted = @males_spotted, females_spotted = @females_spotted, feeder_type = @feeder_type, food_blend = @food_blend, notes = @notes";
            string sqlOutput =  " OUTPUT INSERTED.id";
            string sqlFrom =    " FROM bird_sighting JOIN bird ON bird.id = bird_id JOIN list ON list.id = list_id";
            string sqlWhere =   " WHERE bird_sighting.id = @id AND user_id = (SELECT user_id FROM users WHERE username = @username);";
            string sql = sqlUpdate + sqlSet + sqlOutput + sqlFrom + sqlWhere;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@date_sighted", birdSighting.DateSpotted);
                    cmd.Parameters.AddWithValue("@males_spotted", birdSighting.NumMales);
                    cmd.Parameters.AddWithValue("@females_spotted", birdSighting.NumFemales);
                    cmd.Parameters.AddWithValue("@feeder_type", birdSighting.FeederType);
                    cmd.Parameters.AddWithValue("@food_blend", birdSighting.FoodBlend);
                    cmd.Parameters.AddWithValue("@notes", birdSighting.Notes);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        throw new DaoException("Unable to update note.");
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
        }

        public BirdNote getBirdSighting(int id)
        {
            BirdNote birdSighting = null;
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

        public List<BirdNote> getSightings(int birdId)
        {
            List<BirdNote> sightingList = new List<BirdNote>();

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
                        BirdNote birdSighting = MapRowToBirdSighting(reader);
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

        private BirdNote MapRowToBirdSighting(SqlDataReader reader)
        {
            BirdNote birdSighting = new BirdNote();
            birdSighting.NoteId = Convert.ToInt32(reader["id"]);
            birdSighting.BirdId = Convert.ToInt32(reader["bird_id"]);
            birdSighting.DateSpotted = Convert.ToDateTime(reader[("date_sighted")]);
            birdSighting.NumMales = Convert.ToInt32(reader["males_spotted"]);
            birdSighting.NumFemales = Convert.ToInt32(reader["females_spotted"]);
            birdSighting.FeederType = Convert.ToString(reader["feeder_type"]);
            birdSighting.FoodBlend = Convert.ToString(reader["food_blend"]);
            birdSighting.Notes = Convert.ToString(reader["notes"]);

            return birdSighting;
        }
    }
 
}
