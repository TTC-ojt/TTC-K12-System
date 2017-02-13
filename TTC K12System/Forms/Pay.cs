using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace TTC_K12System.Forms
{
    public partial class Pay : Form
    {
        internal Pay()
        {
            InitializeComponent();
        }

        private Classes.Student student = new Classes.Student();
        private Classes.Batch batch = new Classes.Batch();
        private List<Classes.Student> students = new List<Classes.Student>();
        private List<Classes.Transaction> transactions = new List<Classes.Transaction>();

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            Program.main.Show();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadStudent();
        }

        private void LoadStudent()
        {
            lblStudentNumber.Text = student.Number;
            string studentname = student.LastName + ", " + student.FirstName + " " + student.MiddleName;
            lblStudentName.Text = studentname;
            batch = Classes.Batch.GetByID(student.BatchID);
            lblBatch.Text = batch.Number.ToString();
            LoadTransactions();
        }

        private void LoadTransactions()
        {
            lblCurrentBalance.Text = Classes.Transaction.GetBalance(student.ID).ToString("N");
            transactions = Classes.Transaction.getTransactionsByStudent(student.ID);
            dgvPaymentHistory.Rows.Clear();
            foreach (Classes.Transaction transaction in transactions)
            {
                dgvPaymentHistory.Rows.Add(transaction.DateTime, transaction.Amount);
            }
        }

        private void lbxStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxStudents.SelectedIndex > -1)
            {
                txtSearch.Clear();
                student = students[lbxStudents.SelectedIndex];
                lbxStudents.Hide();
                LoadStudent();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.TextLength > 2)
            {
                students = Classes.Student.Find(txtSearch.Text, 0);
                lbxStudents.Items.Clear();
                foreach (Classes.Student student in students)
                {
                    if (txtSearch.Text.Any(char.IsDigit))
                    {
                        lbxStudents.Items.Add(student.Number);
                    }
                    else
                    {
                        lbxStudents.Items.Add(student.GetFullName());
                    }
                }
                lbxStudents.Show();
            }
            else
            {
                lbxStudents.Hide();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Classes.Transaction transaction = new Classes.Transaction();
            transaction.StudentID = student.ID;
            transaction.Code = txtORNumber.Text;
            transaction.Amount = nudAmount.Value;
            transaction.Save();
            LoadTransactions();
        }

        private void txtCash_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal cash = Convert.ToDecimal(txtCash.Text);
                txtChange.Text = (cash - nudAmount.Value).ToString("N");
            }
            catch (Exception ex)
            {
                ex = null;
                txtChange.Text = (0m).ToString("N");
            }
        }
    }
}
