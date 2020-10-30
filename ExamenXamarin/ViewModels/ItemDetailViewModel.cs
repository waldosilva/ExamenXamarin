using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ExamenXamarin.Models;
using ExamenXamarin.Views;
using Xamarin.Forms;

namespace ExamenXamarin.ViewModels
{
  [QueryProperty(nameof(ItemId), nameof(ItemId))]
  public class ItemDetailViewModel : BaseViewModel
  {

    public Command EditItemCommand { get; }
    public Command DeleteCommand { get; }
    

    private string itemId;
    private string userName;
    private string password;
    public int Id { get; set; }
    public Usuario Item { get; set; }

    public string UserName
    {
      get => userName;
      set => SetProperty(ref userName, value);
    }

    public string Password
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

    public async void LoadItemId(string itemId)
    {
      try
      {
        var item = await App.Database.GetUserAsync(int.Parse(itemId));
        Id = item.Id;
        Password = item.Password;
        UserName = item.UserName;
        Item = item;
      }
      catch (Exception)
      {
        Debug.WriteLine("Failed to Load Item");
      }
    }
    public async void DeleteItemId(string itemId)
    {
      try
      {
        var item = await App.Database.GetUserAsync(int.Parse(itemId));
        await App.Database.DeleteUserAsync(item);
        
      }
      catch (Exception e)
      {
        Debug.WriteLine("Errror:"+ e);
      }
    }
    private async void OnDelete()
    {
      // This will pop the current page off the navigation stack
      DeleteItemId(ItemId);

      await Shell.Current.GoToAsync("..");
    }
    public ItemDetailViewModel()
    {
      
      EditItemCommand = new Command(OnEditItem);
      DeleteCommand = new Command(OnDelete);

    }
    private async void OnEditItem(object obj)
    {

      await Shell.Current.GoToAsync($"{nameof(NewItemPage)}?{nameof(NewItemViewModel.ItemId)}={Id.ToString()}");
    }
  }
}
