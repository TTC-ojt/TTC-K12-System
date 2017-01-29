using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TTC_K12System.Forms
{
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
        }

        private Classes.Student student = new Classes.Student();
        private Classes.Batch batch = new Classes.Batch();
        private Classes.Program program = new Classes.Program();
        private Classes.Contact contact = new Classes.Contact();
        private Classes.Profile profile = new Classes.Profile();

        private void btnChangeCourse_Click(object sender, EventArgs e)
        {
            ChooseProgram cp = new ChooseProgram();
            cp.ShowDialog();
            program = cp.program;
            LoadProgram();
        }

        private void LoadProgram()
        {
            student.Tuition = program.Tuition;
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

        private void btnNewBatch_Click(object sender, EventArgs e)
        {
            batch = Classes.Batch.New(program.ID);
            cbxBatch.Items.Add(batch.Number);
            cbxBatch.SelectedIndex = cbxBatch.Items.Count - 1;
        }

        private void cbxBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            short Number = Convert.ToInt16(cbxBatch.SelectedItem);
            batch = Classes.Batch.GetByProgramAndNumber(program.ID, Number);
            txtStudentNumber.Text = DateTime.Today.ToString("yy") + "-" + Number.ToString("D2") + "-" + Classes.Student.GetNextNumber();
        }

        private void CalculateAge(object sender, EventArgs e)
        {
            DateTime bdate = new DateTime(Convert.ToInt32(nudByear.Value), Convert.ToInt32(nudBmonth.Value), Convert.ToInt32(nudBday.Value));
            int age = DateTime.Today.Year - bdate.Year;
            if (DateTime.Today.DayOfYear < bdate.DayOfYear)
            {
                age--;
            }
            txtAge.Text = age.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            student = new Classes.Student();
            student.BatchID = batch.ID;
            student.Number = txtStudentNumber.Text;
            student.LRN = txtStudentLRN.Text;
            student.LastName = txtStudentLastName.Text;
            student.FirstName = txtStudentFirstName.Text;
            student.MiddleName = txtStudentMiddleName.Text;
            student.ExtName = txtStudentExtName.Text;
            student.Tuition = program.Tuition;
            student.Status = "1st Year";
            student.Save();
            profile = new Classes.Profile();
            profile.StudentID = student.ID;
            profile.Gender = (rbnProfileMale.Checked) ? rbnProfileMale.Text : rbnProfileFemale.Text;
            profile.Birthdate = new DateTime(Convert.ToInt32(nudByear.Value), Convert.ToInt32(nudBmonth.Value), Convert.ToInt32(nudBday.Value));
            profile.Birthplace = txtProfileBirthplace.Text;
            profile.Save();
            contact = new Classes.Contact();
            contact.StudentID = student.ID;
            contact.Phone = txtContactPhone.Text;
            contact.Email = txtContactEmail.Text;
            contact.Address = txtContactAddress.Text;
            contact.Guardian = txtContactGuardian.Text;
            contact.GuardianPhone = txtContactGuardianPhone.Text;
            contact.Save();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            Program.main.Show();
        }
    }
}
