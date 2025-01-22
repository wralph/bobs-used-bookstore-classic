using Bookstore.Domain.ReferenceData;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;


namespace Bookstore.Web.ViewModel.Resale
{
    public class ResaleCreateViewModel
    {
        public ResaleCreateViewModel() { }

        public ResaleCreateViewModel(IEnumerable<ReferenceDataItem> referenceDataItems)
        {
            BookTypes = referenceDataItems.Where(x => x.DataType == ReferenceDataType.BookType).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });
            Publishers = referenceDataItems.Where(x => x.DataType == ReferenceDataType.Publisher).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });
            Genres = referenceDataItems.Where(x => x.DataType == ReferenceDataType.Genre).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });
            Conditions = referenceDataItems.Where(x => x.DataType == ReferenceDataType.Condition).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });
        }

        public IEnumerable<SelectListItem> BookTypes { get; internal set; }

        public IEnumerable<SelectListItem> Publishers { get; internal set; }

        public IEnumerable<SelectListItem> Genres { get; internal set; }

        public IEnumerable<SelectListItem> Conditions { get; internal set; }

        public int SelectedBookTypeId { get; set; }

        public int SelectedPublisherId { get; set; }

        public int SelectedGenreId { get; set; }

        public int SelectedConditionId { get; set; }

        public decimal BookPrice { get; set; }

        public string BookName { get; set; }

        public string Author { get; set; }

        public string ISBN { get; set; }
    }
}
