using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rank.Card.Models
{
    public class Card
    {

        
        public string Id;
        [Required]
        public string CardHolderName;

        
        [RegularExpression(@"^(?:(?<Visa>4\\d{3})|(?<MasterCard> 5[1 - 5]\\d{2})|(?<Discover>6011)|(?(Amex)(?:\\d{ 6}\\1\\d{ 5})|(?:\\d{ 4}\\1\\d{ 4}\\1\\d{ 4})))$")]
        [Required]        
        public string CardNumber;

        [Required]
        public string CardType;

        [Display(Name = "DateCreated")]
        [DataType(DataType.Date)]
        public string DateCreated;

        [Display(Name = "DateExpiryYear")]
        [DataType(DataType.PostalCode)]
        public string DateExpiryYear;

        [Display(Name = "DateExpiryMonth")]
        [DataType(DataType.Custom)]
        public string DateExpiryMonth;
               
        public CardTypes.CardTypeList CardTypeList;
        public CardTypes.MonthList MonthList;
        public SelectList GetCartTypes { get; set; }

      
    }
}