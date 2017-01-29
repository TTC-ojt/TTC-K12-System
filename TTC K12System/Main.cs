using MySql.Connection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TTC_K12System
{
    public partial class Main : Form
    {
        internal Main()
        {
            InitializeComponent();
        }

        private void btnRegistration_Click(object sender, EventArgs e)
        {
            Forms.Registration registration = new Forms.Registration();
            registration.Show();
            this.Hide();
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            Forms.ListOfStudents list = new Forms.ListOfStudents();
            list.Show();
            this.Hide();
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            Forms.Pay pay = new Forms.Pay();
            pay.Show();
            this.Hide();
        }
        private void btnTransactionHistory_Click(object sender, EventArgs e)
        {
            Forms.TransactionHistory th = new Forms.TransactionHistory();
            th.Show();
            this.Hide();
        }

        private void btnProgramProfile_Click(object sender, EventArgs e)
        {
            Forms.ProgramProfile program = new Forms.ProgramProfile();
            program.Show();
            this.Hide();
        }

        private void btnSubjects_Click(object sender, EventArgs e)
        {
            Forms.SubjectMaintenance subject = new Forms.SubjectMaintenance();
            subject.Show();
            this.Hide();
        }

        private void btnTeachers_Click(object sender, EventArgs e)
        {
            Forms.Teachers teachers = new Forms.Teachers();
            teachers.Show();
            this.Hide();
        }

        private void btnGrades_Click(object sender, EventArgs e)
        {
            Forms.Grades g = new Forms.Grades();
            g.Show();
            this.Hide();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Builder.UserID = Properties.Settings.Default.UserID;
            Builder.Password = Properties.Settings.Default.Password;
            Builder.Database = Properties.Settings.Default.Database;
            Builder.Server = Properties.Settings.Default.Server;
            Builder.Port = Properties.Settings.Default.Port;
        }
    }
}
