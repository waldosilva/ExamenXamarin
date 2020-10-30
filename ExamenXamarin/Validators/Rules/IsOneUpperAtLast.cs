using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ExamenXamarin.Validators.Rules
{
  public class IsOneUpperAtLast<T> : IValidationRule<T>
  {
    public string ValidationMessage { get; set; }
    public Regex RegexPassword { get; set; } = new Regex("[^\\w\\d]*(([0-9]+.*[A-Za-z]+.*)|[A-Za-z]+.*([0-9]+.*))");

    public bool Check(T value)
    {
      return (RegexPassword.IsMatch($"{value}"));
    }
  }
}
