using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace POCKETPAL.Models
{
    public class SSITCountModel
    {
        public int CountScrap { get; set; }
        public int CountPR { get; set; }
    }

    public class RootSSITCountModel
    {
        public SSITCountModel Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

    }

    public class SSITModel : INotifyPropertyChanged
    {
        public int No { get; set; }
        public string Items { get; set; }
        public string SSIT_ID { get; set; }
        public string Material { get; set; }
        public string MaterialDesc { get; set; }
        public string DeptSubmitted { get; set; }
        public string Qty { get; set; }
        public string BaseUOM { get; set; }
        public string UnitPrice { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string SubmitBy { get; set; }
        public string SubmitDate { get; set; }
        public string NCCode { get; set; }
        public string NCCodeDet { get; set; }
        public string NCDetail { get; set; }
        public string RootCause { get; set; }
        public string CanApproved { get; set; }

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set { _isChecked = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RootSSITModel
    {
        public SSITModel Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

    }

    public class SSITPlantModel : INotifyPropertyChanged
    {
        public string PlantCode { get; set; }
        public string PlantDesc { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //protected bool SetProperty<T>(ref T backfield, T value, [CallerMemberName] string propertyName = null)
        //{
        //    if (EqualityComparer<T>.Default.Equals(backfield, value))
        //    {
        //        return false;
        //    }
        //    backfield = value;
        //    OnPropertyChanged(propertyName);
        //    return true;
        //}
    }
    public class SSITVendorModel
    {
        public string VendorCode { get; set; }
        public string VendorDesc { get; set; }
    }
    public class SSITProductModel
    {
        public string ProductCode { get; set; }
        public string ProductDesc { get; set; }
    }
    public class SSITActionModel
    {
        public string ActionCode { get; set; }
        public string ActionDesc { get; set; }
    }

}
