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

namespace eBus.WinUI.Linija
{
    public partial class frmCijenaDetalji : Form
    {
        private APIService _cijena = new APIService("Cijena");
        private APIService _kompanijeService = new APIService("Kompanija");
        private APIService _linijeService = new APIService("Linija");
        private int? _idLinije;
        public frmCijenaDetalji(int? id = null)
        {
            InitializeComponent();
            _idLinije = id;
        }

        private async void btnSačuvaj_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateChildren())
                {

                
                    var cijena = new CijenaUpsertRequest();

                    cijena.LinijaId = int.Parse(cmbLinija.SelectedValue.ToString());
                    cijena.Iznos = decimal.Parse(txtCijena.Text);
                    cijena.Popust = float.Parse(txtPopust.Text);
                    cijena.KompanijaId = int.Parse( cmbKompanija.SelectedValue.ToString());

                    await _cijena.Insert<Model.Cijena>(cijena);

                    MessageBox.Show("Cijena je uspješno sačuvana");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Cijena detalji", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadKompanije()
        {
            var lista = await _kompanijeService.Get<List<Model.Kompanija>>(null);

            lista.Insert(0, new Model.Kompanija());

            cmbKompanija.DataSource = lista;
            cmbKompanija.ValueMember = "Id";
            cmbKompanija.DisplayMember = "Naziv";
            cmbKompanija.SelectedText = "--Odaberi kompaniju--";
        }
        private async Task LoadLinija()
        {
            var lista = await _linijeService.Get<List<Model.Linija>>(null);

            lista.Insert(0, new Model.Linija());

            cmbLinija.DataSource = lista;
            cmbLinija.ValueMember = "Id";
            cmbLinija.DisplayMember = "Naziv";
            cmbLinija.SelectedText = "--Odaberi liniju--";
        }

        private async void frmCijenaDetalji_Load(object sender, EventArgs e)
        {
            await LoadKompanije();
            await LoadLinija();

            if (_idLinije.HasValue)
            {
                var linija = await _linijeService.GetById<Model.Linija>(_idLinije.Value);

                cmbLinija.SelectedValue = linija.Id;
            }
        }

        private void txtCijena_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCijena.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCijena, Properties.Resources.Upozorenje);
            }else if(decimal.TryParse(txtCijena.Text, out decimal cijena) ? cijena <0 : true)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCijena, Properties.Resources.NeispravanFormat);
            }
            else
            {
                errorProvider1.SetError(txtCijena, null);
            }
        }

        private void txtPopust_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPopust.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPopust, Properties.Resources.Upozorenje);
            }
            else if (decimal.TryParse(txtPopust.Text, out decimal popust) ? popust < 0 || popust > 100 : true)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPopust, "Unos mora biti u obliku decimalnog broja (npr. 0,11 -> 11%)");
            }
            else
            {
                errorProvider1.SetError(txtPopust, null);
            }
        }

        private void cmbKompanija_Validating(object sender, CancelEventArgs e)
        {
            if(cmbKompanija.SelectedIndex == -1)
            {
                e.Cancel = true;
                errorProvider1.SetError(cmbKompanija, Properties.Resources.Upozorenje);
            }
            else
            {
                errorProvider1.SetError(cmbKompanija, null);
            }
        }

        private void cmbLinija_Validating(object sender, CancelEventArgs e)
        {
            if (cmbLinija.SelectedIndex == -1)
            {
                e.Cancel = true;
                errorProvider1.SetError(cmbLinija, Properties.Resources.Upozorenje);
            }
            else
            {
                errorProvider1.SetError(cmbLinija, null);
            }
        }
    }
}
