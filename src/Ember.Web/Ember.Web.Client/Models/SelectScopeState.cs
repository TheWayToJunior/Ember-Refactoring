using Ember.Web.Client.ViewModels;
using System;

namespace Ember.Web.Client.Models
{
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
        private static Action _callBack;

        public string IconHtmlFragment => "<span class='material-icons'>done_all</span>";

        private readonly static Lazy<ISelectScopeState> _lazyInstance = new(
            () => new AutoSelectScopeState());

        public static ISelectScopeState GetInstance(Action callBack)
        {
            _callBack = callBack;
            return _lazyInstance.Value;
        }

        public void Click(PaymentHistoryModel model)
        {
            model.SetCountAllFiltered();
            model.DateFilterChanged += _callBack;
            model.SelectState = DefaultSelectScopeState.GetInstance(_callBack);
        }
    }

    /// <summary>
    /// The state in which records are displayed by default
    /// </summary>
    internal class DefaultSelectScopeState : ISelectScopeState
    {
        private static Action _callBack;

        public string IconHtmlFragment => "<span class='material-icons'>done</span>";

        private readonly static Lazy<ISelectScopeState> _lazyInstance = new(
            () => new DefaultSelectScopeState());

        public static ISelectScopeState GetInstance(Action callBack)
        {
            _callBack = callBack;
            return _lazyInstance.Value;
        }

        public void Click(PaymentHistoryModel model)
        {
            model.PageSize = PaymentHistoryModel.DefaultPageSize;
            model.DateFilterChanged -= _callBack;
            model.SelectState = AutoSelectScopeState.GetInstance(_callBack);
        }
    }
}
