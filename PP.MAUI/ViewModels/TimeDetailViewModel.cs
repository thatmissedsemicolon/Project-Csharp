using PP.Library.DTO;
using PP.Library.Models;
using PP.Library.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PP.MAUI.ViewModels
{
    public class TimeDetailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private TimeService timeService;
        private TimeDTO timeDTOModel;
        private Time timeModel;
        private int timeId;

        public Time TimeModel
        {
            get => timeModel;
            set
            {
                timeModel = value;
                OnPropertyChanged();
            }
        }

        public TimeDTO TimeDTOModel
        {
            get => timeDTOModel;
            set
            {
                if (timeDTOModel != value)
                {
                    timeDTOModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public int TimeId
        {
            get => timeId;
            set
            {
                timeId = value;
                OnPropertyChanged(nameof(TimeId));
            }
        }

        public TimeDetailViewModel()
        {
            timeService = TimeService.Instance;
            timeDTOModel = new TimeDTO();
            timeModel = new Time(timeDTOModel);
        }

        public async void GetTime()
        {
            var timeDetails = await timeService.GetTimeById(TimeId);

            if (timeDetails != null)
            {
                TimeDTOModel = timeDetails;
                TimeModel = new Time(TimeDTOModel);
            }
            else
            {
                TimeDTOModel = null;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
