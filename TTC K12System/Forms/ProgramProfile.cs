using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TTC_K12System.Forms
{
    public partial class ProgramProfile : Form
    {
        internal ProgramProfile()
        {
            InitializeComponent();
        }

       List<Classes.Program> programs = new List<Classes.Program>();
       Classes.Program program = new Classes.Program();

        private void loadProgramProfile()
        {
            programs = Classes.Program.getAll();
            dgvProgramTitle.Rows.Clear();
            foreach(Classes.Program program in programs)
            {
                dgvProgramTitle.Rows.Add(program.ID, program.Title, program.Tuition);
            }
            dgvProgramTitle.ClearSelection();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            program.Title = txtProgramTitle.Text;
            program.Industry = txtIndustrySector.Text;
            program.Status = txtTVETRegStatus.Text;
            program.Copr = txtCOPR.Text;
            program.Calendar = txtTrainingCalendarCode.Text;
            program.Delivery = txtDelivery.Text;
            program.Tuition = nudTuition.Value;
            program.Save();
            loadProgramProfile();
        }

        private void ProgramProfile_Load(object sender, EventArgs e)
        {
            loadProgramProfile();
        }

        private void dgvProgramTitle_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvProgramTitle.SelectedRows.Count>0)
            {
                int id = Convert.ToInt32(dgvProgramTitle.SelectedRows[0].Cells[0].Value);
                program = programs.Find(p => p.ID == id);
            }
            else
            {
                program = new Classes.Program();
            }
            txtProgramTitle.Text = program.Title;
            txtIndustrySector.Text = program.Industry;
            txtTVETRegStatus.Text = program.Status;
            txtCOPR.Text = program.Copr;
            txtTrainingCalendarCode.Text = program.Calendar;
            txtDelivery.Text = program.Delivery;
            nudTuition.Value = program.Tuition;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            Program.main.Show();
        }
    }
}
