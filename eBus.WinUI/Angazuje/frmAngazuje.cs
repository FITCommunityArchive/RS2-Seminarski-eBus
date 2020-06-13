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

namespace eBus.WinUI.Angazuje
{
    public partial class frmAngazuje : Form
    {

        private APIService _angazuje = new APIService("Angazuje");
        private APIService _linije = new APIService("Linija");
        private APIService _vozila = new APIService("Vozilo");
        private APIService _karte = new APIService("Karta");
        public frmAngazuje()
        {
            InitializeComponent();
            dgvAngazuje.AutoGenerateColumns = false;
        }

        private async  void frmAngazuje_Load(object sender, EventArgs e)
        {
            try
            {
                await LoadLinije();
                await LoadVozila();
                //LoadAngazuje(null);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message,"Angazuje", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }

      



        private async Task LoadVozila()
        {

            try
            {
                var lista = await _vozila.Get<List<Model.Vozilo>>(null);

                lista.Insert(0, new Model.Vozilo());

                cmbVozila.DataSource = lista;
                cmbVozila.ValueMember = "Id";
                cmbVozila.DisplayMember = "Model";
                cmbVozila.Text = "--odaberi vozilo--";
               
            }
            catch (Exception)
            {

                throw;
            }

        }



        private async Task LoadLinije()
        {

            try
            {
                var lista = await _linije.Get<List<Model.Linija>>(null);

                lista.Insert(0, new Model.Linija());
                cmbLinije.DataSource = lista;
                cmbLinije.ValueMember = "Id";
                cmbLinije.DisplayMember = "Naziv";
                cmbLinije.Text = "--odaberi liniju--";
                
               
            }
            catch (Exception)
            {

                throw;
            }

        }



        private async void LoadAngazuje(AngazujeSearchRequest search = null)
        {

            try
            {
                var lista = await _angazuje.Get<List<Model.Angazuje>>(search);


                dgvAngazuje.DataSource = lista;
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        private void cmbLinije_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(int.TryParse(cmbLinije.SelectedValue.ToString(), out int id))
                {
                    var search = new AngazujeSearchRequest()
                    {
                        ZaLiniju = false,
                        LinijaId = id
                    };

                    LoadAngazuje(search);
                }
               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Angazuje", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void cmbVozila_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (int.TryParse(cmbVozila.SelectedValue.ToString(), out int id))
                {
                    var search = new AngazujeSearchRequest()
                    {
                        ZaLiniju = false,
                        VoziloId = id
                    };

                    LoadAngazuje(search);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Angazuje", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvAngazuje_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var el = dgvAngazuje.SelectedRows[0].Cells[0].Value.ToString();

            if(int.TryParse(el, out int id))
            {
                frmAngazujeDetalji frm = new frmAngazujeDetalji(id);
                frm.Show();
            }
        }
    }
}
