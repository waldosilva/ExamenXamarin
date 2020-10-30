using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace ExamenXamarin.Behaviors
{
  public class PassValidator : Behavior<Entry>
  {
    private static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(PassValidator), false);

    public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

    public bool IsValid
    {
      get { return (bool)base.GetValue(IsValidProperty); }
      private set { base.SetValue(IsValidPropertyKey, value); }
    }
    protected override void OnAttachedTo(Entry entry)
    {
      entry.TextChanged += OnEntryTextChanged;
      base.OnAttachedTo(entry);
    }

    protected override void OnDetachingFrom(Entry entry)
    {
      entry.TextChanged -= OnEntryTextChanged;
      base.OnDetachingFrom(entry);
    }

    void OnEntryTextChanged(object sender, TextChangedEventArgs args)
    {
      Regex reg = new Regex("[0-9]");
      bool isValid = reg.IsMatch(args.NewTextValue);
      //bool isValid = reg.IsMatch(args.NewTextValue);
      IsValid = false;
      ((Entry)sender).TextColor = isValid ? Color.Red : Color.Default;
      ((Entry)sender).BackgroundColor = isValid ? Color.FromHex("#FBC5D0") : Color.Default;
      
    }
  }
}
