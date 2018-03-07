namespace ForeingEchange2.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using System.Net.Http;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using Xamarin.Forms;
    using ForeingEchange2.Helpers;

    public class MainViewModel : INotifyPropertyChanged
    {

        //Me quede en el video numero 13 por si quieres continuar con lo de la base de datos local
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        ApiService apiService;
        #endregion

        #region Attributes
        bool _IsRunning;
        bool _IsEnable;
        string _Result;
        Rate _SourceRate;
        Rate _TargetRate;
        String _Status;
        ObservableCollection<Rate> _Rates;
        #endregion


        #region Properties

        public string Status
        {
            get
            {
                return _Status;
            }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    // te no tificara cuando el valor de IsRunning cambie 
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
                }
            }
        }
        public string Amount
        {
            get;
            set;

        }
        public ObservableCollection<Rate> Rates
        {
            get
            {
                return _Rates;
            }
            set
            {
                if (_Rates != value)
                {
                    _Rates = value;
                    // te no tificara cuando el valor de IsRunning cambie 
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rates)));
                }
            }
        }
        public Rate SourceRate
        {
            get
            {
                return _SourceRate;
            }
            set
            {
                if (_SourceRate != value)
                {
                    _SourceRate = value;
                    // te no tificara cuando el valor de IsRunning cambie 
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SourceRate)));
                }
            }
        }
        public Rate TargetRate
        {
            get
            {
                return _TargetRate;
            }
            set
            {
                if (_TargetRate != value)
                {
                    _TargetRate = value;
                    // te no tificara cuando el valor de IsRunning cambie 
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TargetRate)));
                }
            }
        }
        public bool IsRunning
        {
            get{
                return _IsRunning;
            }
            set{
                if (_IsRunning != value) 
                {
                    _IsRunning = value;
                    // te no tificara cuando el valor de IsRunning cambie 
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }

        }
        public bool IsEnable
        {
            get
            {
                return _IsEnable;
            }
            set
            {
                if (_IsEnable != value)
                {
                    _IsEnable = value;
                    // te no tificara cuando el valor de IsRunning cambie 
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnable)));
                }
            }
        }

        public String Result
        {
            get
            {
                return _Result;
            }
            set
            {
                if (_Result != value)
                {
                    _Result = value;
                    // te no tificara cuando el valor de Result cambie 
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Result)));
                }
            }
        }

        #endregion

        #region Commands

        public ICommand SwitchCommand
        {
            get { return new RelayCommand(Switch); }
        }

        void Switch(){
            
            var aux = SourceRate;
            SourceRate = TargetRate;
            TargetRate = aux;
            Convert();
        }


        public ICommand ConvertCommand
        {
            get { return new RelayCommand(Convert); }
        }


        async void Convert()
        {
            if (String.IsNullOrEmpty((Amount))){

                await Application.Current.MainPage.DisplayAlert(
                    Lenguages.Error, Lenguages.AmountValidation,
                    Lenguages.Accept);
                return;
            }

            decimal amount = 0;

            if(!decimal.TryParse(Amount, out amount))
            {
                await Application.Current.MainPage.DisplayAlert(
              Lenguages.Error, "You must enter a numeric value in amount",
              Lenguages.Accept);
                return;
            }

            if (SourceRate == null)
            {

                await Application.Current.MainPage.DisplayAlert(
                    Lenguages.Error, "You must select a source rate",
                    Lenguages.Accept);
                return;
            }

            if (TargetRate == null)
            {

                await Application.Current.MainPage.DisplayAlert(
                    Lenguages.Error, "You must select a target rate",
                    Lenguages.Accept);
                return;

            }

            var amountconverted = (amount / (decimal)SourceRate.TaxRate)
                                            * (decimal)TargetRate.TaxRate;

            Result = string.Format("{0} {1:C2} = {2} {3:C2}", 
                                   SourceRate.Code, 
                                   amount, 
                                   TargetRate.Code, 
                                   amountconverted);

        }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            apiService = new ApiService();
            LoadRates();
        }
        #region Methods
        async void LoadRates()
        {
            IsRunning = true;
            Result = "Loading rates...";


            //consumir API GET

            var conection = await apiService.CheckConnection();

            if(!conection.IsSuccess){
                IsRunning = false;
                Result = conection.Message;
                return;
            }

            var response = 
                await apiService.GetList<Rate>
                   ("http://apiexchangerates.azurewebsites.net", "/api/rates");

            if(!response.IsSuccess){
                IsRunning = false;
                Result = response.Message;
                return;
            }

            Rates = new ObservableCollection<Rate>((List<Rate>)response.Result);
            IsRunning = false;
            IsEnable = true;
            Result = "Ready to convert";
            Status = "Rates loades from the API";
        }
        #endregion

        #endregion
    }
}
