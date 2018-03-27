using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Input;
/// M.Svoboda 2011

namespace Pexeso2.Command
{
    
    /// <summary>
    /// RelayCommand - šablona pro tridu
    /// bezne vice vyuzivam ICommand primo
    /// </summary>
    ///     
    public class RelayCommand : ICommand
    {
        #region Fields

        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        public string ActionName { get; set; }

        #endregion // Fields

        #region Constructors

        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action<object> execute,string action_name="")
            : this(execute, null,action_name)
        {
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute,string action_name="")
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            ActionName = action_name;
            _execute = execute;
            _canExecute = canExecute;           
        }

        #endregion // Constructors

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion // ICommand Members
    }

    /// Relay Command - generic
    /// M.Svoboda 2013   
    /// <summary>
    /// RelayCommand - šablona pro tridu
    /// Jedna se o generickou verzi
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        #region Fields

        readonly Action<T> _execute;
        readonly Predicate<T> _canExecute;

        public string ActionName { get; protected set; }

        #endregion 

        #region Constructors

        /// <summary>
        /// Command, ktery jde spustit vzdy
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Novy comand
        /// </summary>
        /// <param name="execute">spusteni comandu</param>
        /// <param name="canExecute">test jestli jde command spustit</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute null argument");

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Funkce CanExecute, Execute

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute((T) parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        #endregion
    }
    
}