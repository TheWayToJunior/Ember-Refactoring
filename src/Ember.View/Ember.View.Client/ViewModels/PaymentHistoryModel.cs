using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ember.View.Client.ViewModels
{
    public class History
    {
        public DateTime Date { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public decimal Amount { get; set; }

        public bool IsSuccess { get; set; }
    }

    public class PaymentHistoryModel : BaseModel
    {
        public const int DefaultPageSize = 5;

        public PaymentHistoryModel()
        {
            Histories = new List<History>();
            PageSize = DefaultPageSize;

            FromDate = DateTime.Now.AddMonths(-1).ToString("dd-MM-yyyy");
            ToDate = DateTime.Now.AddDays(1).ToString("dd-MM-yyyy");

            SelectState = AutoSelectScopeState.GetInstance();
        }

        [Inject]
        public IAccountManager AccountManager { get; set; }

        private IEnumerable<History> _histories;

        public IEnumerable<History> Histories
        {
            get => _histories.Where(h => h.Date > DateTime.Parse(FromDate) && 
                h.Date < DateTime.Parse(ToDate));

            private set => _histories = value;
        }

        public bool IsLoaded => !_histories.Any();

        private int _pageSize;

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value >= 100 || value < 0)
                {
                    return;
                }

                _pageSize = value;
            }
        }

        public event Action DateFilterChanged;

        private string _fromDate;

        public string FromDate
        {
            get => _fromDate;
            set
            {
                _fromDate = value;
                DateFilterChanged?.Invoke();
            }
        }

        private string _toDate;

        public string ToDate 
        {
            get => _toDate;
            set 
            {
                _toDate = value;
                DateFilterChanged?.Invoke();
            }
        }

        public ISelectScopeState SelectState { get; set; }

        protected async override Task OnInitializedAsync()
        {
            Histories = (await AccountManager.GetPaymentHistory())
                .Select(p => new History
                {
                    Date = p.Date,
                    Amount = p.Amount,
                    Description = "Payment account",
                    Type = "Sent",
                    IsSuccess = true
                });
        }

        public void Select()
        {
            SelectState.Click(this);
        }

        public void SetCountAllFiltered()
        {
            PageSize = Histories.Count();
        }
    }

    public interface ISelectScopeState
    {
        string IconHtmlFragment { get; }

        void Click(PaymentHistoryModel model);
    }

    /// <summary>
    /// The state in which all selected records are displayed, and their number automatically changes depending on the filter
    /// </summary>
    internal class AutoSelectScopeState : ISelectScopeState
    {
        public string IconHtmlFragment => "<span class='material-icons'>done_all</span>";

        public void Click(PaymentHistoryModel model)
        {
            model.SetCountAllFiltered();
            model.DateFilterChanged += model.SetCountAllFiltered;
            model.SelectState = DefaultSelectScopeState.GetInstance();
        }

        private readonly static Lazy<ISelectScopeState> _lazyInstance = new(
            () => new AutoSelectScopeState());

        public static ISelectScopeState GetInstance() => _lazyInstance.Value;
    }

    /// <summary>
    /// The state in which records are displayed by default
    /// </summary>
    internal class DefaultSelectScopeState : ISelectScopeState
    {
        public string IconHtmlFragment => "<span class='material-icons'>done</span>";

        public void Click(PaymentHistoryModel model)
        {
            model.PageSize = PaymentHistoryModel.DefaultPageSize;
            model.DateFilterChanged -= model.SetCountAllFiltered;
            model.SelectState = AutoSelectScopeState.GetInstance();
        }

        private readonly static Lazy<ISelectScopeState> _lazyInstance = new(
            () => new DefaultSelectScopeState());

        public static ISelectScopeState GetInstance() => _lazyInstance.Value;
    }
}
