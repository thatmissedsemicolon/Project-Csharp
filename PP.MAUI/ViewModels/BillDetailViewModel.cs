using PP.Library.DTO;
using PP.Library.Models;
using PP.Library.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PP.MAUI.ViewModels
{
    public class BillDetailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private BillService billService;
        private BillDTO billDTOModel;
        private Bill billModel;
        private int billId;

        public Bill BillModel
        {
            get => billModel;
            set
            {
                billModel = value;
                OnPropertyChanged();
            }
        }

        public BillDTO BillDTOModel
        {
            get => billDTOModel;
            set
            {
                if (billDTOModel != value)
                {
                    billDTOModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public int BillId
        {
            get => billId;
            set
            {
                billId = value;
                OnPropertyChanged(nameof(BillId));
            }
        }

        public BillDetailViewModel()
        {
            billService = BillService.Instance;
            billDTOModel = new BillDTO();
            billModel = new Bill(billDTOModel);
        }

        public async void GetBill()
        {
            var billDetails = await billService.GetBillById(BillId);

            if (billDetails != null)
            {
                BillDTOModel = billDetails;
                BillModel = new Bill(BillDTOModel);
            }
            else
            {
                BillDTOModel = null;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
