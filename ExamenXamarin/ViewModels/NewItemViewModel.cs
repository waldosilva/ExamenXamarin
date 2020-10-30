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
    //private string userName;
    
    private string itemId;
    private int id;
    //private Usuario usuario;


    public NewItemViewModel()
    {
      AddValidationRules();
      //_errors = new List<string>();
      //Errors = new ObservableCollection<string>();
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
      //Password.Item1.Validations.Add(new IsValidPasswordRule<string> { ValidationMessage = "Password between 8-20 characters; must contain at least one lowercase letter, one uppercase letter, one numeric digit, and one special character" });
      Password.Validations.Add(new IsLenghtValidRule<string> { ValidationMessage = "Password between 5-12 characters", MaximunLenght = 12, MinimunLenght = 5});
      Password.Validations.Add(new IsOneUpperAtLast<string> { ValidationMessage = "Password must consist of a mixture of letters and numbers"}); 
    }

    private bool ValidateSave()
    {
      //Error="prueba";
      List<string> errors = new List<string>();

      //if (Password == "")
      //  errors.Add("An e-mail address is required.");

      //if (_password == "")
      //errors.Add("A password is required.");

      //Errors = errors;

      bool isUserValid = UserName.Validate();
      //bool isLastNameValid = LastName.Validate();
      //bool isBirthDayValid = BirthDay.Validate();
      //bool isPhoneNumberValid = PhoneNumber.Validate();
      //bool isEmailValid = Email.Validate();
      bool isPasswordValid = Password.Validate();
      //bool isTermsAndConditionValid = TermsAndCondition.Validate();
      return isPasswordValid&& isUserValid;
      //return isFirstNameValid && isLastNameValid && isBirthDayValid
      //       && isPhoneNumberValid && isEmailValid && isPasswordValid && isTermsAndConditionValid;

      //return _errors.Count == 0;
    }

    //public string UserName
    //{
    //  get => userName;
    //  set => SetProperty(ref userName, value);
    //}
    private ValidatableObject<string> userName = new ValidatableObject<string>();
    public ValidatableObject<string> UserName
    {
      get => userName;
      set => SetProperty(ref userName, value);
    }

    //public string Password
    //{
    //  get => password;
    //  set => SetProperty(ref password, value);
    //}
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
        //Password.Item1.Value = item.Password;
        UserName.Value = item.UserName;
        //Item = item;
      }
      catch (Exception)
      {
        Debug.WriteLine("Failed to Load Item");
      }
    }
    //public ObservableCollection<string> Errors { get; set; }
    //private string error;
    //public string Error
    //{
    //  get { return error; }
    //  set
    //  {
    //    error = value;
    //    OnPropertyChanged("Error");
    //    Errors.Add(error);
    //  }
    //}
    //private List<string> _errors=new List<string>();
    //public List<string> Errors
    //{
    //  set { SetProperty(ref _errors, value); }
    //  get { return _errors; }
    //}


    public Command SaveCommand { get; }
    public Command CancelCommand { get; }

    private async void OnCancel()
    {
      // This will pop the current page off the navigation stack
      await Shell.Current.GoToAsync("..");
    }

    private async void OnSave()
    {
      //Errors.Clear();

      if (ValidateSave()) { 

      Usuario newItem = new Usuario()
      {
        Id = itemId == null || itemId.Equals("0")?0:int.Parse(itemId),
        UserName = UserName.Value,
        Password = Password.Value
      };

      //await DataStore.AddItemAsync(newItem);
      await App.Database.SaveUserAsync(newItem);


      // This will pop the current page off the navigation stack
      //await Shell.Current.GoToAsync("..");
      //await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
      //await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
      //await Shell.Current.po();
      await Shell.Current.GoToAsync("../..");
    }
    }
  }
}
