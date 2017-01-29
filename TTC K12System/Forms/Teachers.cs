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
    public partial class Teachers : Form
    {
        internal Teachers()
        {
            InitializeComponent();
        }
        
        List<Classes.Teacher> teachers = new List<Classes.Teacher>();
        Classes.Teacher teacher = new Classes.Teacher();

        internal void loadAllTeachers()
        {
            teachers = Classes.Teacher.getAll();
            dgvTeachers.Rows.Clear();
            foreach(Classes.Teacher teacher in teachers)
            {
                dgvTeachers.Rows.Add(teacher.ID, teacher.Number, teacher.Name);
            }
            dgvTeachers.ClearSelection();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            teacher.Number = txtCode.Text;
            teacher.Name = txtName.Text;
            teacher.Save();
            loadAllTeachers();
        }

        private void Teachers_Load(object sender, EventArgs e)
        {
            loadAllTeachers();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            Program.main.Show();
        }

        private void dgvTeachers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTeachers.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvTeachers.SelectedRows[0].Cells[0].Value);
                teacher = teachers.Find(t => t.ID == id);
            }
            else
            {
                teacher = new Classes.Teacher();
            }
            txtCode.Text = teacher.Number;
            txtName.Text = teacher.Name;
        }
    }
}
