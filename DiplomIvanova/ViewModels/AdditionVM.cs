using DiplomIvanova.DataBase.Entities;
using DiplomIvanova.Helpers;
using DiplomIvanova.Interfaces;
using DiplomIvanova.ViewModels.BaseViewModels;
using DiplomIvanova.Views.Pages.ItemsViewPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomIvanova.ViewModels
{
    public class AdditionVM<T>:BaseViewModel, ISaveViewModel where T : class,IEntityBase,new()
    {
        private T _entity;
        private Dictionary<Entry, string> _properties;
        public Command SaveCommand => new(AddEntityToDb);
        public AdditionVM(T entity,Dictionary<Entry,string> properties)
        {
            _entity = entity;
            _properties = properties;
        }

        private async Task FillPropertiesAsync()
        {
            foreach (var property in _properties)
            {
                if (string.IsNullOrWhiteSpace(property.Key.Text))
                    continue;
                try
                {
                    var prop = _entity.GetType().GetProperty(property.Value);
                    if (prop != null)
                    {
                        if (prop.PropertyType.IsAssignableFrom(typeof(double)))
                        {
                            prop.SetValue(_entity, double.Parse(property.Key.Text));
                        }
                        if (prop.PropertyType.IsAssignableFrom(typeof(int)))
                        {
                            prop.SetValue(_entity, int.Parse(property.Key.Text));
                        }
                        if (prop.PropertyType.IsAssignableFrom(typeof(DateTime)))
                        {
                            prop.SetValue(_entity, DateTime.Parse(property.Key.Text));
                        }
                        prop.SetValue(_entity, property.Key.Text);
                    }
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Ошибка", ex.Message, "Ок");
                }
            }
        }

        private async void AddEntityToDb()
        {
            await FillPropertiesAsync();
            if(await DataBaseHelper.AddItemAsync(_entity))
            {
                await Shell.Current.GoToAsync($"{nameof(ItemsViewPage)}?{nameof(ItemsViewPage.ItemsType)}={_entity.GetType().Name}");
            }
        }
    }
}
