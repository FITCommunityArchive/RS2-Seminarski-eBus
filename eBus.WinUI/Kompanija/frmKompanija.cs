﻿using eBus.Model.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eBus.WinUI.Kompanija
{
    public partial class frmKompanija : Form
    {
        private APIService _kompanija = new APIService("Kompanija");
        private APIService _grad = new APIService("Grad");

        public frmKompanija()
        {
            InitializeComponent();
            dgvKompanije.AutoGenerateColumns = false;
        }

        private  void txtNaziv_TextChanged(object sender, EventArgs e)
        {
            var search = new KompanijaSearchRequest()
            {
                Naziv = txtNaziv.Text
            };

            vrati(search);
        }

        private  void cmbGradovi_SelectedIndexChanged(object sender, EventArgs e)
        {
            var gID = cmbGradovi.SelectedValue.ToString();

            if(int.TryParse(gID, out int id))
            {
                var search = new KompanijaSearchRequest()
                {
                    GradId = id
                };

                vrati(search);

            }
        }

        private async void frmKompanija_Load(object sender, EventArgs e)
        {
            try
            {
                var lista = await _grad.Get<List<Model.Grad>>(null);
                lista.Insert(0, new Model.Grad());
                cmbGradovi.DataSource = null;
                cmbGradovi.DataSource = lista;
                cmbGradovi.ValueMember = "Id";
                cmbGradovi.DisplayMember = "Naziv";
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
          
        }

        

        private async void vrati(KompanijaSearchRequest search)
        {
            try
            {
                var lista = await _kompanija.Get<List<Model.Kompanija>>(search);

                foreach (var item in lista)
                {
                    var grad = await _grad.GetById<Model.Grad>(item.GradId.Value);

                    item.NazivGrada = grad.Naziv;
                }

                dgvKompanije.DataSource = lista;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Kompanija",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }

        private void dgvKompanije_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var el = dgvKompanije.SelectedRows[0].Cells[0].Value.ToString();

            if(int.TryParse(el, out int id))
            {
                frmKompanijaDetalji frm = new frmKompanijaDetalji(id);
                frm.Show();
                Close();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
