using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MySql.Connection;


namespace TTC_K12System.Classes
{
    class Teacher
    {
        //PROPERTIES
        internal int ID { get; set; }
        internal string Number { get; set; }
        internal string Name { get; set; }

        /// <summary>
        /// Gets all teachers
        /// </summary>
        /// <returns>Teacher List</returns>
        internal static List<Teacher> getAll()
        {
            List<Teacher> teachers = new List<Teacher>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Builder.ConnectionString))
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT * FROM teachers";
                    con.Open();
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Teacher teacher = new Teacher();
                            teacher.ID = rdr.GetInt32(0);
                            teacher.Number = rdr.GetString(1);
                            teacher.Name = rdr.GetString(2);
                            teachers.Add(teacher);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorTrapper.Log(ex, LogOptions.PromptTheUser);
            }
            return teachers;
        }

        internal static Teacher getByID(int ID)
        {
            Teacher teacher = new Teacher();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Builder.ConnectionString))
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT * FROM teachers WHERE id = @id";
                    cmd.Parameters.AddWithValue("id", ID);
                    con.Open();
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            teacher.ID = rdr.GetInt32(0);
                            teacher.Number = rdr.GetString(1);
                            teacher.Name = rdr.GetString(2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorTrapper.Log(ex, LogOptions.PromptTheUser);
            }
            return teacher;
        }

        internal static Teacher getByCode(string Code)
        {
            Teacher teacher = new Teacher();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Builder.ConnectionString))
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT * FROM teachers WHERE code = @code";
                    cmd.Parameters.AddWithValue("number", Code);
                    con.Open();
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            teacher.ID = rdr.GetInt32(0);
                            teacher.Number = rdr.GetString(1);
                            teacher.Name = rdr.GetString(2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorTrapper.Log(ex, LogOptions.PromptTheUser);
            }
            return teacher;
        }

        internal static Teacher getByName(string Name)
        {
            Teacher teacher = new Teacher();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Builder.ConnectionString))
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT * FROM teachers WHERE name = @name";
                    cmd.Parameters.AddWithValue("name", Name);
                    con.Open();
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            teacher.ID = rdr.GetInt32(0);
                            teacher.Number = rdr.GetString(1);
                            teacher.Name = rdr.GetString(2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorTrapper.Log(ex, LogOptions.PromptTheUser);
            }
            return teacher;
        }

        /// <summary>
        /// Saves teacher to DB
        /// </summary>
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
                        cmd.CommandText = "UPDATE teachers SET number = @number, name = @name WHERE id = @id";
                        cmd.Parameters.AddWithValue("id", ID);
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO teachers (number, name) VALUES (@number, @name)";
                    }
                    cmd.Parameters.AddWithValue("number", Number);
                    cmd.Parameters.AddWithValue("name", Name);
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
