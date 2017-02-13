using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MySql.Connection;

namespace TTC_K12System.Classes
{
    class Subject
    {
        //PROPERTIES
        internal int ID { get; set; }
        internal string Type { get; set; }
        internal string Code { get; set; }
        internal string Title { get; set; }
        internal string Description { get; set; }
        internal short Hours { get; set; }

        /// <summary>
        /// Gets all subjects by program
        /// </summary>
        /// <param name="ProgramID">Program ID</param>
        /// <returns>Subjects List</returns>
        internal static List<Subject> getAll()
        {
            List<Subject> subjects = new List<Subject>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Builder.ConnectionString))
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT * FROM subjects";
                    con.Open();
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Subject subject = new Subject();
                            subject.ID = rdr.GetInt32(0);
                            subject.Type = rdr.GetString(1);
                            subject.Code = rdr.GetString(2);
                            subject.Title = rdr.GetString(3);
                            subject.Description = rdr.GetString(4);
                            subject.Hours = rdr.GetInt16(5);
                            subjects.Add(subject);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorTrapper.Log(ex, LogOptions.PromptTheUser);
            }
            return subjects;
        }

        /// <summary>
        /// Saves subject to DB
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
                        cmd.CommandText = "UPDATE subjects SET type = @type, code = @code,title = @title, description = @description, hours = @hoursWHERE id = @id";
                        cmd.Parameters.AddWithValue("id", ID);
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO subjects (type, code, title, description, hours ) VALUES (@type, @code, @title, @description ,@hours)";
                    }
                    cmd.Parameters.AddWithValue("type", Type);
                    cmd.Parameters.AddWithValue("code", Code);
                    cmd.Parameters.AddWithValue("title", Title);
                    cmd.Parameters.AddWithValue("description", Description);
                    cmd.Parameters.AddWithValue("hours", Hours);
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
