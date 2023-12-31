﻿using System;
using System.ComponentModel;

namespace Common.WPF.WPFUtilities
{
    /// <summary>
    /// This class is used as the basis for all ViewModel objects
    /// </summary>
    public class ViewModel : INotifyPropertyChanged, IDisposable, IDataErrorInfo
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        ///// <summary>
        ///// This can be used to indicate that all property values have changed.
        ///// </summary>
        //protected void OnPropertyChanged()
        //{
        //    OnPropertyChanged(null);
        //}

        /// <summary>
        /// This raises the INotifyPropertyChanged.PropertyChanged event to indicate
        /// a specific property has changed value.
        /// </summary>
        /// <param name="name"></param>
        public virtual void OnPropertyChanged(string name = "")
        {
            //Debug.Assert(string.IsNullOrEmpty(name) || GetType().GetProperty(name) != null);
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public event Action<EventArgs> CloseView; // view model don't know call the view directly

        public event Action<DialogResult> RequestClose; // to follow ok, cancel apply button reponse

        protected void OnCloseView()
        {
            if (CloseView != null)
                CloseView(EventArgs.Empty);
        }

        protected void OnCloseView(DialogResult result)
        {
            if (RequestClose != null)
                RequestClose(result);
        }

        #region IDisposable Members

        /// <summary>
        /// This disposes of the view model.  It unregisters from the message mediator.
        /// </summary>
        /// <param name="isDisposing">True if IDisposable.Dispose was called</param>
        protected virtual void Dispose(bool isDisposing)
        {
        }

        /// <summary>
        /// Implementation of IDisposable.Dispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IDataErrorInfo Members
        public string this[string propertyName]
        {
            get { return GetValidationError(propertyName); }

        }

        public string Error { get; }

        public virtual string GetValidationError(string propertyName)
        {
            return null;
        }

        #endregion
    }
}
