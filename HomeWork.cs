using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Lecture1
{
    internal class HomeWork
    {

        private string CheckForNearStreat(string address)
        {         
            String[] words = address.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();
            string streatAddress = default;

            foreach (var word in words)
                streatAddress += word;         

            return streatAddress;
        }
        private decimal CurrencyExchanger(decimal amount, string currenciesFrom)
        {
            
            decimal USD = 1.19M;

            decimal newAmount = currenciesFrom switch
            {
                "USD" => amount * USD,
                _ => amount
            };          
            return newAmount;
        }

        private decimal GetFullPrice(
                                    IEnumerable<string> destinations,
                                    IEnumerable<string> clients,
                                    IEnumerable<int> infantsIds,
                                    IEnumerable<int> childrenIds,
                                    IEnumerable<decimal> prices,
                                    IEnumerable<string> currencies)
        {
            decimal fullPrice = default;
            const decimal discountForInfant = 50;
            const decimal discountForChildren = 25;
            const decimal discountForNearbyStreat = 15;

            if (destinations.Count() == clients.Count() && destinations.Count() == prices.Count() && destinations.Count() == currencies.Count())
            {

                List<decimal> discount = new List<decimal>();
                List<decimal> listPrices = new List<decimal>();
              
                foreach (var destination in destinations)
                    discount.Add(0);

                foreach (var price in prices)                
                    listPrices.Add(price);

                for (int index = 0; index < currencies.Count(); index++)
                {
                    var currency = currencies.ElementAt(index);                   
                    if (currency != "USD") listPrices[index] = CurrencyExchanger(listPrices[index], currency);
                }

                for (int index = 0; index < destinations.Count(); index++)
                {
                    var destination = destinations.ElementAt(index);
                    if (destination.Contains("Wayne Street")) listPrices[index] += 10;
                    if (destination.Contains("North Heather Street")) listPrices[index] -= 5.36M;
                }

                foreach (var infantId in infantsIds)
                    discount[infantId - 1] = discountForInfant;

                foreach (var childrenId in childrenIds)
                    discount[childrenId - 1] = discountForChildren;
                CheckForNearStreat("address1");

                string pastAddress = default;
                int pointer = 0;
                foreach (var destination in destinations)
                {
                    if (CheckForNearStreat(destination) == pastAddress) discount[pointer] += discountForNearbyStreat;
                    pointer++;
                    pastAddress = CheckForNearStreat(destination);
                }

                for (int index = 0; index < destinations.Count(); index++)
                {
                   
                    //Console.WriteLine("{0} ==> {1}", listPrices[index],listPrices[index] - (listPrices[index] / 100) * discount[index]);
                    fullPrice += listPrices[index] - ( (listPrices[index] / 100) * discount[index]);
                }

            }
            
            return fullPrice;
        }

        public decimal InvokePriceCalculatiion()
        {
            var destinations = new[]
            {
                "949 Fairfield Court, Madison Heights, MI 48071",
                "367 Wayne Street, Hendersonville, NC 28792",
                "910 North Heather Street, Cocoa, FL 32927",
                "911 North Heather Street, Cocoa, FL 32927",
                "706 Tarkiln Hill Ave, Middleburg, FL 32068",
            };

            var clients = new[]
            {
                "Autumn Baldwin",
                "Jorge Hoffman",
                "Amiah Simmons",
                "Sariah Bennett",
                "Xavier Bowers",
            };

            var infantsIds = new[] { 2 };
            var childrenIds = new[] { 3, 4 };

            var prices = new[] { 100, 25.23m, 58, 23.12m, 125 };
            var currencies = new[] { "USD", "USD", "EUR", "USD", "USD" };

            return GetFullPrice(destinations, clients, infantsIds, childrenIds, prices, currencies);
        }
    }
}
