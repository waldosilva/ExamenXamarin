using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ExamenXamarin.Services;
using ExamenXamarin.Views;
using ExamenXamarin.Data;

namespace ExamenXamarin
{
  public partial class App : Application
  {
    static TestDatabase database;
    public App()
    {
      InitializeComponent();

      //DependencyService.Register<MockDataStore>();
      MainPage = new AppShell();
    }
    public static TestDatabase Database
    {
      get
      {
        if (database == null)
        {
          database = new TestDatabase();
        }
        return database;
      }
    }
    protected override void OnStart()
    {
    }

    protected override void OnSleep()
    {
    }

    protected override void OnResume()
    {
    }
    
  }
}
