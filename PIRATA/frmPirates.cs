using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Runtime.InteropServices;

namespace PIRATA
{
    public partial class frmPirates : Form
    {
        DataTable dt = new DataTable();
        string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\Kyle\\Downloads\\dpPirates.accdb";
        OleDbConnection conn;

        public frmPirates()
        {
            InitializeComponent();
        }

        private void refresh()
        {
            DataTable dt = new DataTable();
            string query = "Select ID as id, piratename as ALIAS, givenname as NAME, age as AGE, pirategroup as PIRATEGROUP, bounty as BOUNTY from pirates";
            conn = new OleDbConnection(connStr);
            conn.Open();
            OleDbDataAdapter adpater = new OleDbDataAdapter(query, conn);
            adpater.Fill(dt);
            conn.Close();

            grdView.DataSource = dt;
            
            grdView.Columns["age"].Visible = false;
            grdView.Columns["ID"].Visible = false;
        }

        //private void distinct()
        //{
        //    string query = "select distinct [pirategroup] from pirates";
        //    DataTable dt = new DataTable();
        //    conn = new OleDbConnection(connStr);
        //    conn.Open();
        //    OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
        //    adapter.Fill(dt);
        //    conn.Close();


        //    grdView.DataSource = dt;

        //    cboGroup.DataSource = dt;
        //    cboGroup.DisplayMember = "pirategroup";
        //    cboPirate.DataSource = dt;
        //    cboPirate.DisplayMember = "pirategroup";
        //}


        private void Form1_Load(object sender, EventArgs e)
        {
            refresh();
            

            DataTable dt = new DataTable();
            string query = "Select ID as id, piratename as ALIAS, givenname as NAME, age as AGE, pirategroup as PIRATEGROUP, bounty as BOUNTY from pirates";
            conn = new OleDbConnection(connStr);
            conn.Open();
            OleDbDataAdapter adpater = new OleDbDataAdapter(query, conn);
            adpater.Fill(dt);
            conn.Close();

            grdView.DataSource = dt;
            grdView.Columns["age"].Visible = false;
            grdView.Columns["ID"].Visible = false;

            cboGroup.DataSource = dt;
            cboGroup.DisplayMember = "pirategroup";
            cboPirate.DataSource = dt;
            cboPirate.DisplayMember = "pirategroup";

            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKeyword.Text) || (string.IsNullOrEmpty(cboGroup.Text)))
            {
                MessageBox.Show("Please fill out necessary field", "ERROR");
            }
            else
            {
                DataTable dt = new DataTable();
                string query = "select piratename as ALIAS, givenname as NAME, age as AGE, pirategroup as PIRATEGROUP, bounty as BOUNTY from pirates where piratename = '" + txtKeyword.Text + "' or givenname = '" + txtKeyword.Text + "'";
                conn = new OleDbConnection(connStr);
                conn.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                adapter.Fill(dt);
                conn.Close();

                grdView.DataSource = dt;
            }
        }

        private void grdView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtAlias.Text = grdView.SelectedCells[1].Value.ToString();
            txtName.Text = grdView.SelectedCells[2].Value.ToString();
            txtAge.Text = grdView.SelectedCells[3].Value.ToString();
            cboPirate.Text = grdView.SelectedCells[4].Value.ToString();
            txtBounty.Text = grdView.SelectedCells[5].Value.ToString();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
            btnNewRecord.Enabled = false;
            txtAlias.Enabled = true;
            txtName.Enabled = true;
            txtAge.Enabled = true;
            cboPirate.Enabled = true;
            txtBounty.Enabled = true;
            DataTable dt = new DataTable();
            string query = "select piratename as ALIAS, givenname as NAME, pirategroup as PIRATEGROUP, bounty as BOUNTY from pirates where piratename = '" + txtAlias.Text+"'";
            conn = new OleDbConnection(connStr);
            conn.Open();
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
            adapter.Fill(dt);
            conn.Close();

            grdView.DataSource = dt;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnSave.Enabled=false;
            txtAlias.Enabled = false;
            txtName.Enabled = false;
            txtAge.Enabled = false;
            cboPirate.Enabled = false;
            txtBounty.Enabled = false;

            txtAlias.Text = " ";
            txtName.Text = " ";
            txtAge.Text = " ";
            cboPirate.Text= " ";
            txtBounty.Text = " ";
            btnNewRecord.Enabled= true;


            refresh();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            

            if (btnNewRecord.Enabled)
            {
                
                
                    btnNewRecord.Enabled = true;
                    string query = "Insert into pirates(piratename, givenname, age, bounty, pirategroup) values(@alias, @name, @age, @bounty, @pirategroup)";
                    conn = new OleDbConnection(connStr);
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    cmd.Parameters.AddWithValue("@alias", txtAlias.Text);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@age", txtAge.Text);
                    cmd.Parameters.AddWithValue("@bounty", txtBounty.Text);
                    cmd.Parameters.AddWithValue("@pirategroup", cboPirate.Text);
                    cmd.ExecuteNonQuery();
                    conn.Close();



                    MessageBox.Show("INPUT ADDED");

                    refresh();
                
            }
            else if(btnView.Enabled)
            {
                string query = "Update pirates set piratename= @alias, givenname = @name, age = @age, bounty = @bounty, pirategroup = @pirategroup where ID = @id";
                conn = new OleDbConnection(connStr);
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@alias", txtAlias.Text);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@age", txtAge.Text);
                cmd.Parameters.AddWithValue("@pirategroup", txtBounty.Text);
                cmd.Parameters.AddWithValue("@pirategroup", cboPirate.Text);
                cmd.Parameters.AddWithValue("@id", grdView.SelectedCells[0].Value.ToString());
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("INPUT UPDATED");

                refresh();
            }

            

            
            
        }

        private void btnNewRecord_Click(object sender, EventArgs e)
        {
            btnNewRecord.Enabled = true;
            txtAlias.Enabled = true;
            txtName.Enabled = true;
            txtAge.Enabled = true;
            cboPirate.Enabled = true;
            txtBounty.Enabled = true;
            btnSave.Enabled = true;
            //btnView.Enabled = false;

            txtAlias.Text = " ";
            txtName.Text = " ";
            txtAge.Text = " ";
            cboPirate.Text = " ";
            txtBounty.Text = " ";

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
             

            string query = "Delete from pirates where piratename = @alias";
            conn = new OleDbConnection(connStr);
            conn.Open();
            OleDbCommand cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@alias", txtAlias.Text);
            cmd.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("Successfully Deleted");
            refresh();

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void cboPirate_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string query = "Select ID as id, piratename as ALIAS, givenname as NAME, age as AGE, pirategroup as PIRATEGROUP, bounty as BOUNTY from pirates";
            conn = new OleDbConnection(connStr);
            conn.Open();
            OleDbDataAdapter adpater = new OleDbDataAdapter(query, conn);
            adpater.Fill(dt);
            conn.Close();

            grdView.DataSource = dt;
            
        }
    }
}
