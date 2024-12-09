using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BachelorProjectII.DomainLayer.Models
{
    public class ApotekModel : INotifyPropertyChanged
    {
        public int Id { get; set; }

        public string ApotekNavn { get; set; }

        public string Adresse { get; set; }

        public int TelefonNummer { get; set; }

        public List<IndlaegsseddelModel> IndlaegssedlerPaaLager { get; set; }

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
