﻿using eBus.Model;
using eBus.Model.Requests;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace eBus.Mobile.ViewModels
{
    public class NotifikacijeViewModel : BaseViewModel
    {
        private readonly APIService _notifikacijeService = new APIService("Notifikacije");
        private readonly APIService _putnikNotifikacijeService = new APIService("PutnikNotifikacije");
        private readonly APIService _putnikService = new APIService("Putnik");

        public NotifikacijeViewModel()
        {
            Title = "Notifikacija";
        }
        public ObservableCollection<Notifikacije> NotifikacijeList { get; set; } = new ObservableCollection<Notifikacije>();

        public async Task Init()
        {
            var lista = await _notifikacijeService.Get<List<Notifikacije>>(null);

            var searchPutnik = new PutnikSearchRequest()
            {
                KorisnickoIme = APIService.KorisnickoIme
            };

            var listaPutnika = await _putnikService.Get<List<Putnik>>(searchPutnik);

            Putnik putnik = null;
            foreach (var item in listaPutnika)
            {
                if (item.KorisnickoIme == APIService.KorisnickoIme)
                {
                    putnik = item;
                    break;
                }
            }

            var searchPN = new PutnikNotifikacijeSearchRequest()
            {
                PutnikId = putnik.Id
            };

            var listaPN = await _putnikNotifikacijeService.Get<List<PutnikNotifikacije>>(searchPN);

            NotifikacijeList.Clear();

            foreach (var item in lista)
            {

                foreach (var item2 in listaPN)
                {
                    if(item.Id == item2.NotifikacijaId && !item2.Pregledana)
                    {
                        NotifikacijeList.Add(item);
                    }
                }
                       
                    
               
               
            }
        }

        public async Task OznaciProcitano(Notifikacije n)
        {
            try
            {
                var searchPutnik = new PutnikSearchRequest()
                {
                    KorisnickoIme = APIService.KorisnickoIme
                };

                var listaPutnika = await _putnikService.Get<List<Putnik>>(searchPutnik);

                Putnik putnik = null;
                foreach (var item in listaPutnika)
                {
                    if (item.KorisnickoIme == APIService.KorisnickoIme)
                    {
                        putnik = item;
                        break;
                    }
                }

                var searchPN = new PutnikNotifikacijeSearchRequest()
                {
                    PutnikId = putnik.Id
                };

                var listaPN = await _putnikNotifikacijeService.Get<List<PutnikNotifikacije>>(searchPN);

                PutnikNotifikacije pn = null;
                foreach (var item in listaPN)
                {
                    if(item.NotifikacijaId == n.Id && item.PutnikId  == putnik.Id)
                    {
                        pn = item;
                        break;
                    }
                }

                if(pn != null)
                {
                    var request = new PutnikNotifikacijeUpsertRequest()
                    {
                        NotifikacijaId = n.Id,
                        PutnikId = putnik.Id,
                        Pregledana = true
                    };

                    await _putnikNotifikacijeService.Update<PutnikNotifikacije>(pn.Id, request);

                    if (n != null)
                    {
                        NotifikacijeList.Remove(n);

                        await Application.Current.MainPage.DisplayAlert(" ", "Označili ste notifikaciju pročitanom", "OK");
                    }
                }

               

               



                //var updateN = new NotifikacijaUpsertRequest()
                //{
                //    DatumSlanja = n.DatumSlanja,
                //    Naslov = n.Naslov,
                //    NovostId = n.NovostId,
                //    Procitano = true
                //};

                //await _notifikacijeService.Update<Notifikacija>(n.Id, updateN);

            
            }
            catch (Exception)
            {

                throw;
            }
           
        }
    }
}
