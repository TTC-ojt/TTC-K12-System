using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TTC_K12System.Forms
{
    public partial class TransactionHistory : Form
    {
        internal TransactionHistory()
        {
            InitializeComponent();
        }
        
        private Classes.Program program;
        private Classes.Batch batch;
        private List<Classes.Student> students = new List<Classes.Student>();

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            Program.main.Show();
        }

        private void btnChangeCourse_Click(object sender, EventArgs e)
        {
            ChooseProgram vCProgram = new ChooseProgram();
            vCProgram.ShowDialog();
            program = vCProgram.program;
            if (program != null) LoadProgram();
        }

        private void LoadProgram()
        {
            txtProgramTitle.Text = program.Title;
            LoadBatches();
        }
        private void LoadBatches()
        {
            List<Classes.Batch> batches = Classes.Batch.GetAllByProgram(program.ID);
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
            batch = Classes.Batch.GetByProgramAndNumber(program.ID, Number);
            LoadStudents();
        }

        private void LoadStudents()
        {
            students = Classes.Student.Find("", program.ID, batch.ID);
            dgvStudents.Rows.Clear();
            foreach (Classes.Student student in students)
            {
                dgvStudents.Rows.Add(student.ID, student.Number, student.GetFullName(), Classes.Transaction.GetBalance(student.ID));
            }
        }
    }
}
