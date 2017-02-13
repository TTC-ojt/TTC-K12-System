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
    public partial class StudentInfo : Form
    {
        public StudentInfo()
        {
            InitializeComponent();
        }

        internal Classes.Student student = new Classes.Student();
        private Classes.Batch batch = new Classes.Batch();
        private Classes.Contact contact = new Classes.Contact();
        private Classes.Profile profile = new Classes.Profile();

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
            student.LRN = txtStudentLRN.Text;
            student.LastName = txtStudentLastName.Text;
            student.FirstName = txtStudentFirstName.Text;
            student.MiddleName = txtStudentMiddleName.Text;
            student.ExtName = txtStudentExtName.Text;
            student.Tuition = nudTuition.Value;
            student.Status = "1st Year";
            student.Save();
            profile.StudentID = student.ID;
            profile.Gender = (rbnProfileMale.Checked) ? rbnProfileMale.Text : rbnProfileFemale.Text;
            profile.Birthdate = new DateTime(Convert.ToInt32(nudByear.Value), Convert.ToInt32(nudBmonth.Value), Convert.ToInt32(nudBday.Value));
            profile.Birthplace = txtProfileBirthplace.Text;
            profile.Save();
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
        }

        private void StudentInfo_Load(object sender, EventArgs e)
        {
            batch = Classes.Batch.GetByID(student.BatchID);
            profile = Classes.Profile.getByStudent(student.ID);
            contact = Classes.Contact.getContactByStudent(student.ID);

            txtBatchNumber.Text = batch.Number.ToString("D2");

            txtStudentNumber.Text = student.Number;
            txtStudentLRN.Text = student.LRN;
            txtStudentLastName.Text = student.LastName;
            txtStudentFirstName.Text = student.FirstName;
            txtStudentMiddleName.Text = student.MiddleName;
            txtStudentExtName.Text = student.ExtName;
            nudTuition.Value = student.Tuition;

            txtProfileBirthplace.Text = profile.Birthplace;
            nudBday.Value = profile.Birthdate.Day;
            nudBmonth.Value = profile.Birthdate.Month;
            nudByear.Value = profile.Birthdate.Year;
            CalculateAge(sender, e);

            txtContactAddress.Text = contact.Address;
            txtContactEmail.Text = contact.Email;
            txtContactGuardian.Text = contact.Guardian;
            txtContactGuardianPhone.Text = contact.GuardianPhone;
            txtContactPhone.Text = contact.Phone;
        }
    }
}
