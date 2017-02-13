using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TTC_K12System.Forms
{
    public partial class ListOfStudents : Form
    {
        internal ListOfStudents()
        {
            InitializeComponent();
        }

        private Classes.Student student = new Classes.Student();
        private Classes.Batch batch = new Classes.Batch();
        private List<Classes.Student> students = new List<Classes.Student>();

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            Program.main.Show();
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            StudentInfo info = new StudentInfo();
            info.student = student;
            info.ShowDialog();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadStudents();
        }

        private void LoadStudents()
        {
            students = Classes.Student.Find(txtSearch.Text, batch.ID);
            dgvStudentsLists.Rows.Clear();
            foreach (Classes.Student student in students)
            {
                Classes.Batch batch = Classes.Batch.GetByID(student.BatchID);
                dgvStudentsLists.Rows.Add(student.ID, student.Number, student.GetFullName(), batch.Number);
            }
        }

        private void dgvStudentsLists_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStudentsLists.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvStudentsLists.SelectedRows[0].Cells[0].Value);
                student = students.Find(s => s.ID == id);
            }
            else
            {
                student = new Classes.Student();
            }
        }

        private void LoadBatches()
        {
            List<Classes.Batch> batches = Classes.Batch.GetAll();
            cbxBatch.Items.Clear();
            foreach (Classes.Batch batch in batches)
            {
                cbxBatch.Items.Add(batch.Number);
            }
            cbxBatch.SelectedIndex = cbxBatch.Items.Count - 1;
        }
        private void cbxBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            short Number = Convert.ToInt16(cbxBatch.SelectedItem);
            batch = Classes.Batch.GetByNumber(Number);
            LoadStudents();
        }

        Bitmap img;
        private void btnPrint_Click(object sender, EventArgs e)
        {
            img = new Bitmap(dgvStudentsLists.Width, dgvStudentsLists.Height);
            dgvStudentsLists.DrawToBitmap(img, new Rectangle(0, 0, dgvStudentsLists.Width, dgvStudentsLists.Height));
            printPreviewDialog.ShowDialog();
        }

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(img, e.MarginBounds);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.TextLength > 2)
            {
                LoadStudents();
            }
            else
            {
                dgvStudentsLists.Rows.Clear();
            }
        }

        private void ListOfStudents_Load(object sender, EventArgs e)
        {
            LoadBatches();
        }
    }
}
