using System;
using MySql.Data.MySqlClient;
using MySql.Connection;

namespace TTC_K12System.Classes
{ 
    class Contact
    {
        internal Contact()
        {
            ID = 0;
            StudentID = 0;
            Phone = "";
            Email = "";
            Address = "";
        }

        //PROPERTIES
        internal int ID { get; set; }
        internal int StudentID { get; set; }
        internal string Phone { get; set; }
        internal string Email { get; set; }
        internal string Address { get; set; }
        internal string Guardian { get; set; }
        internal string GuardianPhone { get; set; }

        /// <summary>
        /// Gets Contact by Student
        /// </summary>
        /// <param name="StudentID">Student ID</param>
        /// <returns>Contact</returns>
        internal static Contact getContactByStudent(int StudentID)
        {
            Contact contact = new Contact();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Builder.ConnectionString))
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT * FROM contacts WHERE student_id = @student_id";
                    cmd.Parameters.AddWithValue("@student_id", StudentID);
                    con.Open();
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            contact.ID = rdr.GetInt32(0);
                            contact.StudentID = rdr.GetInt32(1);
                            contact.Phone = rdr.GetString(2);
                            contact.Email = rdr.GetString(3);
                            contact.Address = rdr.GetString(4);
                            contact.Guardian = rdr.GetString(5);
                            contact.GuardianPhone = rdr.GetString(6);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorTrapper.Log(ex, LogOptions.PromptTheUser);
            }
            return contact;
        }

        /// <summary>
        /// Saves Contact to DB
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
                        cmd.CommandText = "UPDATE contacts SET phone = @phone, email = @email, address = @address, guardian = @guardian, guardian_phone = @guardian_phone WHERE id = @id";
                        cmd.Parameters.AddWithValue("id", ID);
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO contacts (student_id, phone, email, address, guardian, guardian_phone) VALUES (@student_id, @phone, @email, @address, @guardian, @guardian_phone)";
                        cmd.Parameters.AddWithValue("student_id", StudentID);
                    }
                    cmd.Parameters.AddWithValue("phone", Phone);
                    cmd.Parameters.AddWithValue("email", Email);
                    cmd.Parameters.AddWithValue("address", Address);
                    cmd.Parameters.AddWithValue("guardian", Guardian);
                    cmd.Parameters.AddWithValue("guardian_phone", GuardianPhone);
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
