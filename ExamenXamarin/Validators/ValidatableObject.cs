using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace ExamenXamarin.Validators
{
  public class ValidatableObject<T> : IValidatable<T>
  {
    
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

    public List<IValidationRule<T>> Validations { get; } = new List<IValidationRule<T>>();


    private List<string> errors = new List<string>();
    public List<string> Errors
    {
      get { return errors; }
      set { SetProperty(ref errors, value); }
    }

    public bool CleanOnChange { get; set; } = true;

    T _value;
    public T Value
    {
      get => _value;
      set
      {
        //_value = value;
        SetProperty(ref _value, value);

        if (CleanOnChange)
          IsValid = true;
      }
    }

    private bool isValid = true;
    public bool IsValid
    {
      get { return isValid; }
      set { SetProperty(ref isValid, value); }
    }

    public virtual bool Validate()
    {
      Errors.Clear();

      IEnumerable<string> errors = Validations.Where(v => !v.Check(Value))
          .Select(v => v.ValidationMessage);

      Errors = errors.ToList();
      IsValid = !Errors.Any();

      return this.IsValid;
    }
    public override string ToString()
    {
      return $"{Value}";
    }
  }
}
