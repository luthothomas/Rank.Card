using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rank.Card.Models
{
    public class CardTypes
    {
       
        private const string cardRegex = "^(?:(?<Visa>4\\d{3})|(?< MasterCard > 5[1 - 5]\\d{2})|(?<Discover>6011)|(?<DinersClub>(?:3[68]\\d{2})| (?: 30[0 - 5]\\d))|(?<Amex> 3[47]\\d{ 2}))([-] ?)(?(DinersClub)(?:\\d{ 6}\\1\\d{ 4})| (? (Amex)(?:\\d{ 6}\\1\\d{ 5})|(?:\\d{ 4}\\1\\d{ 4}\\1\\d{ 4})))$";


        public enum MonthList
        {
            January = 01,
            February = 02,
            March = 03,
            April = 04,
            May = 05,
            June = 06,
            July = 07,
            August = 08,
            September = 09,
            October = 10,
            November = 11,
            December = 12
        }
        //public string YearList()
        //{
        //    2022,
        //    2023,
        //    2024,
        //    2025,
        //    2026,
        //    2027,
        //    2028,         
        //}
        public enum CardTypeList
        {
            Visa = 0,
            Amex = 1,
            MasterCard = 2,           
            Discover = 3
           
        }
    }
}