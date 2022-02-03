using Ember.Web.Client.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ember.Web.Client.ViewModels
{
    public class PaymentHistoryModel : BaseModel, IDisposable
    {
        public const int DefaultPageSize = 5;

        public PaymentHistoryModel()
        {
            Histories = new List<History>();
            PageSize = DefaultPageSize;

            FromDate = DateTime.Now.AddMonths(-12).ToString("dd-MM-yyyy");
            ToDate = DateTime.Now.AddDays(1).ToString("dd-MM-yyyy");

            SelectState = AutoSelectScopeState.GetInstance(SetCountAllFiltered);
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

        public bool IsLoading => AccountManager.Account is null;

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
            var paymentHistory = await AccountManager.GetPaymentHistory();

            /// Temporary mapping of entities
            Histories = paymentHistory?.Select(p => new History
            {
                Date = p.Date,
                Amount = p.Amount,
                Description = "Payment account",
                Type = "Sent",
                IsSuccess = true
            }) ?? new List<History>();
        }

        public void Select()
        {
            SelectState.Click(this);
        }

        public void SetCountAllFiltered()
        {
            PageSize = Histories.Count();
        }

        public void Dispose()
        {
            var invocationList = DateFilterChanged?.GetInvocationList();

            if (invocationList is null) return;

            foreach (var @delegate in invocationList)
            {
                DateFilterChanged -= @delegate as Action;
            }
        }
    }
}
