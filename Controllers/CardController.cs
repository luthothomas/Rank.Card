using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Rank.Card.Models;

namespace Rank.Card.Controllers
{
    public class CardController : Controller
    {
        string expre = @"^4[0-9]{12}(?:[0-9]{3})|(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}|3[47][0-9]{13}|6(?:011|5[0-9]{2})[0-9]{12}$";
        string expeVisa = @"^4[0-9]{12}(?:[0-9]{3})?$";
        string experAmex = @"^3[47][0-9]{13}$";
        string experMasterCard = @"^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$";
        string experDiscover = @"^6(?:011 | 5[0 - 9]{ 2})[0 - 9]{ 12}$";

// GET: Card
        public  ActionResult Index()
        {
            IEnumerable<Models.Card> CardList = new List<Models.Card>();
            int id = 0;
            string cardNumber = "";
            CardList = GetCardList(id, cardNumber);
            return View(CardList);          
        }
       
        public bool CardExist(string cardNumber)
        {
            List<Models.Card> CardList = new List<Models.Card>();
            int id = 0;
            
            CardList = GetCardList(id, cardNumber);

            if (CardList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            

        }
        public ActionResult Details(int id)
        {
            return View();
        }

        //public List<CardTypes.CreditCardType> GetCardTypeList()
        //{
        //    List<CardTypes.CreditCardType> list = new List<CardTypes.CreditCardType>();
        //    return list;
        //}
        public static List<Rank.Card.Models.Card> GetCardList(int? Id,string CardNumber)
        {

                List<Rank.Card.Models.Card> CardDetailsList = new List<Rank.Card.Models.Card>();
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection();
                con.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + AppDomain.CurrentDomain.BaseDirectory + "App_Data\\Database1.mdf" + ";Integrated Security = True";

                try
                {
                
                    var cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_GetCardList";
                    cmd.CommandTimeout = 300;
                    cmd.Parameters.AddWithValue("@Id", Id.ToString());
                    cmd.Parameters.AddWithValue("@CardNumber", CardNumber);
                  
                    con.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var card = new Rank.Card.Models.Card();

                            card.Id = reader.GetValue(0).ToString();
                            card.CardHolderName = reader.GetValue(1).ToString();
                            card.CardNumber = reader.GetValue(2).ToString();
                            card.CardType = reader.GetValue(3).ToString();
                            card.DateCreated = reader.GetValue(4).ToString();
                            card.DateExpiryYear = reader.GetValue(5).ToString();
                            card.DateExpiryMonth = reader.GetValue(6).ToString();
                            CardDetailsList.Add(card);
                        }
                        return CardDetailsList;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
         }        

       
        public ActionResult Create(Rank.Card.Models.Card card)
        {
            
                var enumList = Enum.GetValues(typeof(Card.Models.CardTypes.CardTypeList)).Cast<Card.Models.CardTypes.CardTypeList>().Select(v => new SelectListItem
                {
                Text = v.ToString(),
                Value = (v).ToString()
                }).ToList();
                ViewData["CardTypeList"] = enumList;
                ViewBag.LogoList = enumList;

                var enumCardList = Enum.GetValues(typeof(Card.Models.CardTypes.CardTypeList)).Cast<Card.Models.CardTypes.CardTypeList>().Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = (v).ToString()
                }).ToList();
                ViewData["enumCardList"] = enumCardList;
                ViewBag.enumCardList = enumCardList;



            var enumMonthList = Enum.GetValues(typeof(Card.Models.CardTypes.MonthList)).Cast<Card.Models.CardTypes.MonthList>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = (v).ToString()
            }).ToList();
            ViewData["enumMonthList"] = enumMonthList;
            ViewBag.enumMonthList = enumMonthList;

            if (card == null){ card = new Models.Card(); }             


                string CardHolderName = Request["txtCardHolderName"];
                string CardNumber = Request["txtCardNumber"];                       
                string DateExpiryYear = Request["txtDateExpiryYear"];
                string DateExpiryMonth = Request["txtDateExpiryMonth"];
                string CardType = Request["enumCardList"];
                string Month = Request["enumMonthList"];     
           
                if (string.IsNullOrEmpty(CardHolderName) == false)
                {
                    if (!CardExist(CardNumber)) // Validate if the Card exist
                    {
                            if (ValidateCardByType(CardNumber, CardType))// Validate if the Card matches the correct format
                            {
                                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection();
                                con.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + AppDomain.CurrentDomain.BaseDirectory + "App_Data\\Database1.mdf" + ";Integrated Security = True";

                                var cmd = con.CreateCommand();
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = "sp_InsertCard";

                                cmd.Parameters.AddWithValue("@CardHolderName", CardHolderName);
                                cmd.Parameters.AddWithValue("@CardNumber", CardNumber);
                                cmd.Parameters.AddWithValue("@CardType", CardType);
                                cmd.Parameters.AddWithValue("@DateExpiryYear", DateExpiryYear);
                                cmd.Parameters.AddWithValue("@DateExpiryMonth", Month);
                                try
                                {
                                    cmd.CommandTimeout = 300;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    throw new ArgumentException(ex.Message);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError(nameof(CardNumber), "CarNumber does not match the format.");                               
                            }
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(CardNumber), "CarNumber alreadyt exist");                       
                    }
                }
            else
            {
                ModelState.AddModelError(nameof(CardHolderName), "CardHolderName is empty");                
            }
            return View(card);
        }

        private bool ValidateCardByType(string CardNumber,string Type)
        {
            bool results = false;
            
            if (Type == "Visa")
            {
                if (!Regex.Match(CardNumber, expeVisa).Success)
                {
                    results= true;
                }
            }
            if (Type == "Amex")
            {
                if (!Regex.Match(CardNumber, experAmex).Success)
                {
                    results = true;
                }

            }
            if (Type == "MasterCard")
            {
                if (!Regex.Match(CardNumber, experMasterCard).Success)
                {
                    results = true;
                }
            }
            if (Type == "Discover")
            {
                if (!Regex.Match(CardNumber, experDiscover).Success)
                {
                    results = true;
                }
            }
            return results;        
        }
        // GET: Card/Edit/5
        public ActionResult Edit(int id)
        {
            Models.Card card = new Models.Card();
            string cardNumber = "";
            card = GetCardList(id, cardNumber)[0];
                        
            var enumList = Enum.GetValues(typeof(Card.Models.CardTypes.CardTypeList)).Cast<Card.Models.CardTypes.CardTypeList>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = (v).ToString()
            }).ToList();

            ViewData["CardTypeList"] = enumList;
            ViewBag.CardTypeList = enumList;

            var enumCardList = Enum.GetValues(typeof(Card.Models.CardTypes.CardTypeList)).Cast<Card.Models.CardTypes.CardTypeList>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = (v).ToString()
            }).ToList();
            ViewData["enumCardList"] = enumCardList;
            ViewBag.enumCardList = enumCardList;

            return View(card);
        }

        // POST: Card/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Card/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Card/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
