using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using ExamenXamarin.Models;
using ExamenXamarin.Validators;
using ExamenXamarin.Validators.Rules;
using ExamenXamarin.Views;
using Xamarin.Forms;

namespace ExamenXamarin.ViewModels
{
  [QueryProperty(nameof(ItemId), nameof(ItemId))]
  public class NewItemViewModel : BaseViewModel
  {
    
    
    private string itemId;
    private int id;
    


    public NewItemViewModel()
    {
      AddValidationRules();
      SaveCommand = new Command(OnSave);
      CancelCommand = new Command(OnCancel);
      this.PropertyChanged +=
          (_, __) => SaveCommand.ChangeCanExecute();
    }
    public void AddValidationRules()
    {

      UserName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "UserName Required" });
      //Password Validation Rules
      Password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Password Required" });
      Password.Validations.Add(new IsLenghtValidRule<string> { ValidationMessage = "Password between 5-12 characters", MaximunLenght = 12, MinimunLenght = 5});
      Password.Validations.Add(new IsOneUpperAtLast<string> { ValidationMessage = "Password must consist of a mixture of letters and numbers"}); 
    }

    private bool ValidateSave()
    {
      List<string> errors = new List<string>();

    
      bool isUserValid = UserName.Validate();
      bool isPasswordValid = Password.Validate();
      return isPasswordValid&& isUserValid;
      
    }

    
    private ValidatableObject<string> userName = new ValidatableObject<string>();
    public ValidatableObject<string> UserName
    {
      get => userName;
      set => SetProperty(ref userName, value);
    }

    private ValidatableObject<string> password = new ValidatableObject<string>();
    public ValidatableObject<string> Password
    {
      get => password;
      set => SetProperty(ref password, value);
    }
    public string ItemId
    {
      get
      {
        return itemId;
      }
      set
      {
        itemId = value;
        LoadItemId(value);
      }
    }
    public int Id
    {
      get
      {
        return id;
      }
      set
      {
        id = value;
      }
    }
    public async void LoadItemId(string itemId)
    {
      try
      {
        var item = await App.Database.GetUserAsync(int.Parse(itemId));
        Id = item.Id;
        UserName.Value = item.UserName;
      }
      catch (Exception)
      {
        Debug.WriteLine("Failed to Load Item");
      }
    }


    public Command SaveCommand { get; }
    public Command CancelCommand { get; }

    private async void OnCancel()
    {
      // This will pop the current page off the navigation stack
      await Shell.Current.GoToAsync("..");
    }

    private async void OnSave()
    {

      if (ValidateSave()) { 

      Usuario newItem = new Usuario()
      {
        Id = itemId == null || itemId.Equals("0")?0:int.Parse(itemId),
        UserName = UserName.Value,
        Password = Password.Value
      };

      await App.Database.SaveUserAsync(newItem);

      await Shell.Current.GoToAsync("../..");
    }
    }
  }
}
