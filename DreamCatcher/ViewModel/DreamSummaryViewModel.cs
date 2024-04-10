using DreamCatcher.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace DreamCatcher.ViewModel
{
    public class DreamSummaryViewModel : RequiredViewModel
    {
        #region View model map
        private static AutoMapper.MapperConfiguration config = new AutoMapper.MapperConfiguration(cfg => {
            cfg.CreateMap<Dream, DreamSummaryViewModel>()
               .AfterMap((src, dest) => { if (src.Picture == null) dest.Picture = null; }); 

            cfg.CreateMap<DreamSummaryViewModel, Dream>()
                .ForMember(x => x.Title, y => y.MapFrom(z => z.Title.Trim()))
                .ForMember(x => x.ShortDescription, y => y.MapFrom(z => !string.IsNullOrEmpty(z.ShortDescription) ? z.ShortDescription.Trim() : ""))
                .ForMember(x => x.Description, y => y.MapFrom(z => !string.IsNullOrEmpty(z.Description) ? z.Description.Trim() : ""));
        });
        #endregion

        #region Properties
        public byte[]? Picture { get; set; }

        public bool HasPicture
        {
            get { return Picture != null && Picture.Length > 0; }
        }

        private DateTime _dateTime;
        public DateTime DateTime
        {
            get => _dateTime;
            set
            {
                if(_dateTime != value)
                {
                    _dateTime = value;
                    OnPropertyChanged();
                }
            }
        }

        [Required]
        public string Title { get; set; } = null!;

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public DreamIntensity Intensity { get; set; } = DreamIntensity.Normal;

        public DreamType Type { get; set; } = DreamType.Normal;
        #endregion

        public DreamSummaryViewModel(DateTime dateTime) { 
            DateTime = dateTime;
        }

        #region Methods
        /// <summary>
        /// Convert view model to entity model
        /// </summary>
        /// <returns></returns>
        public Dream ToModel()
        {
            AutoMapper.IMapper mapper = config.CreateMapper();
            var dream = mapper.Map<DreamSummaryViewModel, Dream>(this);

            return dream;
        }

        /// <summary>
        /// Convert entity model to viewmodel
        /// </summary>
        /// <param name="item">Entity model</param>
        /// <returns></returns>
        public static DreamSummaryViewModel ToViewModel(Dream item)
        {
            AutoMapper.IMapper mapper = config.CreateMapper();
            var viewModel= mapper.Map<Dream, DreamSummaryViewModel>(item);

            return viewModel;
        }
        #endregion

    }
}
