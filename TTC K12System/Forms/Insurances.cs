using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TTC_K12System.Forms
{
    public partial class Insurances : Form
    {
        internal Insurances()
        {
            InitializeComponent();
        }
        
        private Classes.Batch batch = new Classes.Batch();
        private List<Classes.Student> students = new List<Classes.Student>();
        private Bitmap img;

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            Program.main.Show();
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
            batch = Classes.Batch.GetByNumber( Number);
            LoadStudents();
        }

        private void LoadStudents()
        {
            students = Classes.Student.Find(txtSearch.Text,batch.ID);
            dgvInsurances.Rows.Clear();
            int c = 1;
            foreach (Classes.Student student in students)
            {
                Classes.Insurance insurance = Classes.Insurance.getByStudent(student.ID);
                dgvInsurances.Rows.Add(student.ID, c, student.GetFullName(), insurance.Beneficiary, insurance.Relationship, insurance.Amount.ToString("N"), insurance.Expiration.ToShortDateString());
                c++;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvInsurances.Rows)
            {
                int id = Convert.ToInt32(row.Cells[0].Value);
                Classes.Insurance insurance = Classes.Insurance.getByStudent(id);
                insurance.StudentID = id;
                insurance.Company = txtCompany.Text;
                insurance.Beneficiary = row.Cells[dgcBeneficiary.Name].Value.ToString();
                insurance.Relationship = row.Cells[dgcRelationship.Name].Value.ToString();
                insurance.Amount = Convert.ToDecimal(row.Cells[dgcAmount.Name].Value);
                insurance.Expiration = Convert.ToDateTime(row.Cells[dgcExpiration.Name].Value);
                insurance.Save();
            }
            LoadStudents();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            dgvInsurances.ClearSelection();
            int height = dgvInsurances.Height;
            dgvInsurances.Height = dgvInsurances.RowCount * dgvInsurances.RowTemplate.Height + dgvInsurances.Columns[0].HeaderCell.Size.Height;
            dgvInsurances.ScrollBars = ScrollBars.None;
            img = new Bitmap(dgvInsurances.Width, dgvInsurances.Height);
            dgvInsurances.DrawToBitmap(img, new Rectangle(0, 0, dgvInsurances.Width, dgvInsurances.Height));
            dgvInsurances.Height = height;
            dgvInsurances.ScrollBars = ScrollBars.Vertical;
            printPreviewDialog.ShowDialog();
        }

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.Bicubic;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.DrawImage(img, e.PageBounds.Width / 2 - img.Width / 2, 50);
        }

        private void Insurances_Load(object sender, EventArgs e)
        {
            LoadBatches();
            foreach (DataGridViewColumn col in dgvInsurances.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void dtpExpiration_Leave(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvInsurances.Rows)
            {
                row.Cells[dgcExpiration.Name].Value = dtpExpiration.Value.ToShortDateString();
            }
        }

        private void nudAmount_ValueChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvInsurances.Rows)
            {
                row.Cells[dgcAmount.Name].Value = nudAmount.Value;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadStudents();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.TextLength > 2)
            {
                LoadStudents();
            }
            else
            {
                dgvInsurances.Rows.Clear();
            }
        }
    }
}
