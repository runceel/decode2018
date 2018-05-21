using System;
using System.Collections.Generic;
using System.Linq;
using DemoApp.Helpers;
using DemoApp.Models;

namespace DemoApp.ViewModels
{
    public class DataGridViewModel : Observable
    {
        public IEnumerable<Person> People { get; } = Enumerable.Range(1, 1000)
            .Select(x => new Person
            {
                Id = x,
                Name = $"Tanaka Taro {x}",
            })
            .ToArray();

        public DataGridViewModel()
        {
        }
    }
}
