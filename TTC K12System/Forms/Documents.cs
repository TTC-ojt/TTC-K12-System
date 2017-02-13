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
    public partial class Documents : Form
    {
        internal Documents()
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
            batch = Classes.Batch.GetByNumber(Number);
            LoadStudents();
        }

        private void LoadStudents()
        {
            students = Classes.Student.getByBatch(batch.ID);
            dgvChecklist.Rows.Clear();
            int c = 1;
            foreach (Classes.Student student in students)
            {
                Classes.Document document = Classes.Document.getByStudent(student.ID);
                if (rbtnComplete.Checked && document.IsComplete())
                {
                    dgvChecklist.Rows.Add(student.ID, c, student.GetFullName(), document.C137, document.C138, document.GoodMoral, document.BirthCert, document.Pictures, document.Diploma, document.BrgyClearance, document.Envelope, document.Encae, document.Remarks);
                }
                else if (rbtnIncomplete.Checked && !document.IsComplete())
                {
                    dgvChecklist.Rows.Add(student.ID, c, student.GetFullName(), document.C137, document.C138, document.GoodMoral, document.BirthCert, document.Pictures, document.Diploma, document.BrgyClearance, document.Envelope, document.Encae, document.Remarks);
                }
                else if (rbtnALL.Checked)
                {
                    dgvChecklist.Rows.Add(student.ID, c, student.GetFullName(), document.C137, document.C138, document.GoodMoral, document.BirthCert, document.Pictures, document.Diploma, document.BrgyClearance, document.Envelope, document.Encae, document.Remarks);
                }
                c++;
            }
        }

        private void Filter(object sender, EventArgs e)
        {
            LoadStudents();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvChecklist.Rows)
            {
                int StudentID = Convert.ToInt32(row.Cells[0].Value);
                Classes.Document document = Classes.Document.getByStudent(StudentID);
                document.StudentID = StudentID;
                document.C137 = Convert.ToBoolean(row.Cells["dgc137"].Value);
                document.C138 = Convert.ToBoolean(row.Cells["dgc138"].Value);
                document.GoodMoral = Convert.ToBoolean(row.Cells["dgcGoodMoral"].Value);
                document.BirthCert = Convert.ToBoolean(row.Cells["dgcBirthCert"].Value);
                document.Pictures = Convert.ToBoolean(row.Cells["dgcPictures"].Value);
                document.Diploma = Convert.ToBoolean(row.Cells["dgcDiploma"].Value);
                document.BrgyClearance = Convert.ToBoolean(row.Cells["dgcClearance"].Value);
                document.Envelope = Convert.ToBoolean(row.Cells["dgcEnvelope"].Value);
                document.Encae = Convert.ToBoolean(row.Cells[dgcEncae.Name].Value);
                document.Remarks = row.Cells["dgcRemarks"].Value.ToString();
                document.Save();
            }
            LoadStudents();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            dgvChecklist.ClearSelection();
            int height = dgvChecklist.Height;
            dgvChecklist.Height = dgvChecklist.RowCount * dgvChecklist.RowTemplate.Height + dgvChecklist.Columns[0].HeaderCell.Size.Height;
            dgvChecklist.ScrollBars = ScrollBars.None;
            img = new Bitmap(dgvChecklist.Width, dgvChecklist.Height);
            dgvChecklist.DrawToBitmap(img, new Rectangle(0, 0, dgvChecklist.Width, dgvChecklist.Height));
            dgvChecklist.Height = height;
            dgvChecklist.ScrollBars = ScrollBars.Vertical;
            printPreviewDialog.ShowDialog();
        }

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.Bicubic;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.DrawImage(img, 0,50);
        }

        private void Documents_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn col in dgvChecklist.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            LoadBatches();
        }
    }
}
