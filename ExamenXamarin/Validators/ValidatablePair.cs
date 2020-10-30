using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace ExamenXamarin.Validators
{
  public class ValidatablePair<T> : IValidatable<ValidatablePair<T>>
  {
    public List<IValidationRule<ValidatablePair<T>>> Validations { get; } = new List<IValidationRule<ValidatablePair<T>>>();

    private bool isValid = true;
    public bool IsValid {
      get { return isValid; }
      set { SetProperty(ref isValid, value); }
    }

    
    private List<string> errors = new List<string>();
    public List<string> Errors
    {
      get { return errors; }
      set { SetProperty(ref errors, value); } 
    }


    public ValidatableObject<T> Item1 { get; set; } = new ValidatableObject<T>();

    public ValidatableObject<T> Item2 { get; set; } = new ValidatableObject<T>();


    protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
    {
      if (EqualityComparer<T>.Default.Equals(backingStore, value))
        return false;

      backingStore = value;
      onChanged?.Invoke();
      OnPropertyChanged(propertyName);
      return true;
    }
    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
      var changed = PropertyChanged;
      if (changed == null)
        return;

      changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion

    public bool Validate()
    {
      var item1IsValid = Item1.Validate();
      var item2IsValid = Item2.Validate();
      if (item1IsValid && item2IsValid)
      {
        Errors.Clear();

        IEnumerable<string> errors = Validations.Where(v => !v.Check(this))
            .Select(v => v.ValidationMessage);

        Errors = errors.ToList();
        Item2.Errors = Errors;
        Item2.IsValid = !Errors.Any();
      }

      IsValid = !Item1.Errors.Any() && !Item2.Errors.Any();
      Errors = Item1.Errors.ToList();

      return this.IsValid;
    }
  }
}
