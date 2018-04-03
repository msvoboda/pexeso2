using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Pexeso2.ModelView
{
    /// <summary>
    /// Zakladni trida pro pattern ViewModel
    /// Obsahuje spolecne DiplayName
    /// Vyvolava zmeny na Property
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {    
        #region Debugging
        /// <summary>
        /// Test na existenci property, pokud nexistuje zavyjimkuj
        /// V Release rezimu bychom mohli byt rychlejsi
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify - property name !!! 
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(msg);
                else
                    Debug.Fail(msg);
            }
        }

        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }
        #endregion 

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Signalizujem zmenu na property ... wpfko reaguj
        /// </summary>
        /// <param name="propertyName">notify property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (_disposed == true)
            {
                Debug.WriteLine("Object disposed:" + this.GetType().ToString() + " - " + propertyName);
                return;
            }

            // nebudeme verifikovat property ... uz neni vyvojova faze projektu
            //this.VerifyPropertyName(propertyName);                        
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion // INotifyPropertyChanged Members
        #region IDisposable Members

        public void Dispose()
        {
            this.OnDispose();
            GC.SuppressFinalize(this);
        }

        private bool _disposed = false;
        protected virtual void OnDispose()
        {
            _disposed = true;
        }

        #if DEBUG
        /// <summary>
        /// destructor - garbage collected.
        /// </summary>
        ~ViewModelBase()
        {
            string msg = string.Format("{0} ({1}) call destructor ~ViewModelBase", this.GetType().Name, this.GetHashCode());
            Debug.WriteLine(msg);
        }
        #endif

        #endregion // IDisposable Members
    }
}