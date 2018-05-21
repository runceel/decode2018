using System;
using System.ComponentModel.DataAnnotations;

namespace DemoApp.Models
{
    public class Person
    {
        private static Random Random { get; } = new Random();
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Display(Name = "名前")]
        public string Name { get; set; }
        [Display(Name = "基本給")]
        public int BaseSalary { get; set; }
        [Display(Name = "入社日")]
        public DateTime HireDate { get; set; }
        [Display(Name = "管理職フラグ")]
        public bool IsManager { get; set; }
        [Display(Name = "性別")]
        public Sex Sex { get; set; }

        public Person()
        {
            BaseSalary = 300000 + Random.Next(200000);
            HireDate = new DateTime(1990, 4, 1) + TimeSpan.FromDays(Random.Next(2000));
            IsManager = Random.Next(10) % 10 == 0;
            Sex = Random.Next(2) % 2 == 0 ? Sex.Male : Sex.Female;
        }
    }

    public enum Sex
    {
        [Display(Name = "未選択")]
        None,
        [Display(Name = "女性")]
        Female,
        [Display(Name = "男性")]
        Male,
    }
}
