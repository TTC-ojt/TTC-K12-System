using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TTC_K12System.Forms
{
    public partial class TransactionHistory : Form
    {
        internal TransactionHistory()
        {
            InitializeComponent();
            
        }
        
        private Classes.Batch batch;
        private List<Classes.Student> students = new List<Classes.Student>();

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
            students = Classes.Student.Find("", batch.ID);
            dgvStudents.Rows.Clear();
            int c = 1;
            foreach (Classes.Student student in students)
            {
                dgvStudents.Rows.Add(student.ID, c, student.GetFullName(), student.Tuition.ToString("N"), Classes.Transaction.GetBalance(student.ID).ToString("N"));
                c++;
            }
        }

        private void btnEditTuition_Click(object sender, EventArgs e)
        {
            UpdateTuition ut = new UpdateTuition();
            if (ut.ShowDialog() == DialogResult.OK)
            {
                decimal tuition = ut.tuition;
                foreach (DataGridViewRow row in dgvStudents.SelectedRows)
                {
                    int id = Convert.ToInt32(row.Cells[0].Value);
                    Classes.Student student = students.Find(s => s.ID == id);
                    student.Tuition = tuition;
                    student.Save();
                }
                LoadStudents();
            }
        }

        Bitmap image;
        private void btnPrintGrp_Click(object sender, EventArgs e)
        {
            psPrint = false;
            dgvStudents.ClearSelection();
            int height = dgvStudents.Height;
            dgvStudents.Height = dgvStudents.RowCount * dgvStudents.RowTemplate.Height + dgvStudents.Columns[0].HeaderCell.Size.Height;
            dgvStudents.ScrollBars = ScrollBars.None;
            image = new Bitmap(dgvStudents.Width, dgvStudents.Height);
            dgvStudents.DrawToBitmap(image, new Rectangle(0, 0, dgvStudents.Width, dgvStudents.Height));
            dgvStudents.Height = height;
            dgvStudents.ScrollBars = ScrollBars.Vertical;
            printPreviewDialog.ShowDialog();
        }

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.Bicubic;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            if (psPrint)
            {
                if (psIndex < images.Count)
                {
                    Bitmap bitmap = new Bitmap(images[0].Width * 3, images[0].Height);

                    for (int i = 0; i < 3; i++)
                    {
                        if (psIndex == images.Count) break;
                        Graphics g = Graphics.FromImage(bitmap);
                        g.DrawImage(images[psIndex], i * images[0].Width, 0);
                        psIndex++;
                    }
                    e.Graphics.DrawImage(bitmap, 30, 50);
                    if (psIndex == images.Count)
                    {
                        e.HasMorePages = false;
                        psIndex = 0;
                    }
                    else
                    {
                        e.HasMorePages = true;
                    }
                }
            }
            else
            {
                e.Graphics.DrawImage(image, 30, 50);
            }
        }


        List<Bitmap> images = new List<Bitmap>();
        bool psPrint = false;
        int psIndex = 0;
        
        private void btnPrintIndividual_Click(object sender, EventArgs e)
        {
            images.Clear();
            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                if (Convert.ToInt32(row.Cells[0].Value) > 0)
                {
                    Print.StatementOfAccount statement = new Print.StatementOfAccount();
                    int id = Convert.ToInt32(row.Cells[0].Value);
                    Classes.Student student = students.Find(s => s.ID == id);
                    Classes.Batch batch = Classes.Batch.GetByID(student.BatchID);
                    statement.lblDate.Text = DateTime.Today.ToShortDateString();
                    statement.lblStudentNumber.Text = student.Number;
                    statement.lblStudentName.Text = student.GetFullName();
                    statement.lblStudentProgram.Text = batch.Number.ToString();
                    statement.lblTuitionFee.Text = student.Tuition.ToString("N");
                    List<Classes.Transaction> trans = Classes.Transaction.getTransactionsByStudent(student.ID);
                    foreach (Classes.Transaction tr in trans)
                    {
                        statement.lblPayments.Text += tr.DateTime.ToShortDateString() + " " + tr.Amount.ToString("N") + Environment.NewLine;
                    }
                    statement.lblBalance.Text = Classes.Transaction.GetBalance(student.ID).ToString("N");
                    statement.Show();
                    image = new Bitmap(statement.Width, statement.Height);
                    statement.DrawToBitmap(image, new Rectangle(0, 0, statement.Width, statement.Height));
                    statement.Close();
                    images.Add(image);
                }
            }
            psPrint = true;
            printDocument.DefaultPageSettings.Landscape = true;
            printPreviewDialog.ShowDialog();
        }

        private void TransactionHistory_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn col in dgvStudents.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            LoadBatches();
        }
    }
}
