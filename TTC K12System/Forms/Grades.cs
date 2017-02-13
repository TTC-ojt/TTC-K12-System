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
    public partial class Grades : Form
    {
        internal Grades()
        {
            InitializeComponent();
        }
        
        private Classes.Batch batch;
        private List<Classes.Student> students = new List<Classes.Student>();
        private List<Classes.Subject> subjects = new List<Classes.Subject>();

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            Program.main.Show();
        }

        private void LoadProgram()
        {
            LoadSubjects();
            LoadBatches();
        }

        private void LoadSubjects()
        {
            subjects = Classes.Subject.getAll();
            if (dgvGrades.Columns.Count > 2)
            {
                int cols = dgvGrades.Columns.Count;
                for (int i = 2; i < cols; i++)
                {
                    dgvGrades.Columns.RemoveAt(2);
                }
            }
            foreach (Classes.Subject subject in subjects)
            {
                var dgcSubject = new DataGridViewTextBoxColumn();
                dgcSubject.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgcSubject.FillWeight = 7;
                dgcSubject.Name = "dgc" + subject.Code;
                dgcSubject.HeaderText = subject.Code;
                dgvGrades.Columns.AddRange(new DataGridViewColumn[] { dgcSubject });
            }
            var dgc = new DataGridViewTextBoxColumn();
            dgc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgc.FillWeight = 15;
            dgc.Name = "dgcRemarks";
            dgc.HeaderText = "Remarks";
            dgvGrades.Columns.AddRange(new DataGridViewColumn[] { dgc });
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

        private void LoadStudents()
        {
            students = Classes.Student.getByBatch(batch.ID);
            dgvGrades.Rows.Clear();
            foreach (Classes.Student student in students)
            {
                string[] grades = new string[subjects.Count + 3];
                grades[0] = student.ID.ToString();
                grades[1] = student.GetFullName();
                int i = 2;
                decimal sum = 0m;
                int count = 0;
                foreach (Classes.Subject subject in subjects)
                {
                    grades[i] = Classes.Grade.getBySubjectAndStudent(subject.ID, student.ID).Score;
                    if (!string.IsNullOrWhiteSpace(grades[i]) && grades[i].Any(char.IsDigit))
                    {
                        sum += Convert.ToDecimal(grades[i]);
                        count += 1;
                    }
                    i++;
                }
                if (count > 0) grades[i] = (sum / count).ToString("N");
                else grades[i] = 0m.ToString("N");
                dgvGrades.Rows.Add(grades);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvGrades.Rows)
            {
                int student_id = Convert.ToInt32(row.Cells[0].Value);
                foreach (Classes.Subject subject in subjects)
                {
                    string score = row.Cells["dgc" + subject.Code].Value.ToString();
                    Classes.Grade grade = Classes.Grade.getBySubjectAndStudent(subject.ID, student_id);
                    grade.StudentID = student_id;
                    grade.SubjectID = subject.ID;
                    grade.Score = score;
                    grade.Save();
                }
            }
            LoadStudents();
        }

        private void dgvGrades_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dgvGrades.Rows[e.RowIndex];
            decimal sum = 0m;
            int count = 0;
            foreach (Classes.Subject subject in subjects)
            {
                string grade = Convert.ToString(row.Cells["dgc" + subject.Code].Value);
                if (!string.IsNullOrWhiteSpace(grade) && grade.All(char.IsDigit))
                {
                    sum += Convert.ToDecimal(grade);
                    count += 1;
                }
            }
            if (count > 0) row.Cells["dgcRemarks"].Value = (sum / count).ToString("N");
            else row.Cells["dgcRemarks"].Value = 0m.ToString("N");
        }

        private void Grades_Load(object sender, EventArgs e)
        {
            LoadProgram();
        }
    }
}
