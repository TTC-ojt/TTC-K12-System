using System;
using MySql.Data.MySqlClient;
using MySql.Connection;
using System.IO;

namespace TTC_K12System.Classes
{
    class Profile
    {
        internal Profile()
        {
            ID = 0;
            StudentID = 0;
            Gender = "Male";
            Birthdate = new DateTime(2000, 1, 1);
            Birthplace = "";
        }

        internal int ID;
        internal int StudentID;
        internal string Gender;
        internal DateTime Birthdate;
        internal string Birthplace;

        internal static Profile getByStudent(int StudentID)
        {
            Profile profile = new Profile();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Builder.ConnectionString))
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT * FROM profiles WHERE student_id = @student_id";
                    cmd.Parameters.AddWithValue("@student_id", StudentID);
                    con.Open();
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            profile.ID = rdr.GetInt32(0);
                            profile.StudentID = rdr.GetInt32(1);
                            profile.Gender = rdr.GetString(2);
                            profile.Birthdate = rdr.GetDateTime(3);
                            profile.Birthplace = rdr.GetString(4);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorTrapper.Log(ex, LogOptions.PromptTheUser);
            }
            return profile;
        }

        internal void Save()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Builder.ConnectionString))
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;
                    if (ID > 0)
                    {
                        cmd.CommandText = "UPDATE profiles SET gender = @gender, birthdate = @birthdate, birthplace = @birthplace WHERE id = @id";
                        cmd.Parameters.AddWithValue("id", ID);
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO profiles (student_id, gender, birthdate, birthplace) VALUES (@student_id, @gender,  @birthdate, @birthplace)";
                        cmd.Parameters.AddWithValue("student_id", StudentID);
                    }
                    cmd.Parameters.AddWithValue("gender", Gender);
                    cmd.Parameters.AddWithValue("birthdate", Birthdate);
                    cmd.Parameters.AddWithValue("birthplace", Birthplace);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    if (ID == 0) ID = Convert.ToInt32(cmd.LastInsertedId);
                }
            }
            catch (Exception ex)
            {
                ErrorTrapper.Log(ex, LogOptions.PromptTheUser);
            }
        }
    }
}