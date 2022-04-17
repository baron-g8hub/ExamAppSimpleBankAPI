using BusinessLogicLayer;
using Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SimpleBankWebAPI
{
    public class AccountsViewModel
    {
        public AccountsManager _repo;

        public AccountsViewModel()
        {
            EntityList = new List<Account>();
            SelectListItems = LoadAccountTypes();
        }

        public List<Account> EntityList { get; set; }
        public List<SelectListItem> SelectListItems { get; set; }

        public List<SelectListItem> LoadAccountTypes()
        {
            var types = new List<SelectListItem>()
            {
                new SelectListItem { Value = "1", Text = "Savings" },
                new SelectListItem { Value = "2", Text = "Checking" },
                new SelectListItem { Value = "3", Text = "Credit" },
            };
            return types;
        }
       
        public int Account_ID { get; set; }

        [Required]
        [Display(Name = "Account Name")]
        public string AccountName { get; set; }

        [Required]
        [Display(Name = "Account Type")]
        public int AccountType { get; set; }

        [Required]
        [Display(Name = "Running Balance")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid amount.")]
        public double SavingsBalance { get; set; }

        public bool IsActive { get; set; }
    }
}
