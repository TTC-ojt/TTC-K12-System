using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using MySql.Connection;
using System.IO;
using System.Windows.Forms;

namespace TTC_K12System.Classes
{
    class Student
    {
        internal Student()
        {
            ID = 0;
            BatchID = 0;
            Number = "";
            Tuition = 0;
            Status = "1st Year";
        }

        //PROPERTIES
        internal int ID { get; set; }
        internal int BatchID { get; set; }
        internal string Number { get; set; }
        internal string LRN { get; set; }
        internal string LastName { get; set; }
        internal string FirstName { get; set; }
        internal string MiddleName { get; set; }
        internal string ExtName { get; set; }
        internal decimal Tuition { get; set; }
        internal string Status { get; set; }

        /// <summary>
        /// Get all students by Batch
        /// </summary>
        /// <param name="BatchID">Batch ID</param>
        /// <returns>Student List</returns>
        internal static List<Student> getByBatch(int BatchID)
        {
            List<Student> students = new List<Student>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Builder.ConnectionString))
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT * FROM students WHERE batch_id = @batch_id";
                    cmd.Parameters.AddWithValue("batch_id", BatchID);
                    con.Open();
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Student student = new Student();
                            student.ID = rdr.GetInt32(0);
                            student.BatchID = rdr.GetInt32(1);
                            student.Number = rdr.GetString(2);
                            student.LRN = rdr.GetString(3);
                            student.LastName = rdr.GetString(4);
                            student.FirstName = rdr.GetString(5);
                            student.MiddleName = rdr.GetString(6);
                            student.ExtName = rdr.GetString(7);
                            student.Tuition = rdr.GetDecimal(8);
                            student.Status = rdr.GetString(9);
                            students.Add(student);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorTrapper.Log(ex, LogOptions.PromptTheUser);
            }
            return students;
        }

        /// <summary>
        /// Get Student by Number
        /// </summary>
        /// <param name="number">Student Number</param>
        /// <returns>Student</returns>
        internal static Student getStudentByNumber(string number)
        {
            Student student = new Student();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Builder.ConnectionString))
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT * FROM students WHERE number = @number";
                    cmd.Parameters.AddWithValue("number", number);
                    con.Open();
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            student.ID = rdr.GetInt32(0);
                            student.BatchID = rdr.GetInt32(1);
                            student.Number = rdr.GetString(2);
                            student.LRN = rdr.GetString(3);
                            student.LastName = rdr.GetString(4);
                            student.FirstName = rdr.GetString(5);
                            student.MiddleName = rdr.GetString(6);
                            student.ExtName = rdr.GetString(7);
                            student.Tuition = rdr.GetDecimal(8);
                            student.Status = rdr.GetString(9);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorTrapper.Log(ex, LogOptions.PromptTheUser);
            }
            return student;
        }

        internal static Student getByID(int ID)
        {
            Student student = new Student();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Builder.ConnectionString))
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT * FROM students WHERE id = @id";
                    cmd.Parameters.AddWithValue("id", ID);
                    con.Open();
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            student.ID = rdr.GetInt32(0);
                            student.BatchID = rdr.GetInt32(1);
                            student.Number = rdr.GetString(2);
                            student.LRN = rdr.GetString(3);
                            student.LastName = rdr.GetString(4);
                            student.FirstName = rdr.GetString(5);
                            student.MiddleName = rdr.GetString(6);
                            student.ExtName = rdr.GetString(7);
                            student.Tuition = rdr.GetDecimal(8);
                            student.Status = rdr.GetString(9);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorTrapper.Log(ex, LogOptions.PromptTheUser);
            }
            return student;
        }

        /// <summary>
        /// Find students by Name //TODO
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        internal static List<Student> Find(string query, int BatchID)
        {
            List<Student> students = new List<Student>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Builder.ConnectionString))
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;
                    if (BatchID > 0)
                    {
                        cmd.CommandText = "SELECT students.id, students.batch_id, students.number, students.lrn_number, students.lastname, students.firstname, students.middlename, students.extname, students.tuition, students.status FROM students JOIN batches ON students.batch_id=batches.id WHERE CONCAT_WS(' ', students.number, students.lrn_number, students.firstname, students.middlename, students.lastname) LIKE '%" + query + "%' AND batches.id=@batch_id";
                        cmd.Parameters.AddWithValue("batch_id", BatchID);
                    }
                    else
                    {
                        cmd.CommandText = "SELECT * FROM students WHERE CONCAT_WS(' ', number, lrn_number, firstname, middlename, lastname) LIKE '%" + query + "%'";
                    }
                    con.Open();
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Student student = new Student();
                            student.ID = rdr.GetInt32(0);
                            student.BatchID = rdr.GetInt32(1);
                            student.Number = rdr.GetString(2);
                            student.LRN = rdr.GetString(3);
                            student.LastName = rdr.GetString(4);
                            student.FirstName = rdr.GetString(5);
                            student.MiddleName = rdr.GetString(6);
                            student.ExtName = rdr.GetString(7);
                            student.Tuition = rdr.GetDecimal(8);
                            student.Status = rdr.GetString(9);
                            students.Add(student);
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                ErrorTrapper.Log(ex, LogOptions.PromptTheUser);
            }
            return students;
        }

        /// <summary>
        /// Gets the Next Sequence for Student Number
        /// </summary>
        /// <returns>Student Number</returns>
        internal static string GetNextNumber()
        {
            int number = 1;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Builder.ConnectionString))
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT number FROM students ORDER BY id DESC LIMIT 1";
                    con.Open();
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            string last = rdr.GetString(0);
                            number = Convert.ToInt32(last.Split('K').Last()) + 1;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorTrapper.Log(ex, LogOptions.PromptTheUser);
            }
            return "K" + number.ToString("D4");
        }

        internal string GetFullName()
        {
            string fullname = LastName;
            if (!string.IsNullOrWhiteSpace(ExtName)) fullname += " " + ExtName;
            fullname += ", " + FirstName + " ";
            if (!string.IsNullOrWhiteSpace(MiddleName))
            {
                var mis = MiddleName.Split(' ');
                foreach (var mi in mis)
                {
                    if (mi.Length > 0) fullname += mi.Substring(0, 1) + ". ";
                }
            }
            return fullname;
        }

        /// <summary>
        /// Saves student to DB
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
                        cmd.CommandText = "UPDATE students SET number = @number, lrn_number = @lrn_number, lastname = @lastname, firstname = @firstname, middlename = @middlename, extname=@extname, tuition = @tuition, status = @status WHERE id = @id";
                        cmd.Parameters.AddWithValue("id", ID);
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO students (batch_id, number, lrn_number, lastname, firstname, middlename, extname, tuition, status) VALUES (@batch_id, @number, @lrn_number, @lastname, @firstname, @middlename, @extname, @tuition, @status)";
                        cmd.Parameters.AddWithValue("batch_id", BatchID);
                    }
                    cmd.Parameters.AddWithValue("number", Number);
                    cmd.Parameters.AddWithValue("lrn_number", LRN);
                    cmd.Parameters.AddWithValue("lastname", LastName);
                    cmd.Parameters.AddWithValue("firstname", FirstName);
                    cmd.Parameters.AddWithValue("extname", ExtName);
                    cmd.Parameters.AddWithValue("middlename", MiddleName);
                    cmd.Parameters.AddWithValue("tuition", Tuition);
                    cmd.Parameters.AddWithValue("status", Status);
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